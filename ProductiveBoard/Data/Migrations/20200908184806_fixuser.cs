using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductiveBoard.Data.Migrations
{
    public partial class fixuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_OurUsers_UserId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "OurUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "OurUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OurUsers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_OurUsers_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "OurUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
