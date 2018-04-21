namespace Book.ViewModels.Samples.Chapter25.Sample01
{
    public struct Validated<T>
    {
        private readonly T value;
        private readonly string error;

        private Validated(
            T value)
        {
            this.value = value;
            this.error = null;
        }

        private Validated(
            string error)
        {
            this.value = default;
            this.error = error;
        }

        public bool IsValid => this.error == null;

        public T Value => this.value;

        public string Error => this.error;

        public static Validated<T> WithValue(T value) =>
            new Validated<T>(value);

        public static Validated<T> WithError(string error) =>
            new Validated<T>(error);

        // Equality elided for brevity.
    }
}