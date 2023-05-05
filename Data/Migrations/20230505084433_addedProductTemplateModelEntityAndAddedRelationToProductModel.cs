using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addedProductTemplateModelEntityAndAddedRelationToProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "CstUserMngt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "CstUserMngt",
                table: "Products");

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                schema: "CstUserMngt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductTemplateId",
                schema: "CstUserMngt",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductsTemplate",
                schema: "CstUserMngt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsTemplate", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductsTemplate_ProductTemplateId",
                schema: "CstUserMngt",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductsTemplate",
                schema: "CstUserMngt");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductTemplateId",
                schema: "CstUserMngt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                schema: "CstUserMngt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductTemplateId",
                schema: "CstUserMngt",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "CstUserMngt",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "CstUserMngt",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
