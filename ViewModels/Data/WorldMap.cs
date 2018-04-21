namespace Book.ViewModels.Data
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Reflection;
    using Genesis.Ensure;
    using Splat;

    public static class WorldMap
    {
        public static IObservable<IBitmap> GetBitmap() =>
            Observable
                .Using(
                    () => typeof(WorldMap).GetTypeInfo().Assembly.GetManifestResourceStream("Book.ViewModels.Data.Images.world_map.gif"),
                    stream =>
                    {
                        Ensure.ArgumentNotNull(stream, nameof(stream));

                        return BitmapLoader
                            .Current
                            .Load(stream, desiredWidth: null, desiredHeight: null)
                            .ToObservable();
                    });
    }
}