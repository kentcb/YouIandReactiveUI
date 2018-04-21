namespace Book.Views.Samples.Chapter02.Sample01
{
    using System;
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using MahApps.Metro.Controls.Dialogs;
    using ReactiveUI;
    using ViewModels.Samples.Chapter02.Sample01;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this.DataContext = this.ViewModel;

                        this
                            .OneWayBind(this.ViewModel, x => x.Illustrations, x => x.illustrationsListBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedIllustration, x => x.illustrationsListBox.SelectedItem)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.SelectedIllustration, x => x.selectedIllustration.ViewModel)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.SelectedIllustration.Paths, x => x.pathsItemsControl.ItemsSource)
                            .DisposeWith(disposables);

                        this
                            .WhenAnyValue(x => x.ViewModel.SelectedIllustration)
                            .Do(
                                _ =>
                                {
                                    RxApp
                                        .MainThreadScheduler
                                        .Schedule(
                                            () =>
                                            {
                                                var duration = this.zoombox.AnimationDuration;
                                                this.zoombox.AnimationDuration = new Duration(TimeSpan.Zero);
                                                this.zoombox.FitToBounds();
                                                this.zoombox.AnimationDuration = duration;
                                            });
                                })
                            .Subscribe()
                            .DisposeWith(disposables);

                        this
                            .OneWayBind(this.ViewModel, x => x.PaletteEntries, x => x.paletteListBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedPaletteEntry, x => x.paletteListBox.SelectedItem)
                            .DisposeWith(disposables);

                        this
                            .Bind(this.ViewModel, x => x.ShowOutline, x => x.showOutlineCheckBox.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.PaintGroups, x => x.paintGroupsCheckBox.IsChecked)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.ClearCommand, x => x.clearButton)
                            .DisposeWith(disposables);

                        this
                            .WhenAnyValue(x => x.ViewModel.ShowOutline)
                            .Do(this.UpdateShowOutline)
                            .Subscribe()
                            .DisposeWith(disposables);

                        this
                            .ViewModel
                            .ConfirmClearInteraction
                            .RegisterHandler(
                                context =>
                                    this
                                        .ShowMessage("Confirm Clear", $"Are you sure you want to clear the illustration?", MessageDialogStyle.AffirmativeAndNegative)
                                        .Do(result => context.SetOutput(result == MessageDialogResult.Affirmative)))
                            .DisposeWith(disposables);
                    });
        }

        private void UpdateShowOutline(bool showOutline)
        {
            var pathStyle = new Style(typeof(Path));
            pathStyle.Setters.Add(new Setter(Path.StrokeThicknessProperty, showOutline ? 0.25d : 0d));
            pathStyle.Setters.Add(new Setter(Path.StrokeProperty, Brushes.Black));
            pathStyle.Setters.Add(new Setter(Path.FillProperty, Brushes.White));
            pathStyle.Seal();

            var polygonStyle = new Style(typeof(Polygon));
            polygonStyle.Setters.Add(new Setter(Polygon.StrokeThicknessProperty, showOutline ? 0.25d : 0d));
            polygonStyle.Setters.Add(new Setter(Polygon.StrokeProperty, Brushes.Black));
            polygonStyle.Setters.Add(new Setter(Polygon.FillProperty, Brushes.White));
            polygonStyle.Seal();

            this.Resources.Remove(typeof(Path));
            this.Resources.Remove(typeof(Polygon));
            this.Resources.Add(typeof(Path), pathStyle);
            this.Resources.Add(typeof(Polygon), polygonStyle);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            VisualTreeHelper.HitTest(
                this,
                potentialResult => potentialResult is Path ? HitTestFilterBehavior.Continue : HitTestFilterBehavior.ContinueSkipSelf,
                result =>
                {
                    var visual = result.VisualHit;

                    if (visual is Path path && LogicalTreeHelper.GetParent(path) is PathView pathView)
                    {
                        pathView.ViewModel.PaletteEntry = this.ViewModel.SelectedPaletteEntry;
                        return HitTestResultBehavior.Stop;
                    }

                    return HitTestResultBehavior.Continue;
                },
                new PointHitTestParameters(e.GetPosition(this)));
        }
    }
}