namespace Book.Views.Samples.Chapter04.Sample04
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter04.Sample04;

    public partial class ListExhibitionsView : ReactiveUserControl<ListExhibitionsViewModel>
    {
        public ListExhibitionsView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Exhibitions, x => x.exhibitionsListView.ItemsSource)
                            .DisposeWith(disposables);
                    });
        }
    }
}