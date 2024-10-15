using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveServers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Servers_ServerId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_ServerId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "ServerId",
                table: "Books",
                newName: "Server");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Server",
                table: "Books",
                newName: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ServerId",
                table: "Books",
                column: "ServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Servers_ServerId",
                table: "Books",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
