using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
using Newtonsoft.Json;
using NodaTime;
using System.Data.SqlTypes;
using System.Security.AccessControl;
using WalletConnectSharp.Auth.Models;

namespace ShibAPI.Controllers
{
    [Route("api/SL")]
    [ApiController]
    //[Authorize]

    public class SLController : Controller
    {
        private readonly SL_ApplicationDbContext SL_dbContext;
        private readonly IAmazonS3 _s3Client;
        public SLController(SL_ApplicationDbContext SLdbContext, IAmazonS3 s3Client)
        {
            SL_dbContext = SLdbContext;
            _s3Client = s3Client;
        }


        [HttpPost("ADDGAME")]

        public async Task<IActionResult> AddGame([FromBody] SLResponse Requests)
        {


            foreach (var gameList in Requests.GameLists)
            {
                //Adding the Main Game Table
                #region GameList

                var SLGameListData = new SLGameList
                {
                    Title = gameList.Title,
                    Description = gameList.Description,
                    ProfilePictureID = gameList.ProfilePictureID,
                    CoverPictureID = gameList.CoverPictureID,
                    LatestVersionID = gameList.LatestVersionID,
                    EntryPoint = gameList.EntryPoint
                    //System_Req_Id = SLGameRequirementdata.Id,
                };

                SL_dbContext.SL_GameList.Add(SLGameListData);
                await SL_dbContext.SaveChangesAsync();

                #endregion GameList
                //Adding the Requirement
                #region Requirement
                foreach (var requirement in gameList.Requirement)
                {
                    var SLGameRequirementdata = new SLGameRequirement
                    {
                        Requirement_Type = requirement.Requirement_Type,
                        Req_desc = requirement.Req_desc,
                        OS = requirement.OS,
                        Processor = requirement.Processor,
                        Memory = requirement.Memory,
                        Graphics = requirement.Graphics,
                        Storage = requirement.Storage,
                        AdditionalNotes = requirement.AdditionalNotes,
                        // GameId = SLGameListData.Id,
                        SLGameListId = SLGameListData.Id
                    };

                    SL_dbContext.SL_GameRequirement.Add(SLGameRequirementdata);
                    await SL_dbContext.SaveChangesAsync();
                }
                #endregion Requirement
                //Adding the  Game Media
                #region GameMedia

                foreach (var GameMediaData in gameList.GameMedia)
                {
                    var SLGameMediaData = new SLGameMedia
                    {
                        // GameId = SLGameListData.Id,
                        Image = GameMediaData.Image,
                        Type = GameMediaData.Type,
                        SLGameListId = SLGameListData.Id
                    };

                    SL_dbContext.SL_GameMedia.Add(SLGameMediaData);
                    await SL_dbContext.SaveChangesAsync();
                }
                #endregion Gamemedia

                //Adding the  Game Version
                #region GameVersion

                var SLGameVersionData = new SLGameVersion
                {
                    //GameId = SLGameListData.Id,
                    version = gameList.GameVersion.version,
                    ReleaseDate = DateTime.UtcNow,
                    isSupported = true,
                    ReleaseNote = gameList.GameVersion.ReleaseNote,
                    SLGameListId = SLGameListData.Id
                };

                SL_dbContext.SL_GameVersion.Add(SLGameVersionData);
                await SL_dbContext.SaveChangesAsync();

                #endregion Game Version
            }
            return Content("True");
        }

        [HttpGet("SHOWGAMES")]

        public async Task<IActionResult> ShowGames(IAmazonS3 client)
        {
            var urlString = new List<string>();

            AWSConfigsS3.UseSignatureVersion4 = true;
            IAmazonS3 s3Client = new AmazonS3Client(RegionEndpoint.USEast1);
            string bucketName = "shiblauncher";
            string objectName = "";
            double duration = 12;

            int GameMediaCount = SL_dbContext.SL_GameMedia.Count();
            var GameMedia = SL_dbContext.SL_GameMedia
                           
                            .Select(x => new
                            {
                                Image = x.Image ?? string.Empty
                            })
                            .ToList();

            for (int i = 0; i <= GameMediaCount - 1; i++)
            {
                objectName = GameMedia[i].Image;

                urlString.Add(GeneratePresignedURL(s3Client, bucketName, objectName, duration));

            }


            var response = SL_dbContext.SL_GameList
                .Select(gl => new
                {
                    gl.Id,
                    gl.Title,
                    ProfilePicture = gl.GameMedia
                                .Where(gm => gm.Id == gl.ProfilePictureID)
                                .Select(gm => new
                                {
                                    Image = urlString.FirstOrDefault(url => url.Contains(gm.Image))
                                })
                                .FirstOrDefault(),
                                gl.Description
                })
              .ToList();

            string jsonResponse = JsonConvert.SerializeObject(response);
            return Content(jsonResponse, "application/json");
        }

        [HttpPost("UPDATEVERSION")]

        public async Task<IActionResult> UpdateVersion([FromBody] SLversionResponse RequestVersion)
        {

            foreach (var Request in RequestVersion.GameVersion)
            {

                var SLGameVersionData = new SLGameVersion
                {
                    version = Request.version,
                    ReleaseDate = DateTime.UtcNow,
                    isSupported = Request.isSupported,
                    ReleaseNote = Request.ReleaseNote,
                    SLGameListId = Request.SLGameListId
                };

                SL_dbContext.SL_GameVersion.Add(SLGameVersionData);
                await SL_dbContext.SaveChangesAsync();

                if (SLGameVersionData.isSupported == true)
                {
                    SL_dbContext.SL_GameList
                                .Where(x => x.Id == Request.SLGameListId)
                                .ExecuteUpdate(y => y
                                .SetProperty(x => x.LatestVersionID, SLGameVersionData.Id)
                                    );
                    await SL_dbContext.SaveChangesAsync();
                }
            }
            return Content("True");
        }


        [HttpPost("ADDIMAGE")]

        public async Task<IActionResult> AddImage([FromBody] SLMediaResponse RequestMedia)
        {

            foreach (var Request in RequestMedia.GameMedia)
            {

                var SLGameMediaData = new SLGameMedia
                {
                    Image = Request.Image,
                    Type = Request.Type,
                    SLGameListId = Request.SLGameListId
                };

                SL_dbContext.SL_GameMedia.Add(SLGameMediaData);
                await SL_dbContext.SaveChangesAsync();


            }
            return Content("True");
        }


        [HttpGet("DOWNLOADVERSION")]
        public async Task<IActionResult> DownloadObjectFromBucketAsync(
           int GameId,
           string VersionNumber)
        {
            string bucketName = "shiblauncher";
            int duration = 12;
            var Game = SL_dbContext.SL_GameList
                .Where(x => x.Id == GameId)
                .Select(x => new
                {
                    Title = x.Title ?? string.Empty
                })
                .FirstOrDefault();

            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = Game.Title + "/Versions/" + VersionNumber + ".zip",
            };

            // Issue request and remember to dispose of the response
            //using GetObjectResponse response = await _s3Client.GetObjectAsync(request);

            //try
            //{

            //    using (var reader = new StreamReader(response.ResponseStream))
            //    {
            //        //string fileContent = await reader.ReadToEndAsync();

            //        //string jsonResponse = JsonConvert.SerializeObject(fileContent);
            //        //return Content(jsonResponse, "application/json");
            //        await response.WriteResponseStreamToFileAsync($"{filePath}\\{objectName}", true, CancellationToken.None);
            //        return Content("True"); ;
            //    }
            //}
            //catch (AmazonS3Exception ex)
            //{
            //    Console.WriteLine($"Error saving {objectName}: {ex.Message}");
            //    return Content("False"); ;
            //}

            AWSConfigsS3.UseSignatureVersion4 = true;
            IAmazonS3 s3Client = new AmazonS3Client(RegionEndpoint.USEast1);

            var urlObject = new
            {
                URL = GeneratePresignedURL(s3Client, bucketName, request.Key, duration)
            };          
            string jsonResponse = JsonConvert.SerializeObject(urlObject);
            return Content(jsonResponse, "application/json");
        }


        //[HttpGet("GETGAMEURL")]
        [HttpGet("GETGAMEDATA")]

        public async Task<IActionResult> GetGameURL(IAmazonS3 client, int GameId)
        {
            AWSConfigsS3.UseSignatureVersion4 = true;
            IAmazonS3 s3Client = new AmazonS3Client(RegionEndpoint.USEast1);
            string bucketName = "shiblauncher";
            string objectName = "";
            var urlString = new List<string>();
            double duration = 12;

            int GameMediaCount = SL_dbContext.SL_GameMedia.Where(x => x.SLGameListId == GameId).Count();
            var GameMedia = SL_dbContext.SL_GameMedia
                            .Where(x => x.SLGameListId == GameId)
                            .Select(x => new
                            {
                                Image = x.Image ?? string.Empty
                            })
                            .ToList();

            for (int i = 0; i <= GameMediaCount - 1; i++)
            {
                objectName = GameMedia[i].Image;

                urlString.Add(GeneratePresignedURL(s3Client, bucketName, objectName, duration));

            }
            var response = SL_dbContext.SL_GameList
                       .Include(x => x.Requirement)
                       .Include(x => x.GameVersion)
                       .Where(x => x.Id == GameId)
                       .Select(game => new
                       {
                           game.Id,
                           game.Title,
                           game.Description,
                           game.Requirement,
                           game.GameVersion,
                           ProfilePicture = game.GameMedia
                                .Where(gm => gm.Id == game.ProfilePictureID)
                                .Select(gm => new
                                {
                                    Image = urlString.FirstOrDefault(url => url.Contains(gm.Image))
                                })
                                .FirstOrDefault(),
                           CoverPicture = game.GameMedia
                                .Where(gm => gm.Id == game.CoverPictureID)
                                .Select(gm => new
                                {
                                    Image = urlString.FirstOrDefault(url => url.Contains(gm.Image))
                                })
                                .FirstOrDefault(),
                           GameMedia = game.GameMedia
                           .Where(gm => gm.Id != game.ProfilePictureID && gm.Id != game.CoverPictureID)
                             .Select(gm => new
                             {
                                 gm.Id,
                                 gm.Image,
                                 gm.Type,
                                 PresignedUrl = urlString.FirstOrDefault(url => url.Contains(gm.Image)) 
                             })
                                 .ToList()
                       })
                        .ToList();


            string jsonResponse = JsonConvert.SerializeObject(response);
            return Content(jsonResponse, "application/json");
        }


        [HttpPost("GETS3URL")]

        public async Task<IActionResult> GetURL(IAmazonS3 client,
                string bucketName,
                string objectName,
                double duration)
        {
            AWSConfigsS3.UseSignatureVersion4 = true;
            IAmazonS3 s3Client = new AmazonS3Client(RegionEndpoint.USEast1);

            string urlString = GeneratePresignedURL(s3Client, bucketName, objectName, duration);
            string jsonResponse = JsonConvert.SerializeObject(urlString);
            return Content(jsonResponse, "application/json");
        }

        public static string GeneratePresignedURL(IAmazonS3 client, string bucketName, string objectKey, double duration)
        {
            string urlString = string.Empty;
            try
            {
                var request = new GetPreSignedUrlRequest()
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    Expires = DateTime.UtcNow.AddHours(duration),
                };
                urlString = client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error:'{ex.Message}'");
            }

            return urlString;
        }
    }


}

