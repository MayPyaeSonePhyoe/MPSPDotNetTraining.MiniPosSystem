using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPSPDotNetTraining.MiniPosSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixSaleItemColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subtotal",
                table: "SaleItems",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "Qty",
                table: "SaleItems",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "SaleItems",
                newName: "UnitPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "SaleItems",
                newName: "Subtotal");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "SaleItems",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "SaleItems",
                newName: "Qty");
        }
    }
}
