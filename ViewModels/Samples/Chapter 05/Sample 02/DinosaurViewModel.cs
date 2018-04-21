namespace Book.ViewModels.Samples.Chapter05.Sample02
{
    using global::ReactiveUI;

    public sealed class DinosaurViewModel : ReactiveObject
    {
        private string name;
        private int? weight;

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
    }
}