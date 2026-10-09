using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingComplaintFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BadgeNumber",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComplainantPhone",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComplainantSignature",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReadyForApproval",
                table: "Complaints",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BadgeNumber",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ComplainantPhone",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ComplainantSignature",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ReadyForApproval",
                table: "Complaints");
        }
    }
}
