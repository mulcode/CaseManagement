using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Summoncreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Summons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetitionId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsAliasAndPluriesSummons = table.Column<bool>(type: "bit", nullable: false),
                    IsOrder = table.Column<bool>(type: "bit", nullable: false),
                    IsFirstAppearance = table.Column<bool>(type: "bit", nullable: false),
                    IsAdjudicatoryHearing = table.Column<bool>(type: "bit", nullable: false),
                    MustAttendScheduledHearings = table.Column<bool>(type: "bit", nullable: false),
                    CourtJurisdiction = table.Column<bool>(type: "bit", nullable: false),
                    HasCourtAppointedLawyer = table.Column<bool>(type: "bit", nullable: false),
                    AttorneyId = table.Column<int>(type: "int", nullable: true),
                    WillBeAdvisedOfAppointedLawyer = table.Column<bool>(type: "bit", nullable: false),
                    JudicialOfficialId = table.Column<int>(type: "int", nullable: false),
                    JudicialName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JudicialESignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Summons_Users_AttorneyId",
                        column: x => x.AttorneyId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Summons_Users_JudicialOfficialId",
                        column: x => x.JudicialOfficialId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Summons_AttorneyId",
                table: "Summons",
                column: "AttorneyId");

            migrationBuilder.CreateIndex(
                name: "IX_Summons_JudicialOfficialId",
                table: "Summons",
                column: "JudicialOfficialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Summons");
        }
    }
}
