using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantStoreAPI.Migrations
{
    public partial class addColumnVoucherID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VoucherID",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoucherID",
                table: "Orders");
        }
    }
}
