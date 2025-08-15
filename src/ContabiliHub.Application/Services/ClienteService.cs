using ContabiliHub.Application.Interfaces;
using ContabiliHub.Domain.Entities;
using ContabiliHub.Domain.Repositories;
using ContabiliHub.Domain.ValueObjects;

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
            //normaliza e valida cpf via value object
            var normalizado = Cpf.Normalizar(cliente.CPF);
            if (!Cpf.isValido(normalizado))
                throw new ArgumentException("CPF inválido.");

            cliente.CPF = normalizado;

            if (await _clienteRepository.ExisteCpfAsync(cliente.CPF))
                throw new InvalidOperationException("Já existe um cliente com este CPF.");

            if (await _clienteRepository.ExisteEmailAsync(cliente.Email))
                throw new InvalidOperationException("Já existe um cliente com este email.");

            await _clienteRepository.AdicionarAsync(cliente);
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            //regra: não permite alteraçao de cpf, para permitir necessario validar novamente
            if (await _clienteRepository.ExisteEmailAsync(cliente.Email))
            {
                //verifica se email já está em uso por outro cliente
                var existente = await _clienteRepository.ObterPorIdAsync(cliente.Id);
                if (existente != null && existente.Email != cliente.Email)
                    throw new InvalidOperationException("Já existe um cliente com este email.");
            }

            await _clienteRepository.AtualizarAsync(cliente);
        }

        public async Task RemoverAsync(Guid id)
        {
            await _clienteRepository.RemoverAsync(id);
        }
    }
}