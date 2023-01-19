using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
    public partial class AddPendingInquiries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PendingInquires",
                columns: table => new
                {
                    InquireId = table.Column<string>(type: "text", nullable: false),
                    SourceBank = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingInquires", x => x.InquireId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PendingInquires_InquireId",
                table: "PendingInquires",
                column: "InquireId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingInquires");
        }
    }
}
