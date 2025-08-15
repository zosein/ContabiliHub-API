using ContabiliHub.Domain.Entities;
using ContabiliHub.Domain.Repositories;
using ContabiliHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContabiliHub.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteCpfAsync(string cpf)
        {
            return await _context.Clientes.AnyAsync(c => c.CPF == cpf);
        }
        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Clientes.AnyAsync(c => c.Email == email);
        }

        public async Task<Cliente?> ObterPorIdAsync(Guid id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<IEnumerable<Cliente>> ObterTodosAsync()
        {
            return await _context.Clientes
                .OrderBy(c => c.NomeCompleto)
                .ToListAsync();

        }

        public async Task RemoverAsync(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}