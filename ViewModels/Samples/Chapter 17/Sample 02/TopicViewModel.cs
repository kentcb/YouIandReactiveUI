namespace Book.ViewModels.Samples.Chapter17.Sample02
{
    using System;
    using ReactiveUI;

    public sealed class TopicViewModel
    {
        private readonly string name;
        private readonly Func<IRoutableViewModel> createViewModel;

        public TopicViewModel(
            string name,
            Func<IRoutableViewModel> createViewModel)
        {
            this.name = name;
            this.createViewModel = createViewModel;
        }

        public string Name => this.name;

        public Func<IRoutableViewModel> CreateViewModel => this.createViewModel;
    }
}