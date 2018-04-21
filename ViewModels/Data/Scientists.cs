namespace Book.ViewModels.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;

    public sealed class Scientists : List<Scientist>
    {
        public static readonly Scientists All;

        static Scientists()
        {
            using (var stream = typeof(Scientists).GetTypeInfo().Assembly.GetManifestResourceStream("Book.ViewModels.Data.scientists.json"))
            using (var streamReader = new StreamReader(stream))
            {
                var json = streamReader.ReadToEnd();
                All = JsonConvert.DeserializeObject<Scientists>(json);
            }
        }
    }
}