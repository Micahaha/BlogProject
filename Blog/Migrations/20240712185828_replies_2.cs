using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogProject.Migrations
{
    public partial class replies_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Reply",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Reply",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reply_ApplicationUserId",
                table: "Reply",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reply_ApplicationUserId1",
                table: "Reply",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reply_AspNetUsers_ApplicationUserId",
                table: "Reply",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reply_AspNetUsers_ApplicationUserId1",
                table: "Reply",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reply_AspNetUsers_ApplicationUserId",
                table: "Reply");

            migrationBuilder.DropForeignKey(
                name: "FK_Reply_AspNetUsers_ApplicationUserId1",
                table: "Reply");

            migrationBuilder.DropIndex(
                name: "IX_Reply_ApplicationUserId",
                table: "Reply");

            migrationBuilder.DropIndex(
                name: "IX_Reply_ApplicationUserId1",
                table: "Reply");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Reply");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Reply");
        }
    }
}
