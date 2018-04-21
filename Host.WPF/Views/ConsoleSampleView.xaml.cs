namespace Book.Views
{
    using System;
    using System.Reactive.Disposables;
    using System.Windows;
    using ReactiveUI;
    using ViewModels;

    public partial class ConsoleSampleView : ReactiveUserControl<ConsoleSampleViewModel>
    {
        public ConsoleSampleView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.Output, x => x.outputTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.ClearCommand, x => x.clearButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.ExecuteCommand, x => x.executeButton)
                            .DisposeWith(disposables);

                        this.ViewModel.Activated(disposables);

                        Console.SetOut(this.ViewModel.OutputSink);
                        Console.SetError(this.ViewModel.OutputSink);
                    });
        }

        private void OnCopySample(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.outputTextBox.Text);
            e.Handled = true;
        }
    }
}