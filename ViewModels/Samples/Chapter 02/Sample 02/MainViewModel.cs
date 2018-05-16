namespace Book.ViewModels.Samples.Chapter02.Sample02
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Threading.Tasks;
    using ReactiveUI;
    using Tweetinvi;
    using Tweetinvi.Models;
    using Tweetinvi.Parameters;

    [Sample(
        "Twitter Client",
        @"This sample is a Twitter client implementation that allows you to search tweets, as well as publish your own.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private const string consumerKey = "FILL_THIS_IN";
        private const string consumerSecret = "FILL_THIS_IN";
        private const string userAccessToken = "FILL_THIS_IN";
        private const string userAccessSecret = "FILL_THIS_IN";

        private readonly ReactiveCommand<Unit, Unit> immediateSearchCommand;
        private readonly ReactiveCommand<Unit, Unit> tweetCommand;
        private readonly ReactiveCommand<byte[], TweetMediaAttachmentViewModel> addAttachmentCommand;
        private readonly ReactiveCommand<TweetMediaAttachmentViewModel, Unit> removeAttachmentCommand;
        private readonly Interaction<CancelViewModel, bool> confirmTweetInteraction;
        private readonly ObservableAsPropertyHelper<TweetList> tweets;
        private readonly ObservableAsPropertyHelper<bool> isLoadingTweets;
        private readonly ObservableAsPropertyHelper<bool> isTweeting;
        private readonly ObservableAsPropertyHelper<int> tweetTextCharactersRemaining;
        private readonly ReactiveList<TweetMediaAttachmentViewModel> attachments;
        private readonly IObservable<string> errors;
        private string query;
        private string tweetText;

        public MainViewModel()
        {
            Auth.SetUserCredentials(
                consumerKey,
                consumerSecret,
                userAccessToken,
                userAccessSecret);

            this.query = "#dinosaurs";
            this.confirmTweetInteraction = new Interaction<CancelViewModel, bool>();
            this.attachments = new ReactiveList<TweetMediaAttachmentViewModel>();

            // TODO: replace all this with SerialReactiveCommand once this is addressed: https://github.com/reactiveui/ReactiveUI/issues/1536
            var cancelSearchCommand = ReactiveCommand.Create(() => { });
            var performSearchCommand = ReactiveCommand.CreateFromObservable<(string query, long? maxId), TweetList>(
                parameters =>
                    GetTweets(parameters.query, parameters.maxId)
                        .Select(initialTweets => new TweetList(initialTweets))
                        .TakeUntil(cancelSearchCommand));

            var searchCommand = ReactiveCommand.CreateFromObservable<string, TweetList>(
                query =>
                    cancelSearchCommand
                        .Execute()
                        .SelectMany(
                            _ =>
                                performSearchCommand
                                    .Execute((query ?? this.Query, null))
                                    .StartWith(new TweetList())));

            this.tweets = searchCommand
                .ToProperty(this, x => x.Tweets, scheduler: RxApp.MainThreadScheduler);

            this.isLoadingTweets = performSearchCommand
                .IsExecuting
                .ToProperty(this, x => x.IsLoadingTweets);

            var throttlePeriod = TimeSpan.FromMilliseconds(500);
            this.immediateSearchCommand = ReactiveCommand.Create(() => { });
            var queryChange = this
                .WhenAnyValue(x => x.Query)
                .Skip(1)
                .Throttle(throttlePeriod);
            var immediateSearch = immediateSearchCommand
                .Select(_ => this.Query);

            Observable
                .Merge(
                    queryChange.Timestamp(),
                    immediateSearch.Timestamp())
                .Scan(
                    (acc, next) =>
                    {
                        if (next.Timestamp > acc.Timestamp)
                        {
                            return next;
                        }

                        return new Timestamped<string>(null, acc.Timestamp);
                    })
                .Select(timestampedQuery => timestampedQuery.Value)
                .Where(query => query != null)
                .StartWith(this.Query)
                .DistinctUntilChanged()
                .InvokeCommand(searchCommand);

            // Monitor tweet scrolling and load the next page of tweets as the user nears the end of the current data set.
            this
                .WhenAnyValue(x => x.Tweets)
                .Select(
                    tweets =>
                        tweets == null ?
                            Observable.Empty<(TweetList tweets, int index)>() :
                            tweets
                                .ItemRetrieved
                                .Select(index => (tweets, index)))
                .Switch()
                .Where(info => info.index >= info.tweets.Count - 10)
                .DistinctUntilChanged(info => info.tweets.Count)
                .SelectMany(
                    info =>
                        performSearchCommand
                            .Execute((this.query, info.tweets.Last().Id))
                            .Select(newTweets => (info.tweets, newTweets)))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(info => this.Tweets.AddRange(info.newTweets))
                .Subscribe();

            var canAddAttachment = this
                .attachments
                .CountChanged
                .Select(count => count < 4)
                .StartWith(true);
            this.addAttachmentCommand = ReactiveCommand.Create<byte[], TweetMediaAttachmentViewModel>(
                imageData =>
                {
                    var attachment = new TweetMediaAttachmentViewModel(imageData);
                    this.attachments.Add(attachment);
                    return attachment;
                },
                canAddAttachment);

            this.removeAttachmentCommand = ReactiveCommand.Create<TweetMediaAttachmentViewModel>(
                attachment => this.attachments.Remove(attachment));

            var canTweet = Observable
                .CombineLatest(
                    this.WhenAnyValue(x => x.TweetText),
                    this.attachments.CountChanged.StartWith(0),
                    (tweetText, attachmentCount) => (!string.IsNullOrWhiteSpace(tweetText) || attachmentCount > 0) && (tweetText?.Length ?? 0) <= 140);

            this.tweetCommand = ReactiveCommand
                .CreateFromObservable(
                    () =>
                        this
                            .confirmTweetInteraction
                            .Handle(new CancelViewModel())
                            .Where(answer => answer)
                            .SelectMany(_ => PublishTweet(this.TweetText, this.Attachments.ToImmutableArray()))
                            .Select(_ => Unit.Default),
                    canTweet);

            this.tweetTextCharactersRemaining = this
                .WhenAnyValue(x => x.TweetText)
                .Select(tweetText => 140 - tweetText?.Length ?? 0)
                .ToProperty(this, x => x.TweetTextCharactersRemaining);

            this.isTweeting = this
                .tweetCommand
                .IsExecuting
                .ToProperty(this, x => x.IsTweeting);

            this
                .tweetCommand
                .Do(
                    _ =>
                    {
                        this.TweetText = null;
                        this.Attachments.Clear();
                    })
                .Subscribe();

            this.errors = Observable
                .Merge(
                    performSearchCommand.ThrownExceptions,
                    searchCommand.ThrownExceptions,
                    this.addAttachmentCommand.ThrownExceptions,
                    this.tweetCommand.ThrownExceptions)
                .Select(exception => exception.Message)
                .DistinctUntilChanged();
        }

        public ReactiveCommand<Unit, Unit> ImmediateSearchCommand => this.immediateSearchCommand;

        public ReactiveCommand<byte[], TweetMediaAttachmentViewModel> AddAttachmentCommand => this.addAttachmentCommand;

        public ReactiveCommand<TweetMediaAttachmentViewModel, Unit> RemoveAttachmentCommand => this.removeAttachmentCommand;

        public ReactiveCommand<Unit, Unit> TweetCommand => this.tweetCommand;

        public Interaction<CancelViewModel, bool> ConfirmTweetInteraction => this.confirmTweetInteraction;

        public int TweetTextCharactersRemaining => this.tweetTextCharactersRemaining.Value;

        public TweetList Tweets => this.tweets.Value;

        public bool IsLoadingTweets => this.isLoadingTweets.Value;

        public bool IsTweeting => this.isTweeting.Value;

        public IObservable<string> Errors => this.errors;

        public string Query
        {
            get => this.query;
            set => this.RaiseAndSetIfChanged(ref this.query, value);
        }

        public string TweetText
        {
            get => this.tweetText;
            set => this.RaiseAndSetIfChanged(ref this.tweetText, value);
        }

        public ReactiveList<TweetMediaAttachmentViewModel> Attachments => this.attachments;

        // Below is code to work with Tweetinvi, the library being used to integrate with Twitter
        //
        // NOTE: Tweetinvi has a *very* funky async model - do NOT use Sync.ExecuteTaskAsync (directly or indirectly) because it does very weird things, including blocking the UI thread.
        private static IObservable<IEnumerable<TweetViewModel>> GetTweets(string query, long? maxId = null)
        {
            ValidateApiKeys();

            if (string.IsNullOrWhiteSpace(query))
            {
                return Observable.Return(Enumerable.Empty<TweetViewModel>());
            }

            var searchParameters = new SearchTweetsParameters(query)
            {
                MaximumNumberOfResults = 100,
                // -1 means get the latest tweets
                MaxId = maxId ?? -1
            };

            return Task
                .Run(() => Search.SearchTweets(searchParameters))
                .ToObservable()
                .Select(tweets => tweets?.Select(tweet => new TweetViewModel(tweet)) ?? Enumerable.Empty<TweetViewModel>());
        }

        private static IObservable<TweetViewModel> PublishTweet(string text, ImmutableArray<TweetMediaAttachmentViewModel> imagesData)
        {
            ValidateApiKeys();

            var uploadImage = Observable.Return<IList<IMedia>>(null);

            if (imagesData.Length > 0)
            {
                uploadImage = Observable
                    .CombineLatest(
                        imagesData
                            .Select(
                                imageData =>
                                    Task
                                        .Run(() => Upload.UploadImage(imageData.ImageData))
                                        .ToObservable()));
            }

            return uploadImage
                .SelectMany(
                    medias =>
                    {
                        var parameters = new PublishTweetOptionalParameters
                        {
                            Medias = medias?.ToList()
                        };

                        return Task
                            .Run(() => Tweet.PublishTweet(text, parameters));
                    })
                .Select(tweet => new TweetViewModel(tweet));
        }

        private static void ValidateApiKeys()
        {
            // This is just used for comparison - don't update this string!
            // Instead, update the constants checked below, like consumerKey and consumerSecret.
            const string unfilled = "FILL_THIS_IN";

            if (consumerKey == unfilled || consumerSecret == unfilled || userAccessToken == unfilled || userAccessSecret == unfilled)
            {
                throw new ApiException();
            }
        }
    }
}