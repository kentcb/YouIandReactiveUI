namespace Book.Views.Samples.Chapter02.Sample01
{
    using System;
    using System.Reactive.Linq;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using ReactiveUI;
    using ViewModels.Samples.Chapter02.Sample01;

    public partial class PathView : ReactiveUserControl<PathViewModel>
    {
        private static readonly Brush highlightBrush = new SolidColorBrush(Colors.Red);

        public PathView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                     disposables =>
                     {
                         this.UpdateFromViewModel(this.ViewModel);

                         this
                            .WhenAnyValue(x => x.ViewModel.PaletteEntry)
                            .Do(
                                paletteEntry =>
                                {
                                    var brush = paletteEntry?.ToVisualBrush();

                                    if (brush == null)
                                    {
                                        this.path.ClearValue(Path.FillProperty);
                                    }
                                    else
                                    {
                                        this.path.Fill = brush;
                                    }
                                })
                            .Subscribe();

                         this
                            .WhenAnyValue(x => x.ViewModel.Highlight)
                            .Do(
                                highlight =>
                                {
                                    var brush = highlight ? highlightBrush : null;

                                    if (brush == null)
                                    {
                                        this.path.ClearValue(Path.StrokeProperty);
                                        this.path.ClearValue(Path.StrokeThicknessProperty);
                                    }
                                    else
                                    {
                                        this.path.Stroke = brush;
                                        this.path.StrokeThickness = 1;
                                    }
                                })
                            .Subscribe();
                     });
        }

        private void UpdateFromViewModel(PathViewModel viewModel)
        {
            this.path.Data = Geometry.Parse(viewModel.Data);

            if (viewModel.Clip != null)
            {
                this.path.Clip = Geometry.Parse(viewModel.Clip);
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.ViewModel.Highlight = true;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.ViewModel.Highlight = false;
        }
    }
}