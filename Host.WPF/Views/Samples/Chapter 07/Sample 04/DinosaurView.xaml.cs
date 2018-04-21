namespace Book.Views.Samples.Chapter07.Sample04
{
    using System.Reactive.Disposables;
    using System.Windows.Media;
    using global::Splat;
    using ReactiveUI;
    using ViewModels.Samples.Chapter07.Sample04;

    public partial class DinosaurView : ReactiveUserControl<DinosaurViewModel>
    {
        public DinosaurView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Periods, x => x.periodComboBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Name, x => x.nameTextBlock.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Eras, x => x.eraComboBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Color, x => x.periodColorBorder.Background, color => new SolidColorBrush(color.ToNative()))
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedEra, x => x.eraComboBox.SelectedItem)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedPeriod, x => x.periodComboBox.SelectedItem)
                            .DisposeWith(disposables);
                    });
        }
    }
}