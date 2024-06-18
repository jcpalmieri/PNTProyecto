using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PNTProyecto.Migrations
{
    /// <inheritdoc />
    public partial class RenameContraseñaToPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contraseña",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "Teléfono",
                table: "Usuarios",
                newName: "Telefono");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "Usuarios",
                newName: "Teléfono");

            migrationBuilder.AddColumn<string>(
                name: "Contraseña",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
