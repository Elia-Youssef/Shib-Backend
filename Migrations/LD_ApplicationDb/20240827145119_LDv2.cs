using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShibAPI.Migrations.LD_ApplicationDb
{
    /// <inheritdoc />
    public partial class LDv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "XP",
                table: "LD_Player",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "User_Id",
                table: "LD_Player",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Total_time_Played",
                table: "LD_Player",
                type: "interval",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LD_Player",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "LD_Player",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Coins",
                table: "LD_Player",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AI_time_Played",
                table: "LD_Player",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Custom_time_Played",
                table: "LD_Player",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DogClass1_time_Played",
                table: "LD_Player",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DogClass2_time_Played",
                table: "LD_Player",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DogClass3_time_Played",
                table: "LD_Player",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DogClass4_time_Played",
                table: "LD_Player",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Host_time_Played",
                table: "LD_Player",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Join_time_Played",
                table: "LD_Player",
                type: "interval",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LD_PlayerStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Player_Id = table.Column<int>(type: "integer", nullable: true),
                    Game_Finish_Top3 = table.Column<int>(type: "integer", nullable: true),
                    Time_Played = table.Column<TimeSpan>(type: "interval", nullable: true),
                    Best_record = table.Column<TimeSpan>(type: "interval", nullable: true),
                    Deaths_In_Games = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LD_PlayerStats", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LD_PlayerStats");

            migrationBuilder.DropColumn(
                name: "AI_time_Played",
                table: "LD_Player");

            migrationBuilder.DropColumn(
                name: "Custom_time_Played",
                table: "LD_Player");

            migrationBuilder.DropColumn(
                name: "DogClass1_time_Played",
                table: "LD_Player");

            migrationBuilder.DropColumn(
                name: "DogClass2_time_Played",
                table: "LD_Player");

            migrationBuilder.DropColumn(
                name: "DogClass3_time_Played",
                table: "LD_Player");

            migrationBuilder.DropColumn(
                name: "DogClass4_time_Played",
                table: "LD_Player");

            migrationBuilder.DropColumn(
                name: "Host_time_Played",
                table: "LD_Player");

            migrationBuilder.DropColumn(
                name: "Join_time_Played",
                table: "LD_Player");

            migrationBuilder.AlterColumn<int>(
                name: "XP",
                table: "LD_Player",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "User_Id",
                table: "LD_Player",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Total_time_Played",
                table: "LD_Player",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LD_Player",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "LD_Player",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Coins",
                table: "LD_Player",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
