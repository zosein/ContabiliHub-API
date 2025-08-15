using ContabiliHub.Domain.Entities;
using ContabiliHub.Domain.Repositories;
using ContabiliHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace ContabiliHub.Infrastructure.Repositories
{
    public class ServicoPrestadoRepository : IServicoPrestadoRepository
    {
        private readonly AppDbContext _context;

        public ServicoPrestadoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(ServicoPrestado servicoPrestado)
        {
            await _context.ServicosPrestados.AddAsync(servicoPrestado);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(ServicoPrestado servicoPrestado)
        {
            _context.ServicosPrestados.Update(servicoPrestado);
            await _context.SaveChangesAsync();
        }

        public async Task<ServicoPrestado?> ObterPorIdAsync(Guid id)
        {
            return await _context.ServicosPrestados
                .Include(s => s.Cliente)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<ServicoPrestado>> ObterTodosAsync()
        {
            return await _context.ServicosPrestados
                .Include(s => s.Cliente)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServicoPrestado>> ObterPorClienteIdAsync(Guid clienteId)
        {
            return await _context.ServicosPrestados
                .Where(s => s.ClienteId == clienteId)
                .Include(s => s.Cliente)
                .ToListAsync();
        }

        public async Task<(IEnumerable<ServicoPrestado> Itens, int Total)> ObterTodosPaginadoAsync(int pagina, int paginaTamanho, Guid? clienteId, bool? pago, string? busca)
        {
            var query = _context.ServicosPrestados.AsQueryable();

            if (clienteId.HasValue)
                query = query.Where(s => s.ClienteId == clienteId.Value);

            if (pago.HasValue)
                query = query.Where(s => s.Pago == pago.Value);

            if (!string.IsNullOrEmpty(busca))
                query = query.Where(s => s.Descricao.Contains(busca));

            var total = await query.CountAsync();

            var itens = await query
                .Include(s => s.Cliente)
                .OrderByDescending(s => s.DataPrestacao)
                .Skip((pagina - 1) * paginaTamanho)
                .Take(paginaTamanho)
                .ToListAsync();

            return (itens, total);


        }

        public async Task RemoverAsync(Guid id)
        {
            var servicoPrestado = await _context.ServicosPrestados.FindAsync(id);
            if (servicoPrestado != null)
            {
                _context.ServicosPrestados.Remove(servicoPrestado);
                await _context.SaveChangesAsync();
            }
        }
    }
}