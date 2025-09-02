using ContabiliHub.Domain.ValueObjects;
using FluentAssertions;
using System;
using Xunit;

namespace ContabiliHub.Tests.ValueObjectsTests;

public class CpfTests
{
    #region Testes de Validação

    [Theory]
    [InlineData("11144477735", true)]   // CPF válido
    [InlineData("52998224725", true)]   // CPF válido
    [InlineData("12345678909", true)]   // CPF válido (substitui o problemático)
    [InlineData("11111111111", false)]  // Todos os dígitos iguais
    [InlineData("22222222222", false)]  // Todos os dígitos iguais
    [InlineData("12345678901", false)]  // CPF inválido
    [InlineData("00000000000", false)]  // Zeros
    [InlineData("99999999999", false)]  // Noves
    [InlineData("123456789", false)]    // Muito curto
    [InlineData("123456789012", false)] // Muito longo
    [InlineData("", false)]             // Vazio
    [InlineData("abcdefghijk", false)]  // Letras
    public void Deve_Validar_Cpf_Corretamente(string cpf, bool esperado)
    {
        var resultado = Cpf.isValido(cpf);
        resultado.Should().Be(esperado);
    }

    #endregion

    #region Testes de Normalização

    [Theory]
    [InlineData("111.444.777-35", "11144477735")]
    [InlineData("111 444 777 35", "11144477735")]
    [InlineData("111-444-777-35", "11144477735")]
    [InlineData("111.444.777.35", "11144477735")]
    [InlineData("11144477735", "11144477735")]
    [InlineData("", "")]
    [InlineData("abc123def456ghi789jkl", "123456789")]
    public void Deve_Normalizar_Cpf_Removendo_Formatacao(string cpfFormatado, string esperado)
    {
        var resultado = Cpf.Normalizar(cpfFormatado);
        resultado.Should().Be(esperado);
    }

    #endregion

    #region Testes de Criação

    [Fact]
    public void Deve_Criar_Cpf_Valido()
    {
        var cpfValido = "11144477735";

        var cpf = Cpf.Create(cpfValido);

        cpf.Valor.Should().Be(cpfValido);
        ((string)cpf).Should().Be(cpfValido);
        cpf.ToString().Should().Be(cpfValido);
    }

    [Fact]
    public void Deve_Criar_Cpf_Com_Formatacao()
    {
        var cpfFormatado = "111.444.777-35";
        var cpfLimpo = "11144477735";

        var cpf = Cpf.Create(cpfFormatado);

        cpf.Valor.Should().Be(cpfLimpo);
    }

    [Theory]
    [InlineData("11111111111")]
    [InlineData("12345678901")]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("abcdefghijk")]
    public void Deve_Falhar_Ao_Criar_Cpf_Invalido(string cpfInvalido)
    {
        Action acao = () => Cpf.Create(cpfInvalido);
        acao.Should().Throw<ArgumentException>()
            .WithMessage("CPF inválido.*");
    }

    #endregion

    #region Testes de Casos Específicos

    [Fact]
    public void Deve_Validar_Digitos_Verificadores_Corretamente()
    {
        var cpfValido = "11144477735";
        Cpf.isValido(cpfValido).Should().BeTrue();
    }

    [Fact]
    public void Deve_Rejeitar_Cpf_Com_Digitos_Verificadores_Incorretos()
    {
        var cpfInvalido = "11144477734"; // último dígito errado
        Cpf.isValido(cpfInvalido).Should().BeFalse();
    }

    [Fact]
    public void Deve_Tratar_Cpf_Null_Como_Invalido()
    {
        Cpf.isValido(null!).Should().BeFalse();
    }

    #endregion

    #region Testes de Operadores Implícitos

    [Fact]
    public void Deve_Converter_Implicitamente_Para_String()
    {
        var cpf = Cpf.Create("11144477735");
        string cpfString = cpf;
        cpfString.Should().Be("11144477735");
    }

    #endregion
}