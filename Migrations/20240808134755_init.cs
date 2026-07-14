using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShibAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NftItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimationUrl = table.Column<string>(type: "text", nullable: true),
                    ExternalAppUrl = table.Column<string>(type: "text", nullable: true),
                    NFT_Id = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    IsUnique = table.Column<bool>(type: "boolean", nullable: true),
                    MetadataId = table.Column<int>(type: "integer", nullable: true),
                    Owner = table.Column<string>(type: "text", nullable: true),
                    TokenId = table.Column<int>(type: "integer", nullable: true),
                    TokenType = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    SerializedAttributes = table.Column<string>(type: "text", nullable: true),
                    SerializedItems = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NftItem", x => x.Id);
                    //table.ForeignKey(
                    //    name: "FK_NftItem_NftMetadata_MetadataId",
                    //    column: x => x.MetadataId,
                    //    principalTable: "NftMetadata",
                    //    principalColumn: "Id");
                    //table.ForeignKey(
                    //    name: "FK_NftItem_NftToken_TokenId",
                    //    column: x => x.TokenId,
                    //    principalTable: "NftToken",
                    //    principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NftMetadata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ExternalUrl = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    FK_NFT_Id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NftMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NftMetadata_NftItem_Id",
                        column: x => x.FK_NFT_Id,
                        principalTable: "NftItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NftToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "text", nullable: true),
                    CirculatingMarketCap = table.Column<string>(type: "text", nullable: true),
                    Decimals = table.Column<string>(type: "text", nullable: true),
                    ExchangeRate = table.Column<string>(type: "text", nullable: true),
                    Holders = table.Column<string>(type: "text", nullable: true),
                    IconUrl = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Symbol = table.Column<string>(type: "text", nullable: true),
                    TotalSupply = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Volume24h = table.Column<string>(type: "text", nullable: true),
                    FK_NFT_Id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NftToken", x => x.Id);
                    table.ForeignKey(
                      name: "FK_NftItem_NftToken_TokenId",
                      column: x => x.FK_NFT_Id,
                      principalTable: "NftItem",
                      principalColumn: "Id"
                      );
                });

            migrationBuilder.CreateTable(
                name: "NftAttribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisplayType = table.Column<string>(type: "text", nullable: true),
                    TraitType = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    MetadataId = table.Column<int>(type: "integer", nullable: true),
                    System_NFT_MetadataId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NftAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NftAttribute_NftMetadata_System_NFT_MetadataId",
                        column: x => x.System_NFT_MetadataId,
                        principalTable: "NftMetadata",
                        principalColumn: "Id");
                });

            

            //migrationBuilder.CreateIndex(
            //    name: "IX_NftAttribute_System_NFT_MetadataId",
            //    table: "NftAttribute",
            //    column: "System_NFT_MetadataId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_NftItem_MetadataId",
            //    table: "NftItem",
            //    column: "MetadataId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_NftItem_TokenId",
            //    table: "NftItem",
            //    column: "TokenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NftAttribute");

            migrationBuilder.DropTable(
                name: "NftItem");

            migrationBuilder.DropTable(
                name: "NftMetadata");

            migrationBuilder.DropTable(
                name: "NftToken");
        }
    }
}
