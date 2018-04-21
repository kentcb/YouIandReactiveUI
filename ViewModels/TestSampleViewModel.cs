namespace Book.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reflection;
    using ReactiveUI;
    using Xunit;

    public abstract class TestSampleViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, Unit> executeAllTestsCommand;
        private readonly ReactiveCommand<Unit, TestResult> executeSelectedTestCommand;
        private readonly IList<TestViewModel> tests;
        private TestViewModel selectedTest;

        protected TestSampleViewModel()
        {
            this.tests = this
                .GetType()
                .GetTypeInfo()
                .DeclaredMethods
                .Where(method => method.IsPublic && !method.IsStatic && method.GetCustomAttribute<FactAttribute>() != null)
                .Select((method, index) => new TestViewModel(method, this, index))
                .ToList();

            this.executeAllTestsCommand = ReactiveCommand.CreateFromObservable(
                () => Observable.Concat(this.tests.Select(test => test.ExecuteCommand.Execute())).Select(_ => Unit.Default));

            var canExecuteSelectedTest = this
                .WhenAnyValue(x => x.SelectedTest)
                .Select(selectedTest => selectedTest != null);
            this.executeSelectedTestCommand = ReactiveCommand.CreateFromObservable(
                () => this.selectedTest.ExecuteCommand.Execute(),
                canExecuteSelectedTest);
        }

        public ReactiveCommand<Unit, Unit> ExecuteAllTestsCommand => this.executeAllTestsCommand;

        public ReactiveCommand<Unit, TestResult> ExecuteSelectedTestCommand => this.executeSelectedTestCommand;

        public IList<TestViewModel> Tests => this.tests;

        public TestViewModel SelectedTest
        {
            get => this.selectedTest;
            set => this.RaiseAndSetIfChanged(ref this.selectedTest, value);
        }
    }
}