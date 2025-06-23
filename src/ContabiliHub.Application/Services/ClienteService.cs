using ContabiliHub.Application.Interfaces;
using ContabiliHub.Domain.Entities;
using ContabiliHub.Domain.Repositories;

namespace ContabiliHub.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Cliente>> ObterTodosAsync()
        {
            return await _clienteRepository.ObterTodosAsync();
        }

        public async Task<Cliente?> ObterPorIdAsync(Guid id)
        {
            return await _clienteRepository.ObterPorIdAsync(id);
        }

        public async Task AdicionarAsync(Cliente cliente)
        {
            if (await _clienteRepository.ExisteCpfAsync(cliente.CPF))
                throw new InvalidOperationException("Já existe um cliente com este CPF.");

            await _clienteRepository.AdicionarAsync(cliente);
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            await _clienteRepository.AtualizarAsync(cliente);
        }

        public async Task RemoverAsync(Guid id)
        {
            await _clienteRepository.RemoverAsync(id);
        }
    }
}