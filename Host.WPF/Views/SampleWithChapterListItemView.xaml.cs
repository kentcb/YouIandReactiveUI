namespace Book.Views
{
    using System;
    using System.Reactive.Disposables;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using ReactiveUI;
    using ViewModels;

    public partial class SampleWithChapterListItemView : ReactiveUserControl<SampleWithChapterViewModel>
    {
        private static readonly ImageSource console = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/ConsoleSample.png"));
        private static readonly ImageSource performance = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/PerformanceSample.png"));
        private static readonly ImageSource test = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/TestSample.png"));
        private static readonly ImageSource ui = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/UISample.png"));

        public SampleWithChapterListItemView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .WhenAnyValue(x => x.ViewModel.Chapter.Number, x => x.ViewModel.Sample.Number, (chapterNumber, sampleNumber) => $"{chapterNumber:00}.{sampleNumber:00}")
                            .BindTo(this, x => x.numberRun.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Sample.Name, x => x.nameRun.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Sample.SampleType, x => x.sampleImage.Source, GetImageSourceFor)
                            .DisposeWith(disposables);
                    });
        }

        private static ImageSource GetImageSourceFor(SampleType sampleType)
        {
            switch (sampleType)
            {
                case SampleType.Console:
                    return console;
                case SampleType.Performance:
                    return performance;
                case SampleType.Test:
                    return test;
                case SampleType.UI:
                    return ui;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}