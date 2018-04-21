namespace Book.ViewModels.Samples.Chapter17.Sample02
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using ReactiveUI;

    public sealed class TopicsViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly IScreen hostScreen;
        private readonly IList<TopicViewModel> topics;
        private TopicViewModel selectedTopic;

        public TopicsViewModel(
            IScreen hostScreen)
        {
            this.hostScreen = hostScreen;
            this.topics = new List<TopicViewModel>
            {
                new TopicViewModel("Scientists", () => new ScientistsViewModel(this.hostScreen)),
                new TopicViewModel("Museums", () => new MuseumsViewModel(this.hostScreen)),
                new TopicViewModel("Dinosaurs", () => new DinosaursViewModel(this.hostScreen))
            };

            this
                .WhenAnyValue(x => x.SelectedTopic)
                .Where(selectedTopic => selectedTopic != null)
                .Do(_ => this.SelectedTopic = null)
                .SelectMany(selectedTopic => this.hostScreen.Router.Navigate.Execute(selectedTopic.CreateViewModel()))
                .Subscribe();
        }

        public string UrlPathSegment => "Topics";

        public IScreen HostScreen => this.hostScreen;

        public IList<TopicViewModel> Topics => this.topics;

        public TopicViewModel SelectedTopic
        {
            get => this.selectedTopic;
            set => this.RaiseAndSetIfChanged(ref this.selectedTopic, value);
        }
    }
}