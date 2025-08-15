using ContabiliHub.Domain.Entities;

namespace ContabiliHub.Domain.Repositories
{
    public interface IServicoPrestadoRepository
    {
        Task<ServicoPrestado?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<ServicoPrestado>> ObterTodosAsync();
        Task<IEnumerable<ServicoPrestado>> ObterPorClienteIdAsync(Guid clienteId);
        Task<(IEnumerable<ServicoPrestado> Itens, int Total)> ObterTodosPaginadoAsync(int pagina, int paginaTamanho, Guid? clienteId, bool? pago, string? busca);
        Task AdicionarAsync(ServicoPrestado servicoPrestado);
        Task AtualizarAsync(ServicoPrestado servicoPrestado);
        Task RemoverAsync(Guid id);
    }
}