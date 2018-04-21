namespace Book.Views.Samples.Chapter02.Sample02
{
    using System;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using ReactiveUI;
    using ViewModels.Samples.Chapter02.Sample02;

    // If you've set up FFME and FFMPEG and you want to try out video support, uncomment all commented code in this file.
    // See instructions at https://github.com/unosquare/ffmediaelement.
    //
    // NOTE: the FFME MediaElement is very buggy, which is why I've elided this support by default.

    public partial class TweetView : ReactiveUserControl<TweetViewModel>
    {
        //private Unosquare.FFME.MediaElement mediaElement;

        //static TweetView()
        //{
        //    // Set this to whatever directory you extracted FFMPEG to
        //    Unosquare.FFME.MediaElement.FFmpegDirectory = @"C:\ffmpeg";
        //}

        public TweetView()
        {
            InitializeComponent();

            this
                .WhenAnyValue(x => x.ViewModel)
                .Where(viewModel => viewModel != null)
                .Do(this.UpdateFromViewModel)
                .Subscribe();

            //this.Unloaded += (s, e) =>
            //{
            //    if (this.mediaElement != null)
            //    {
            //        // The FFME MediaElement is a bit finicky about disposal. We have to do so in a separate message or else we get an exception.
            //        RxApp
            //            .MainThreadScheduler
            //            .Schedule(
            //                () => this.mediaElement.Dispose());
            //    }
            //};
        }

        private void UpdateFromViewModel(TweetViewModel viewModel)
        {
            this.nameRun.Text = viewModel.Name;
            this.screenNameRun.Text = viewModel.ScreenName;
            this.image.ImageSource = new BitmapImage(viewModel.ProfileImageUri);
            this.textTextBlock.Text = viewModel.Text;
            this.mediaPanel.Visibility = viewModel.Media.Length > 0 ? Visibility.Visible : Visibility.Collapsed;
            this.mediaPanel.Children.Clear();

            if (mediaPanel.Visibility == Visibility.Visible)
            {
                foreach (var media in viewModel.Media)
                {
                    switch (media.Type)
                    {
                        case TweetMediaType.Image:
                            mediaPanel.Children.Add(CreateImageFor(media));
                            break;
                        //case TweetMediaType.Video:
                        //    this.mediaElement = CreateMediaElement();
                        //    UpdateMediaElementFor(this.mediaElement, media);
                        //    mediaPanel.Children.Add(this.mediaElement);
                        //    break;
                    }
                }
            }
        }

        private static Image CreateImageFor(TweetMediaViewModel viewModel) =>
            new Image
            {
                Source = new BitmapImage(viewModel.Uri),
                Width = DetermineMediaWidth(viewModel, 200),
                Height = DetermineMediaHeight(viewModel, 200),
                Stretch = Stretch.Uniform
            };

        //private static Unosquare.FFME.MediaElement CreateMediaElement()
        //{
        //    var mediaElement = new Unosquare.FFME.MediaElement
        //    {
        //        Stretch = Stretch.Uniform,
        //        LoadedBehavior = MediaState.Play,
        //        UnloadedBehavior = MediaState.Close,
        //        Volume = 0
        //    };

        //    mediaElement.MediaFailed += (s, e) =>
        //    {
        //        Console.WriteLine("Failed to open media: " + e.ErrorException);
        //    };
        //    mediaElement.MediaOpened += (s, e) =>
        //    {
        //        Console.WriteLine("Media opened.");
        //    };

        //    return mediaElement;
        //}

        //private static void UpdateMediaElementFor(Unosquare.FFME.MediaElement mediaElement, TweetMediaViewModel viewModel)
        //{
        //    mediaElement.Source = viewModel.Uri;
        //    mediaElement.Width = DetermineMediaWidth(viewModel, 300);
        //    mediaElement.Height = DetermineMediaHeight(viewModel, 300);
        //}

        private static double DetermineMediaWidth(TweetMediaViewModel viewModel, double maximumWidth) =>
            viewModel.AspectRatio >= 1 ? maximumWidth : double.NaN;

        private static double DetermineMediaHeight(TweetMediaViewModel viewModel, double maximumHeight) =>
            viewModel.AspectRatio < 1 ? maximumHeight : double.NaN;
    }
}