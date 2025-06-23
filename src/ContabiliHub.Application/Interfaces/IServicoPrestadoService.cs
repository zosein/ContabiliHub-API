using ContabiliHub.Domain.Entities;

namespace ContabiliHub.Application.Interfaces
{
    public interface IServicoPrestadoService
    {
        Task<ServicoPrestado?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<ServicoPrestado>> ObterTodosAsync();
        Task<IEnumerable<ServicoPrestado>> ObterPorClienteIdAsync(Guid clienteId);
        Task AdicionarAsync(ServicoPrestado servicoPrestado);
        Task AtualizarAsync(ServicoPrestado servicoPrestado);
        Task RemoverAsync(Guid id);
    }
}