namespace Book.Views.Samples.Chapter02.Sample01
{
    using System;
    using System.Reactive.Linq;
    using ReactiveUI;
    using ViewModels.Samples.Chapter02.Sample01;

    public partial class TextureView : ReactiveUserControl<TextureViewModel>
    {
        public TextureView()
        {
            InitializeComponent();

            this
                .WhenAnyValue(x => x.ViewModel)
                .Where(viewModel => viewModel != null)
                .Do(this.UpdateFromViewModel)
                .Subscribe();
        }

        private void UpdateFromViewModel(TextureViewModel viewModel)
        {
            this.texture.Child = viewModel.ToImage();
        }
    }
}