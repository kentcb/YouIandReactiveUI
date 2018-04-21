namespace Book.ViewModels.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class Names
    {
        private static readonly IList<string> firstNames;
        private static readonly IList<string> lastNames;

        static Names()
        {
            using (var firstNamesStream = typeof(Dinosaurs).GetTypeInfo().Assembly.GetManifestResourceStream("Book.ViewModels.Data.first_names.txt"))
            using (var lastNamesStream = typeof(Dinosaurs).GetTypeInfo().Assembly.GetManifestResourceStream("Book.ViewModels.Data.last_names.txt"))
            using (var firstNamesStreamReader = new StreamReader(firstNamesStream))
            using (var lastNamesStreamReader = new StreamReader(lastNamesStream))
            {
                firstNames = firstNamesStreamReader
                    .ReadToEnd()
                    .Split(new [] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                lastNames = lastNamesStreamReader
                    .ReadToEnd()
                    .Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
            }
        }

        public static IList<string> FirstNames => firstNames;

        public static IList<string> LastNames => lastNames;
    }
}