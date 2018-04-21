namespace Book.Views.Samples.Chapter14.Sample15
{
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Controls.Primitives;
    using ReactiveUI;
    using ViewModels.Samples.Chapter14.Sample15;

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
                            .nameTextBox
                            .Events()
                            .TextChanged
                            .Select(_ => Unit.Default)
                            .StartWith(Unit.Default)
                            .Select(_ => string.IsNullOrWhiteSpace(this.nameTextBox.Text) ? "Your dinosaur does not yet have a name." : $"Your dinosaur is named '{this.nameTextBox.Text}'.")
                            .BindTo(this.displayLabel, x => x.Content)
                            .DisposeWith(disposables);
                    });
        }
    }
}