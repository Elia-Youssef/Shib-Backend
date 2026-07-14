using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShibAPI.Migrations.LD_ApplicationDb
{
    /// <inheritdoc />
    public partial class LD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LD_GameSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Created_At = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    Nbr_of_laps = table.Column<int>(type: "integer", nullable: false),
                    Map_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LD_GameSession", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LD_Player",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    User_Id = table.Column<int>(type: "integer", nullable: false),
                    XP = table.Column<int>(type: "integer", nullable: false),
                    Coins = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Total_time_Played = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LD_Player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LD_PlayerSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Player_Id = table.Column<int>(type: "integer", nullable: false),
                    Game_id = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    Xp = table.Column<int>(type: "integer", nullable: false),
                    coin = table.Column<int>(type: "integer", nullable: false),
                    Time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Dog_Class = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LD_PlayerSession", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LD_GameSession");

            migrationBuilder.DropTable(
                name: "LD_Player");

            migrationBuilder.DropTable(
                name: "LD_PlayerSession");
        }
    }
}
