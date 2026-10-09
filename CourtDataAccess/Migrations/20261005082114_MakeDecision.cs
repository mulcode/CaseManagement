using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MakeDecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDecisionSaved",
                table: "Complaints",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDecisionSaved",
                table: "Complaints");
        }
    }
}
