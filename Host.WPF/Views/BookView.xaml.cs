namespace Book.Views
{
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using ReactiveUI;
    using ViewModels;

    public partial class BookView : ReactiveUserControl<BookViewModel>
    {
        public BookView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .Bind(this.ViewModel, x => x.Filter, x => x.filterTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedSampleWithChapter, x => x.samplesListBox.SelectedItem)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.SelectedSampleWithChapter, x => x.advertising.Visibility, x => x == null ? Visibility.Visible : Visibility.Collapsed)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.SelectedSampleWithChapter, x => x.sampleViewModelViewHost.ViewModel)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.FilterCommand, x => x.filterButton)
                            .DisposeWith(disposables);
                        this
                            .filterTextBox
                            .Events()
                            .KeyDown
                            .Select(keyEventArgs => keyEventArgs.Key)
                            .Where(key => key == Key.Enter)
                            .Select(_ => Unit.Default)
                            .InvokeCommand(this.ViewModel, x => x.FilterCommand)
                            .DisposeWith(disposables);
                    });
        }
    }
}