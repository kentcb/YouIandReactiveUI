namespace Book.ViewModels.Samples.Chapter09.Sample04
{
    using System;
    using System.Reactive;
    using ReactiveUI;

    public static class SharedInteractions
    {
        public static readonly Interaction<Exception, Unit> UnhandledException = new Interaction<Exception, Unit>();
    }
}