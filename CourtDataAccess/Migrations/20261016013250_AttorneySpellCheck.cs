using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AttorneySpellCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arraignments_Users_AttornyId",
                table: "Arraignments");

            migrationBuilder.DropIndex(
                name: "IX_Arraignments_AttornyId",
                table: "Arraignments");

            migrationBuilder.DropColumn(
                name: "AttornyId",
                table: "Arraignments");

            migrationBuilder.CreateIndex(
                name: "IX_Arraignments_AttorneyId",
                table: "Arraignments",
                column: "AttorneyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arraignments_Users_AttorneyId",
                table: "Arraignments",
                column: "AttorneyId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arraignments_Users_AttorneyId",
                table: "Arraignments");

            migrationBuilder.DropIndex(
                name: "IX_Arraignments_AttorneyId",
                table: "Arraignments");

            migrationBuilder.AddColumn<int>(
                name: "AttornyId",
                table: "Arraignments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Arraignments_AttornyId",
                table: "Arraignments",
                column: "AttornyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arraignments_Users_AttornyId",
                table: "Arraignments",
                column: "AttornyId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
