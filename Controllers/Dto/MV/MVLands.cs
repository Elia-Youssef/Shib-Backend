using System.Text.Json.Serialization;

public class MVLands
{
    public int id { get; set; }
    public string? _id { get; set; }
    public int? x { get; set; }
    public int? y { get; set; }
    public string? tierName { get; set; }
    public decimal? price { get; set; }
    public bool? NoBillAllowedOnLand { get; set; }
    public string? district { get; set; }
    public bool? isShiboshiZone { get; set; }
    public bool? isRoad { get; set; }
    public bool? reserved { get; set; }
    public string? primaryRoadName { get; set; }
    public string? secondaryRoadName { get; set; }
    public bool? intersection { get; set; }
    public string? hubName { get; set; }

    [JsonPropertyName("id")]
    public long? Land_id { get; set; }

    public string? currentBidWinner { get; set; }
    public string? currentMintWinner { get; set; }
    public List<MVBids>? bids { get; set; }

    public List<MVMints>? mints { get; set; }

    public int? bidCount { get; set; }
    public string? owner { get; set; }
    public bool? minted { get; set; }


}
//public class MVCoordinates
//{
//    public int id {  get; set; }
//    public int? x { get; set; }
//    public int? y { get; set; }
//    public int? FK_land_Id { get; set; }
//}

public class MVBids
{
    public int id { get; set; }
    public bool? confirmed { get; set; }
    public long? landId { get; set; }
    public decimal? bidPrice { get; set; }
    public string? bidBy { get; set; }
    public DateTime? createdAt { get; set; }
    public int? block_number { get; set; }
    public int? FK_land_Id { get; set; }
}

public class MVMints
{
    public int id { get; set; }
    public bool? confirmed { get; set; }
    public long? landId { get; set; }
    public decimal? mintPrice { get; set; }
    public string? mintBy { get; set; }
    public DateTime? createdAt { get; set; }
    public int? block_number { get; set; }
    public int? FK_land_Id { get; set; }
}
public class landResponse
{
    public List<MVLands> Items { get; set; }
}
