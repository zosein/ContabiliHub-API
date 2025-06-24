using ContabiliHub.Application.DTOs;
using ContabiliHub.Domain.Entities;
using AutoMapper;

namespace ContabiliHub.Application.Mappings
{
    public class ServicoPrestadoProfile : Profile
    {
        public ServicoPrestadoProfile()
        {
            CreateMap<ServicoPrestado, ServicoPrestadoReadDto>()
                .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.NomeCompleto : null));

            CreateMap<ServicoPrestadoCreateDto, ServicoPrestado>();

            CreateMap<ServicoPrestado, ReciboDto>()
                .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.NomeCompleto : string.Empty))
                .ForMember(dest => dest.DescricaoServico, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor))
                .ForMember(dest => dest.DataPrestacao, opt => opt.MapFrom(src => src.DataPrestacao))
                .ForMember(dest => dest.DataEmissao, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.AssinaturaResponsavel, opt => opt.MapFrom(src => "____________________"));
        }
    }
}