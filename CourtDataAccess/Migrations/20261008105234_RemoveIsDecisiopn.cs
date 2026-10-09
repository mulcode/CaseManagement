using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsDecisiopn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDecisionSaved",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "Complaints");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDecisionSaved",
                table: "Complaints",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "Complaints",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
