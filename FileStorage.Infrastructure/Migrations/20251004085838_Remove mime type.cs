using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileStorage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removemimetype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "FilesMetadata");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "FilesMetadata",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }
    }
}
