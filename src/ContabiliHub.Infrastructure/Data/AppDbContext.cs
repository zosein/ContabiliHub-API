using ContabiliHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContabiliHub.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ServicoPrestado> ServicosPrestados { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
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
    }
}