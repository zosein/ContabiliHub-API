using ContabiliHub.Domain.Entities;

namespace ContabiliHub.Application.DTOs
{
    public record UsuarioRegisterDto(string NomeCompleto, string Email, string Senha)
    {
        public Usuario ToEntity(string senhaHash) => new()
        {
            NomeCompleto = this.NomeCompleto,
            Email = this.Email,
            SenhaHash = senhaHash
        };
    }
}