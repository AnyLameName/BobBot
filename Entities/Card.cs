using System.Text.Json.Serialization;

class Card
{
    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("battlegrounds")]
    public BattlegroundsData battlegroundsData { get; set; }

    public override string ToString()
    {
        return Slug;
    }
}
