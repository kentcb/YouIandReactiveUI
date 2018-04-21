namespace Book.Views
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels;

    public partial class ChapterView : ReactiveUserControl<ChapterViewModel>
    {
        public ChapterView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Number, x => x.chapterNumberRun.Text, x => x.ToString("00"))
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Name, x => x.chapterNameRun.Text)
                            .DisposeWith(disposables);
                    });
        }
    }
}