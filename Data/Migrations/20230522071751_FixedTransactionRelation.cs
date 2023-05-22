using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedTransactionRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Carts_CartId",
                schema: "CstUserMngt",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "CartId",
                schema: "CstUserMngt",
                table: "Transaction",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_CartId",
                schema: "CstUserMngt",
                table: "Transaction",
                newName: "IX_Transaction_OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Order_OrderId",
                schema: "CstUserMngt",
                table: "Transaction",
                column: "OrderId",
                principalSchema: "CstUserMngt",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Order_OrderId",
                schema: "CstUserMngt",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                schema: "CstUserMngt",
                table: "Transaction",
                newName: "CartId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_OrderId",
                schema: "CstUserMngt",
                table: "Transaction",
                newName: "IX_Transaction_CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Carts_CartId",
                schema: "CstUserMngt",
                table: "Transaction",
                column: "CartId",
                principalSchema: "CstUserMngt",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
