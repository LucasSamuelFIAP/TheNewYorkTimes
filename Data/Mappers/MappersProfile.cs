using AutoMapper;
using TheNewYorkTimes.Models;
using TheNewYorkTimes.Models.InputModels;
using TheNewYorkTimes.Models.ViewModels;

namespace TheNewYorkTimes.Data.Mappers
{
    public class MappersProfile : Profile
    {
        public MappersProfile()
        {
            CreateMap<NoticiaViewModel, NoticiaInputModel>().ReverseMap();
            CreateMap<Noticia, NoticiaViewModel>().ReverseMap();

            CreateMap<UsuarioViewModel, UsuarioInputModel>().ReverseMap();
            CreateMap<Usuario, UsuarioViewModel>().ReverseMap();

            CreateMap<LoginViewModel, LoginInputModel>().ReverseMap();
            CreateMap<Usuario, LoginViewModel>().ReverseMap();

            AddGlobalIgnore("Id");
        }
    }
}
