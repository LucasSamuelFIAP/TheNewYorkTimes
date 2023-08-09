using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using TheNewYorkTimes;
using TheNewYorkTimes.Data;
using TheNewYorkTimes.Infrastructure.Configurations;
using TheNewYorkTimes.Interfaces.Repositories;
using TheNewYorkTimes.Interfaces.Services;
using TheNewYorkTimes.Models;
using TheNewYorkTimes.Models.InputModels;
using TheNewYorkTimes.Models.ViewModels;
using TheNewYorkTimes.Services;

var builder = WebApplication.CreateBuilder(args);

#region COLOCAR EM UMA VARIAVEL AMBIENTE NO GIT

var key = Encoding.ASCII.GetBytes(Settings.Secret);
var connectionString = builder.Configuration.GetConnectionString("database");

#endregion

builder.Services.AddDbContext<SqlContext>(options => options.UseSqlServer(connectionString));

builder.Services.DependencyMap();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"JWT Authorization header usando o schema Bearer
                       \r\n\r\n Informe 'Bearer'[space].
                        Exemplo: \'Bearer 12345abcdef\'",
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });

    x.ResolveConflictingActions(x => x.First());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("User", policy => policy.RequireRole("user"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapPost("/api/v1/IncluirNoticia", async (INoticiaRepository repository, IMapper _mapper, NoticiaInputModel model) =>
{
    var viewModel = _mapper.Map<NoticiaViewModel>(model);
    var objModel = _mapper.Map<Noticia>(viewModel);

    try
    {
        await repository.Add(objModel);

        return Results.Ok(new
        {
            message = "Notícia cadastrada com sucesso!"
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            message = $"Erro ao cadastrar a notícia. {ex.Message}"
        });
    }
})
    .RequireAuthorization()
    .WithTags("Notícia");

app.MapGet("/api/v1/GetNoticiasAll", async (INoticiaRepository repository, IMapper _mapper) =>
{
    var noticias = await repository.GetAll();

    if (noticias != null)
        return Results.Ok(noticias);

    return Results.NoContent();
})
    .RequireAuthorization()
    .WithTags("Notícia");

app.MapGet("/api/v1/GetNoticiaById", async (INoticiaRepository repository, IMapper _mapper, int idNoticia) =>
{
    var noticia = await repository.GetById(idNoticia);

    if (noticia != null)
        return Results.Ok(noticia);

    return Results.NoContent();
})
    .RequireAuthorization()
    .WithTags("Notícia");

app.MapPost("/api/v1/IncluirUsuario", async (IUsuarioRepository usuarioRepository, IMapper _mapper, IHashService hashService, UsuarioInputModel model) =>
{
    try
    {
        if (model == null)
            return Results.BadRequest(new
            {
                message = $"Informe os dados solicitados."
            });

        var viewModel = _mapper.Map<UsuarioViewModel>(model);

        if (await usuarioRepository.VerificaSeUsuarioExiste(viewModel.LoginUser))
            return Results.Ok(new
            {
                message = "E-mail informado já está cadastrado!"
            });

        if (viewModel != null)
        {
            viewModel.Senha = hashService.CriptografarSenha(model.Senha);
            viewModel.Role = "user";
            viewModel.Ativo = true;
        }

        var objModel = _mapper.Map<Usuario>(viewModel);

        await usuarioRepository.Add(objModel);

        return Results.Ok(new
        {
            message = $"Usuário {objModel.Nome} cadastrado com sucesso!"
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            message = $"Erro ao cadastrar o usuário. {ex.Message}"
        });
    }
})
    .AllowAnonymous()
    .WithTags("Usuário");

app.MapPost("/api/v1/login", async (IUsuarioRepository usuarioRepository, IMapper _mapper, IHashService hashService, LoginInputModel model) =>
{
    if (string.IsNullOrEmpty(model.LoginUser) || string.IsNullOrEmpty(model.Senha))
        return Results.NotFound(new
        {
            message = "Informe o usuário e senha!"
        });

    var viewModel = _mapper.Map<LoginViewModel>(model);
    var objModel = _mapper.Map<Usuario>(viewModel);

    var usuario = await usuarioRepository.GetUsuarioByLogin(objModel.LoginUser);

    if (usuario == null)
        return Results.NotFound(new
        {
            message = "Usuário não cadastrado!"
        });

    var senhaDigitadaCripto = hashService.CriptografarSenha(model.Senha);

    if (senhaDigitadaCripto != usuario.Senha)
        return Results.NotFound(new
        {
            message = "Usuário ou senha incorreto!"
        });

    if (!usuario.Ativo)
        return Results.Ok(new
        {
            message = "Usuário bloqueado!"
        });

    var token = TokenService.GenereteToken(usuario);

    usuario.Senha = "";

    return Results.Ok(new
    {
        usuario = usuario,
        token = token
    });
})
    .AllowAnonymous()
    .WithTags("Usuário");

app.Run();
