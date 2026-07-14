using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;

namespace ShibAPI.Controllers
{

    public class NFTCreator
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _dbContext;

        public NFTCreator(IHttpClientFactory httpClientFactory, ApplicationDbContext dbContext)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }

        public async Task FetchAndSaveNftMetadata(string data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var nftResponse = JsonSerializer.Deserialize<NFTResponse>(data, options);
            var serializedAttributes = "";

            foreach (var item in nftResponse.Items)
            {

                //Adding the Main Items
                #region Items

                if (item.Metadata != null)
                {
                    serializedAttributes = JsonSerializer.Serialize(item.Metadata.Attributes, options);
                }

                var serializedItems = JsonSerializer.Serialize(item, options);

                var nftEntityItem = new System_NFT_Item
                {
                    AnimationUrl = item.AnimationUrl,
                    ExternalAppUrl = item.ExternalAppUrl,
                    NFT_Id = item.NFT_Id,
                    ImageUrl = item.ImageUrl,
                    IsUnique = item.IsUnique,
                    Owner = item.Owner,
                    TokenType = item.TokenType,
                    Value = item.Value,
                    SerializedAttributes = serializedAttributes,
                    SerializedItems = serializedItems

                };

                _dbContext.NftItem.Add(nftEntityItem);
                await _dbContext.SaveChangesAsync();

                #endregion Items

                //Adding the Metadata
                #region Metadata

                if (item.Metadata != null)
                {
                    var nftEntitymetdata = new System_NFT_Metadata
                    {
                        Description = item.Metadata.Description,
                        ExternalUrl = item.Metadata.ExternalUrl,
                        Image = item.Metadata.Image,
                        Name = item.Metadata.Name,
                        FK_NFT_Id = nftEntityItem.Id

                    };
                    _dbContext.NftMetadata.Add(nftEntitymetdata);
                    await _dbContext.SaveChangesAsync();

                    //Adding the Attributes
                    #region Attributes

                    foreach (var Attributes in item.Metadata.Attributes)
                    {

                        var nftEntityAttributes = new System_NFT_Attribute
                        {
                            DisplayType = Attributes.DisplayType,
                            TraitType = Attributes.TraitType,
                            Value = Attributes.Value,
                            MetadataId = nftEntitymetdata.Id
                        };

                        _dbContext.NftAttribute.Add(nftEntityAttributes);
                        await _dbContext.SaveChangesAsync();
                    }
                    #endregion Attributes
                }
                #endregion Metadata

                //Adding the Token
                #region Token

                var nftEntityToken = new System_NFT_Token
                {
                    Address = item.Token.Address,
                    CirculatingMarketCap = item.Token.CirculatingMarketCap,
                    Decimals = item.Token.Decimals,
                    ExchangeRate = item.Token.ExchangeRate,
                    Holders = item.Token.Holders,
                    IconUrl = item.Token.IconUrl,
                    Name = item.Token.Name,
                    Symbol = item.Token.Symbol,
                    TotalSupply = item.Token.TotalSupply,
                    Type = item.Token.Type,
                    Volume24h = item.Token.Volume24h,
                    FK_NFT_Id = nftEntityItem.Id
                };

                _dbContext.NftToken.Add(nftEntityToken);
                await _dbContext.SaveChangesAsync();

                #endregion Token


            }
            
            
        }

    }

}



