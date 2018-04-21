namespace Book.Views.Samples.Chapter02.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using Splat;
    using ViewModels.Samples.Chapter02.Sample02;

    public partial class TweetMediaAttachmentView : ReactiveUserControl<TweetMediaAttachmentViewModel>
    {
        public TweetMediaAttachmentView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Bitmap, x => x.image.Source, x => x?.ToNative())
                            .DisposeWith(disposables);
                    });
        }
    }
}