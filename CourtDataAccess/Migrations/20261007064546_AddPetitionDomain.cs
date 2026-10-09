using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPetitionDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Users_ComplainantId",
                table: "Complaints");

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
                name: "IsPetitionSaved",
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReceivedDateByMagistrate",
                table: "Complaints",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "MagistrateDecisionTime",
                table: "Complaints",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "MagistrateDecisionDate",
                table: "Complaints",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MagistrateDecision",
                table: "Complaints",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ComplainantId",
                table: "Complaints",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Petitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplaintId = table.Column<int>(type: "int", nullable: false),
                    PetitionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PetitionTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    MagistrateDecision = table.Column<int>(type: "int", nullable: false),
                    MagistrateDecisionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MagistrateDecisionTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    MagistrateId = table.Column<int>(type: "int", nullable: false),
                    MagistrateSignature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disobedient = table.Column<bool>(type: "bit", nullable: false),
                    FoundInUnlawfulPlace = table.Column<bool>(type: "bit", nullable: false),
                    RanAway = table.Column<bool>(type: "bit", nullable: false),
                    AbsentFromSchool = table.Column<bool>(type: "bit", nullable: false),
                    FinishAndLock = table.Column<bool>(type: "bit", nullable: false),
                    SupportingFacts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPetitionSaved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Petitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Petitions_Complaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalTable: "Complaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Petitions_Users_MagistrateId",
                        column: x => x.MagistrateId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_ComplaintId",
                table: "Petitions",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_MagistrateId",
                table: "Petitions",
                column: "MagistrateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Users_ComplainantId",
                table: "Complaints",
                column: "ComplainantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Users_ComplainantId",
                table: "Complaints");

            migrationBuilder.DropTable(
                name: "Petitions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReceivedDateByMagistrate",
                table: "Complaints",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "MagistrateDecisionTime",
                table: "Complaints",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MagistrateDecisionDate",
                table: "Complaints",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "MagistrateDecision",
                table: "Complaints",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ComplainantId",
                table: "Complaints",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddColumn<bool>(
                name: "IsPetitionSaved",
                table: "Complaints",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PetitionDate",
                table: "Complaints",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PetitionTime",
                table: "Complaints",
                type: "time",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Users_ComplainantId",
                table: "Complaints",
                column: "ComplainantId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
