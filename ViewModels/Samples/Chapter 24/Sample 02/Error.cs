namespace Book.ViewModels.Samples.Chapter24.Sample02
{
    using System;
    using System.Windows.Input;

    public sealed class Error
    {
        private readonly Exception exception;
        private readonly ICommand retryCommand;

        public Error(
            Exception exception,
            ICommand retryCommand)
        {
            this.exception = exception;
            this.retryCommand = retryCommand;
        }

        public Exception Exception => this.exception;

        public ICommand RetryCommand => this.retryCommand;
    }
}