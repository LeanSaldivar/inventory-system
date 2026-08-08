using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Data.migrations
{
    /// <inheritdoc />
    public partial class OAuth2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarLarge",
                table: "Users",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarNormal",
                table: "Users",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarSmall",
                table: "Users",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarLarge",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarNormal",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarSmall",
                table: "Users");
        }
    }
}
