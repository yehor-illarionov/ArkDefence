using Microsoft.EntityFrameworkCore.Migrations;

namespace ArkDefence.AspNetCore.Host.Migrations
{
    public partial class usercontrollers_index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ArkDefence_PersonSystemController_PersonId_SystemControllerId",
                table: "ArkDefence_PersonSystemController");

            migrationBuilder.CreateIndex(
                name: "IX_ArkDefence_PersonSystemController_PersonId_SystemControllerId",
                table: "ArkDefence_PersonSystemController",
                columns: new[] { "PersonId", "SystemControllerId" },
                unique: true,
                filter: "[SystemControllerId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ArkDefence_PersonSystemController_PersonId_SystemControllerId",
                table: "ArkDefence_PersonSystemController");

            migrationBuilder.CreateIndex(
                name: "IX_ArkDefence_PersonSystemController_PersonId_SystemControllerId",
                table: "ArkDefence_PersonSystemController",
                columns: new[] { "PersonId", "SystemControllerId" });
        }
    }
}
