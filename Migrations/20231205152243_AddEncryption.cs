using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenThumb.Migrations
{
    public partial class AddEncryption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Gardens_user_id",
                table: "Gardens");

            migrationBuilder.CreateIndex(
                name: "IX_Gardens_user_id",
                table: "Gardens",
                column: "user_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Gardens_user_id",
                table: "Gardens");

            migrationBuilder.CreateIndex(
                name: "IX_Gardens_user_id",
                table: "Gardens",
                column: "user_id");
        }
    }
}
