using ContabiliHub.Application.DTOs;
using ContabiliHub.Application.Services;
using ContabiliHub.Domain.Entities;
using ContabiliHub.Tests.Fakes;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System;
using Xunit;

namespace ContabiliHub.Tests;

public class UsuarioServiceTests
{
    private readonly FakeUsuarioRepository _repo;
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _repo = new FakeUsuarioRepository();

        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Jwt:Key", "chave-hiper-mega-ultra-secreta-ContabiliHub-2025" },
            { "Jwt:Issuer", "ContabiliHub.API" },
            { "Jwt:ExpiryHours", "2" }
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _service = new UsuarioService(_repo, config);
    }

    [Fact]
    public async Task Deve_Registrar_Usuario()
    {
        var dto = new UsuarioRegisterDto("Usuário teste", "user@exemplo.com", "senha321");
        var usuario = await _service.RegistrarAsync(dto);

        usuario.Id.Should().NotBe(Guid.Empty);
        usuario.Email.Should().Be("user@exemplo.com");
    }

    [Fact]
    public async Task Deve_Gerar_Token_Com_Claims()
    {
        var dto = new UsuarioRegisterDto("Usuário teste", "claims@exemplo.com", "senha321");
        var usuario = await _service.RegistrarAsync(dto);

        var token = await _service.LoginAsync(new UsuarioLoginDto(usuario.Email, "senha321"));
        token.Should().NotBeNullOrWhiteSpace();

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == usuario.Email);
        jwt.Claims.Should().Contain(c => c.Type == "nome" && c.Value == usuario.NomeCompleto);

        //verifica a expiracao de ~ 2 horas de tolerancia
        var expiraEm = jwt.ValidTo;
        (expiraEm - DateTime.UtcNow).TotalHours.Should().BeGreaterThan(1.5);
    }
}