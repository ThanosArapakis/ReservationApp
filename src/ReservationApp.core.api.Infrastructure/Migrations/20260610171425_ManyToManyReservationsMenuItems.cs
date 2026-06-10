using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationApp.core.api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyReservationsMenuItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Reservations_ReservationId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_ReservationId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "MenuItems");

            migrationBuilder.CreateTable(
                name: "ReservationMenuItems",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationMenuItems", x => new { x.ReservationId, x.MenuItemId });
                    table.ForeignKey(
                        name: "FK_ReservationMenuItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationMenuItems_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationMenuItems_MenuItemId",
                table: "ReservationMenuItems",
                column: "MenuItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationMenuItems");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "MenuItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ReservationId",
                table: "MenuItems",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Reservations_ReservationId",
                table: "MenuItems",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
