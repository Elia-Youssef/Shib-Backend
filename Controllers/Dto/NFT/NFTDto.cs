using System.Collections.Generic;
using System.Text.Json.Serialization;

public class System_NFT_Item
{
    public int Id { get; set; }

    [JsonPropertyName("animation_url")]
    public string? AnimationUrl { get; set; }

    [JsonPropertyName("external_app_url")]
    public string? ExternalAppUrl { get; set; }

    [JsonPropertyName("NFT_Id")]
    public string? NFT_Id { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("is_unique")]
    public bool? IsUnique { get; set; }

    [JsonPropertyName("metadata")]
    public System_NFT_Metadata? Metadata { get; set; }

    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("token")]
    public System_NFT_Token? Token { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
    public string? SerializedAttributes { get; set; }
    public string? SerializedItems { get; set; }
}
public class System_NFT_Attribute
{
    public int Id { get; set; }

    [JsonPropertyName("display_type")]
    public string? DisplayType { get; set; }

    [JsonPropertyName("trait_type")]
    public string? TraitType { get; set; }

    [JsonPropertyName("value")]
    [JsonConverter(typeof(StringValueConverter))]
    public string? Value { get; set; }
    public int? MetadataId { get; set; }
}

public class System_NFT_Metadata
{
    public int Id { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("external_url")]
    public string? ExternalUrl { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("attributes")]
    public List<System_NFT_Attribute>? Attributes { get; set; }

    public int? FK_NFT_Id { get; set; }
}

public class System_NFT_Token
{
    public int Id { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("circulating_market_cap")]
    public string? CirculatingMarketCap { get; set; }

    [JsonPropertyName("decimals")]
    public string? Decimals { get; set; }

    [JsonPropertyName("exchange_rate")]
    public string? ExchangeRate { get; set; }

    [JsonPropertyName("holders")]
    public string? Holders { get; set; }

    [JsonPropertyName("icon_url")]
    public string? IconUrl { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("total_supply")]
    public string? TotalSupply { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("volume_24h")]
    public string? Volume24h { get; set; }

    public int? FK_NFT_Id { get; set; }
}

public class NFTResponse
{
    [JsonPropertyName("items")]
    public List<System_NFT_Item> Items { get; set; }

    [JsonPropertyName("next_page_params")]
    public string? NextPageParams { get; set; }
}
