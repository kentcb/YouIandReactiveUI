namespace Book.Views.Samples.Chapter02.Sample01
{
    using ReactiveUI;
    using Splat;
    using System;
    using System.Reactive.Linq;
    using ViewModels.Samples.Chapter02.Sample01;

    public partial class IllustrationView : ReactiveUserControl<IllustrationViewModel>
    {
        public IllustrationView()
        {
            InitializeComponent();

            this
                .WhenAnyValue(x => x.ViewModel)
                .Where(viewModel => viewModel != null)
                .Do(this.UpdateFromViewModel)
                .Subscribe();
        }

        private void UpdateFromViewModel(IllustrationViewModel viewModel)
        {
            this.nameTextBlock.Text = viewModel.PreviewResourceName;
            viewModel
                .GetBitmap(thumbnail: true)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(bitmap => this.image.Source = bitmap.ToNative())
                .Subscribe();
        }
    }
}