using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductiveBoard.Data.Migrations
{
    public partial class fixingusr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_CompanyUsers_UserId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyUsers",
                table: "CompanyUsers");

            migrationBuilder.RenameTable(
                name: "CompanyUsers",
                newName: "OurUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OurUsers",
                table: "OurUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_OurUsers_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "OurUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_OurUsers_UserId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OurUsers",
                table: "OurUsers");

            migrationBuilder.RenameTable(
                name: "OurUsers",
                newName: "CompanyUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyUsers",
                table: "CompanyUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_CompanyUsers_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "CompanyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
