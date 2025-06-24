using ContabiliHub.Domain.Entities;
using ContabiliHub.Domain.Repositories;
using ContabiliHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContabiliHub.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }
    }
}