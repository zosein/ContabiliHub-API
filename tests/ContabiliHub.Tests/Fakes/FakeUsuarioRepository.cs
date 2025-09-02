using ContabiliHub.Domain.Entities;
using ContabiliHub.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContabiliHub.Tests.Fakes;

public class FakeUsuarioRepository : IUsuarioRepository
{
    private readonly List<Usuario> _usuarios = new();

    public Task<Usuario?> ObterPorEmailAsync(string email) =>
        Task.FromResult(_usuarios.FirstOrDefault(u => u.Email == email));

    public Task AdicionarAsync(Usuario usuario)
    {
        _usuarios.Add(usuario);
        return Task.CompletedTask;
    }
}