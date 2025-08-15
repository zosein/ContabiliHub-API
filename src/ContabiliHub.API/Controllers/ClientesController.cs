using ContabiliHub.Application.Interfaces;
using ContabiliHub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ContabiliHub.Application.DTOs;

namespace ContabiliHub.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protege todos os endpoints deste controller
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteReadDto>>> GetAll()
        {
            var clientes = await _clienteService.ObterTodosAsync();
            var resultado = clientes.Select(ClienteReadDto.FromEntity);
            return Ok(resultado);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClienteReadDto>> GetById(Guid id)
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound(new { message = "Cliente não encontrado." });

            return Ok(ClienteReadDto.FromEntity(cliente));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ClienteCreateDto clienteDto)
        {
            var cliente = new Cliente
            {
                NomeCompleto = clienteDto.NomeCompleto,
                CPF = clienteDto.Cpf,
                Email = clienteDto.Email,
                Telefone = clienteDto.Telefone,
                Endereco = clienteDto.Endereco
            };

            await _clienteService.AdicionarAsync(cliente);

            var lerDto = ClienteReadDto.FromEntity(cliente);
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, lerDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ClienteUpdateDto clienteDto)
        {
            var existente = await _clienteService.ObterPorIdAsync(id);
            if (existente == null)
                return NotFound(new { message = "Cliente não encontrado." });

            existente.NomeCompleto = clienteDto.NomeCompleto;
            existente.Email = clienteDto.Email;
            existente.Telefone = clienteDto.Telefone;
            existente.Endereco = clienteDto.Endereco;

            await _clienteService.AtualizarAsync(existente);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {

            var existente = await _clienteService.ObterPorIdAsync(id);
            if (existente == null)
                return NotFound(new { message = "Cliente não encontrado." });

            await _clienteService.RemoverAsync(id);
            return NoContent();
        }
    }
}