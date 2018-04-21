namespace Book.Views.Samples.Chapter14.Sample06
{
    using System;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using MahApps.Metro.Controls;
    using ReactiveUI;
    using ViewModels.Samples.Chapter14.Sample06;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            TextBoxHelper.SetButtonCommandParameter(this.nameTextBox, Unit.Default);

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .Bind(this.ViewModel, x => x.Name, x => x.nameTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Display, x => x.displayLabel.Content)
                            .DisposeWith(disposables);
                        this
                            .WhenAnyValue(x => x.ViewModel.ClearNameCommand)
                            .Do(clearCommand => TextBoxHelper.SetButtonCommand(this.nameTextBox, clearCommand))
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }
    }
}