namespace Book.ViewModels.Data
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Reflection;
    using Genesis.Ensure;
    using Newtonsoft.Json;
    using Splat;

    public sealed class Dinosaur
    {
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty("image_resource_name")]
        public string ImageResourceName
        {
            get;
            set;
        }

        [JsonProperty("era")]
        public Era? Era
        {
            get;
            set;
        }

        [JsonProperty("period")]
        public Period? Period
        {
            get;
            set;
        }

        [JsonProperty("fossil_locations")]
        public IList<PointF> FossilLocations
        {
            get;
            set;
        }

        [JsonProperty("diet")]
        public Diet? Diet
        {
            get;
            set;
        }

        public IObservable<IBitmap> GetBitmap(bool thumbnail = false)
        {
            if (this.ImageResourceName == null)
            {
                return Observable.Return((IBitmap)null);
            }

            var resourceName = "Book.ViewModels.Data.Images." + this.ImageResourceName + (thumbnail ? "_thumb" : "") + ".png";

            return Observable
                .Using(
                    () => this.GetType().GetTypeInfo().Assembly.GetManifestResourceStream(resourceName),
                    stream =>
                    {
                        if (stream == null)
                        {
                            throw new InvalidOperationException($"Unable to find resource named '{resourceName}'.");
                        }

                        return BitmapLoader
                            .Current
                            .Load(stream, desiredWidth: null, desiredHeight: null)
                            .ToObservable();
                    });
        }
    }
}