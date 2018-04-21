namespace Book.ViewModels.Samples.Chapter04.Sample05
{
    using System;
    using System.Reactive.Subjects;
    using global::Splat;

    // an implementation of Splat's ILogger interface that forwards all messages to an observable
    public sealed class ObservableLogger : ILogger
    {
        public static readonly ObservableLogger Instance = new ObservableLogger();
        private readonly ISubject<(LogLevel level, string message), (LogLevel level, string message)> messages;

        private ObservableLogger()
        {
            this.messages = Subject.Synchronize(new ReplaySubject<(LogLevel level, string message)>(100));
        }

        public IObservable<(LogLevel level, string message)> Messages => this.messages;

        public LogLevel Level
        {
            get;
            set;
        }

        public void Write(string message, LogLevel logLevel)
        {
            if (logLevel < this.Level)
            {
                return;
            }

            this.messages.OnNext((logLevel, message));
        }
    }
}