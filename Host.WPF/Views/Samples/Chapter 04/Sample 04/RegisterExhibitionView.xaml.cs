namespace Book.Views.Samples.Chapter04.Sample04
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter04.Sample04;

    public partial class RegisterExhibitionView : ReactiveUserControl<RegisterExhibitionViewModel>
    {
        public RegisterExhibitionView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .Bind(this.ViewModel, x => x.What, x => x.whatTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.WhenEntry, x => x.whenTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.Where, x => x.whereTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.RegisterCommand, x => x.registerButton)
                            .DisposeWith(disposables);
                    });
        }
    }
}