namespace Book.Views.Samples.Chapter09.Sample04
{
    using System;
    using System.IO;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using MahApps.Metro.Controls.Dialogs;
    using ReactiveUI;
    using ViewModels.Samples.Chapter09.Sample04;

    public partial class ChildView : ReactiveUserControl<ChildViewModel>
    {
        public ChildView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .BindCommand(this.ViewModel, x => x.BookReservationCommand, x => x.bookReservationButton)
                            .DisposeWith(disposables);

                        this
                            .ViewModel
                            .BookReservationCommand
                            .SelectMany(_ => this.ShowMessage("Booking Successful!", ""))
                            .Subscribe()
                            .DisposeWith(disposables);

                        SharedInteractions
                            .UnhandledException
                            .RegisterHandler(
                                context =>
                                {
                                    if (context.Input is IOException)
                                    {
                                        return this
                                            .ShowMessage("Booking Failed", "Check your connection.")
                                            .Do(_ => context.SetOutput(Unit.Default));
                                    }

                                    return Observable.Empty<MessageDialogResult>();
                                })
                            .DisposeWith(disposables);
                    });
        }
    }
}