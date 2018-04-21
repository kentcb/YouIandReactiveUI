namespace Book.ViewModels.Data
{
    using Newtonsoft.Json;

    public sealed class Scientist
    {
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty("bio")]
        public string Bio
        {
            get;
            set;
        }

        [JsonProperty("image_uri")]
        public string ImageUri
        {
            get;
            set;
        }
    }
}