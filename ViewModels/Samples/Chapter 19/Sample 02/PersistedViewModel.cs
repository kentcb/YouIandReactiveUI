namespace Book.ViewModels.Samples.Chapter19.Sample02
{
    using System.Runtime.Serialization;
    using ReactiveUI;

    [DataContract]
    public sealed class PersistedViewModel : ReactiveObject
    {
        private string name;
        private string weight;
        private string transient;

        [DataMember]
        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        [DataMember]
        public string Weight
        {
            get => this.weight;
            set => this.RaiseAndSetIfChanged(ref this.weight, value);
        }

        public string Transient
        {
            get => this.transient;
            set => this.RaiseAndSetIfChanged(ref this.transient, value);
        }
    }
}