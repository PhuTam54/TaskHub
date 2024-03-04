using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskHub.Migrations
{
    /// <inheritdoc />
    public partial class EmailSend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailSend",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSend",
                table: "User");
        }
    }
}
