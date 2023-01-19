using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
    public partial class ChangePendingInquiriesName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PendingInquires",
                table: "PendingInquires");

            migrationBuilder.RenameTable(
                name: "PendingInquires",
                newName: "PendingInquiries");

            migrationBuilder.RenameIndex(
                name: "IX_PendingInquires_BankInquireId",
                table: "PendingInquiries",
                newName: "IX_PendingInquiries_BankInquireId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PendingInquiries",
                table: "PendingInquiries",
                column: "BankInquireId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PendingInquiries",
                table: "PendingInquiries");

            migrationBuilder.RenameTable(
                name: "PendingInquiries",
                newName: "PendingInquires");

            migrationBuilder.RenameIndex(
                name: "IX_PendingInquiries_BankInquireId",
                table: "PendingInquires",
                newName: "IX_PendingInquires_BankInquireId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PendingInquires",
                table: "PendingInquires",
                column: "BankInquireId");
        }
    }
}
