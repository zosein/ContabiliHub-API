using ContabiliHub.Domain.Entities;

namespace ContabiliHub.Application.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ObterTodosAsync();
        Task<Cliente?> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(Cliente cliente);
        Task AtualizarAsync(Cliente cliente);
        Task RemoverAsync(Guid id);
    }
}