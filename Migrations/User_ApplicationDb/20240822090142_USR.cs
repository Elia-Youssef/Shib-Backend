using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShibAPI.Migrations.User_ApplicationDb
{
    /// <inheritdoc />
    public partial class USR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
              name: "USR_Users",
              columns: table => new
              {
                  Id = table.Column<int>(type: "integer", nullable: false)
                      .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  UserCode = table.Column<string>(type: "text", nullable: true),
                  UserName = table.Column<string>(type: "text", nullable: true),
                  Email = table.Column<string>(type: "text", nullable: true),
                  NftId = table.Column<int>(type: "integer", nullable: true),
                  CountryId = table.Column<int>(type: "integer", nullable: true),
                  CreatedOn = table.Column<DateTime>(type: "timestamptz", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_USR_Users", x => x.Id);
              });


            migrationBuilder.CreateTable(
                name: "USR_Machine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MachineId = table.Column<string>(type: "text", nullable: true),
                    ClientKey = table.Column<string>(type: "text", nullable: true),
                    TokenID = table.Column<Guid>(type: "uuid", nullable: true),
                    isNewUser = table.Column<bool>(type: "boolean", nullable: true),
                    isLogged = table.Column<bool>(type: "boolean", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USR_Machine", x => x.Id);
                    table.ForeignKey(
                            name: "FK_USR_Machine_USR_Users_USRUsersId",
                            column: x => x.UserId,
                            principalTable: "USR_Users",
                            principalColumn: "Id");
                });

          
            migrationBuilder.CreateTable(
                name: "USR_Wallet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    WalletAddress = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USR_Wallet", x => x.Id);
                    table.ForeignKey(
                            name: "FK_USR_Wallet_USR_Users_USRUsersId",
                            column: x => x.UserId,
                            principalTable: "USR_Users",
                            principalColumn: "Id");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "USR_Machine");

            migrationBuilder.DropTable(
                name: "USR_Users");

            migrationBuilder.DropTable(
                name: "USR_Wallet");
        }
    }
}
