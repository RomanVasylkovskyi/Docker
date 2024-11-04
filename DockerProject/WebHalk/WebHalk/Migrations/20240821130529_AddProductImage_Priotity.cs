using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebHalk.Migrations
{
    /// <inheritdoc />
    public partial class AddProductImage_Priotity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priotity",
                table: "ProductImages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priotity",
                table: "ProductImages");
        }
    }
}
