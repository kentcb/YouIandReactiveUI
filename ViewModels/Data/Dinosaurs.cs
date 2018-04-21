namespace Book.ViewModels.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;
    using Serializers;

    public sealed class Dinosaurs : List<Dinosaur>
    {
        public static readonly Dinosaurs All;

        static Dinosaurs()
        {
            using (var stream = typeof(Dinosaurs).GetTypeInfo().Assembly.GetManifestResourceStream("Book.ViewModels.Data.dinosaurs.json"))
            using (var streamReader = new StreamReader(stream))
            {
                var json = streamReader.ReadToEnd();
                var settings = GetJsonSerializerSettings();
                All = JsonConvert.DeserializeObject<Dinosaurs>(json, settings);
            }
        }

        private static JsonSerializerSettings GetJsonSerializerSettings() =>
            new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new PointSerializer()
                }
            };
    }
}