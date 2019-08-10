using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArkDefence.AspNetCore.Host.Migrations
{
    public partial class usercontrollers_long_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArkDefence_PersonSystemController_ArkDefence_SystemController_SystemControllerId",
                table: "ArkDefence_PersonSystemController");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArkDefence_PersonSystemController",
                table: "ArkDefence_PersonSystemController");

            migrationBuilder.AlterColumn<string>(
                name: "SystemControllerId",
                table: "ArkDefence_PersonSystemController",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "ArkDefence_PersonSystemController",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArkDefence_PersonSystemController",
                table: "ArkDefence_PersonSystemController",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ArkDefence_PersonSystemController_PersonId_SystemControllerId",
                table: "ArkDefence_PersonSystemController",
                columns: new[] { "PersonId", "SystemControllerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ArkDefence_PersonSystemController_ArkDefence_SystemController_SystemControllerId",
                table: "ArkDefence_PersonSystemController",
                column: "SystemControllerId",
                principalTable: "ArkDefence_SystemController",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArkDefence_PersonSystemController_ArkDefence_SystemController_SystemControllerId",
                table: "ArkDefence_PersonSystemController");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArkDefence_PersonSystemController",
                table: "ArkDefence_PersonSystemController");

            migrationBuilder.DropIndex(
                name: "IX_ArkDefence_PersonSystemController_PersonId_SystemControllerId",
                table: "ArkDefence_PersonSystemController");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ArkDefence_PersonSystemController");

            migrationBuilder.AlterColumn<string>(
                name: "SystemControllerId",
                table: "ArkDefence_PersonSystemController",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArkDefence_PersonSystemController",
                table: "ArkDefence_PersonSystemController",
                columns: new[] { "PersonId", "SystemControllerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ArkDefence_PersonSystemController_ArkDefence_SystemController_SystemControllerId",
                table: "ArkDefence_PersonSystemController",
                column: "SystemControllerId",
                principalTable: "ArkDefence_SystemController",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
