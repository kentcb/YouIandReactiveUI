namespace Book.ViewModels.Samples.Chapter02.Sample02
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Subjects;
    using ReactiveUI;

    // Just a reactive list that gives us an extra hook to tell us which indices in the list are retrieved.
    public sealed class TweetList : ReactiveList<TweetViewModel>
    {
        private readonly Subject<int> itemRetrieved;

        public TweetList()
            : this(Enumerable.Empty<TweetViewModel>())
        {
        }

        public TweetList(IEnumerable<TweetViewModel> initialTweets)
            : base(initialTweets)
        {
            this.itemRetrieved = new Subject<int>();
        }

        public override TweetViewModel this[int index]
        {
            get
            {
                var item = base[index];
                this.itemRetrieved.OnNext(index);
                return item;
            }
            set => base[index] = value;
        }

        public IObservable<int> ItemRetrieved => this.itemRetrieved;
    }
}