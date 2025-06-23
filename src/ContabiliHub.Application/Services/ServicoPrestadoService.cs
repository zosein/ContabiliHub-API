using ContabiliHub.Application.Interfaces;
using ContabiliHub.Domain.Entities;
using ContabiliHub.Domain.Repositories;

namespace ContabiliHub.Application.Services
{
    public class ServicoPrestadoService : IServicoPrestadoService
    {
        private readonly IServicoPrestadoRepository _repository;
        private readonly IClienteRepository _clienteRepository;

        public ServicoPrestadoService(IServicoPrestadoRepository repository, IClienteRepository clienteRepository)
        {
            _repository = repository;
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<ServicoPrestado>> ObterTodosAsync()
        {
            return await _repository.ObterTodosAsync();
        }

        public async Task<ServicoPrestado?> ObterPorIdAsync(Guid id)
        {
            return await _repository.ObterPorIdAsync(id);
        }

        public async Task<IEnumerable<ServicoPrestado>> ObterPorClienteIdAsync(Guid clienteId)
        {
            return await _repository.ObterPorClienteIdAsync(clienteId);
        }

        public async Task AdicionarAsync(ServicoPrestado servicoPrestado)
        {

            var cliente = await _clienteRepository.ObterPorIdAsync(servicoPrestado.ClienteId);
            if (cliente == null)
            {
                throw new InvalidOperationException("Cliente não encontrado.");
            }

            await _repository.AdicionarAsync(servicoPrestado);
        }

        public async Task AtualizarAsync(ServicoPrestado servicoPrestado)
        {
            await _repository.AtualizarAsync(servicoPrestado);
        }

        public async Task RemoverAsync(Guid id)
        {
            await _repository.RemoverAsync(id);
        }
    }
}