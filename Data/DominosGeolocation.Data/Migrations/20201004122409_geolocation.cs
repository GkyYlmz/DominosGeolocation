using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DominosGeolocation.Data.Migrations
{
    public partial class geolocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DestinationSource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkOrderId = table.Column<int>(nullable: false),
                    Source_latitude = table.Column<string>(maxLength: 50, nullable: true),
                    Source_longitude = table.Column<string>(maxLength: 50, nullable: true),
                    Destination_latitude = table.Column<string>(maxLength: 50, nullable: true),
                    Destination_longitude = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestinationSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MqStartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    MqEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DbStartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DbEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsMqSuccess = table.Column<bool>(nullable: false),
                    IsDbSuccess = table.Column<bool>(nullable: false),
                    FilePath = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrder", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DestinationSource");

            migrationBuilder.DropTable(
                name: "WorkOrder");
        }
    }
}
