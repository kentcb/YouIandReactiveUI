namespace Book.Views.Samples.Chapter04.Sample03
{
    using System;
    using System.Drawing;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading;
    using global::Splat;
    using ReactiveUI;
    using ViewModels.Samples.Chapter04.Sample03;
    using WPF = System.Windows.Media;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        private readonly MemoizingMRUCache<Color, WPF.Brush> brushes;

        public MainView()
        {
            InitializeComponent();

            // this is our cache of WPF brushes. Notice we're only allowing for 2 brushes to be cached
            this.brushes = new MemoizingMRUCache<Color, WPF.Brush>(
                this.CreateBrush,
                2,
                _ => this.ViewModel.ResourcesReleased += 1);

            this
                .WhenActivated(
                    disposables =>
                    {
                        // make sure cache is invalidated whenever our data resets
                        this
                            .WhenAnyObservable(x => x.ViewModel.Reset)
                            .Do(_ => this.brushes.InvalidateAll())
                            .Subscribe()
                            .DisposeWith(disposables);

                        this
                            .OneWayBind(this.ViewModel, x => x.Dinosaurs, x => x.dinosaursListView.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.ColorKeys, x => x.colorKeyItemsControl.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.IsCachingEnabled, x => x.isCachingEnabledCheckBox.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.IsDataOrderCacheFriendly, x => x.isDataOrderCacheFriendlyCheckBox.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.IsFakeDelayImposed, x => x.isFakeDelayImposedCheckBox.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.ResourcesRequested, x => x.resourcesRequestedRun.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.ResourcesCreated, x => x.resourcesCreatedRun.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.ResourcesReleased, x => x.resourcesReleasedRun.Text)
                            .DisposeWith(disposables);
                    });
        }

        public WPF.Brush GetBrush(Color color)
        {
            this.ViewModel.ResourcesRequested += 1;

            if (this.ViewModel.IsCachingEnabled)
            {
                // caching is enabled, so use our MRU cache
                return this.brushes.Get(color);
            }

            // caching is disabled - always create a brush
            return this.CreateBrush(color);
        }

        private WPF.Brush CreateBrush(Color color, object _ = null)
        {
            this.ViewModel.ResourcesCreated += 1;

            var brush = new WPF.SolidColorBrush(color.ToNative());
            brush.Freeze();

            if (this.ViewModel.IsFakeDelayImposed)
            {
                // fake delay to simulate the creation of an even more expensive resource
                Thread.Sleep(25);
            }

            return brush;
        }
    }
}