namespace Book.Views.Samples.Chapter09.Sample01
{
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using MahApps.Metro.Controls.Dialogs;
    using ReactiveUI;
    using ViewModels.Samples.Chapter09.Sample01;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .Bind(this.ViewModel, x => x.Name, x => x.nameTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Dinosaurs, x => x.dinosaursListBox.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.SelectedDinosaur, x => x.dinosaursListBox.SelectedItem)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.AddDinosaurCommand, x => x.addDinosaurButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.DeleteDinosaurCommand, x => x.deleteDinosaurButton)
                            .DisposeWith(disposables);

                        this
                            .ViewModel
                            .ConfirmDeleteDinosaur
                            .RegisterHandler(
                                context =>
                                    this
                                        .ShowMessage("Confirm Delete", $"Are you sure you want to delete '{context.Input}'?", MessageDialogStyle.AffirmativeAndNegative)
                                        .Do(result => context.SetOutput(result == MessageDialogResult.Affirmative)))
                            .DisposeWith(disposables);
                    });
        }
    }
}