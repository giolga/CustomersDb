using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customers.Migrations
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomersTb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersTb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProductsTb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProductsTb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProductsTb_CustomersTb_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CustomersTb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProductsTb_ProductTb_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductTb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductsTb_CustomerId",
                table: "CustomerProductsTb",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductsTb_ProductId",
                table: "CustomerProductsTb",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProductsTb");

            migrationBuilder.DropTable(
                name: "CustomersTb");

            migrationBuilder.DropTable(
                name: "ProductTb");
        }
    }
}
