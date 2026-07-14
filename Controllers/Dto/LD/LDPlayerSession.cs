
using System.Text.Json.Serialization;

public class LDPlayerSession
{
    public int Id { get; set; }
    public int Player_Id { get; set; }
    public int Game_id { get; set; }
    public int Position { get; set; }
    public int Xp { get; set; }
    public int coin { get; set; }
    public int Deaths { get; set; }
    public TimeSpan Time { get; set; }
    public int Dog_Class { get; set; }
    public int? Total_Distance_Running { get; set; }
    public int? Total_Race_Finishes { get; set; }
    public int? Tumbled_other_shibs { get; set; }
    public int? Pickup_items_Used { get; set; }
}

public class Result
{
    public List<LDPlayerSession> Players { get; set; }

}
