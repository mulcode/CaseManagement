using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnOfServiceReasonAndSummonFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "ReturnOfServices",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOfServices_SummonId",
                table: "ReturnOfServices",
                column: "SummonId");

            // Clean up legacy rows before the foreign key is enforced.
            // These are records whose summons no longer exists, plus rows saved
            // before ReturnType was validated (0 = nothing selected).
            migrationBuilder.Sql(@"
DELETE FROM [ReturnOfServices]
WHERE [SummonId] NOT IN (SELECT [Id] FROM [Summons])
   OR [ReturnType] NOT IN (1, 2, 3, 4);
");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnOfServices_Summons_SummonId",
                table: "ReturnOfServices",
                column: "SummonId",
                principalTable: "Summons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnOfServices_Summons_SummonId",
                table: "ReturnOfServices");

            migrationBuilder.DropIndex(
                name: "IX_ReturnOfServices_SummonId",
                table: "ReturnOfServices");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "ReturnOfServices");
        }
    }
}
