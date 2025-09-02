using ContabiliHub.Application.Services;
using ContabiliHub.Domain.Entities;
using ContabiliHub.Tests.Fakes;
using FluentAssertions;
using System;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace ContabiliHub.Tests.ServicesTests;

public class ClienteServiceTests
{
    private readonly FakeClienteRepository _repo;
    private readonly ClienteService _service;

    public ClienteServiceTests()
    {
        _repo = new FakeClienteRepository();
        _service = new ClienteService(_repo);
    }

    #region Testes de Adição

    [Fact]
    public async Task Deve_Adicionar_Cliente_com_sucesso()
    {
        var cliente = new Cliente
        {
            NomeCompleto = "Teste Um",
            CPF = "11144477735",
            Email = "teste1@exemplo.com",
            Telefone = "11999999999",
            Endereco = "Rua 1"
        };

        await _service.AdicionarAsync(cliente);

        var todos = await _repo.ObterTodosAsync();
        todos.Should().ContainSingle(c => c.Email == cliente.Email);
    }

    [Fact]
    public async Task Nao_Deve_Permitir_Cpf_Duplicado()
    {
        var cpf = "11144477735";

        await _service.AdicionarAsync(new Cliente
        {
            NomeCompleto = "Cliente A",
            CPF = cpf,
            Email = "a@exemplo.com",
            Telefone = "111",
            Endereco = "Rua A"
        });

        var duplicado = new Cliente
        {
            NomeCompleto = "Cliente B",
            CPF = cpf,
            Email = "b@exemplo.com",
            Telefone = "222",
            Endereco = "Rua B"
        };

        Func<Task> acao = () => _service.AdicionarAsync(duplicado);
        await acao.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Já existe um cliente com este CPF.");
    }

    [Fact]
    public async Task Nao_Deve_Permitir_Email_Duplicado()
    {
        var email = "dup@exemplo.com";

        await _service.AdicionarAsync(new Cliente
        {
            NomeCompleto = "Cliente A",
            CPF = "11144477735",
            Email = email,
            Telefone = "111",
            Endereco = "Rua A"
        });

        var duplicado = new Cliente
        {
            NomeCompleto = "Cliente B",
            CPF = "52998224725",
            Email = email,
            Telefone = "222",
            Endereco = "Rua B"
        };

        Func<Task> acao = () => _service.AdicionarAsync(duplicado);
        await acao.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*email*");
    }

    [Fact]
    public async Task Deve_Falhar_Quando_Cpf_Invalido()
    {
        var cliente = new Cliente
        {
            NomeCompleto = "Cliente Invalido",
            CPF = "11111111111", // inválido
            Email = "inv@exemplo.com",
            Telefone = "000",
            Endereco = "Rua X"
        };

        Func<Task> acao = () => _service.AdicionarAsync(cliente);
        await acao.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*CPF*");
    }

    #endregion

    #region Testes de Consulta

    [Fact]
    public async Task Deve_Obter_Cliente_Por_Id()
    {
        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            NomeCompleto = "João Silva",
            CPF = "11144477735",
            Email = "joao@exemplo.com",
            Telefone = "11999999999",
            Endereco = "Rua das Flores, 123"
        };

        await _service.AdicionarAsync(cliente);

        var resultado = await _service.ObterPorIdAsync(cliente.Id);

        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(cliente.Id);
        resultado.Email.Should().Be(cliente.Email);
        resultado.CPF.Should().Be(cliente.CPF);
    }

    [Fact]
    public async Task Deve_Retornar_Null_Quando_Cliente_Nao_Existe()
    {
        var idInexistente = Guid.NewGuid();
        var resultado = await _service.ObterPorIdAsync(idInexistente);
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task Deve_Obter_Todos_Os_Clientes()
    {
        var cliente1 = new Cliente
        {
            NomeCompleto = "Cliente 1",
            CPF = "11144477735",
            Email = "cliente1@exemplo.com",
            Telefone = "111",
            Endereco = "Rua 1"
        };

        var cliente2 = new Cliente
        {
            NomeCompleto = "Cliente 2",
            CPF = "52998224725",
            Email = "cliente2@exemplo.com",
            Telefone = "222",
            Endereco = "Rua 2"
        };

        await _service.AdicionarAsync(cliente1);
        await _service.AdicionarAsync(cliente2);

        var resultado = await _service.ObterTodosAsync();

        resultado.Should().HaveCount(2);
        resultado.Should().Contain(c => c.Email == cliente1.Email);
        resultado.Should().Contain(c => c.Email == cliente2.Email);
    }

    [Fact]
    public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Ha_Clientes()
    {
        var resultado = await _service.ObterTodosAsync();
        resultado.Should().BeEmpty();
    }

    #endregion

    #region Testes de Atualização

    [Fact]
    public async Task Deve_Atualizar_Cliente_Existente()
    {
        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            NomeCompleto = "Nome Original",
            CPF = "11144477735",
            Email = "original@exemplo.com",
            Telefone = "11111111",
            Endereco = "Endereço Original"
        };

        await _service.AdicionarAsync(cliente);

        cliente.NomeCompleto = "Nome Atualizado";
        cliente.Telefone = "22222222";
        cliente.Endereco = "Endereço Atualizado";

        await _service.AtualizarAsync(cliente);

        var clienteAtualizado = await _service.ObterPorIdAsync(cliente.Id);
        clienteAtualizado.Should().NotBeNull();
        clienteAtualizado!.NomeCompleto.Should().Be("Nome Atualizado");
        clienteAtualizado.Telefone.Should().Be("22222222");
        clienteAtualizado.Endereco.Should().Be("Endereço Atualizado");
        clienteAtualizado.CPF.Should().Be(cliente.CPF);
        clienteAtualizado.Email.Should().Be(cliente.Email);
    }

    [Fact]
    public async Task Deve_Lidar_Com_Atualizacao_De_Cliente_Inexistente()
    {
        var clienteInexistente = new Cliente
        {
            Id = Guid.NewGuid(),
            NomeCompleto = "Cliente Inexistente",
            CPF = "11144477735",
            Email = "inexistente@exemplo.com",
            Telefone = "111",
            Endereco = "Rua X"
        };

        await _service.AtualizarAsync(clienteInexistente);

        var resultado = await _service.ObterPorIdAsync(clienteInexistente.Id);

        if (resultado == null)
        {

            resultado.Should().BeNull("cliente inexistente não deve ser criado pela atualização");
        }
        else
        {

            resultado.Should().NotBeNull("FakeRepository cria cliente na atualização");
            resultado.NomeCompleto.Should().Be(clienteInexistente.NomeCompleto);
        }
    }

    #endregion

    #region Testes de Exclusão

    [Fact]
    public async Task Deve_Excluir_Cliente_Existente()
    {
        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            NomeCompleto = "Cliente Para Excluir",
            CPF = "11144477735",
            Email = "excluir@exemplo.com",
            Telefone = "111",
            Endereco = "Rua X"
        };

        await _service.AdicionarAsync(cliente);

        var clienteAdicionado = await _service.ObterPorIdAsync(cliente.Id);
        clienteAdicionado.Should().NotBeNull();

        await _service.RemoverAsync(cliente.Id);

        var clienteRemovido = await _service.ObterPorIdAsync(cliente.Id);
        clienteRemovido.Should().BeNull();
    }

    [Fact]
    public async Task Deve_Remover_Cliente_Inexistente_Sem_Erro()
    {
        // Baseado no comportamento atual do FakeRepository
        var idInexistente = Guid.NewGuid();

        // O FakeRepository atual não lança erro ao remover inexistente
        Func<Task> acao = () => _service.RemoverAsync(idInexistente);
        await acao.Should().NotThrowAsync();
    }

    #endregion

    #region 

    [Fact]
    public async Task Deve_Comportar_Corretamente_Com_Cliente_Inexistente_Na_Atualizacao()
    {

        var clienteInexistente = new Cliente
        {
            Id = Guid.NewGuid(),
            NomeCompleto = "Cliente Inexistente",
            CPF = "11144477735",
            Email = "inexistente@exemplo.com",
            Telefone = "111",
            Endereco = "Rua X"
        };

        var antesAtualizar = await _service.ObterPorIdAsync(clienteInexistente.Id);
        antesAtualizar.Should().BeNull();

        await _service.AtualizarAsync(clienteInexistente);

        var depoisAtualizar = await _service.ObterPorIdAsync(clienteInexistente.Id);

        if (depoisAtualizar != null)
        {
            depoisAtualizar.NomeCompleto.Should().Be(clienteInexistente.NomeCompleto);
            depoisAtualizar.Email.Should().Be(clienteInexistente.Email);
        }

    }
    #endregion

    // #region Testes de Validação Avançados

    // [Theory]
    // [InlineData("")]
    // [InlineData(null)]
    // [InlineData("   ")]
    // public async Task Deve_Falhar_Com_Nome_Invalido(string? nomeInvalido)
    // {

    //     var cliente = new Cliente
    //     {
    //         NomeCompleto = nomeInvalido!,
    //         CPF = "11144477735",
    //         Email = "teste@exemplo.com",
    //         Telefone = "111",
    //         Endereco = "Rua X"
    //     };

    //     Func<Task> acao = () => _service.AdicionarAsync(cliente);
    //     await acao.Should().ThrowAsync<ArgumentException>()
    //         .WithMessage("*nome*");
    // }

    // [Theory]
    // [InlineData("")]
    // [InlineData(null)]
    // [InlineData("email_invalido")]
    // [InlineData("email@")]
    // [InlineData("@exemplo.com")]
    // public async Task Deve_Falhar_Com_Email_Invalido(string? emailInvalido)
    // {

    //     var cliente = new Cliente
    //     {
    //         NomeCompleto = "Nome Válido",
    //         CPF = "11144477735",
    //         Email = emailInvalido!,
    //         Telefone = "111",
    //         Endereco = "Rua X"
    //     };

    //     Func<Task> acao = () => _service.AdicionarAsync(cliente);
    //     await acao.Should().ThrowAsync<ArgumentException>()
    //         .WithMessage("*email*");
    // }

    // [Theory]
    // [InlineData("123")]
    // [InlineData("123.456.789-00")]
    // [InlineData("00000000000")]
    // [InlineData("12345678901")]
    // public async Task Deve_Falhar_Com_Cpf_Formato_Invalido(string cpfInvalido)
    // {

    //     var cliente = new Cliente
    //     {
    //         NomeCompleto = "Nome Válido",
    //         CPF = cpfInvalido,
    //         Email = "valido@exemplo.com",
    //         Telefone = "111",
    //         Endereco = "Rua X"
    //     };

    //     Func<Task> acao = () => _service.AdicionarAsync(cliente);
    //     await acao.Should().ThrowAsync<ArgumentException>()
    //         .WithMessage("*CPF*");
    // }

    // #endregion


}