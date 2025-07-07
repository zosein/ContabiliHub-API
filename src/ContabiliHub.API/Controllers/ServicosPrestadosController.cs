using ContabiliHub.Application.DTOs;
using ContabiliHub.Application.Interfaces;
using ContabiliHub.Application.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ContabiliHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protege todos os endpoints deste controller
    public class ServicosPrestadosController : ControllerBase
    {
        private readonly IServicoPrestadoService _servico;
        private readonly IValidator<ServicoPrestadoCreateDto> _validator;
        public ServicosPrestadosController(IServicoPrestadoService servico, IValidator<ServicoPrestadoCreateDto> validator)
        {
            _servico = servico;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicoPrestadoReadDto>>> GetAll()
        {
            var servicos = await _servico.ObterTodosAsync();
            var servicosDtos = servicos.Select(s => new ServicoPrestadoReadDto(s));
            return Ok(servicosDtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServicoPrestadoReadDto>> GetById(Guid id)
        {
            var servico = await _servico.ObterPorIdAsync(id);
            if (servico == null)
                return NotFound();

            var servicoDto = new ServicoPrestadoReadDto(servico);
            return Ok(servicoDto);
        }

        [HttpGet("cliente/{clienteId:guid}")]
        public async Task<ActionResult<IEnumerable<ServicoPrestadoReadDto>>> GetByClienteId(Guid clienteId)
        {
            var servicos = await _servico.ObterPorClienteIdAsync(clienteId);
            var servicosDtos = servicos.Select(s => new ServicoPrestadoReadDto(s));
            return Ok(servicosDtos);
        }

        [HttpGet("{id:guid}/recibo")]
        public async Task<ActionResult<ReciboDto>> EmitirRecibo(Guid id)
        {
            var servico = await _servico.ObterPorIdAsync(id);

            if (servico == null)
                return NotFound(new { message = "Serviço não encontrado." });

            var recibo = new ReciboDto(servico);
            return Ok(recibo);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ServicoPrestadoCreateDto dto)
        {

            //validação usando sistema nativo
            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Dados inválidos.",
                    errors = validationResult.Errors
                });
            }

            try
            {
                var servico = dto.ToEntity();
                await _servico.AdicionarAsync(servico);
                var readDto = new ServicoPrestadoReadDto(servico);
                return CreatedAtAction(nameof(GetById), new { id = servico.Id }, readDto);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ServicoPrestadoCreateDto dto)
        {
            if (id == Guid.Empty)
                return BadRequest("O ID do serviço não pode ser vazio.");

            //validação usando sistema nativo
            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Dados inválidos.",
                    errors = validationResult.Errors
                });
            }

            try
            {
                var servicoExistente = await _servico.ObterPorIdAsync(id);
                if (servicoExistente == null)
                    return NotFound(new { message = "Serviço não encontrado." });

                //Aplicar as alterações do DTO na entidade existente
                dto.ApplyTo(servicoExistente);
                await _servico.AtualizarAsync(servicoExistente);
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