using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadedComments.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddChildrenComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_comments_comments_ParentId",
                table: "comments",
                column: "ParentId",
                principalTable: "comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_comments_ParentId",
                table: "comments");
        }
    }
}
