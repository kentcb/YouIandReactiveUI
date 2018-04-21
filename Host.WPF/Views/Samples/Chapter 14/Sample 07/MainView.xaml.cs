namespace Book.Views.Samples.Chapter14.Sample07
{
    using System;
    using System.Globalization;
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter14.Sample07;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .Bind(
                                this.ViewModel,
                                x => x.Time,
                                x => x.timeTextBox.Text,
                                this.ConvertTimestampToString,
                                this.ConvertStringToTimestamp)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Display, x => x.displayLabel.Content)
                            .DisposeWith(disposables);
                    });
        }

        private string ConvertTimestampToString(Timestamp? timestamp) =>
            timestamp == null ? "" : new DateTime(timestamp.Value.Ticks).ToString("HH:mm:ss");

        private Timestamp? ConvertStringToTimestamp(string input)
        {
            DateTime dateTime;

            if (!DateTime.TryParseExact(
                input,
                "HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateTime))
            {
                return null;
            }

            return new Timestamp(dateTime.TimeOfDay.Ticks);
        }
    }
}