using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantStoreAPI.Migrations
{
    public partial class ModifyColumnInProductTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizeBig",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SizeMedium",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SizeSmall",
                table: "Products",
                newName: "Quantity");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Products",
                newName: "SizeSmall");

            migrationBuilder.AddColumn<int>(
                name: "SizeBig",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SizeMedium",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
