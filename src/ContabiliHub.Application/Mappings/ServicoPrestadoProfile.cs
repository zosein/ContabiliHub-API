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
        }
    }
}