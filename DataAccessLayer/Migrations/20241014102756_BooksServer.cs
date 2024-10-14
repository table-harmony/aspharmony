using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class BooksServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServerId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Servers_ServerId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_ServerId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ServerId",
                table: "Books");
        }
    }
}
