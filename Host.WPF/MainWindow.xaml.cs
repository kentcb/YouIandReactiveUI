namespace Book
{
    public partial class MainWindow
    {
        private static MainWindow instance;

        public MainWindow()
        {
            InitializeComponent();

            instance = this;
        }

        public static MainWindow Instance => instance;

        public bool IsScreenshotModeEnabled => this.enableScreenshotModeMenuItem.IsChecked;
    }
}