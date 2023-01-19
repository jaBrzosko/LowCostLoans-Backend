using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
    public partial class FixPendingInquiries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PendingInquires",
                table: "PendingInquires");

            migrationBuilder.DropIndex(
                name: "IX_PendingInquires_InquireId",
                table: "PendingInquires");

            migrationBuilder.AlterColumn<Guid>(
                name: "InquireId",
                table: "PendingInquires",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "BankInquireId",
                table: "PendingInquires",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PendingInquires",
                table: "PendingInquires",
                column: "BankInquireId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingInquires_BankInquireId",
                table: "PendingInquires",
                column: "BankInquireId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PendingInquires",
                table: "PendingInquires");

            migrationBuilder.DropIndex(
                name: "IX_PendingInquires_BankInquireId",
                table: "PendingInquires");

            migrationBuilder.DropColumn(
                name: "BankInquireId",
                table: "PendingInquires");

            migrationBuilder.AlterColumn<string>(
                name: "InquireId",
                table: "PendingInquires",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PendingInquires",
                table: "PendingInquires",
                column: "InquireId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingInquires_InquireId",
                table: "PendingInquires",
                column: "InquireId");
        }
    }
}
