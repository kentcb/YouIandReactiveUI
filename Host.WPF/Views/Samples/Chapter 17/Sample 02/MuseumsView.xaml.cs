namespace Book.Views.Samples.Chapter17.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter17.Sample02;

    public partial class MuseumsView : ReactiveUserControl<MuseumsViewModel>
    {
        public MuseumsView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Museums, x => x.museumsListBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedMuseum, x => x.museumsListBox.SelectedItem)
                            .DisposeWith(disposables);
                    });
        }
    }
}