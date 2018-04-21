namespace Book.Views.Samples.Chapter07.Sample01
{
    using System.Reactive.Disposables;
    using System.Windows.Media;
    using global::Splat;
    using ReactiveUI;
    using ViewModels.Samples.Chapter07.Sample01;

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
                            .OneWayBind(this.ViewModel, x => x.Eras, x => x.erasComboBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedEra, x => x.erasComboBox.SelectedItem)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.SelectedEra, x => x.eraRun.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Color, x => x.eraRun.Foreground, x => new SolidColorBrush(x.ToNative()))
                            .DisposeWith(disposables);
                    });
        }
    }
}