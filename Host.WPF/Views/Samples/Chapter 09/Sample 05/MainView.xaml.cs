namespace Book.Views.Samples.Chapter09.Sample05
{
    using System;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using MahApps.Metro.Controls.Dialogs;
    using ReactiveUI;
    using ViewModels.Samples.Chapter09.Sample05;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        private bool? rememberedResult;

        public MainView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .BindCommand(this.ViewModel, x => x.Draw1Command, x => x.draw1Button)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.Draw10Command, x => x.draw10Button)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.HandDisplay, x => x.handLabel.Content)
                            .DisposeWith(disposables);

                        this
                            .ViewModel
                            .DrawAnotherInteraction
                            .RegisterHandler(
                                context =>
                                {
                                    if (this.rememberedResult.HasValue)
                                    {
                                        context.SetOutput(this.rememberedResult.Value);
                                        return Observable.Empty<bool>();
                                    }

                                    return this
                                        .ShowMessage(
                                            "Draw another card?",
                                            null,
                                            MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary,
                                            new MetroDialogSettings
                                            {
                                                AffirmativeButtonText = "Yes",
                                                FirstAuxiliaryButtonText = "Yes to All",
                                                NegativeButtonText = "No",
                                                SecondAuxiliaryButtonText = "No to All"
                                            })
                                        .Select(
                                            result =>
                                            {
                                                var accepted = result == MessageDialogResult.Affirmative || result == MessageDialogResult.FirstAuxiliary;
                                                var toAll = result == MessageDialogResult.FirstAuxiliary || result == MessageDialogResult.SecondAuxiliary;

                                                if (toAll)
                                                {
                                                    this.rememberedResult = accepted;
                                                }

                                                return accepted;
                                            })
                                        .Do(draw => context.SetOutput(draw));
                                })
                            .DisposeWith(disposables);

                        Observable
                            .Merge(
                                this.ViewModel.Draw1Command,
                                this.ViewModel.Draw10Command)
                            .Do(_ => this.rememberedResult = null)
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }
    }
}