namespace Book.ViewModels.Samples.Chapter18.Sample02
{
    using ReactiveUI;

    public sealed class ChildViewModel : ReactiveObject
    {
        private readonly bool useExpensiveResource;

        public ChildViewModel(bool useExpensiveResource)
        {
            this.useExpensiveResource = useExpensiveResource;
        }

        public bool UseExpensiveResource => this.useExpensiveResource;
    }
}