using ContabiliHub.Domain.Entities;
using ContabiliHub.Application.DTOs;

namespace ContabiliHub.Application.Interfaces
{
    public interface IServicoPrestadoService
    {
        Task<ServicoPrestado?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<ServicoPrestado>> ObterTodosAsync();
        Task<IEnumerable<ServicoPrestado>> ObterPorClienteIdAsync(Guid clienteId);
        Task<ResultadoPaginado<ServicoPrestado>> ObterTodosPaginadoAsync(int pagina, int paginaTamanho, Guid? clienteId, bool? pago, string? busca);
        Task AdicionarAsync(ServicoPrestado servicoPrestado);
        Task AtualizarAsync(ServicoPrestado servicoPrestado);
        Task RemoverAsync(Guid id);
    }
}