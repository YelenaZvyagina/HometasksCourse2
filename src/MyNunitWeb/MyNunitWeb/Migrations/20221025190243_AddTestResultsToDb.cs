using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyNunitWeb.Migrations
{
    public partial class AddTestResultsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestAssmblies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAssmblies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IgnoreReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutionTime = table.Column<long>(type: "bigint", nullable: false),
                    AssemblyModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResults_TestAssmblies_AssemblyModelId",
                        column: x => x.AssemblyModelId,
                        principalTable: "TestAssmblies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_AssemblyModelId",
                table: "TestResults",
                column: "AssemblyModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "TestAssmblies");
        }
    }
}
