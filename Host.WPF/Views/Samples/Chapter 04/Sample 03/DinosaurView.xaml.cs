namespace Book.Views.Samples.Chapter04.Sample03
{
    using System.Reactive.Disposables;
    using System.Windows;
    using System.Windows.Media;
    using ReactiveUI;
    using ViewModels.Samples.Chapter04.Sample03;

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
                            .OneWayBind(this.ViewModel, x => x.Color, x => x.periodColorBorder.Background, color => this.FindMainView()?.GetBrush(color))
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Name, x => x.nameTextBlock.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.PeriodDisplay, x => x.periodTextBlock.Text)
                            .DisposeWith(disposables);
                    });
        }

        // this is terrible and you should try not to do this in a real app (only necessary here because the MainView is holding the cache for us)
        private MainView FindMainView()
        {
            var view = VisualTreeHelper.GetParent(this) as FrameworkElement;

            while (view != null && !(view is MainView))
            {
                view = VisualTreeHelper.GetParent(view) as FrameworkElement;
            }

            return view as MainView;
        }
    }
}