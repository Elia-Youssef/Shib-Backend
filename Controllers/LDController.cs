using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ShibAPI.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class LDController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly LD_ApplicationDbContext _dbContext;

        public LDController(IHttpClientFactory httpClientFactory, LD_ApplicationDbContext dbContext)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }


        [HttpPost("SIGNUP")]
        public async Task<IActionResult> CreatePlayer(int userId, string name)
        {
            if (userId == 0)
            {
                return BadRequest("Invalid player data.");
            }
            if (name == null)
            {
                return BadRequest("Please fill out the name");
            }

            var LDPlayerData = new LDPlayer
            {
                Name = name,
                User_Id = userId,
                XP = 0,
                Coins = 0,
                Level = 0,
                Total_time_Played = TimeSpan.Zero
            };

            _dbContext.LD_Player.Add(LDPlayerData);
            await _dbContext.SaveChangesAsync();
            return Content("Success");

        }


        [HttpPost("HOST")]
        public async Task<IActionResult> CreateGame(int MapId, int LapNumber)
        {
            if (MapId == 0)
            {
                return BadRequest("Invalid Map data.");
            }
            if (LapNumber == 0)
            {
                return BadRequest("Please fill out the Lap Number");
            }

            var LDGameSessionData = new LDGameSession
            {
                Created_At = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),//DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), ("MM/dd/yyyy"),CultureInfo.InvariantCulture),
                Map_Id = MapId,
                Nbr_of_laps = LapNumber
            };

            _dbContext.LD_GameSession.Add(LDGameSessionData);
            await _dbContext.SaveChangesAsync();
            return Content("Success");

        }


        [HttpPost("START")]
        /*Json template 
          {
              "players": [
                {
                  "player_Id": 1,
                  "game_id": 1,
                  "dog_Class": 1
                },
             {
                  "player_Id": 2,
                  "game_id": 1,
                  "dog_Class": 2
                }
              ]
          }
}
        */
        public async Task<IActionResult> GameStart([FromBody] Result Requests)
        {

            foreach (var Request in Requests.Players)
            {
                var LDPlayerSessionData = new LDPlayerSession
                {
                    Player_Id = Request.Player_Id,
                    Game_id = Request.Game_id,
                    Position = 0,
                    Time = TimeSpan.Zero,
                    coin = 0,
                    Xp = 0,
                    Dog_Class = Request.Dog_Class,
                    Total_Distance_Running = 0,
                    Total_Race_Finishes = 0,
                    Tumbled_other_shibs = 0,
                    Pickup_items_Used = 0

                };

                var LDPlayerStatData = new LDPlayerStats
                {
                    Player_Id = Request.Player_Id,
                    Game_Finish_Top3 = 0,
                    Time_Played = TimeSpan.Zero,
                    Best_record = TimeSpan.Zero,
                    Deaths_In_Games = 0
                };


                _dbContext.LD_PlayerSession.Add(LDPlayerSessionData);
                _dbContext.LD_PlayerStats.Add(LDPlayerStatData);
                await _dbContext.SaveChangesAsync();
            }
            return Content("Success");

        }


        [HttpPost("ENDGAME")]

        public async Task<IActionResult> GameFinish([FromBody] Result Requests)
        {

            foreach (var Request in Requests.Players)
            {

                _dbContext.LD_PlayerSession
                .Where(u => u.Player_Id == Request.Player_Id && u.Game_id == Request.Game_id)
                .ExecuteUpdate(b => b
                    .SetProperty(u => u.Position, Request.Position)
                    .SetProperty(u => u.Time, Request.Time)
                    .SetProperty(u => u.coin, Request.coin)
                    .SetProperty(u => u.Xp, Request.Xp)
                    .SetProperty(u => u.Total_Distance_Running, u => u.Total_Distance_Running + Request.Total_Distance_Running)
                    .SetProperty(u => u.Total_Race_Finishes, u => u.Total_Race_Finishes + Request.Total_Race_Finishes)
                    .SetProperty(u => u.Tumbled_other_shibs, u => u.Tumbled_other_shibs + Request.Tumbled_other_shibs)
                    .SetProperty(u => u.Pickup_items_Used, u => u.Pickup_items_Used + Request.Pickup_items_Used)
                    );


                var BestTime = _dbContext.LD_PlayerStats
                   .Where(x => x.Id == Request.Player_Id)
                   .Select(x => new
                   {
                       BestTime = x.Best_record,
                   })
                  .FirstOrDefault();


                _dbContext.LD_PlayerStats
                .Where(x => x.Id == Request.Player_Id)
                .ExecuteUpdate(y => y
                    .SetProperty(x => x.Time_Played, x => x.Time_Played + Request.Time)
                    .SetProperty(x => x.Best_record, x => (BestTime != null && BestTime.BestTime != TimeSpan.Zero && BestTime.BestTime < Request.Time) ? BestTime.BestTime : Request.Time)
                    .SetProperty(x => x.Game_Finish_Top3, x => x.Game_Finish_Top3 + (Request.Position <= 3 ? 1 : 0))
                    .SetProperty(x => x.Deaths_In_Games, x => x.Deaths_In_Games + Request.Deaths)
    );
                await _dbContext.SaveChangesAsync();

                var MapID = _dbContext.LD_GameSession
                  .Where(x => x.Id == Request.Game_id)
                  .Select(x => x.Map_Id) 
                  .FirstOrDefault();


                var LDPlayerMapRecordData = new LDPlayerMapRecord
                {
                    MapId = MapID,
                    PlayerId = Request.Player_Id,
                    Time_record = Request.Time,
                    Map_Ranking = Request.Position,
                    Class_Played = Request.Dog_Class
                };
            }
            return Content("Success");

        }

    }

}

