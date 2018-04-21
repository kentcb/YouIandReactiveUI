namespace Book.Views.Samples.Chapter02.Sample01
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Book.ViewModels.Samples.Chapter02.Sample01;
    using Splat;

    public static class Extensions
    {
        private static readonly Dictionary<int, VisualBrush> brushCache = new Dictionary<int, VisualBrush>();

        public static VisualBrush ToVisualBrush(this PaletteEntryViewModel @this)
        {
            if (!brushCache.TryGetValue(@this.Id, out var brush))
            {
                brush = new VisualBrush
                {
                    TileMode = TileMode.Tile,
                    Viewport = new Rect(0, 0, 32, 32),
                    ViewportUnits = BrushMappingMode.Absolute
                };
            }

            brush.Visual = @this.ToSwatch();
            brushCache[@this.Id] = brush;
            return brush;
        }

        public static UIElement ToSwatch(this PaletteEntryViewModel @this) =>
            new Border
            {
                Background = new SolidColorBrush(@this.SelectedColor.ToNative()),
                Width = 32,
                Height = 32,
                Child = @this.SelectedTexture.ToImage()
            };

        public static Image ToImage(this TextureViewModel @this)
        {
            if (@this.ResourceName == null)
            {
                return null;
            }

            var brush = new VisualBrush
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, 32, 32),
                ViewportUnits = BrushMappingMode.Absolute
            };

            var textureImage = new BitmapImage();
            textureImage.BeginInit();
            textureImage.UriSource = new Uri($"pack://application:,,,/Resources/{@this.ResourceName}.png");
            textureImage.EndInit();

            return new Image
            {
                Source = textureImage
            };
        }
    }
}