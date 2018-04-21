namespace Book.ViewModels.Samples.Chapter02.Sample01
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Drawing;
    using System.Linq;
    using ReactiveUI;

    public sealed class PaletteEntryViewModel : ReactiveObject
    {
        private static readonly ImmutableArray<TextureViewModel> textures = new string[] { null }
            .Concat(GetTextureResourceNames())
            .Select(resourceName => new TextureViewModel(resourceName))
            .ToImmutableArray();

        private readonly int id;
        private Color selectedColor;
        private TextureViewModel selectedTexture;

        public PaletteEntryViewModel(
            int id,
            Color selectedColor)
        {
            this.id = id;
            this.selectedColor = selectedColor;
            this.selectedTexture = textures.First();
        }

        public int Id => this.id;

        public ImmutableArray<TextureViewModel> Textures => textures;

        public Color SelectedColor
        {
            get => this.selectedColor;
            set => this.RaiseAndSetIfChanged(ref this.selectedColor, value);
        }

        public TextureViewModel SelectedTexture
        {
            get => this.selectedTexture;
            set => this.RaiseAndSetIfChanged(ref this.selectedTexture, value);
        }

        private static IEnumerable<string> GetTextureResourceNames()
        {
            yield return "ReptileSkinTexture";
            yield return "SpeckleTexture";
        }
    }
}