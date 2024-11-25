using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLoans_LibraryMemberships_LibraryMembershipId",
                table: "BookLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_AuthorId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryBooks_Books_BookId",
                table: "LibraryBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryMemberships_AspNetUsers_UserId",
                table: "LibraryMemberships");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoans_LibraryMemberships_LibraryMembershipId",
                table: "BookLoans",
                column: "LibraryMembershipId",
                principalTable: "LibraryMemberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryBooks_Books_BookId",
                table: "LibraryBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryMemberships_AspNetUsers_UserId",
                table: "LibraryMemberships",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLoans_LibraryMemberships_LibraryMembershipId",
                table: "BookLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_AuthorId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryBooks_Books_BookId",
                table: "LibraryBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryMemberships_AspNetUsers_UserId",
                table: "LibraryMemberships");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoans_LibraryMemberships_LibraryMembershipId",
                table: "BookLoans",
                column: "LibraryMembershipId",
                principalTable: "LibraryMemberships",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryBooks_Books_BookId",
                table: "LibraryBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryMemberships_AspNetUsers_UserId",
                table: "LibraryMemberships",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
