using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ezpeleta2023.Migrations.Ezpeleta2023Db
{
    public partial class CuartaMigracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    ServicioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Eliminado = table.Column<bool>(type: "bit", nullable: false),
                    SubcategoriaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.ServicioID);
                    table.ForeignKey(
                        name: "FK_Servicios_Subcategorias_SubcategoriaID",
                        column: x => x.SubcategoriaID,
                        principalTable: "Subcategorias",
                        principalColumn: "SubcategoriaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_SubcategoriaID",
                table: "Servicios",
                column: "SubcategoriaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Servicios");
        }
    }
}
