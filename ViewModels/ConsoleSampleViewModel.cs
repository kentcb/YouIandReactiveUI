namespace Book.ViewModels
{
    using System;
    using System.IO;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Text;
    using ReactiveUI;

    public abstract class ConsoleSampleViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<Unit, Unit> executeCommand;
        private readonly ReactiveCommand<Unit, Unit> clearCommand;
        private readonly OutputSinkImpl outputSink;
        private readonly ObservableAsPropertyHelper<string> output;

        protected ConsoleSampleViewModel()
        {
            this.executeCommand = ReactiveCommand.CreateFromObservable(() => this.Execute(), outputScheduler: RxApp.MainThreadScheduler);
            this.clearCommand = ReactiveCommand.Create(() => { }, outputScheduler: RxApp.MainThreadScheduler);
            this.outputSink = new OutputSinkImpl();
            this.output = this
                .clearCommand
                .Select(_ => Unit.Default)
                .StartWith(Unit.Default)
                .Select(_ => this
                    .outputSink
                    .Lines
                    .Scan(new StringBuilder(), (sb, next) => sb.AppendLine(next))
                    .Select(x => x.ToString())
                    .StartWith(""))
                .Switch()
                .ToProperty(this, x => x.Output, scheduler: RxApp.MainThreadScheduler);
        }

        public ReactiveCommand<Unit, Unit> ExecuteCommand => this.executeCommand;

        public ReactiveCommand<Unit, Unit> ClearCommand => this.clearCommand;

        public string Output => this.output.Value;

        public TextWriter OutputSink => this.outputSink;

        public virtual void Activated(CompositeDisposable disposables) =>
            this.ActivatedCore(disposables);

        protected abstract IObservable<Unit> Execute();

        protected virtual void ActivatedCore(CompositeDisposable disposables)
        {
        }

        protected void WriteLine(string message)
        {
            this.outputSink.WriteLine(message);
        }

        protected void WriteLine(string message, params object[] args)
        {
            this.outputSink.WriteLine(message, args);
        }

        private sealed class OutputSinkImpl : TextWriter
        {
            private readonly StringBuilder buffer;
            private readonly Subject<string> lines;

            public OutputSinkImpl()
            {
                this.buffer = new StringBuilder();
                this.lines = new Subject<string>();
            }

            public Subject<string> Lines => this.lines;

            public override Encoding Encoding => Encoding.UTF8;

            public override void WriteLine(string value)
            {
                this.buffer.Append(value);
                this.WriteLine();
            }

            public override void WriteLine()
            {
                this.lines.OnNext(this.buffer.ToString());
                this.buffer.Length = 0;
            }

            public override void Write(char value) =>
                this.buffer.Append(value);
        }
    }
}