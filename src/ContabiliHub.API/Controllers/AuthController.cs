using ContabiliHub.Application.DTOs;
using ContabiliHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContabiliHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Resgistra um novo usuário no sistema.
        /// </summary>
        /// <param name="usuarioRegisterDto">Dados para registro do usuário</param>
        /// <returns>Dados do usuário registrado</returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UsuarioRegisterDto dto)
        {
            try
            {
                var usuario = await _usuarioService.RegistrarAsync(dto);

                var resposta = new
                {
                    Id = usuario.Id,
                    NomeCompleto = usuario.NomeCompleto,
                    Email = usuario.Email,
                    CriadoEm = usuario.CriadoEm,
                    Message = "Usuário registrado com sucesso."
                };

                return CreatedAtAction(nameof(Register), new { id = usuario.Id }, resposta);
            }

            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }

            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro interno do servidor.", details = ex.Message });
            }
        }

        /// <summary>
        /// Realiza login do usuário e retorna um token JWT.
        /// </summary>
        /// <param name="usuarioLoginDto">Credenciais de login</param>
        ///  <returns>Token JWT para autenticação</returns>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioLoginDto dto)
        {
            try
            {
                var token = await _usuarioService.LoginAsync(dto);

                var resposta = new
                {
                    Toke = token,
                    TokenType = "Bearer",
                    ExpiresIn = "6h",
                    Message = "Login realizado com sucesso."
                };

                return Ok(resposta);
            }

            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }

            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro interno do servidor.", details = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para validar se o token JWT ainda é válido.
        /// </summary>
        /// <returns>Status de validação do token</returns>
        [HttpGet("validate")]
        public ActionResult ValidateToken()
        {
            // Se chegou ate aqui, o token é válido (middleware já validou)
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();

            if (authHeader == null || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Token não fornecido." });
            }

            return Ok(new
            {
                valid = true,
                message = "Token válido.",
                timestamp = DateTime.UtcNow
            });
        }
    }
}