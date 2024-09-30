using AutoMapper;
using SixConsult.NET.Foundation.Menu.Contracts.ConfiguracaoMenu;
using TesteWebCanToWebAPI.Models;

namespace TesteWebCanToWebAPI.Config
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            RegisterMaps();
        }

        private void RegisterMaps()
        {

            ///Ajustar quando tiver o contrato
            #region Notificacao
            CreateMap<MenuVo, NotificacaoViewModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UrlIcone))
                    .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Url))
                    .ForMember(dest => dest.MsgLida, opt => opt.MapFrom(src => src.Nome))
                    .ForMember(dest => dest.Hora, opt => opt.MapFrom(src => src.GuidProduto))
                    .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.LogTela))
                    .ReverseMap();
            #endregion
        }
    }
}