namespace Book.Views
{
    using Behaviors;
    using CommonMark.Syntax;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using ViewModels;
    using WPF = System.Windows.Documents;

    public partial class SampleHostView : ReactiveUserControl<SampleWithChapterViewModel>
    {
        public SampleHostView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .WhenAnyValue(x => x.ViewModel.Chapter.NumberDisplay, x => x.ViewModel.Sample.NumberDisplay, (chapterNumberDisplay, sampleNumberDisplay) => chapterNumberDisplay + "." + sampleNumberDisplay)
                            .BindTo(this, x => x.numberRun.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Sample.Name, x => x.nameRun.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Sample.SampleType, x => x.openViewSourceButton.Visibility, x => x == SampleType.UI ? Visibility.Visible : Visibility.Collapsed)
                            .DisposeWith(disposables);
                        this
                            .WhenAnyValue(x => x.ViewModel.Sample.SampleDescriptionDocument)
                            .Select(this.GetInlineCollectionFor)
                            .Do(
                                inlines =>
                                {
                                    this.descriptionTextBlock.Inlines.Clear();
                                    this.descriptionTextBlock.Inlines.AddRange(inlines);
                                })
                            .Subscribe()
                            .DisposeWith(disposables);

                        this
                            .WhenAnyValue(x => x.ViewModel.Sample)
                            .Select(sampleDetails => sampleDetails.GetMainViewModel())
                            .Do(
                                _ =>
                                {
                                    this.sampleBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
                                    this.sampleBorder.VerticalAlignment = VerticalAlignment.Stretch;
                                    this.sampleBorder.ClearValue(WidthProperty);
                                    this.sampleBorder.ClearValue(HeightProperty);
                                    this.sampleBorder.BorderThickness = new Thickness(0);
                                })
                            .BindTo(this, x => x.sampleViewModelViewHost.ViewModel)
                            .DisposeWith(disposables);

                        this
                            .sampleViewModelViewHost
                            .WhenAnyValue(x => x.Content)
                            .Where(content => content != null)
                            .Do(
                                content =>
                                {
                                    if (MainWindow.Instance.IsScreenshotModeEnabled)
                                    {
                                        var width = Screenshot.GetWidth((DependencyObject)content);
                                        var height = Screenshot.GetHeight((DependencyObject)content);

                                        if (width != null && height != null)
                                        {
                                            this.sampleBorder.HorizontalAlignment = HorizontalAlignment.Center;
                                            this.sampleBorder.VerticalAlignment = VerticalAlignment.Center;
                                            this.sampleBorder.Width = width.Value;
                                            this.sampleBorder.Height = height.Value;
                                            this.sampleBorder.BorderThickness = new Thickness(1);
                                        }
                                    }
                                })
                            .Subscribe()
                            .DisposeWith(disposables);

                        this
                            .openViewSourceButton
                            .Events()
                            .Click
                            .SelectMany(_ => this.OpenFolder(this.ViewModel.GetViewsPathFromSolution))
                            .Subscribe()
                            .DisposeWith(disposables);

                        this
                            .openViewModelSourceButton
                            .Events()
                            .Click
                            .SelectMany(_ => this.OpenFolder(this.ViewModel.GetViewModelsPathFromSolution))
                            .Subscribe()
                            .DisposeWith(disposables);

                        this
                            .viewBookInfoButton
                            .Events()
                            .Click
                            .Do(_ => this.ViewModel.SelectSample(((int,int)?)null))
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }

        private IObservable<Unit> OpenFolder(Func<string, string> getPathFromSolutionRoot)
        {
            var solutionRoot = GetSolutionRoot();

            if (solutionRoot == null)
            {
                return this
                    .ShowMessage("Error", "Unable to find solution root.")
                    .Select(_ => Unit.Default);
            }

            var path = getPathFromSolutionRoot(solutionRoot);

            Process.Start(path);
            return Observable.Empty<Unit>();
        }

        private string GetSolutionRoot()
        {
            var directory = new DirectoryInfo(Environment.CurrentDirectory);

            while (directory != null)
            {
                if (directory.GetFiles("Book.sln").Length > 0)
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }

            return null;
        }

        // only supports the small subset of markdown that we actually care about for the purposes of this application
        private IEnumerable<WPF.Inline> GetInlineCollectionFor(Block document)
        {
            var firstParagraph = true;
            var inlines = new Stack<List<WPF.Inline>>();
            inlines.Push(new List<WPF.Inline>());

            foreach (var node in document.AsEnumerable())
            {
                if (node.Block != null)
                {
                    var block = node.Block;

                    switch (block.Tag)
                    {
                        case BlockTag.Paragraph:
                            if (node.IsOpening)
                            {
                                if (firstParagraph)
                                {
                                    firstParagraph = false;
                                }
                                else
                                {
                                    // HACK: see http://stackoverflow.com/a/38866716/5380 for details
                                    inlines.Peek().Add(new WPF.LineBreak());
                                    var span = new WPF.Span();
                                    span.FontSize = 6;
                                    span.Inlines.Add(new WPF.Run("\t"));
                                    span.Inlines.Add(new WPF.LineBreak());
                                    inlines.Peek().Add(span);
                                }
                            }
                            break;
                    }
                }
                else if (node.Inline != null)
                {
                    var inline = node.Inline;

                    switch (inline.Tag)
                    {
                        case InlineTag.String:
                            inlines.Peek().Add(new WPF.Run(inline.LiteralContent));
                            break;
                        case InlineTag.Code:
                            inlines.Peek().Add(new WPF.Run(inline.LiteralContent)
                            {
                                Style = (Style)this.FindResource("CodeRunStyle")
                            });
                            break;
                        case InlineTag.Emphasis:
                            if (node.IsOpening)
                            {
                                inlines.Push(new List<WPF.Inline>());
                            }
                            else
                            {
                                var emphasised = inlines.Pop();
                                var span = new WPF.Span
                                {
                                    Style = (Style)this.FindResource("EmphasisSpanStyle")
                                };
                                span.Inlines.AddRange(emphasised);
                                inlines.Peek().Add(span);
                            }
                            break;
                        case InlineTag.Strong:
                            if (node.IsOpening)
                            {
                                inlines.Push(new List<WPF.Inline>());
                            }
                            else
                            {
                                var emphasised = inlines.Pop();
                                var span = new WPF.Span
                                {
                                    Style = (Style)this.FindResource("StrongSpanStyle")
                                };
                                span.Inlines.AddRange(emphasised);
                                inlines.Peek().Add(span);
                            }
                            break;
                        case InlineTag.Link:
                            if (node.IsOpening)
                            {
                                inlines.Push(new List<WPF.Inline>());
                            }
                            else
                            {
                                WPF.Hyperlink hyperlink;

                                if (Uri.TryCreate(inline.TargetUrl, UriKind.Absolute, out var uri))
                                {
                                    hyperlink = new WPF.Hyperlink
                                    {
                                        NavigateUri = new Uri(inline.TargetUrl, UriKind.Absolute)
                                    };
                                }
                                else
                                {
                                    hyperlink = new WPF.Hyperlink
                                    {
                                        Command = ReactiveCommand.Create(() => this.ViewModel.SelectSample(inline.TargetUrl))
                                    };
                                }

                                hyperlink.Inlines.AddRange(inlines.Pop());
                                inlines.Peek().Add(hyperlink);
                            }
                            break;
                    }
                }
            }

            if (inlines.Count != 1)
            {
                throw new InvalidOperationException("Ended up with more than the single root inlines list.");
            }

            return inlines.Pop();
        }

        private void OnScreenshotSample(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.Instance.IsScreenshotModeEnabled)
            {
                MessageBox.Show("Enable screenshot mode first.", "Not Supported", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (this.sampleViewModelViewHost.Content == null)
            {
                return;
            }

            var notSupported = Screenshot.GetNotSupported((DependencyObject)this.sampleViewModelViewHost.Content);

            if (notSupported != null)
            {
                MessageBox.Show(notSupported, "Not Supported for this Sample", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var renderTargetBitmap = new RenderTargetBitmap(
                (int)this.sampleViewModelViewHost.ActualWidth,
                (int)this.sampleViewModelViewHost.ActualHeight,
                96,
                96,
                PixelFormats.Pbgra32);
            renderTargetBitmap.Render((Visual)this.sampleViewModelViewHost.Content);

            var visual = new DrawingVisual();

            using (var drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(
                    Brushes.White,
                    null,
                    new Rect(0, 0, renderTargetBitmap.Width, renderTargetBitmap.Height));
                drawingContext.DrawImage(
                    renderTargetBitmap,
                    new Rect(
                        0,
                        0,
                        renderTargetBitmap.PixelWidth,
                        renderTargetBitmap.PixelHeight));
            }

            renderTargetBitmap.Render(visual);
            Clipboard.SetImage(renderTargetBitmap);

            e.Handled = true;
        }
    }
}