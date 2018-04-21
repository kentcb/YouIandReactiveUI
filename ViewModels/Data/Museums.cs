namespace Book.ViewModels.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;

    public sealed class Museums : List<Museum>
    {
        public static readonly Museums All;

        static Museums()
        {
            using (var stream = typeof(Museums).GetTypeInfo().Assembly.GetManifestResourceStream("Book.ViewModels.Data.museums.json"))
            using (var streamReader = new StreamReader(stream))
            {
                var json = streamReader.ReadToEnd();
                All = JsonConvert.DeserializeObject<Museums>(json);
            }
        }
    }
}