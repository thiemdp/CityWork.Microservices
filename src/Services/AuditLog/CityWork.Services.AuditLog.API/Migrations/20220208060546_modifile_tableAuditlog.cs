using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityWork.Services.AuditLog.API.Migrations
{
    public partial class modifile_tableAuditlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionTime",
                table: "AuditLogs",
                newName: "EventTime");

            migrationBuilder.AddColumn<string>(
                name: "DataJson_Old",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EventTime",
                table: "AuditLogs",
                column: "EventTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_EventTime",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "DataJson_Old",
                table: "AuditLogs");

            migrationBuilder.RenameColumn(
                name: "EventTime",
                table: "AuditLogs",
                newName: "ActionTime");
        }
    }
}
