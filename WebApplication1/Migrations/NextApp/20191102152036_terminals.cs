using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WebApplication1.Migrations.NextApp
{
    public partial class terminals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RowId = table.Column<string>(maxLength: 50, nullable: false),
                    TableName = table.Column<string>(maxLength: 128, nullable: false),
                    Changed = table.Column<string>(nullable: true),
                    Kind = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HardwareVersionIndices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: false),
                    Alias = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwareVersionIndices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: false),
                    Alias = table.Column<string>(nullable: true),
                    TenantId = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hardwares",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: false),
                    Alias = table.Column<string>(nullable: true),
                    VendorId = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    VersionIndexId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hardwares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hardwares_HardwareVersionIndices_VersionIndexId",
                        column: x => x.VersionIndexId,
                        principalTable: "HardwareVersionIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TerminalConfigTemplates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: false),
                    Alias = table.Column<string>(nullable: true),
                    FingerTimeout = table.Column<int>(nullable: false),
                    BleDistance = table.Column<int>(nullable: false),
                    IsBleEnabled = table.Column<bool>(nullable: false),
                    IsCardEnabled = table.Column<bool>(nullable: false),
                    IsFingerEnabled = table.Column<bool>(nullable: false),
                    HasBle = table.Column<bool>(nullable: false),
                    HasCard = table.Column<bool>(nullable: false),
                    HasFinger = table.Column<bool>(nullable: false),
                    Mode = table.Column<int>(nullable: false),
                    HardwareIndexId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminalConfigTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerminalConfigTemplates_HardwareVersionIndices_HardwareInde~",
                        column: x => x.HardwareIndexId,
                        principalTable: "HardwareVersionIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Controllers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: false),
                    Alias = table.Column<string>(nullable: true),
                    ClientIdHash = table.Column<string>(nullable: true),
                    ClientSecretHash = table.Column<string>(nullable: true),
                    Mac = table.Column<string>(nullable: true),
                    SiteId = table.Column<long>(nullable: true),
                    VersionIndexId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controllers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Controllers_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Controllers_HardwareVersionIndices_VersionIndexId",
                        column: x => x.VersionIndexId,
                        principalTable: "HardwareVersionIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Terminals",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: false),
                    Alias = table.Column<string>(nullable: true),
                    Uuid = table.Column<string>(nullable: true),
                    Port = table.Column<string>(nullable: true),
                    GpioPort = table.Column<int>(nullable: false),
                    ControllerId = table.Column<long>(nullable: true),
                    VersionIndexId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terminals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Terminals_Controllers_ControllerId",
                        column: x => x.ControllerId,
                        principalTable: "Controllers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Terminals_HardwareVersionIndices_VersionIndexId",
                        column: x => x.VersionIndexId,
                        principalTable: "HardwareVersionIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TerminalConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: false),
                    TerminalId = table.Column<long>(nullable: false),
                    ConfigId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminalConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerminalConfigs_TerminalConfigTemplates_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "TerminalConfigTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TerminalConfigs_Terminals_TerminalId",
                        column: x => x.TerminalId,
                        principalTable: "Terminals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Controllers_SiteId",
                table: "Controllers",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Controllers_VersionIndexId",
                table: "Controllers",
                column: "VersionIndexId");

            migrationBuilder.CreateIndex(
                name: "IX_Hardwares_VersionIndexId",
                table: "Hardwares",
                column: "VersionIndexId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminalConfigs_ConfigId",
                table: "TerminalConfigs",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminalConfigs_TerminalId_ConfigId",
                table: "TerminalConfigs",
                columns: new[] { "TerminalId", "ConfigId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TerminalConfigTemplates_HardwareIndexId",
                table: "TerminalConfigTemplates",
                column: "HardwareIndexId");

            migrationBuilder.CreateIndex(
                name: "IX_Terminals_ControllerId",
                table: "Terminals",
                column: "ControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_Terminals_VersionIndexId",
                table: "Terminals",
                column: "VersionIndexId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoHistory");

            migrationBuilder.DropTable(
                name: "Hardwares");

            migrationBuilder.DropTable(
                name: "TerminalConfigs");

            migrationBuilder.DropTable(
                name: "TerminalConfigTemplates");

            migrationBuilder.DropTable(
                name: "Terminals");

            migrationBuilder.DropTable(
                name: "Controllers");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "HardwareVersionIndices");
        }
    }
}
