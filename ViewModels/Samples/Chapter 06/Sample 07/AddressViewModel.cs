namespace Book.ViewModels.Samples.Chapter06.Sample07
{
    using ReactiveUI;

    public sealed class AddressViewModel : ReactiveObject
    {
        private string postcode;

        public string Postcode
        {
            get => this.postcode;
            set => this.RaiseAndSetIfChanged(ref this.postcode, value);
        }
    }
}