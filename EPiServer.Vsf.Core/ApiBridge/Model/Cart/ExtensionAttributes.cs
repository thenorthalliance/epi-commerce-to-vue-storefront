using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Cart
{
    public class ExtensionAttributes
    {
        [JsonProperty("custom_options")]
        public List<object> CustomOptions { get; set; }

        [JsonProperty("configurable_item_options")]
        public List<ConfigurableItemOption> ConfigurableItemOptions { get; set; }

        [JsonProperty("bundle_options")]
        public List<object> BundleOptions { get; set; }
    }
}