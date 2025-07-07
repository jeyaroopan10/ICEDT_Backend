using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICEDT.API.Migrations
{
    /// <inheritdoc />
    public partial class newentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainActivityTypeId",
                table: "ActivityTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MainActivityTypeId",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MainActivityType",
                columns: table => new
                {
                    MainActivityTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainActivityType", x => x.MainActivityTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTypes_MainActivityTypeId",
                table: "ActivityTypes",
                column: "MainActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_MainActivityTypeId",
                table: "Activities",
                column: "MainActivityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_MainActivityType_MainActivityTypeId",
                table: "Activities",
                column: "MainActivityTypeId",
                principalTable: "MainActivityType",
                principalColumn: "MainActivityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTypes_MainActivityType_MainActivityTypeId",
                table: "ActivityTypes",
                column: "MainActivityTypeId",
                principalTable: "MainActivityType",
                principalColumn: "MainActivityTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_MainActivityType_MainActivityTypeId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTypes_MainActivityType_MainActivityTypeId",
                table: "ActivityTypes");

            migrationBuilder.DropTable(
                name: "MainActivityType");

            migrationBuilder.DropIndex(
                name: "IX_ActivityTypes_MainActivityTypeId",
                table: "ActivityTypes");

            migrationBuilder.DropIndex(
                name: "IX_Activities_MainActivityTypeId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "MainActivityTypeId",
                table: "ActivityTypes");

            migrationBuilder.DropColumn(
                name: "MainActivityTypeId",
                table: "Activities");
        }
    }
}
