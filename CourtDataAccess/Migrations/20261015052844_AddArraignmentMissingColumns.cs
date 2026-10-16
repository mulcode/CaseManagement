using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddArraignmentMissingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "BailStatus",
                table: "Arraignments",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttorneyId",
                table: "Arraignments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BailStatusDetails",
                table: "Arraignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChargeDetails",
                table: "Arraignments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttorneyId",
                table: "Arraignments");

            migrationBuilder.DropColumn(
                name: "BailStatusDetails",
                table: "Arraignments");

            migrationBuilder.DropColumn(
                name: "ChargeDetails",
                table: "Arraignments");

            migrationBuilder.AlterColumn<string>(
                name: "BailStatus",
                table: "Arraignments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
