using ContabiliHub.Application.Interfaces;
using ContabiliHub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ContabiliHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicosPrestadosController : ControllerBase
    {
        private readonly IServicoPrestadoService _servico;

        public ServicosPrestadosController(IServicoPrestadoService servico)
        {
            _servico = servico;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicoPrestado>>> GetAll()
        {
            var servicos = await _servico.ObterTodosAsync();
            return Ok(servicos);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServicoPrestado>> GetById(Guid id)
        {
            var servico = await _servico.ObterPorIdAsync(id);
            if (servico == null)
                return NotFound();

            return Ok(servico);
        }

        [HttpGet("cliente/{clienteId:guid}")]
        public async Task<ActionResult<IEnumerable<ServicoPrestado>>> GetByClienteId(Guid clienteId)
        {
            var servicos = await _servico.ObterPorClienteIdAsync(clienteId);
            return Ok(servicos);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ServicoPrestado servico)
        {
            try
            {
                await _servico.AdicionarAsync(servico);
                return CreatedAtAction(nameof(GetById), new { id = servico.Id }, servico);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ServicoPrestado servico)
        {
            if (id != servico.Id)
                return BadRequest("O ID do serviço não corresponde ao ID fornecido na URL.");

            try
            {
                await _servico.AtualizarAsync(servico);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _servico.RemoverAsync(id);
            return NoContent();
        }
    }
}