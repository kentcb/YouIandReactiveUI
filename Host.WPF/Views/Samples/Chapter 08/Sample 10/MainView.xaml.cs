namespace Book.Views.Samples.Chapter08.Sample10
{
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;
    using global::Splat;
    using ReactiveUI;
    using ViewModels.Samples.Chapter08.Sample10;

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
                            .OneWayBind(this.ViewModel, x => x.Zoom, x => x.scaleTransform.ScaleX)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Zoom, x => x.scaleTransform.ScaleY)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Image, x => x.image.Source, x => x?.ToNative())
                            .DisposeWith(disposables);

                        var doubleClick = this
                            .Events()
                            .MouseDoubleClick
                            .Publish()
                            .RefCount();

                        doubleClick
                            .Where(x => x.ChangedButton == MouseButton.Left)
                            .Select(_ => Unit.Default)
                            .InvokeCommand(this.ViewModel.ZoomInCommand)
                            .DisposeWith(disposables);

                        doubleClick
                            .Where(x => x.ChangedButton == MouseButton.Right)
                            .Select(_ => Unit.Default)
                            .InvokeCommand(this.ViewModel.ZoomOutCommand)
                            .DisposeWith(disposables);
                    });
        }
    }
}