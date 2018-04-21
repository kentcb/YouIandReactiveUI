namespace Book.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using ReactiveUI;

    [DebuggerDisplay("Chapter {Number}: {Name}")]
    public abstract class ChapterViewModel : ReactiveObject
    {
        private static readonly Regex chapterNumberRegex = new Regex(".*Chapter(?<number>\\d+).*");
        private static readonly Regex sampleNumberRegex = new Regex(".*Sample(?<number>\\d+).*");

        public abstract string Name
        {
            get;
        }

        public int Number
        {
            get
            {
                var @namespace = this.GetType().Namespace;
                var numberMatch = chapterNumberRegex.Match(@namespace);
                return int.Parse(numberMatch.Groups["number"].Value);
            }
        }

        public string NumberDisplay => Number.ToString("00");

        public IEnumerable<SampleDetailsViewModel> Samples =>
            this
                .GetType()
                .GetTypeInfo()
                .Assembly
                .DefinedTypes
                .Where(typeInfo => typeInfo.Namespace != null && typeInfo.Namespace.StartsWith(this.GetType().Namespace) && typeInfo.GetCustomAttribute<SampleAttribute>() != null)
                .Select(
                    typeInfo =>
                    {
                        var attribute = typeInfo.GetCustomAttribute<SampleAttribute>();
                        var numberMatch = sampleNumberRegex.Match(typeInfo.Namespace);
                        var number = int.Parse(numberMatch.Groups["number"].Value);

                        return new SampleDetailsViewModel(
                            attribute.Name,
                            number,
                            attribute.Description,
                            typeInfo);
                    })
                .OrderBy(sample => sample.Number);
    }
}