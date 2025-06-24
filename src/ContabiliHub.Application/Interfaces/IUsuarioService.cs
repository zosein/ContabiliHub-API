using ContabiliHub.Application.DTOs;
using ContabiliHub.Domain.Entities;

namespace ContabiliHub.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> RegistrarAsync(UsuarioRegisterDto dto);
        Task<string> LoginAsync(UsuarioLoginDto dto);
    }
}