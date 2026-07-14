
public class LDPlayer
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? User_Id { get; set; }
    public int? XP { get; set; }
    public int? Coins { get; set; }
    public int? Level { get; set; }
    public TimeSpan? Total_time_Played { get; set; }
    public TimeSpan? Host_time_Played { get; set; }
    public TimeSpan? Join_time_Played { get; set; }
    public TimeSpan? Custom_time_Played { get; set; }
    public TimeSpan? AI_time_Played { get; set; }
    public TimeSpan? DogClass1_time_Played { get; set; }
    public TimeSpan? DogClass2_time_Played { get; set; }
    public TimeSpan? DogClass3_time_Played { get; set; }
    public TimeSpan? DogClass4_time_Played { get; set; }
    public int? Total_Distance_Running { get; set; }
    public int? Total_Race_Finishes { get; set; }
    public int? Tumbled_other_shibs { get; set; }
    public int? Pickup_items_Used { get; set; }

}

public class LDPlayerStats
{
    public int Id { get; set; }   
    public int? Player_Id { get; set; }
    public int? Game_Finish_Top3 { get; set; }
    public TimeSpan? Time_Played { get; set; }
    public TimeSpan? Best_record { get; set; }
    public int? Deaths_In_Games {  get; set; }

}

public class LDMaps
{
    public int Id { get; set; }
    public string MapTitle { get;set; }
}

public class LDPlayerMapRecord
{
    public int Id { get; set; }
    public int MapId { get; set; }
    public int PlayerId { get; set; }
    public TimeSpan? Time_record { get; set; }
    public int Map_Ranking { get; set; }
    public int Class_Played { get; set; }

}
