namespace Book.Views.Samples.Chapter18.Sample03
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter18.Sample03;

    public partial class ChildView : ReactiveUserControl<ChildViewModel>
    {
        public ChildView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.UseExpensiveResource, x => x.nameLabel.Content, x => x ? "This dinosaur is using an expensive resource" : "This dinosaur is NOT using an expensive resource")
                            .DisposeWith(disposables);
                    });
        }
    }
}