using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMagistrateId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MagistrateDecision",
                table: "Complaints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MagistrateId",
                table: "Complaints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MagistrateSignature",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedDateByMagistrate",
                table: "Complaints",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_MagistrateId",
                table: "Complaints",
                column: "MagistrateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Users_MagistrateId",
                table: "Complaints",
                column: "MagistrateId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Users_MagistrateId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_MagistrateId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "MagistrateDecision",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "MagistrateId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "MagistrateSignature",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ReceivedDateByMagistrate",
                table: "Complaints");
        }
    }
}
