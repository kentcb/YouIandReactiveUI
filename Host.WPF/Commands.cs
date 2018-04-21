namespace Book
{
    using System.Collections;
    using System.Windows.Input;

    public static class Commands
    {
        public static readonly RoutedCommand ScreenshotSample = new RoutedCommand(
            nameof(ScreenshotSample),
            typeof(Commands),
            new InputGestureCollection(
                new ArrayList
                {
                    new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Alt)
                }));
    }
}