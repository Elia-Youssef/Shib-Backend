using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShibAPI.Migrations.User_ApplicationDb
{
    /// <inheritdoc />
    public partial class UserV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDefault",
                table: "USR_NFT",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDefault",
                table: "USR_NFT");
        }
    }
}
