using ContabiliHub.Domain.Entities;
using ContabiliHub.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ContabiliHub.Tests.Fakes;

public class FakeClienteRepository : IClienteRepository
{
    private readonly List<Cliente> _clientes = new();

    public Task AdicionarAsync(Cliente cliente)
    {
        _clientes.Add(cliente);
        return Task.CompletedTask;
    }

    public Task AtualizarAsync(Cliente cliente)
    {
        var index = _clientes.FindIndex(c => c.Id == cliente.Id);
        if (index != -1)
        {
            _clientes[index] = cliente;
        }
        return Task.CompletedTask;
    }

    public Task<bool> ExisteCpfAsync(string cpf) =>
        Task.FromResult(_clientes.Any(c => c.CPF == cpf));

    public Task<bool> ExisteEmailAsync(string email) =>
        Task.FromResult(_clientes.Any(c => c.Email == email));

    public Task<Cliente?> ObterPorIdAsync(Guid id) =>
        Task.FromResult(_clientes.FirstOrDefault(c => c.Id == id));

    public Task<IEnumerable<Cliente>> ObterTodosAsync() =>
        Task.FromResult(_clientes.AsEnumerable());

    public Task RemoverAsync(Guid id)
    {
        _clientes.RemoveAll(c => c.Id == id);
        return Task.CompletedTask;
    }
}