using ContabiliHub.Domain.Entities;

namespace ContabiliHub.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task AdicionarAsync(Usuario usuario);
    }
}