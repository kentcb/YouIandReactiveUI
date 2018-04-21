namespace Book.Behaviors
{
    using System.Windows;
    using System.Windows.Media;

    // allows us to position a FrameworkElement by specifying a Center or TopLeft point
    public static class Position
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.RegisterAttached(
            "Center",
            typeof(Point?),
            typeof(Position),
            new FrameworkPropertyMetadata(CenterChanged));

        public static readonly DependencyProperty TopLeftProperty = DependencyProperty.RegisterAttached(
            "TopLeft",
            typeof(Point?),
            typeof(Position),
            new FrameworkPropertyMetadata(TopLeftChanged));

        public static Point? GetCenter(DependencyObject dependencyObject) =>
            (Point?)dependencyObject.GetValue(CenterProperty);

        public static void SetCenter(DependencyObject dependencyObject, Point? center) =>
            dependencyObject.SetValue(CenterProperty, center);

        public static Point? GetTopLeft(DependencyObject dependencyObject) =>
            (Point?)dependencyObject.GetValue(TopLeftProperty);

        public static void SetTopLeft(DependencyObject dependencyObject, Point? topLeft) =>
            dependencyObject.SetValue(TopLeftProperty, topLeft);

        private static void CenterChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = dependencyObject as FrameworkElement;

            if (frameworkElement == null)
            {
                return;
            }

            UpdateFrameworkElement(frameworkElement);

            frameworkElement.SizeChanged -= OnFrameworkElementSizeChanged;
            frameworkElement.SizeChanged += OnFrameworkElementSizeChanged;
        }

        private static void TopLeftChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = dependencyObject as FrameworkElement;

            if (frameworkElement == null)
            {
                return;
            }

            UpdateFrameworkElement(frameworkElement);
        }

        private static void OnFrameworkElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateFrameworkElement(sender as FrameworkElement);
        }

        private static void UpdateFrameworkElement(FrameworkElement frameworkElement)
        {
            var center = GetCenter(frameworkElement);
            var topLeft = GetTopLeft(frameworkElement);

            if (center == null && topLeft == null)
            {
                frameworkElement.SizeChanged -= OnFrameworkElementSizeChanged;
                frameworkElement.RenderTransform = TranslateTransform.Identity;
                return;
            }

            if (center != null)
            {
                var width = frameworkElement.ActualWidth;
                var height = frameworkElement.ActualHeight;
                var transform = new TranslateTransform(
                    center.Value.X - (width / 2),
                    center.Value.Y - (height / 2));
                frameworkElement.RenderTransform = transform;
            }
            else
            {
                var transform = new TranslateTransform(
                    topLeft.Value.X,
                    topLeft.Value.Y);
                frameworkElement.RenderTransform = transform;
            }
        }
    }
}