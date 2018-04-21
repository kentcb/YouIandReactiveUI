namespace Book.ViewModels.Data
{
    using Newtonsoft.Json;

    public sealed class Museum
    {
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty("location")]
        public string Location
        {
            get;
            set;
        }
    }
}