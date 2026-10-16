using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddArraignmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Arraignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetitionId = table.Column<int>(type: "int", nullable: false),
                    ArraignmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArraignmentTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    WaivedArraignment = table.Column<bool>(type: "bit", nullable: false),
                    WasInformed = table.Column<bool>(type: "bit", nullable: false),
                    LegalCounsel = table.Column<bool>(type: "bit", nullable: false),
                    AttornyId = table.Column<int>(type: "int", nullable: true),
                    WaivedCounsel = table.Column<bool>(type: "bit", nullable: false),
                    FoundGuilty = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedCopyOfCharge = table.Column<bool>(type: "bit", nullable: false),
                    InquiredOfDefendant = table.Column<bool>(type: "bit", nullable: false),
                    EnteredPlea = table.Column<bool>(type: "bit", nullable: false),
                    BailStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExaminedCriminalProcess = table.Column<bool>(type: "bit", nullable: false),
                    AdditionalCharges = table.Column<bool>(type: "bit", nullable: false),
                    CourtDate = table.Column<bool>(type: "bit", nullable: false),
                    NextHearingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextHearingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JudgeId = table.Column<int>(type: "int", nullable: false),
                    JudgeSignature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SignTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    FinishAndLock = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arraignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arraignments_Users_AttornyId",
                        column: x => x.AttornyId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Arraignments_Users_JudgeId",
                        column: x => x.JudgeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Arraignments_AttornyId",
                table: "Arraignments",
                column: "AttornyId");

            migrationBuilder.CreateIndex(
                name: "IX_Arraignments_JudgeId",
                table: "Arraignments",
                column: "JudgeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Arraignments");
        }
    }
}
