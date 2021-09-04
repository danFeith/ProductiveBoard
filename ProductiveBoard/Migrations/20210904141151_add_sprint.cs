using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductiveBoard.Migrations
{
    public partial class add_sprint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sprints",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sprintTasks",
                columns: table => new
                {
                    taskId = table.Column<long>(nullable: false),
                    sprintId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprintTasks", x => new { x.sprintId, x.taskId });
                    table.ForeignKey(
                        name: "FK_sprintTasks_sprints_sprintId",
                        column: x => x.sprintId,
                        principalTable: "sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sprintTasks_Tasks_taskId",
                        column: x => x.taskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sprintTasks_taskId",
                table: "sprintTasks",
                column: "taskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sprintTasks");

            migrationBuilder.DropTable(
                name: "sprints");
        }
    }
}
