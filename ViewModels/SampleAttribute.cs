namespace Book.ViewModels
{
    using System;

    public sealed class SampleAttribute : Attribute
    {
        private readonly string name;
        private readonly string description;

        public SampleAttribute(
            string name,
            string description)
        {
            this.name = name;
            this.description = description;
        }

        public string Name => this.name;

        public string Description => this.description;
    }
}