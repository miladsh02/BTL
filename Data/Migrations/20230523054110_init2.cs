using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductsTemplate_ProductTemplateModelId",
                schema: "CstUserMngt",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductTemplateModelId",
                schema: "CstUserMngt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductTemplateModelId",
                schema: "CstUserMngt",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductTemplateModelId",
                schema: "CstUserMngt",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTemplateModelId",
                schema: "CstUserMngt",
                table: "Products",
                column: "ProductTemplateModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductsTemplate_ProductTemplateModelId",
                schema: "CstUserMngt",
                table: "Products",
                column: "ProductTemplateModelId",
                principalSchema: "CstUserMngt",
                principalTable: "ProductsTemplate",
                principalColumn: "Id");
        }
    }
}
