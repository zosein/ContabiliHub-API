using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContabiliHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "CPF", "DataCadastro", "Email", "Endereco", "NomeCompleto", "Telefone" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), "11144477735", new DateTime(2025, 1, 10, 9, 15, 0, 0, DateTimeKind.Utc), "maria.silva@email.com", "Rua das Flores, 123 - Centro, São Paulo - SP", "Maria Silva Santos", "11987654321" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "52998224725", new DateTime(2025, 1, 15, 14, 20, 0, 0, DateTimeKind.Utc), "joao.oliveira@email.com", "Av. Paulista, 456 - Bela Vista, São Paulo - SP", "João Carlos Oliveira", "11976543210" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "12345678909", new DateTime(2025, 1, 20, 11, 45, 0, 0, DateTimeKind.Utc), "ana.costa@email.com", "Rua Augusta, 789 - Consolação, São Paulo - SP", "Ana Paula Costa", "11965432109" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "98765432100", new DateTime(2025, 1, 25, 16, 10, 0, 0, DateTimeKind.Utc), "pedro.lima@email.com", "Rua Oscar Freire, 321 - Jardins, São Paulo - SP", "Pedro Henrique Lima", "11954321098" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "CriadoEm", "Email", "NomeCompleto", "SenhaHash" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "admin@contabilihub.com", "Administrador ContabiliHub", "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 1, 15, 14, 30, 0, 0, DateTimeKind.Utc), "demo@contabilihub.com", "Demo ContabiliHub", "062TFbe+XdU7MaJzs7Orpd7+cAgIMFqhajBit2ZYp5E=" }
                });

            migrationBuilder.InsertData(
                table: "ServicosPrestados",
                columns: new[] { "Id", "ClienteId", "DataPrestacao", "Descricao", "Pago", "Valor" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2025, 2, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Declaração de Imposto de Renda 2024", true, 180.00m },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2025, 2, 15, 14, 30, 0, 0, DateTimeKind.Utc), "Consultoria Contábil - Abertura MEI", true, 120.00m },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2025, 2, 5, 9, 15, 0, 0, DateTimeKind.Utc), "Declaração de Imposto de Renda 2024", false, 200.00m },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2025, 2, 10, 15, 45, 0, 0, DateTimeKind.Utc), "Organização de Documentos Fiscais", true, 350.00m },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2025, 2, 20, 11, 20, 0, 0, DateTimeKind.Utc), "Consultoria Tributária - Planejamento", false, 500.00m },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2025, 2, 25, 16, 0, 0, 0, DateTimeKind.Utc), "Declaração de Imposto de Renda 2024", false, 220.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServicosPrestados",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "ServicosPrestados",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "ServicosPrestados",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "ServicosPrestados",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "ServicosPrestados",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "ServicosPrestados",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));
        }
    }
}
