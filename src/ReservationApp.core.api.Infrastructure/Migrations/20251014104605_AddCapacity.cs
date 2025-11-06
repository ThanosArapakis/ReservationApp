using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationApp.core.api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCapacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Restaurants");
        }
    }
}
