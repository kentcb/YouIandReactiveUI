namespace Book.Views.Samples.Chapter08.Sample16
{
    using System;
    using System.Reactive.Disposables;
    using System.Windows.Media.Imaging;
    using ReactiveUI;
    using ViewModels.Samples.Chapter08.Sample16;

    public partial class RoarView : ReactiveUserControl<RoarViewModel>
    {
        public RoarView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Image, x => x.image.Source, x => BitmapFrame.Create(new Uri(x)))
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.User, x => x.userNameTextBlock.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Text, x => x.textTextBlock.Text)
                            .DisposeWith(disposables);
                    });
        }
    }
}