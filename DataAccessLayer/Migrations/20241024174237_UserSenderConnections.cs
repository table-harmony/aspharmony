using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UserSenderConnections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserSenders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserSenders_SenderId",
                table: "UserSenders",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSenders_UserId",
                table: "UserSenders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSenders_AspNetUsers_UserId",
                table: "UserSenders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSenders_Senders_SenderId",
                table: "UserSenders",
                column: "SenderId",
                principalTable: "Senders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSenders_AspNetUsers_UserId",
                table: "UserSenders");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSenders_Senders_SenderId",
                table: "UserSenders");

            migrationBuilder.DropIndex(
                name: "IX_UserSenders_SenderId",
                table: "UserSenders");

            migrationBuilder.DropIndex(
                name: "IX_UserSenders_UserId",
                table: "UserSenders");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserSenders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
