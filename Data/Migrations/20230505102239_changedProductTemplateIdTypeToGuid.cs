using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class changedProductTemplateIdTypeToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductsTemplate_ProductTemplateId",
                schema: "CstUserMngt",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductTemplateId",
                schema: "CstUserMngt",
                table: "Products");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductTemplateId",
                schema: "CstUserMngt",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductTemplateId",
                schema: "CstUserMngt",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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
                principalColumn: "Id");
        }
    }
}
