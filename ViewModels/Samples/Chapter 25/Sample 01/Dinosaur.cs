namespace Book.ViewModels.Samples.Chapter25.Sample01
{
    public sealed class Dinosaur
    {
        private readonly string name;
        private readonly int weight;
        private readonly byte[] image;

        public Dinosaur(
            string name,
            int weight,
            byte[] image)
        {
            this.name = name;
            this.weight = weight;
            this.image = image;
        }

        public string Name => this.name;

        public int Weight => this.weight;

        public byte[] Image => this.image;
    }
}