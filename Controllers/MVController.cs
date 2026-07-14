using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBitcoin.Secp256k1;
using Newtonsoft.Json;
using System.Text.Json;
using System.Xml.Linq;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Web3Wallet;
using WalletConnectSharp.Web3Wallet.Interfaces;
namespace ShibAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MVController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MV_ApplicationDbContext MV_dbContext;
        public MVController(IHttpClientFactory httpClientFactory, MV_ApplicationDbContext MVdbContext)
        {
            _httpClientFactory = httpClientFactory;
            MV_dbContext = MVdbContext;
        }

        [HttpPost("COUNTPLOT")]

        public async Task<IActionResult> PlotCounter()
        {


            // Parse the JSON data

            using FileStream fs = new FileStream("jsonformatter.txt", FileMode.Open, FileAccess.Read);

            using JsonDocument doc = JsonDocument.Parse(fs);
            JsonElement root = doc.RootElement;

            // Get the array from the JSON
            JsonElement items = root.GetProperty("items");

            // Count the number of objects in the array
            int count = items.GetArrayLength();

            return Content(" Count:" + count);

        }

        [HttpPost("INSERTPLOT")]
        public async Task<IActionResult> Plotfetching()
        {


            // Parse the JSON data

            using FileStream fs = new FileStream("jsonformatter.txt", FileMode.Open, FileAccess.Read);

            using JsonDocument doc = JsonDocument.Parse(fs);
            JsonElement root = doc.RootElement;

            // Get the array from the JSON
            JsonElement items = root.GetProperty("items");

            foreach (var item in items.EnumerateArray())
            {
                var coordinates = item.GetProperty("coordinates");

                var MVLandsData = new MVLands
                {
                    _id = item.TryGetProperty("_id", out var _id) ? item.GetProperty("_id").GetString() : null,
                    x = coordinates.TryGetProperty("x", out var x) ? coordinates.GetProperty("x").GetInt32() : null,
                    y = coordinates.TryGetProperty("y", out var y) ? coordinates.GetProperty("y").GetInt32() : null,
                    tierName = item.TryGetProperty("tierName", out var tierName) ? item.GetProperty("tierName").GetString() : null,
                    //price = item.TryGetProperty("price", out var price) ? item.GetProperty("price").GetString() : null,
                    NoBillAllowedOnLand = item.TryGetProperty("noBidAllowedOnLand", out var noBillAllowed) ? item.GetProperty("noBidAllowedOnLand").GetBoolean() : false,
                    district = item.TryGetProperty("district", out var district) ? item.GetProperty("district").GetString() : null,
                    isShiboshiZone = item.TryGetProperty("isShiboshiZone", out var isShiboshiZone) ? item.GetProperty("isShiboshiZone").GetBoolean() : false,
                    isRoad = item.TryGetProperty("isRoad", out var isRoad) ? item.GetProperty("isRoad").GetBoolean() : false,
                    reserved = item.TryGetProperty("reserved", out var reserved) ? item.GetProperty("reserved").GetBoolean() : false,
                    primaryRoadName = item.TryGetProperty("primaryRoadName", out var primaryRoadName) ? item.GetProperty("primaryRoadName").GetString() : null,
                    secondaryRoadName = item.TryGetProperty("secondaryRoadName", out var secondaryRoadName) ? item.GetProperty("secondaryRoadName").GetString() : null,
                    //intersection = item.TryGetProperty("intersection", out var intersection) ? item.GetProperty("intersection").GetString() : null,
                    intersection = item.TryGetProperty("intersection", out var intersectionElement) ? (intersectionElement.ValueKind == JsonValueKind.String && string.IsNullOrEmpty(intersectionElement.GetString())) ? false : intersectionElement.GetBoolean() : (bool?)null,
                    hubName = item.TryGetProperty("hubName", out var hubName) ? item.GetProperty("hubName").GetString() : null,
                    Land_id = item.TryGetProperty("id", out var landId) ? item.GetProperty("id").GetInt64() : 0,
                    currentBidWinner = item.TryGetProperty("currentBidWinner", out var currentBidWinner) ? item.GetProperty("currentBidWinner").GetString() : null,
                    currentMintWinner = item.TryGetProperty("currentMintWinner", out var currentMintWinner) ? item.GetProperty("currentMintWinner").GetString() : null,
                    bidCount = item.TryGetProperty("bidCount", out var bidCount) ? item.GetProperty("bidCount").GetInt32() : 0,
                    owner = item.TryGetProperty("owner", out var owner) ? item.GetProperty("owner").GetString() : null,
                    minted = item.TryGetProperty("minted", out var minted) ? item.GetProperty("minted").GetBoolean() : false
                };

                // Handling the price separately with conditional logic
                decimal? price = null;
                if (item.TryGetProperty("price", out var priceElement))
                {
                    if (priceElement.ValueKind == JsonValueKind.Number)
                    {
                        price = priceElement.GetDecimal();
                    }
                    else if (priceElement.ValueKind == JsonValueKind.String && decimal.TryParse(priceElement.GetString(), out var parsedPrice))
                    {
                        price = parsedPrice;
                    }
                }

                // Assign the price after checking and parsing it
                MVLandsData.price = price;
                MV_dbContext.MV_Lands.Add(MVLandsData);
                await MV_dbContext.SaveChangesAsync();

                #region Bids
                if (item.TryGetProperty("bids", out JsonElement bidsElement))
                {
                    foreach (var itemBids in bidsElement.EnumerateArray())
                    {
                        var MVLandBidsData = new MVBids
                        {

                            confirmed = itemBids.GetProperty("confirmed").GetBoolean(),
                            // landId = itemBids.GetProperty("landId").GetInt32(),
                            bidPrice = itemBids.GetProperty("bidPrice").GetDecimal(),
                            bidBy = itemBids.GetProperty("bidBy").GetString(),
                            //createdAt = itemBids.GetProperty("createdAt").GetDateTime(),
                            createdAt = itemBids.TryGetProperty("createdAt", out var createdAtElement) && createdAtElement.ValueKind != JsonValueKind.Null ? (DateTime?)createdAtElement.GetDateTime() : null,
                            block_number = itemBids.GetProperty("block_number").GetInt32(),
                            FK_land_Id = MVLandsData.id
                        };

                        // Handling the landid separately with conditional logic
                        long? land_Id = 0;
                        if (itemBids.TryGetProperty("landId", out var landElement))
                        {
                            if (landElement.ValueKind == JsonValueKind.Number)
                            {
                                land_Id = landElement.GetInt64();
                            }
                            else if (landElement.ValueKind == JsonValueKind.String && long.TryParse(landElement.GetString(), out var parsedland))
                            {
                                land_Id = parsedland;
                            }
                        }
                        MVLandBidsData.landId = land_Id;
                        MV_dbContext.MV_Land_Bids.Add(MVLandBidsData);
                        await MV_dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    // "Bids" does not exist, handle accordingly (if needed)
                }
                #endregion Bids

                #region Mints
                if (item.TryGetProperty("mints", out JsonElement MintsElement))
                {
                    foreach (var itemMints in MintsElement.EnumerateArray())
                    {
                        var MVLandMintsData = new MVMints
                        {

                            confirmed = itemMints.GetProperty("confirmed").GetBoolean(),
                            //landId = itemMints.GetProperty("landId").GetInt32(),
                            mintPrice = itemMints.GetProperty("mintPrice").GetDecimal(),
                            mintBy = itemMints.GetProperty("mintBy").GetString(),
                            //createdAt = itemMints.GetProperty("createdAt").GetDateTime(),
                            createdAt = itemMints.TryGetProperty("createdAt", out var createdAtElement) && createdAtElement.ValueKind != JsonValueKind.Null ? (DateTime?)createdAtElement.GetDateTime() : null,
                            block_number = itemMints.GetProperty("block_number").GetInt32(),
                            FK_land_Id = MVLandsData.id
                        };
                        // Handling the landid separately with conditional logic
                        long? land_Id = 0;
                        if (itemMints.TryGetProperty("landId", out var landElement))
                        {
                            if (landElement.ValueKind == JsonValueKind.Number)
                            {
                                land_Id = landElement.GetInt64();
                            }
                            else if (landElement.ValueKind == JsonValueKind.String && long.TryParse(landElement.GetString(), out var parsedland))
                            {
                                land_Id = parsedland;
                            }
                        }
                        MVLandMintsData.landId = land_Id;
                        MV_dbContext.MV_Land_Mints.Add(MVLandMintsData);
                        await MV_dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    // "Mints" does not exist, handle accordingly (if needed)
                }
                #endregion Mints
            }
            return Content("Success");

        }


    }

}

