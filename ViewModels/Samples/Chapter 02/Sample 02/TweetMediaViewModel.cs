namespace Book.ViewModels.Samples.Chapter02.Sample02
{
    using System;
    using Tweetinvi.Models.Entities;

    public sealed class TweetMediaViewModel
    {
        private readonly long id;
        private readonly TweetMediaType type;
        private readonly Uri uri;
        private readonly double aspectRatio;

        public TweetMediaViewModel(IMediaEntity model)
        {
            this.id = model.Id.Value;
            this.type = model.VideoDetails == null ? TweetMediaType.Image : TweetMediaType.Video;

            if (this.type == TweetMediaType.Video)
            {
                // Video support has been elided in the view layer for these reasons:
                //
                // 1. Amazingly, WPF's MediaElement does not support HTTPS.
                //    See https://connect.microsoft.com/VisualStudio/feedback/details/934355/in-a-wpf-standalone-application-exe-when-the-source-of-a-mediaelement-is-set-to-a-https-uri-it-throws-a-nullreferenceexception.
                // 2. Changing the URI to HTTP does not help. The MediaElement fails to load the video, giving an obscure "Exception from HRESULT: 0xC00D11B1".
                //    Presumably this is because the HTTPS redirects to HTTP, since the videos play fine in Windows Media Player.
                // 3. I considered dropping in https://github.com/unosquare/ffmediaelement as an alternative to MediaElement.
                //    However, the licensing situation is unclear and there were too many bugs related to disposal/memory management.
                //
                // That said, I have included all the data you need here and left stub code in TweetView.xaml.cs. Feel free to drop in the above control to get videos working.

                this.uri = new Uri(model.VideoDetails.Variants[0].URL);
                this.aspectRatio = model.VideoDetails.AspectRatio[0] / model.VideoDetails.AspectRatio[1];
            }
            else
            {
                this.uri = new Uri(model.MediaURLHttps);
                var size = model.Sizes["large"];
                this.aspectRatio = size.Width ?? 1d / size.Height ?? 1d;
            }
        }

        public long Id => this.id;

        public TweetMediaType Type => this.type;

        public Uri Uri => this.uri;

        public double AspectRatio => this.aspectRatio;
    }
}