using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpD.Migrations
{
    /// <inheritdoc />
    public partial class Keys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnvironmentVariables_Containers_ContainerId",
                table: "EnvironmentVariables");

            migrationBuilder.DropForeignKey(
                name: "FK_Mounts_Containers_ContainerId",
                table: "Mounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Ports_Containers_ContainerId",
                table: "Ports");

            migrationBuilder.DropIndex(
                name: "IX_Ports_ContainerId",
                table: "Ports");

            migrationBuilder.DropIndex(
                name: "IX_Mounts_ContainerId",
                table: "Mounts");

            migrationBuilder.DropIndex(
                name: "IX_EnvironmentVariables_ContainerId",
                table: "EnvironmentVariables");

            migrationBuilder.DropColumn(
                name: "ContainerId",
                table: "Ports");

            migrationBuilder.DropColumn(
                name: "ContainerId",
                table: "Mounts");

            migrationBuilder.DropColumn(
                name: "ContainerId",
                table: "EnvironmentVariables");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Ports",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Mounts",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "EnvironmentVariables",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ports_OwnerId",
                table: "Ports",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Mounts_OwnerId",
                table: "Mounts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentVariables_OwnerId",
                table: "EnvironmentVariables",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Containers_Name",
                table: "Containers",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EnvironmentVariables_Containers_OwnerId",
                table: "EnvironmentVariables",
                column: "OwnerId",
                principalTable: "Containers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mounts_Containers_OwnerId",
                table: "Mounts",
                column: "OwnerId",
                principalTable: "Containers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ports_Containers_OwnerId",
                table: "Ports",
                column: "OwnerId",
                principalTable: "Containers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnvironmentVariables_Containers_OwnerId",
                table: "EnvironmentVariables");

            migrationBuilder.DropForeignKey(
                name: "FK_Mounts_Containers_OwnerId",
                table: "Mounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Ports_Containers_OwnerId",
                table: "Ports");

            migrationBuilder.DropIndex(
                name: "IX_Ports_OwnerId",
                table: "Ports");

            migrationBuilder.DropIndex(
                name: "IX_Mounts_OwnerId",
                table: "Mounts");

            migrationBuilder.DropIndex(
                name: "IX_EnvironmentVariables_OwnerId",
                table: "EnvironmentVariables");

            migrationBuilder.DropIndex(
                name: "IX_Containers_Name",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Ports");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Mounts");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "EnvironmentVariables");

            migrationBuilder.AddColumn<Guid>(
                name: "ContainerId",
                table: "Ports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ContainerId",
                table: "Mounts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ContainerId",
                table: "EnvironmentVariables",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ports_ContainerId",
                table: "Ports",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Mounts_ContainerId",
                table: "Mounts",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentVariables_ContainerId",
                table: "EnvironmentVariables",
                column: "ContainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnvironmentVariables_Containers_ContainerId",
                table: "EnvironmentVariables",
                column: "ContainerId",
                principalTable: "Containers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mounts_Containers_ContainerId",
                table: "Mounts",
                column: "ContainerId",
                principalTable: "Containers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ports_Containers_ContainerId",
                table: "Ports",
                column: "ContainerId",
                principalTable: "Containers",
                principalColumn: "Id");
        }
    }
}
