using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddComplainantId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
       name: "ComplainantId",
       table: "Complaints",
       type: "int",
       nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ComplainantId",
                table: "Complaints",
                column: "ComplainantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Users_ComplainantId",
                table: "Complaints",
                column: "ComplainantId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
       name: "FK_Complaints_Users_ComplainantId",
       table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_ComplainantId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ComplainantId",
                table: "Complaints");
        }
    }
}
