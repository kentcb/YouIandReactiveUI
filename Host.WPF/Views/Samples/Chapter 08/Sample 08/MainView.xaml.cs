namespace Book.Views.Samples.Chapter08.Sample08
{
    using System.Reactive.Disposables;
    using ReactiveUI;
    using ViewModels.Samples.Chapter08.Sample08;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .Bind(this.ViewModel, x => x.Text, x => x.textTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.IsConfirmed, x => x.confirmCheckBox.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.RemoveErasCommand, x => x.removeErasButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.RemovePeriodsCommand, x => x.removePeriodsButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.RemoveBothCommand, x => x.removeBothButton)
                            .DisposeWith(disposables);
                    });
        }
    }
}