using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFileContentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileContentType",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileContentType",
                table: "Documents");
        }
    }
}
