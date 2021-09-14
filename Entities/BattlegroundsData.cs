using System.Text.Json.Serialization;

public class BattlegroundsData
{
    [JsonPropertyName("tier")]
    public int Tier { get; set; }

    [JsonPropertyName("hero")]
    public bool Hero { get; set; }

    [JsonPropertyName("upgradeId")]
    public int UpgradeId { get; set; }

    [JsonPropertyName("image")]
    public string Image {get; set; }

    [JsonPropertyName("imageGold")]
    public string ImageGold { get; set; }
}