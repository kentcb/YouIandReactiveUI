namespace Book.ViewModels.Samples.Chapter24.Sample03
{
    using Genesis.TestUtil;

    public sealed class DinosaurBuilder : IBuilder
    {
        private string name;
        private int weight;
        private byte[] image;

        public DinosaurBuilder()
        {
            this.name = "Barney";
            this.weight = 42;
            this.image = new byte[] { 1, 2, 3 };
        }

        public DinosaurBuilder WithName(string name) =>
            this.With(ref this.name, name);

        public DinosaurBuilder WithWeight(int weight) =>
            this.With(ref this.weight, weight);

        public DinosaurBuilder WithImage(params byte[] image) =>
            this.With(ref this.image, image);

        public Dinosaur Build() =>
            new Dinosaur(
                this.name,
                this.weight,
                this.image);

        public static implicit operator Dinosaur(DinosaurBuilder builder) =>
            builder.Build();
    }
}