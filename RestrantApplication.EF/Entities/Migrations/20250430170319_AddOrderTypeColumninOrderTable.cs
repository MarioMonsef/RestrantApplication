using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestrantApplication.EF.Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTypeColumninOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "orderType",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "orderType",
                table: "Order");
        }
    }
}
