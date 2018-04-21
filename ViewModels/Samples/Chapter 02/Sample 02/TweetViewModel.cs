namespace Book.ViewModels.Samples.Chapter02.Sample02
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using Tweetinvi.Models;

    public sealed class TweetViewModel
    {
        private readonly long id;
        private readonly string name;
        private readonly string screenName;
        private readonly string text;
        private readonly Uri profileImageUri;
        private readonly ImmutableArray<TweetMediaViewModel> media;

        public TweetViewModel(ITweet model)
        {
            this.id = model.Id;
            this.name = model.CreatedBy.Name;
            this.screenName = "@" + model.CreatedBy.ScreenName;
            this.text = model.FullText;
            this.profileImageUri = new Uri(model.CreatedBy.ProfileImageUrl);
            this.media = model
                .Media
                .Select(media => new TweetMediaViewModel(media))
                .ToImmutableArray();
        }

        public long Id => this.id;

        public string Name => this.name;

        public string ScreenName => this.screenName;

        public string Text => this.text;

        public Uri ProfileImageUri => this.profileImageUri;

        public ImmutableArray<TweetMediaViewModel> Media => this.media;
    }
}