using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addTransactionRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CartId",
                schema: "CstUserMngt",
                table: "Transaction",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_StudentId",
                schema: "CstUserMngt",
                table: "Transaction",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Carts_CartId",
                schema: "CstUserMngt",
                table: "Transaction",
                column: "CartId",
                principalSchema: "CstUserMngt",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Students_StudentId",
                schema: "CstUserMngt",
                table: "Transaction",
                column: "StudentId",
                principalSchema: "CstUserMngt",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Carts_CartId",
                schema: "CstUserMngt",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Students_StudentId",
                schema: "CstUserMngt",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_CartId",
                schema: "CstUserMngt",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_StudentId",
                schema: "CstUserMngt",
                table: "Transaction");
        }
    }
}
