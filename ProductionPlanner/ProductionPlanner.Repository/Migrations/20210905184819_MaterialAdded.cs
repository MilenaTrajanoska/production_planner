using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductionPlanner.Repository.Migrations
{
    public partial class MaterialAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartOfProductId = table.Column<long>(nullable: false),
                    MaterialName = table.Column<string>(nullable: true),
                    UnitsNeeded = table.Column<double>(nullable: false),
                    CostPerUnit = table.Column<double>(nullable: false),
                    CostPerProduct = table.Column<double>(nullable: false),
                    EndingInventory = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_Products_PartOfProductId",
                        column: x => x.PartOfProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_PartOfProductId",
                table: "Materials",
                column: "PartOfProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Materials");
        }
    }
}
