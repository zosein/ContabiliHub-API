using ContabiliHub.Application.DTOs;
using ContabiliHub.Application.Interfaces;
using ContabiliHub.Domain.Entities;
using ContabiliHub.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ContabiliHub.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public UsuarioService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<Usuario> RegistrarAsync(UsuarioRegisterDto dto)
        {
            var existente = await _usuarioRepository.ObterPorEmailAsync(dto.Email);
            if (existente != null)
                throw new InvalidOperationException("E-mail já está em uso.");

            var senhaHash = HashSenha(dto.Senha);
            var usuario = dto.ToEntity(senhaHash);

            await _usuarioRepository.AdicionarAsync(usuario);
            return usuario;
        }

        public async Task<string> LoginAsync(UsuarioLoginDto dto)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);
            if (usuario == null || !VerificarSenha(dto.Senha, usuario.SenhaHash))
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

            return GerarTokenJwt(usuario);
        }

        // --- Utilitários internos ---

        private static string HashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private static bool VerificarSenha(string senha, string hashArmazenado)
        {
            var hashDigitado = HashSenha(senha);
            return hashArmazenado == hashDigitado;
        }

        private string GerarTokenJwt(Usuario usuario)
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("nome", usuario.NomeCompleto)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
