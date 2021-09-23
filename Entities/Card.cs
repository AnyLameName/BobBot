using System.Text.Json.Serialization;
using System.Collections.Generic;

class Card
{
    /* TODO: Card Type comes back as in int from the API, but there is a metadata endpoint:
     * https://us.api.blizzard.com/hearthstone/metadata?locale=en_US
     * This can tell us which types are for minion, hero, hero power, etc.
     * Cheating for now: 4 is minon, 3 is hero, 10 is hero power, 5 is spell (coin, map).
     */

    [JsonPropertyName("cardTypeId")]
    public int CardType { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set;}

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("battlegrounds")]
    public BattlegroundsData BgData { get; set; }

    [JsonPropertyName("childIds")]
    public List<int> Children { get; set; }

    public bool IsUpgradedMinion { 
        get
        {
            return (BgData.UpgradeId == 0 && CardType == 4);
        }
    }

    public override string ToString()
    {
        return Name;
    }
}
