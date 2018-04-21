namespace Book.ViewModels.Samples.Chapter05.Sample05
{
    using global::ReactiveUI;

    public sealed class DinosaurViewModel : IReactiveObject
    {
        private AgeViewModel age;
        private string name;
        private int? weight;

        public AgeViewModel Age
        {
            get => this.age;
            set => this.RaiseAndSetIfChanged(ref this.age, value);
        }

        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        public int? Weight
        {
            get => this.weight;
            set => this.RaiseAndSetIfChanged(ref this.weight, value);
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public void RaisePropertyChanged(System.ComponentModel.PropertyChangedEventArgs args) =>
            this.PropertyChanged?.Invoke(this, args);

        public void RaisePropertyChanging(PropertyChangingEventArgs args) =>
            this.PropertyChanging?.Invoke(this, args);
    }
}