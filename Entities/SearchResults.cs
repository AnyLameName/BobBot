using System.Text.Json.Serialization;
using System.Collections.Generic;

class SearchResults
{
    [JsonPropertyName("cards")]
    public List<Card> Cards { get; set; }

    [JsonPropertyName("cardCount")]
    public int CardCount { get; set; }

    [JsonPropertyName("pageCount")]
    public int PageCount { get; set; }

    [JsonPropertyName("page")]
    public int PageNum { get; set; }
}
