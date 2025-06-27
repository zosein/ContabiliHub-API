using AutoMapper;
using ContabiliHub.Application.DTOs;
using ContabiliHub.Application.Interfaces;
using ContabiliHub.Domain.Entities;
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
        private readonly IMapper _mapper;

        public ServicosPrestadosController(IServicoPrestadoService servico, IMapper mapper)
        {
            _servico = servico;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicoPrestadoReadDto>>> GetAll()
        {
            var servicos = await _servico.ObterTodosAsync();
            var servicosDtos = _mapper.Map<IEnumerable<ServicoPrestadoReadDto>>(servicos);
            return Ok(servicosDtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServicoPrestadoReadDto>> GetById(Guid id)
        {
            var servico = await _servico.ObterPorIdAsync(id);
            if (servico == null)
                return NotFound();

            var servicoDto = _mapper.Map<ServicoPrestadoReadDto>(servico);
            return Ok(servicoDto);
        }

        [HttpGet("cliente/{clienteId:guid}")]
        public async Task<ActionResult<IEnumerable<ServicoPrestadoReadDto>>> GetByClienteId(Guid clienteId)
        {
            var servicos = await _servico.ObterPorClienteIdAsync(clienteId);
            var servicosDtos = _mapper.Map<IEnumerable<ServicoPrestadoReadDto>>(servicos);
            return Ok(servicosDtos);
        }

        [HttpGet("{id:guid}/recibo")]
        public async Task<ActionResult<ReciboDto>> EmitirRecibo(Guid id)
        {
            var servico = await _servico.ObterPorIdAsync(id);

            if (servico == null)
                return NotFound(new { message = "Serviço não encontrado." });

            var recibo = _mapper.Map<ReciboDto>(servico);
            return Ok(recibo);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ServicoPrestadoCreateDto dto)
        {
            try
            {
                var servico = _mapper.Map<ServicoPrestado>(dto);
                await _servico.AdicionarAsync(servico);
                var readDto = _mapper.Map<ServicoPrestadoReadDto>(servico);
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
                return BadRequest("O ID do serviço não corresponde ao ID fornecido na URL.");

            var servico = _mapper.Map<ServicoPrestado>(dto);
            servico.Id = id;

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