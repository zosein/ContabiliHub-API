using ContabiliHub.Application.DTOs;
using ContabiliHub.Application.Interfaces;
using ContabiliHub.Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContabiliHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IValidator<UsuarioRegisterDto> _registerValidator;
        private readonly IValidator<UsuarioLoginDto> _loginValidator;

        public AuthController(IUsuarioService usuarioService, IValidator<UsuarioRegisterDto> registerValidator, IValidator<UsuarioLoginDto> loginValidator)
        {
            _usuarioService = usuarioService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        /// <summary>
        /// Resgistra um novo usuário no sistema.
        /// </summary>
        /// <param name="usuarioRegisterDto">Dados para registro do usuário</param>
        /// <returns>Dados do usuário registrado</returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UsuarioRegisterDto dto)
        {
            //Validação usando sistema nativo
            var validationResult = _registerValidator.Validate(dto);
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
                var usuario = await _usuarioService.RegistrarAsync(dto);

                return CreatedAtAction(
                     nameof(Register),
                     new { id = usuario.Id },
                     new { message = "Usuário registrado com sucesso.", usuario.Email }
                );
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
            // Validação usando sistema nativo
            var validationResult = _loginValidator.Validate(dto);
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
                var token = await _usuarioService.LoginAsync(dto);

                return Ok(new { token, message = "Login realizado com sucesso." });
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
        [Authorize]
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