using Newtonsoft.Json;
namespace Heist;

public class Asset
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("token_id")]
    public int TokenId { get; set; }

    [JsonProperty("image_url")]
    public string? ImageUrl { get; set; }
    
    [JsonProperty("image_original_url")]
    public string? ImageOriginalUrl { get; set; }

    public Asset(int id, int tokenId, string? imageUrl, string? imageOriginalUrl)
    {
        Id = id;
        TokenId = tokenId;
        ImageUrl = imageUrl;
        ImageOriginalUrl = imageOriginalUrl;
    }
}