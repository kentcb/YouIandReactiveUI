namespace Book.Views.Samples.Chapter04.Sample04
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter04.Sample04;

    public partial class DinosaurExhibitionView : ReactiveUserControl<DinosaurExhibitionViewModel>
    {
        public DinosaurExhibitionView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.What, x => x.whatTextBlock.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.WhenDisplay, x => x.whenTextBlock.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Where, x => x.whereTextBlock.Text)
                            .DisposeWith(disposables);
                    });
        }
    }
}