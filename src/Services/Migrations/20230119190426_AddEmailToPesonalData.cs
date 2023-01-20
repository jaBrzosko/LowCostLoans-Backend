using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
    public partial class AddEmailToPesonalData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonalData_Email",
                table: "Users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "zstap.mario.z.rio@aleeas.com");

            migrationBuilder.AddColumn<string>(
                name: "PersonalData_Email",
                table: "Inquiries",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                defaultValue: "zstap.mario.z.rio@aleeas.com");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalData_Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PersonalData_Email",
                table: "Inquiries");
        }
    }
}
