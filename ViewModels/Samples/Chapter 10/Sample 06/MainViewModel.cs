namespace Book.ViewModels.Samples.Chapter10.Sample06
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "CreateDerivedList: complex",
        @"This sample is a 'live' display of votes for people's favorite dinosaurs. The user can sort and filter the data. 'New' votes are highlighted briefly in green. Finally, there is a tally of votes per dinosaur on the right, which is also dependent upon the current filter.")]
    public sealed class MainViewModel : ReactiveObject, ISupportsActivation
    {
        private static readonly Random random = new Random();
        private readonly ViewModelActivator activator;
        private readonly ReactiveCommand<OrderBy, Unit> orderByCommand;
        private readonly IReactiveList<Vote> voteModels;
        private readonly IReactiveDerivedList<VoteViewModel> votes;
        private readonly ObservableAsPropertyHelper<IList<VoteCountViewModel>> voteCounts;
        private string filter;
        private OrderBy orderBy;
        private OrderDirection orderDirection;

        public MainViewModel()
        {
            this.activator = new ViewModelActivator();
            this.voteModels = new ReactiveList<Vote>(
                Enumerable
                    .Range(0, 50)
                    .Select(_ => this.CreateRandomVote(DateTime.MinValue)));

            this.orderByCommand = ReactiveCommand.Create<OrderBy>(
                this.ApplyOrderBy,
                outputScheduler: RxApp.MainThreadScheduler);

            var reset = this
                .WhenAnyValue(
                    x => x.OrderBy,
                    x => x.OrderDirection,
                    x => x.Filter,
                    (_, __, ___) => Unit.Default)
                .Throttle(TimeSpan.FromMilliseconds(500), RxApp.MainThreadScheduler);
            this.votes = this
                .voteModels
                .CreateDerivedCollection(
                    selector: vote => new VoteViewModel(vote),
                    filter: OnFilter,
                    orderer: GetOrder,
                    signalReset: reset);

            this.voteCounts = this
                .votes
                .Changed
                .Select(_ => Unit.Default)
                .StartWith(Unit.Default)
                .Select(
                    _ =>
                        this
                            .votes
                            .GroupBy(x => x.DinosaurName)
                            .Select(groupedVotes => new VoteCountViewModel(groupedVotes.Key, groupedVotes.Count()))
                            .OrderByDescending(voteCount => voteCount.Count)
                            .Take(10)
                            .ToList())
                .ToProperty(this, x => x.VoteCounts);

            this
                .WhenActivated(
                    disposables =>
                    {
                        Observable
                            .Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                            .Do(_ => MakeRandomChange())
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }

        public ViewModelActivator Activator => this.activator;

        public ReactiveCommand<OrderBy, Unit> OrderByCommand => this.orderByCommand;

        public IReactiveDerivedList<VoteViewModel> Votes => this.votes;

        public IList<VoteCountViewModel> VoteCounts => this.voteCounts.Value;

        public string Filter
        {
            get => this.filter;
            set => this.RaiseAndSetIfChanged(ref this.filter, value);
        }

        public OrderBy OrderBy
        {
            get => this.orderBy;
            set => this.RaiseAndSetIfChanged(ref this.orderBy, value);
        }

        public OrderDirection OrderDirection
        {
            get => this.orderDirection;
            set => this.RaiseAndSetIfChanged(ref this.orderDirection, value);
        }

        private bool OnFilter(Vote vote)
        {
            var lowerCaseFilter = this.filter?.ToLower();

            return string.IsNullOrEmpty(lowerCaseFilter)
                || vote.Voter.ToLower().Contains(lowerCaseFilter)
                || vote.Dinosaur.Name.ToLower().Contains(lowerCaseFilter)
                || vote.Dinosaur.Diet.ToString().ToLower().Contains(lowerCaseFilter);
        }

        private void ApplyOrderBy(OrderBy orderBy)
        {
            var direction = OrderDirection.Ascending;

            if (this.OrderBy == orderBy)
            {
                direction = this.OrderDirection == OrderDirection.Ascending ? OrderDirection.Descending : OrderDirection.Ascending;
            }

            this.OrderBy = orderBy;
            this.OrderDirection = direction;
        }

        private int GetOrder(VoteViewModel left, VoteViewModel right)
        {
            var leftData = GetSortDataFor(left);
            var rightData = GetSortDataFor(right);

            return Comparer<object>.Default.Compare(leftData, rightData) * (this.OrderDirection == OrderDirection.Ascending ? 1 : -1);
        }

        private object GetSortDataFor(VoteViewModel vote)
        {
            switch (this.OrderBy)
            {
                case OrderBy.Name:
                    return vote.Name;
                case OrderBy.DinosaurName:
                    return vote.DinosaurName;
                case OrderBy.DinosaurDiet:
                    return vote.DinosaurDiet;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void MakeRandomChange()
        {
            var add = random.Next(2) == 1;

            if (add || this.voteModels.Count == 0)
            {
                this.voteModels.Add(this.CreateRandomVote());
            }
            else
            {
                var index = random.Next(0, this.voteModels.Count);
                this.voteModels.RemoveAt(index);
            }
        }

        private Vote CreateRandomVote(DateTime? timestamp = null)
        {
            var firstName = Data
                .Names
                .FirstNames[random.Next(0, Data.Names.FirstNames.Count)];
            var lastName = Data
                .Names
                .LastNames[random.Next(0, Data.Names.LastNames.Count)];
            var dinosaur = Data
                .Dinosaurs
                .All[random.Next(0, Data.Dinosaurs.All.Count)];
            return new Vote(firstName + " " + lastName, dinosaur, timestamp);
        }
    }
}