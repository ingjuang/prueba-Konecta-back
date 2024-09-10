using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "prueba_konecta");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "prueba_konecta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Users",
                schema: "prueba_konecta",
                newName: "Users");
        }
    }
}
