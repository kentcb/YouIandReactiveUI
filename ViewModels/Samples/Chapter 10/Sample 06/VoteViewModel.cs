namespace Book.ViewModels.Samples.Chapter10.Sample06
{
    using System;
    using System.Reactive.Linq;
    using Data;
    using ReactiveUI;

    public sealed class VoteViewModel : ReactiveObject
    {
        private static readonly TimeSpan newPeriod = TimeSpan.FromSeconds(1.5);

        private readonly string name;
        private readonly Dinosaur dinosaur;
        private readonly ObservableAsPropertyHelper<bool> isNew;

        public VoteViewModel(
            Vote vote)
        {
            this.name = vote.Voter;
            this.dinosaur = vote.Dinosaur;

            var age = DateTime.UtcNow - vote.Timestamp;

            if (age > newPeriod)
            {
                this.isNew = Observable
                    .Return(false)
                    .ToProperty(this, x => x.IsNew);
            }
            else
            {
                var delta = newPeriod - age;
                this.isNew = Observable
                    .Return(false)
                    .Delay(delta, RxApp.MainThreadScheduler)
                    .StartWith(true)
                    .ToProperty(this, x => x.IsNew);
            }
        }

        public string Name => this.name;

        public string DinosaurName => this.dinosaur.Name;

        public string DinosaurDiet => this.dinosaur.Diet.ToString();

        public bool IsNew => this.isNew.Value;
    }
}