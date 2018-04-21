namespace Book.ViewModels.Samples.Chapter05.Sample01
{
    using global::ReactiveUI;

    public sealed class DinosaurViewModel : ReactiveObject
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

        // if necessary, you can take more control and still enable change notifications as follows
        //public string Name
        //{
        //    get => this.name;
        //    set
        //    {
        //        if (string.Equals(this.name, value))
        //        {
        //            return;
        //        }

        //        this.RaisePropertyChanging();
        //        this.name = value;
        //        this.RaisePropertyChanged();
        //    }
        //}

        public int? Weight
        {
            get => this.weight;
            set => this.RaiseAndSetIfChanged(ref this.weight, value);
        }
    }
}