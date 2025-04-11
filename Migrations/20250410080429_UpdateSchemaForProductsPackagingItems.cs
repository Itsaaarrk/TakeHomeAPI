using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TakeHomeAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaForProductsPackagingItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packagings_Packagings_ParentPackagingId",
                table: "Packagings");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Packagings_PackagingId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PackagingId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PackagingId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Packagings");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ProductID");

            migrationBuilder.RenameColumn(
                name: "ParentPackagingId",
                table: "Packagings",
                newName: "ParentPackagingID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Packagings",
                newName: "PackagingID");

            migrationBuilder.RenameIndex(
                name: "IX_Packagings_ParentPackagingId",
                table: "Packagings",
                newName: "IDX_Packaging_ParentPackagingID");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PackagingName",
                table: "Packagings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PackagingType",
                table: "Packagings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Packagings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackagingID = table.Column<int>(type: "int", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_Items_Packagings_PackagingID",
                        column: x => x.PackagingID,
                        principalTable: "Packagings",
                        principalColumn: "PackagingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_Packaging_ProductID",
                table: "Packagings",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IDX_Item_PackagingID",
                table: "Items",
                column: "PackagingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Packagings_Packagings_ParentPackagingID",
                table: "Packagings",
                column: "ParentPackagingID",
                principalTable: "Packagings",
                principalColumn: "PackagingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Packagings_Products_ProductID",
                table: "Packagings",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packagings_Packagings_ParentPackagingID",
                table: "Packagings");

            migrationBuilder.DropForeignKey(
                name: "FK_Packagings_Products_ProductID",
                table: "Packagings");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropIndex(
                name: "IDX_Packaging_ProductID",
                table: "Packagings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PackagingName",
                table: "Packagings");

            migrationBuilder.DropColumn(
                name: "PackagingType",
                table: "Packagings");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Packagings");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ParentPackagingID",
                table: "Packagings",
                newName: "ParentPackagingId");

            migrationBuilder.RenameColumn(
                name: "PackagingID",
                table: "Packagings",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IDX_Packaging_ParentPackagingID",
                table: "Packagings",
                newName: "IX_Packagings_ParentPackagingId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PackagingId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Packagings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PackagingId",
                table: "Products",
                column: "PackagingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packagings_Packagings_ParentPackagingId",
                table: "Packagings",
                column: "ParentPackagingId",
                principalTable: "Packagings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Packagings_PackagingId",
                table: "Products",
                column: "PackagingId",
                principalTable: "Packagings",
                principalColumn: "Id");
        }
    }
}
