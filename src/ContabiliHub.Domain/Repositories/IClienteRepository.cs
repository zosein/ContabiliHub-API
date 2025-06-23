using ContabiliHub.Domain.Entities;

namespace ContabiliHub.Domain.Repositories
{
    public interface IClienteRepository
    {
        Task<Cliente?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Cliente>> ObterTodosAsync();
        Task AdicionarAsync(Cliente cliente);
        Task AtualizarAsync(Cliente cliente);
        Task RemoverAsync(Guid id);
        Task<bool> ExisteCpfAsync(string cpf);
    }
}