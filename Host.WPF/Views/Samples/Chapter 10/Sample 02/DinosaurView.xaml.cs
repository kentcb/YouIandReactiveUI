namespace Book.Views.Samples.Chapter10.Sample02
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter10.Sample02;

    public partial class DinosaurView : ReactiveUserControl<DinosaurViewModel>
    {
        public DinosaurView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Name, x => x.nameTextBlock.Text)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.FossilCount, x => x.fossilCountNumericUpDown.Value, x => (double?)x, x => (int?)x)
                            .DisposeWith(disposables);
                    });
        }
    }
}