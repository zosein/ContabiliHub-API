using ContabiliHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;


namespace ContabiliHub.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<ServicoPrestado> ServicosPrestados { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações das entidades
        ConfigurarCliente(modelBuilder);
        ConfigurarServicoPrestado(modelBuilder);
        ConfigurarUsuario(modelBuilder);

        // Data Seeding
        SeedUsuarios(modelBuilder);
        SeedClientes(modelBuilder);
        SeedServicosPrestados(modelBuilder);
    }

    private static void ConfigurarCliente(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.NomeCompleto)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(c => c.CPF)
                .IsRequired()
                .HasMaxLength(11);
            entity.HasIndex(c => c.CPF)
                .IsUnique();
            entity.Property(c => c.Email)
                .HasMaxLength(100);
            entity.HasIndex(c => c.Email)
                .IsUnique();
            entity.Property(c => c.Telefone)
                .HasMaxLength(15);
            entity.Property(c => c.Endereco)
                .HasMaxLength(250);
        });
    }

    private static void ConfigurarServicoPrestado(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServicoPrestado>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Descricao)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(s => s.Valor)
                .HasColumnType("decimal(10,2)")
                .IsRequired();
            entity.Property(s => s.DataPrestacao)
                .IsRequired();

            entity.HasOne(s => s.Cliente)
                .WithMany()
                .HasForeignKey(s => s.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigurarUsuario(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.NomeCompleto)
                .IsRequired();
            entity.Property(u => u.Email)
                .IsRequired();
            entity.Property(u => u.SenhaHash)
                .IsRequired();
            entity.Property(u => u.CriadoEm)
                .IsRequired();
        });
    }

    #region Data Seeding

    private static void SeedUsuarios(ModelBuilder modelBuilder)
    {
        var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var demoId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = adminId,
                NomeCompleto = "Administrador ContabiliHub",
                Email = "admin@contabilihub.com",
                SenhaHash = HashSenha("admin123"),
                CriadoEm = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new Usuario
            {
                Id = demoId,
                NomeCompleto = "Demo ContabiliHub",
                Email = "demo@contabilihub.com",
                SenhaHash = HashSenha("demo123"),
                CriadoEm = new DateTime(2025, 1, 15, 14, 30, 0, DateTimeKind.Utc)
            }
        );
    }

    private static void SeedClientes(ModelBuilder modelBuilder)
    {
        var cliente1Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var cliente2Id = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var cliente3Id = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var cliente4Id = Guid.Parse("66666666-6666-6666-6666-666666666666");

        modelBuilder.Entity<Cliente>().HasData(
            new Cliente
            {
                Id = cliente1Id,
                NomeCompleto = "Maria Silva Santos",
                CPF = "11144477735", // CPF válido
                Email = "maria.silva@email.com",
                Telefone = "11987654321",
                Endereco = "Rua das Flores, 123 - Centro, São Paulo - SP",
                DataCadastro = new DateTime(2025, 1, 10, 9, 15, 0, DateTimeKind.Utc)
            },
            new Cliente
            {
                Id = cliente2Id,
                NomeCompleto = "João Carlos Oliveira",
                CPF = "52998224725", // CPF válido
                Email = "joao.oliveira@email.com",
                Telefone = "11976543210",
                Endereco = "Av. Paulista, 456 - Bela Vista, São Paulo - SP",
                DataCadastro = new DateTime(2025, 1, 15, 14, 20, 0, DateTimeKind.Utc)
            },
            new Cliente
            {
                Id = cliente3Id,
                NomeCompleto = "Ana Paula Costa",
                CPF = "12345678909", // CPF válido
                Email = "ana.costa@email.com",
                Telefone = "11965432109",
                Endereco = "Rua Augusta, 789 - Consolação, São Paulo - SP",
                DataCadastro = new DateTime(2025, 1, 20, 11, 45, 0, DateTimeKind.Utc)
            },
            new Cliente
            {
                Id = cliente4Id,
                NomeCompleto = "Pedro Henrique Lima",
                CPF = "98765432100", // CPF válido  
                Email = "pedro.lima@email.com",
                Telefone = "11954321098",
                Endereco = "Rua Oscar Freire, 321 - Jardins, São Paulo - SP",
                DataCadastro = new DateTime(2025, 1, 25, 16, 10, 0, DateTimeKind.Utc)
            }
        );
    }

    private static void SeedServicosPrestados(ModelBuilder modelBuilder)
    {
        var servico1Id = Guid.Parse("77777777-7777-7777-7777-777777777777");
        var servico2Id = Guid.Parse("88888888-8888-8888-8888-888888888888");
        var servico3Id = Guid.Parse("99999999-9999-9999-9999-999999999999");
        var servico4Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var servico5Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var servico6Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

        // IDs dos clientes (mesmos usados no SeedClientes)
        var cliente1Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var cliente2Id = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var cliente3Id = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var cliente4Id = Guid.Parse("66666666-6666-6666-6666-666666666666");

        modelBuilder.Entity<ServicoPrestado>().HasData(
            new ServicoPrestado
            {
                Id = servico1Id,
                ClienteId = cliente1Id,
                Descricao = "Declaração de Imposto de Renda 2024",
                Valor = 180.00m,
                DataPrestacao = new DateTime(2025, 2, 1, 10, 0, 0, DateTimeKind.Utc),
                Pago = true
            },
            new ServicoPrestado
            {
                Id = servico2Id,
                ClienteId = cliente1Id,
                Descricao = "Consultoria Contábil - Abertura MEI",
                Valor = 120.00m,
                DataPrestacao = new DateTime(2025, 2, 15, 14, 30, 0, DateTimeKind.Utc),
                Pago = true
            },
            new ServicoPrestado
            {
                Id = servico3Id,
                ClienteId = cliente2Id,
                Descricao = "Declaração de Imposto de Renda 2024",
                Valor = 200.00m,
                DataPrestacao = new DateTime(2025, 2, 5, 9, 15, 0, DateTimeKind.Utc),
                Pago = false
            },
            new ServicoPrestado
            {
                Id = servico4Id,
                ClienteId = cliente3Id,
                Descricao = "Organização de Documentos Fiscais",
                Valor = 350.00m,
                DataPrestacao = new DateTime(2025, 2, 10, 15, 45, 0, DateTimeKind.Utc),
                Pago = true
            },
            new ServicoPrestado
            {
                Id = servico5Id,
                ClienteId = cliente3Id,
                Descricao = "Consultoria Tributária - Planejamento",
                Valor = 500.00m,
                DataPrestacao = new DateTime(2025, 2, 20, 11, 20, 0, DateTimeKind.Utc),
                Pago = false
            },
            new ServicoPrestado
            {
                Id = servico6Id,
                ClienteId = cliente4Id,
                Descricao = "Declaração de Imposto de Renda 2024",
                Valor = 220.00m,
                DataPrestacao = new DateTime(2025, 2, 25, 16, 0, 0, DateTimeKind.Utc),
                Pago = false
            }
        );
    }

    /// <summary>
    /// Hash SHA256 da senha (mesmo método usado no UsuarioService)
    /// </summary>
    private static string HashSenha(string senha)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(senha);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    #endregion
}
