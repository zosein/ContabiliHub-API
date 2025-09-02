using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContabiliHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueEmailToClientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Email",
                table: "Clientes",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clientes_Email",
                table: "Clientes");
        }
    }
}
