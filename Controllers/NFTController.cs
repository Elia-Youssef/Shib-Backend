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
    public class NFTController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly NFTCreator _usercreator;
        string wallet_id = "0x52b496b21b719f96Cde2C366C8Ea0b322F8EfF64";
        string type_ids = "ERC-721,ERC-404,ERC-1155";
        public NFTController(IHttpClientFactory httpClientFactory, NFTCreator nftcreator)
        {
            _httpClientFactory = httpClientFactory;
            _usercreator = nftcreator;
        }


        [HttpPost("GETNFT")]
        public async Task<IActionResult> FetchNftData()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"https://puppyscan.shib.io/api/v2/addresses/{wallet_id}/nft?type={type_ids}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                await _usercreator.FetchAndSaveNftMetadata(data.Replace("\"id\":", "\"NFT_Id\":"));
                return Content(data, "application/json");
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }

        }


    }

}

