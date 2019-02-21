using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel.Cart
{
    public class ConfigurableItemOption
    {
        [JsonProperty("option_id")]
        public string OptionId { get; set; }

        [JsonProperty("option_value")]
        public int OptionValue { get; set; }
    }
}