using Newtonsoft.Json;
using Pdnink_Coremvc.Helpers;
using System.Text.Json.Serialization;

namespace Pdnink_Coremvc.Models
{
    public class ApiResponse
    {
        private string _data;

        [JsonProperty("Success")]
        public bool Success { get; set; }

        [JsonProperty("Data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data
        {
            get => _data;
            set => _data = AesUtils.Encrypt(value);
        }

        [JsonProperty("ClearData", NullValueHandling = NullValueHandling.Ignore)]
        public string ClearData { get; set; }

        [JsonProperty("Mistake", NullValueHandling = NullValueHandling.Ignore)]
        public string Mistake { get; set; }

        [JsonProperty("Items", NullValueHandling = NullValueHandling.Ignore)]
        public int? Items { get; set; }

        public static ApiResponse FromJson(string json) =>
            JsonConvert.DeserializeObject<ApiResponse>(json, Converter.Settings);

        [JsonProperty("AdditionalData", NullValueHandling = NullValueHandling.Ignore)]
        public string AdditionalData { get; set; }

        [JsonProperty("UserName", NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }

        public string  Token { get; set; }

    }
}
