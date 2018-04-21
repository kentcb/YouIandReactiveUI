namespace Book.ViewModels
{
    using System.Runtime.Serialization;
    using Book.ViewModels.Samples.Chapter19.Sample02;

    [DataContract]
    public sealed class AppState
    {
        [DataMember]
        public string Filter
        {
            get;
            set;
        }

        [DataMember]
        public string DinosaurName
        {
            get;
            set;
        }

        [DataMember]
        public PersistedViewModel PersistedViewModel
        {
            get;
            set;
        }
    }
}