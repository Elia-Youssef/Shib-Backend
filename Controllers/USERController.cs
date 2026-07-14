using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShibAPI.Controllers.Auth.Services;
using ShibAPI.Migrations.User_ApplicationDb;
using System.Threading.Tasks;
using System;
namespace ShibAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class USERController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly User_ApplicationDbContext User_dbContext;
        private readonly IAuthenticationServices _authenticationService;
        public USERController(IHttpClientFactory httpClientFactory, User_ApplicationDbContext UserdbContext, IAuthenticationServices authenticationService)
        {
            _httpClientFactory = httpClientFactory;
            User_dbContext = UserdbContext;
            _authenticationService = authenticationService;
        }


        //[HttpPost("CONNECTWALLET")]
        [HttpPost("SIGNUP")]
        public async Task<IActionResult> ConnectWallet([FromBody] LoginUser Requests)
        {
            foreach (var Request in Requests.Params)
            {

                if (Request.WalletAddress == "")
                {
                    return BadRequest("Wallet Address Missing.");
                }

                else
                {
                    int Count = User_dbContext.USR_Wallet.Where(x => x.WalletAddress == Request.WalletAddress).Count();

                    if (Count == 0)
                    {

                        var UserData = new User
                        {
                            // creating the Id of the use
                            UserName = Request.name != null ? Request.name : "",
                            CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
                        };

                        User_dbContext.USR_Users.Add(UserData);
                        await User_dbContext.SaveChangesAsync();

                        var authresult = _authenticationService.ConnectWallet(Request.WalletAddress,
                                             UserData.Id,
                                             Request.name,
                                             "",
                                             DateTime.UtcNow.AddDays(7),
                                             true,
                                             true);

                        var response = new AuthenticationResult(
                            authresult.WalletAddress,
                            authresult.UserId,
                            authresult.UserName,
                            authresult.Email,
                            authresult.Expirydate.AddDays(7),
                            authresult.isNewUser,
                            authresult.Success,
                            authresult.token);
                        
                        var UserMachineData = new UserMachine
                        {
                            UserId = UserData.Id,
                            ClientKey = Request.ClientKey,
                            TokenID = authresult.token,
                            isNewUser = true,
                            EffectiveDate = DateTime.UtcNow,
                            ExpiryDate = DateTime.UtcNow.AddDays(7)
                        };

                        var UserWalletData = new UserWallet
                        {
                            UserId = UserData.Id,
                            WalletAddress = Request.WalletAddress
                        };

                        User_dbContext.USR_Machine.Add(UserMachineData);
                        User_dbContext.USR_Wallet.Add(UserWalletData);
                        await User_dbContext.SaveChangesAsync();

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Content(jsonResponse, "application/json");

                    }
                    else
                    {
                        var userId = User_dbContext.USR_Wallet
                          .Where(x => x.WalletAddress == Request.WalletAddress)
                          .Select(x => x.UserId)
                          .FirstOrDefault() ?? -1;

                        var user = User_dbContext.USR_Users
                            .Where(x => x.Id == userId)
                            .Select(x => new
                            {
                                UserCode = x.UserCode ?? string.Empty,
                                UserName = x.UserName ?? string.Empty,
                                Email = x.Email ?? string.Empty
                            })
                            .FirstOrDefault();

                        if (User_dbContext.USR_Machine.Any(x => x.ExpiryDate >= DateTimeOffset.UtcNow && x.UserId == userId))
                        {
                            User_dbContext.USR_Machine
                           .Where(x => x.TokenID == Request.Token)
                           .ExecuteUpdate(y => y
                               .SetProperty(x => x.ExpiryDate, x => DateTime.UtcNow.AddDays(7)));

                                await User_dbContext.SaveChangesAsync();

                            var authresult = _authenticationService.ConnectWallet(Request.WalletAddress,
                                                  userId,
                                                  user.UserName,
                                                  user.Email,
                                                  DateTime.UtcNow.AddDays(7),
                                                  false,
                                                  true);

                            var response = new AuthenticationResult(
                               authresult.WalletAddress,
                               authresult.UserId,
                               authresult.UserName,
                               authresult.Email,
                               authresult.Expirydate,
                               authresult.isNewUser,
                               authresult.Success,
                               authresult.token);

                            string jsonResponse = JsonConvert.SerializeObject(response);
                            return Content(jsonResponse, "application/json");

                        }
                        else if (User_dbContext.USR_Machine.Any(x => x.ExpiryDate <= DateTimeOffset.UtcNow && x.UserId == userId))
                        { 
                            var authresult = _authenticationService.ConnectWallet(Request.WalletAddress,
                                                  userId,
                                                  user.UserName,
                                                  user.Email,
                                                  DateTime.UtcNow.AddDays(7),
                                                  false,
                                                  true);

                            var response = new AuthenticationResult(
                               authresult.WalletAddress,
                               authresult.UserId,
                               authresult.UserName,
                               authresult.Email,
                               authresult.Expirydate,
                               authresult.isNewUser,
                               authresult.Success,
                               authresult.token);

                            var UserMachineData = new UserMachine
                            {
                                UserId = userId,
                                ClientKey = Request.ClientKey,
                                TokenID = authresult.token,
                                isNewUser = false
                            };

                            User_dbContext.USR_Machine.Add(UserMachineData);
                            await User_dbContext.SaveChangesAsync();

                            string jsonResponse = JsonConvert.SerializeObject(response);
                            return Content(jsonResponse, "application/json");

                        }
                        
                    }
                }
            }
            return Content("Success");
        }


        [HttpPost("LOGIN")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUser Requests)
        {
            string jsonResponse = "";
            foreach (var Request in Requests.Params)
            {

                if (Request.WalletAddress == "")
                {
                    return BadRequest("Wallet Address Missing.");
                }

                else
                {
                    int Count = User_dbContext.USR_Wallet.Where(x => x.WalletAddress == Request.WalletAddress).Count();

                    if (Count != 0)
                    {
                        var userId = User_dbContext.USR_Wallet
                          .Where(x => x.WalletAddress == Request.WalletAddress)
                          .Select(x => x.UserId)
                          .FirstOrDefault() ?? -1;

                        var user = User_dbContext.USR_Users
                            .Where(x => x.Id == userId)
                            .Select(x => new
                            {
                                UserCode = x.UserCode ?? string.Empty,
                                UserName = x.UserName ?? string.Empty,
                                Email = x.Email ?? string.Empty
                            })
                            .FirstOrDefault();

                        if (userId != -1)
                        {
                            var UserMachineDataDeleted = User_dbContext.USR_Machine
                                .Where(x => x.UserId == userId);

                            User_dbContext.USR_Machine.RemoveRange(UserMachineDataDeleted);
                            User_dbContext.SaveChanges();
                        }
                            var authresult = _authenticationService.ConnectWallet(Request.WalletAddress,
                                                  userId,
                                                  user.UserName,
                                                  user.Email,
                                                  DateTime.UtcNow.AddDays(7),
                                                  false,
                                                  true);

                            var response = new AuthenticationResult(
                               authresult.WalletAddress,
                               authresult.UserId,
                               authresult.UserName,
                               authresult.Email,
                               authresult.Expirydate,
                               authresult.isNewUser,
                               authresult.Success,
                               authresult.token);

                            var UserMachineData = new UserMachine
                            {
                                UserId = userId,
                                ClientKey = Request.ClientKey,
                                TokenID = authresult.token,
                                isNewUser = false
                            };

                            User_dbContext.USR_Machine.Add(UserMachineData);
                            await User_dbContext.SaveChangesAsync();

                             jsonResponse = JsonConvert.SerializeObject(response);
                            return Content(jsonResponse, "application/json");

                        

                    }
                }
            }
           // return Content(jsonResponse, "application/json");
            return Content("Success");
        }


        [HttpPost("CONNECTMAIL")]

        public async Task<IActionResult> ConnectEmail([FromBody] LoginUser Requests)
        {
            foreach (var Request in Requests.Params)
            {

                if (Request.Email == "")
                {
                    return BadRequest("Email Missing.");
                }
                else
                {
                    int Count = User_dbContext.USR_Users.Where(x => x.Email == Request.Email).Count();

                    if (Count == 0)
                    {

                        var UserData = new User
                        {
                            // creating the Id of the user
                            UserName = Request.name != null ? Request.name : "",
                            Email = Request.Email,
                            CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
                        };

                        User_dbContext.USR_Users.Add(UserData);
                        await User_dbContext.SaveChangesAsync();

                        var authresult = _authenticationService.ConnectMail("",
                                             UserData.Id,
                                             UserData.UserName,
                                             Request.Email,
                                             DateTime.UtcNow.AddDays(7),
                                             true,
                                             true);

                        var response = new AuthenticationResult(
                            authresult.WalletAddress,
                            authresult.UserId,
                            authresult.UserName,
                            authresult.Email,
                            authresult.Expirydate.AddDays(7),
                            authresult.isNewUser,
                            authresult.Success,
                            authresult.token);



                        var UserMachineData = new UserMachine
                        {
                            UserId = UserData.Id,
                            ClientKey = Request.ClientKey,
                            TokenID = authresult.token,
                            isNewUser = true,
                            EffectiveDate = DateTime.UtcNow,
                            ExpiryDate = DateTime.UtcNow.AddDays(7)
                        };

                        var UserWalletData = new UserWallet
                        {
                            UserId = UserData.Id,
                            WalletAddress = Request.WalletAddress
                        };

                        User_dbContext.USR_Machine.Add(UserMachineData);
                        User_dbContext.USR_Wallet.Add(UserWalletData);
                        await User_dbContext.SaveChangesAsync();

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Content(jsonResponse, "application/json");

                    }
                    else
                    {
                        var user = User_dbContext.USR_Users
                          .Where(x => x.Email == Request.Email)
                          .Select(x => new
                          {
                              UserId = x.Id,
                              UserCode = x.UserCode ?? string.Empty,
                              UserName = x.UserName ?? string.Empty,
                              Email = x.Email ?? string.Empty
                          })
                          .FirstOrDefault();

                        if (User_dbContext.USR_Machine.Any(x => x.ExpiryDate >= DateTime.UtcNow))
                        {
                            User_dbContext.USR_Machine
                           .Where(x => x.TokenID == Request.Token)
                           .ExecuteUpdate(y => y
                               .SetProperty(x => x.ExpiryDate, x => DateTime.UtcNow.AddDays(7)));

                            await User_dbContext.SaveChangesAsync();

                            var authresult = _authenticationService.ConnectMail("",
                                                  user.UserId,
                                                  user.UserName,
                                                  user.Email,
                                                  DateTime.UtcNow.AddDays(7),
                                                  false,
                                                  true);

                            var response = new AuthenticationResult(
                               authresult.WalletAddress,
                               authresult.UserId,
                               authresult.UserName,
                               authresult.Email,
                               authresult.Expirydate,
                               authresult.isNewUser,
                               authresult.Success,
                               authresult.token);

                            string jsonResponse = JsonConvert.SerializeObject(response);
                            return Content(jsonResponse, "application/json");

                        }
                        else if (User_dbContext.USR_Machine.Any(x => x.ExpiryDate <= DateTime.UtcNow ))
                        {


                            var authresult = _authenticationService.ConnectMail("",
                                                  user.UserId,
                                                  user.UserName,
                                                  user.Email,
                                                  DateTime.UtcNow.AddDays(7),
                                                  false,
                                                  true);

                            var response = new AuthenticationResult(
                               authresult.WalletAddress,
                               authresult.UserId,
                               authresult.UserName,
                               authresult.Email,
                               authresult.Expirydate,
                               authresult.isNewUser,
                               authresult.Success,
                               authresult.token);

                            var UserMachineData = new UserMachine
                            {
                                UserId = user.UserId,
                                ClientKey = Request.ClientKey,
                                TokenID = authresult.token,
                                isNewUser = false
                            };

                            User_dbContext.USR_Machine.Add(UserMachineData);
                            await User_dbContext.SaveChangesAsync();

                            string jsonResponse = JsonConvert.SerializeObject(response);
                            return Content(jsonResponse, "application/json");

                        }


                    }
                }
            }
            return Content("Success");
        }

        [HttpGet("WALLETEXISTENCE")]
        //[Authorize]
        //public async Task<IActionResult> ValidateWallet(string clientkey)
        public IActionResult ValidateWallet(string WalletAddress)
        {

            //if (clientkey == "")
            //{
            //    return BadRequest("Token ID Missing.");
            //}

            //int Count = User_dbContext.USR_Machine.Where(x => x.ClientKey == clientkey).Count();

            //if (Count != 0)
            //{
            //    var userKeys = User_dbContext.USR_Machine
            //     .Where(x => x.ClientKey == clientkey)
            //     .Select(x => new
            //     {
            //         x.UserId,
            //         x.ClientKey,
            //         x.ExpiryDate,
            //         x.TokenID,
            //         x.isNewUser

            //     })
            //     .FirstOrDefault();


            //    var UserWalletAddress = User_dbContext.USR_Wallet
            //              .Where(x => x.UserId == userKeys.UserId)
            //              .Select(x => x.WalletAddress)
            //              .FirstOrDefault() ?? "";

            //    var user = User_dbContext.USR_Users
            //        .Where(x => x.Id == userKeys.UserId)
            //        .Select(x => new
            //        {
            //            UserCode = x.UserCode ?? string.Empty,
            //            UserName = x.UserName ?? string.Empty,
            //            Email = x.Email ?? string.Empty
            //        })
            //        .FirstOrDefault();

            //    var response = (
            //        clientkey,
            //       UserWalletAddress,
            //        user.UserCode,
            //        user.UserName,
            //        user.Email,
            //        (bool)userKeys.isNewUser,
            //        true);

            //    string jsonResponse = JsonConvert.SerializeObject(response);

            //    return Content(jsonResponse, "application/json");

            //}
            //else
            //{
            //    var jsonResponse = JsonConvert.SerializeObject(new { Success = false });
            //    return Content(jsonResponse, "application/json");
            //}

            int Count = User_dbContext.USR_Wallet.Where(x => x.WalletAddress == WalletAddress).Count();

            if (Count == 0)
            {
                    var jsonResponse = JsonConvert.SerializeObject(new { Success = false });
                    return Content(jsonResponse, "application/json");
            }
            else
            {
                var jsonResponse = JsonConvert.SerializeObject(new { Success = true });
                return Content(jsonResponse, "application/json");
            }
        }

        [HttpPost("FILLUSERINFO")]
        /*Json template 
          {
              "Users": [
                {
                  "UserId": 1,
                  "UserCode": "",
                  "UserName": "",
                  "Email": "",
                  "NftId": 1,
                  "CountryId":1
                }
              ]
          }
}
        */
        public async Task<IActionResult> FetchUserInfo([FromBody] infoUser Requests)
        {
            foreach (var Request in Requests.UserInfo)
            {

                if (Request.UserName == "")
                {
                    return BadRequest("UserName Missing.");
                }
                else if (Request.UserCode == "")
                {
                    return BadRequest("UserCode Missing.");
                }
                else if (Request.Email == "")
                {
                    return BadRequest("Email Missing.");
                }
                else
                {
                    int Count = User_dbContext.USR_Users.Where(x => x.Id == Request.Id).Count();

                    if (Count == 0)
                    {

                        User_dbContext.USR_Users
                            .Where(u => u.Id == Request.Id)
                            .ExecuteUpdate(b => b
                            .SetProperty(u => u.UserCode, Request.UserCode)
                            .SetProperty(u => u.UserName, Request.UserName)
                            .SetProperty(u => u.Email, Request.Email)
                            .SetProperty(u => u.NftId, Request.NftId)
                            .SetProperty(u => u.CountryId, Request.CountryId)
                            );
                        await User_dbContext.SaveChangesAsync();

                        var jsonResponse = JsonConvert.SerializeObject(new { Success = true });
                        return Content(jsonResponse, "application/json");
                    }
                    else
                    {
                        var jsonResponse = JsonConvert.SerializeObject(new { Success = false });
                        return Content(jsonResponse, "application/json");
                    }
                }
            }
            return Content("Success");
        }

        [HttpGet("SHOWUSERINFO")]

        public async Task<IActionResult> ShowUser(string Token)
        {
            var userId = User_dbContext.USR_Machine
                          .Where(x => x.TokenID == Token)
                          .Select(x => x.UserId)
                          .FirstOrDefault() ;

            //var responseMachine = User_dbContext.USR_Machine
            //             .Where(x => x.TokenID == Token)
            //             .ToList();
            var responseUser = User_dbContext.USR_Users
                         .Where(x => x.Id == userId)
                           .FirstOrDefault();
            var responseWallet = User_dbContext.USR_Wallet
                         .Where(x => x.UserId == userId)
                         .ToList();

            var responseNFT = User_dbContext.USR_NFT
                         .Where(x => x.UserId == userId)
                         .ToList();

            // Combine them into an anonymous object
            var combinedResponse = new
            {
               // Machines = responseMachine,
                User = responseUser,
                Wallets = responseWallet,
                NFTs = responseNFT
            };

            // Serialize the combined response to JSON
            string jsonResponse = JsonConvert.SerializeObject(combinedResponse, Formatting.Indented);

            return Content(jsonResponse, "application/json");
        }

    }

}

