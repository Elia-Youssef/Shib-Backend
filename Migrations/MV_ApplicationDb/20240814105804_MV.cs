using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShibAPI.Migrations.MV_ApplicationDb
{
    /// <inheritdoc />
    public partial class MV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MV_Lands",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    _id = table.Column<string>(type: "text", nullable: true),
                    x = table.Column<int>(type: "integer", nullable: true),
                    y = table.Column<int>(type: "integer", nullable: true),
                    tierName = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: true),
                    NoBillAllowedOnLand = table.Column<bool>(type: "boolean", nullable: true),
                    district = table.Column<string>(type: "text", nullable: true),
                    isShiboshiZone = table.Column<bool>(type: "boolean", nullable: true),
                    isRoad = table.Column<bool>(type: "boolean", nullable: true),
                    reserved = table.Column<bool>(type: "boolean", nullable: true),
                    primaryRoadName = table.Column<string>(type: "text", nullable: true),
                    secondaryRoadName = table.Column<string>(type: "text", nullable: true),
                    intersection = table.Column<bool>(type: "boolean", nullable: true),
                    hubName = table.Column<string>(type: "text", nullable: true),
                    Land_id = table.Column<long>(type: "bigint", nullable: true),
                    currentBidWinner = table.Column<string>(type: "text", nullable: true),
                    currentMintWinner = table.Column<string>(type: "text", nullable: true),
                    bidCount = table.Column<int>(type: "integer", nullable: true),
                    owner = table.Column<string>(type: "text", nullable: true),
                    minted = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MV_Lands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MV_Land_Bids",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    confirmed = table.Column<bool>(type: "boolean", nullable: true),
                    landId = table.Column<long>(type: "bigint", nullable: true),
                    bidPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    bidBy = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    block_number = table.Column<int>(type: "integer", nullable: true),
                    FK_land_Id = table.Column<int>(type: "integer", nullable: true),
                    MVLandsid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MV_Land_Bids", x => x.id);
                    table.ForeignKey(
                        name: "FK_MV_Land_Bids_MV_Lands_MVLandsid",
                        column: x => x.FK_land_Id,
                        principalTable: "MV_Lands",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "MV_Land_Mints",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    confirmed = table.Column<bool>(type: "boolean", nullable: true),
                    landId = table.Column<long>(type: "bigint", nullable: true),
                    mintPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    mintBy = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    block_number = table.Column<int>(type: "integer", nullable: true),
                    FK_land_Id = table.Column<int>(type: "integer", nullable: true),
                    MVLandsid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MV_Land_Mints", x => x.id);
                    table.ForeignKey(
                        name: "FK_MV_Land_Mints_MV_Lands_MVLandsid",
                        column: x => x.FK_land_Id,
                        principalTable: "MV_Lands",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MV_Land_Bids_MVLandsid",
                table: "MV_Land_Bids",
                column: "MVLandsid");

            migrationBuilder.CreateIndex(
                name: "IX_MV_Land_Mints_MVLandsid",
                table: "MV_Land_Mints",
                column: "MVLandsid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MV_Land_Bids");

            migrationBuilder.DropTable(
                name: "MV_Land_Mints");

            migrationBuilder.DropTable(
                name: "MV_Lands");
        }
    }
}
