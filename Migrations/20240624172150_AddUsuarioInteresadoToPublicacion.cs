using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PNTProyecto.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioInteresadoToPublicacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioInteresado",
                table: "Publicaciones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioInteresado",
                table: "Publicaciones");
        }
    }
}
