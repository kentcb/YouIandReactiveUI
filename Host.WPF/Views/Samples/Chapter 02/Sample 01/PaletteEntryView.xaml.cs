namespace Book.Views.Samples.Chapter02.Sample01
{
    using System;
    using System.Drawing;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using ReactiveUI;
    using Splat;
    using ViewModels.Samples.Chapter02.Sample01;

    public partial class PaletteEntryView : ReactiveUserControl<PaletteEntryViewModel>
    {
        public PaletteEntryView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .WhenAnyObservable(x => x.ViewModel.Changed)
                            .Select(_ => Unit.Default)
                            .StartWith(Unit.Default)
                            .Select(_ => this.ViewModel.ToVisualBrush())
                            .Do(brush => this.swatch.Background = brush)
                            .Subscribe()
                            .DisposeWith(disposables);

                        this
                            .Bind(this.ViewModel, x => x.SelectedColor, x => x.colorColorCanvas.SelectedColor, x => x.ToNative(), x => x?.FromNative() ?? Color.Black)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Textures, x => x.textureListBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedTexture, x => x.textureListBox.SelectedItem)
                            .DisposeWith(disposables);
                    });

        }
    }
}