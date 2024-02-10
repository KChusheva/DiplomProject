using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KristaRecords.Infrastructure.Migrations
{
    public partial class Added_Reservation_Hours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "FromHour",
                table: "Reservations",
                type: "Time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ToHour",
                table: "Reservations",
                type: "Time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromHour",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ToHour",
                table: "Reservations");
        }
    }
}
