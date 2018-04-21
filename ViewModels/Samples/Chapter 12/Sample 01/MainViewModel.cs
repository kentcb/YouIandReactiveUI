namespace Book.ViewModels.Samples.Chapter12.Sample01
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using ReactiveUI;

    [Sample(
        "Events",
        @"This sample demonstrates the use of ReactiveUI's events package to easily obtain an observable representing an event.

In this example, the important code is in the view. It uses the `Events` extension method to gain access to observables representing events in a control. Specifically, it ensures that the focused field is known by the view model so that it can choose an appropriate tip for that field. In addition, if a field is not focused the user can also point at one with the mouse to view the same tip.")]
    public sealed class MainViewModel : ReactiveObject
    {
        private string name;
        private string description;
        private Field focusedField;
        private readonly ObservableAsPropertyHelper<string> fieldTip;

        public MainViewModel()
        {
            this.fieldTip = this
                .WhenAnyValue(x => x.FocusedField)
                .Select(DetermineFieldTip)
                .ToProperty(this, x => x.FieldTip);
        }

        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        public string Description
        {
            get => this.description;
            set => this.RaiseAndSetIfChanged(ref this.description, value);
        }

        public Field FocusedField
        {
            get => this.focusedField;
            set => this.RaiseAndSetIfChanged(ref this.focusedField, value);
        }

        public string FieldTip => this.fieldTip.Value;

        private static string DetermineFieldTip(Field field)
        {
            if (field == Field.None)
            {
                return null;
            }

            switch (field)
            {
                case Field.Name:
                    return "Enter the name of your imaginary dinosaur. Be creative!";
                case Field.Description:
                    return "Enter a description of your imaginary dinosaur.";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}