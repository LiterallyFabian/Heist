using Newtonsoft.Json;

namespace Heist;

public class AssetSet
{
    public AssetSet(Asset[] assets)
    {
        Assets = assets;
    }

    [JsonProperty("assets")]
    public Asset[] Assets { get; set; }
}