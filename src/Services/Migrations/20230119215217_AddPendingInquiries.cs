using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
    public partial class AddPendingInquiries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PendingInquiries",
                columns: table => new
                {
                    BankInquireId = table.Column<string>(type: "text", nullable: false),
                    InquireId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceBank = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingInquiries", x => x.BankInquireId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PendingInquiries_BankInquireId",
                table: "PendingInquiries",
                column: "BankInquireId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingInquiries");
        }
    }
}
