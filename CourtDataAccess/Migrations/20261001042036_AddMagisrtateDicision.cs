using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMagisrtateDicision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AbsentFromSchool",
                table: "Complaints",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Disobedient",
                table: "Complaints",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FinishAndLock",
                table: "Complaints",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FoundInUnlawfulPlace",
                table: "Complaints",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "MagistrateDecisionDate",
                table: "Complaints",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MagistrateDecisionTime",
                table: "Complaints",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PetitionDate",
                table: "Complaints",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PetitionTime",
                table: "Complaints",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "RanAway",
                table: "Complaints",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SupportingFacts",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbsentFromSchool",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Disobedient",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "FinishAndLock",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "FoundInUnlawfulPlace",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "MagistrateDecisionDate",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "MagistrateDecisionTime",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "PetitionDate",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "PetitionTime",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "RanAway",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "SupportingFacts",
                table: "Complaints");
        }
    }
}
