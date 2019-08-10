using Microsoft.EntityFrameworkCore.Migrations;

namespace ArkDefence.AspNetCore.Host.Migrations
{
    public partial class card_personid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArkDefence_Cards_ArkDefence_Users_PersonId1",
                table: "ArkDefence_Cards");

            migrationBuilder.DropIndex(
                name: "IX_ArkDefence_Cards_PersonId1",
                table: "ArkDefence_Cards");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "ArkDefence_Cards");

            migrationBuilder.AlterColumn<long>(
                name: "PersonId",
                table: "ArkDefence_Cards",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArkDefence_Cards_PersonId",
                table: "ArkDefence_Cards",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArkDefence_Cards_ArkDefence_Users_PersonId",
                table: "ArkDefence_Cards",
                column: "PersonId",
                principalTable: "ArkDefence_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArkDefence_Cards_ArkDefence_Users_PersonId",
                table: "ArkDefence_Cards");

            migrationBuilder.DropIndex(
                name: "IX_ArkDefence_Cards_PersonId",
                table: "ArkDefence_Cards");

            migrationBuilder.AlterColumn<string>(
                name: "PersonId",
                table: "ArkDefence_Cards",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "PersonId1",
                table: "ArkDefence_Cards",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArkDefence_Cards_PersonId1",
                table: "ArkDefence_Cards",
                column: "PersonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ArkDefence_Cards_ArkDefence_Users_PersonId1",
                table: "ArkDefence_Cards",
                column: "PersonId1",
                principalTable: "ArkDefence_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
