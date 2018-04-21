namespace Book.Views
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using ReactiveUI;
    using ViewModels;

    public partial class TestView : ReactiveUserControl<TestViewModel>
    {
        private static readonly ImageSource executing = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/ExecutingStatus.png"));
        private static readonly ImageSource failure = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/FailureStatus.png"));
        private static readonly ImageSource success = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/SuccessStatus.png"));
        private static readonly ImageSource unknown = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/UnknownStatus.png"));

        public TestView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel, x => x.TestLetter, x => x.letterRun.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Name, x => x.nameRun.Text)
                            .DisposeWith(disposables);
                        this
                            .WhenAnyValue(x => x.ViewModel.Status, x => x.ViewModel.Result, GetImageSourceFor)
                            .Do(resource => this.resultImage.Source = resource)
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }

        private ImageSource GetImageSourceFor(TestStatus status, TestResult result)
        {
            if (status == TestStatus.Executing)
            {
                return executing;
            }

            switch (result)
            {
                case TestResult.Unknown:
                    return unknown;
                case TestResult.Success:
                    return success;
                case TestResult.Failure:
                    return failure;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}