using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ezpeleta2023.Migrations.Ezpeleta2023Db
{
    public partial class TerceraMigracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subcategorias",
                columns: table => new
                {
                    SubcategoriaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriaID = table.Column<int>(type: "int", nullable: false),
                    Eliminada = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategorias", x => x.SubcategoriaID);
                    table.ForeignKey(
                        name: "FK_Subcategorias_Categorias_CategoriaID",
                        column: x => x.CategoriaID,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subcategorias_CategoriaID",
                table: "Subcategorias",
                column: "CategoriaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subcategorias");
        }
    }
}
