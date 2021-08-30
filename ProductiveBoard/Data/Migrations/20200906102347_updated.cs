using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductiveBoard.Data.Migrations
{
    public partial class updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskStatus_StatusId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskStatus",
                table: "TaskStatus");

            migrationBuilder.RenameTable(
                name: "TaskStatus",
                newName: "TaskStatuses");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskStatuses",
                table: "TaskStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskStatuses_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "TaskStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskStatuses_StatusId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskStatuses",
                table: "TaskStatuses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "TaskStatuses",
                newName: "TaskStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskStatus",
                table: "TaskStatus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskStatus_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "TaskStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
