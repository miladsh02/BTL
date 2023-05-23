using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class init23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTemplateId",
                schema: "CstUserMngt",
                table: "Products",
                column: "ProductTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductsTemplate_ProductTemplateId",
                schema: "CstUserMngt",
                table: "Products",
                column: "ProductTemplateId",
                principalSchema: "CstUserMngt",
                principalTable: "ProductsTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductsTemplate_ProductTemplateId",
                schema: "CstUserMngt",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductTemplateId",
                schema: "CstUserMngt",
                table: "Products");
        }
    }
}
