using TheNewYorkTimes.Data.Repositories;
using TheNewYorkTimes.Interfaces.Repositories;
using TheNewYorkTimes.Interfaces.Services;
using TheNewYorkTimes.Services;

namespace TheNewYorkTimes.Infrastructure.Configurations
{
    public static class Extension
    {
        public static IServiceCollection DependencyMap(this IServiceCollection services)
        {
            services.AddTransient<INoticiaRepository, NoticiaRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IHashService, HashService>();

            return services;
        }
    }
}
