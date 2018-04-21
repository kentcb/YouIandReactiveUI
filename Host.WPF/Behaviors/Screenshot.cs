namespace Book.Behaviors
{
    using System.Windows;

    public static class Screenshot
    {
        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
            "Width",
            typeof(int?),
            typeof(Screenshot));

        public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached(
            "Height",
            typeof(int?),
            typeof(Screenshot));

        public static readonly DependencyProperty NotSupportedProperty = DependencyProperty.RegisterAttached(
            "NotSupported",
            typeof(string),
            typeof(Screenshot));

        public static int? GetWidth(DependencyObject dependencyObject) =>
            (int?)dependencyObject.GetValue(WidthProperty);

        public static int? GetHeight(DependencyObject dependencyObject) =>
            (int?)dependencyObject.GetValue(HeightProperty);

        public static string GetNotSupported(DependencyObject dependencyObject) =>
            (string)dependencyObject.GetValue(NotSupportedProperty);

        public static void SetWidth(DependencyObject dependencyObject, int? width) =>
            dependencyObject.SetValue(WidthProperty, width);

        public static void SetHeight(DependencyObject dependencyObject, int? height) =>
            dependencyObject.SetValue(HeightProperty, height);

        public static void SetNotSupported(DependencyObject dependencyObject, string notSupported) =>
            dependencyObject.SetValue(NotSupportedProperty, notSupported);
    }
}