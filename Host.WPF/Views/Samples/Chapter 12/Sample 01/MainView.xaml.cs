namespace Book.Views.Samples.Chapter12.Sample01
{
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Controls.Primitives;
    using ReactiveUI;
    using ViewModels.Samples.Chapter12.Sample01;

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
                            .Bind(this.ViewModel, x => x.Description, x => x.descriptionTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.FieldTip, x => x.tipLabel.Content)
                            .DisposeWith(disposables);

                        var nameEvents = this.nameTextBox.Events();
                        var descriptionEvents = this.descriptionTextBox.Events();
                        var keyboardField = Observable
                            .Merge(
                                nameEvents
                                    .GotFocus
                                    .Select(_ => Field.Name),
                                nameEvents
                                    .LostFocus
                                    .Select(_ => Field.None),
                                descriptionEvents
                                    .GotFocus
                                    .Select(_ => Field.Description),
                                descriptionEvents
                                    .LostFocus
                                    .Select(_ => Field.None))
                            .StartWith(Field.None);

                        var mouseField = Observable
                            .Merge(
                                nameEvents
                                    .MouseEnter
                                    .Select(_ => Field.Name),
                                nameEvents
                                    .MouseLeave
                                    .Select(_ => Field.None),
                                descriptionEvents
                                    .MouseEnter
                                    .Select(_ => Field.Description),
                                descriptionEvents
                                    .MouseLeave
                                    .Select(_ => Field.None))
                            .StartWith(Field.None);

                        Observable
                            .CombineLatest(
                                keyboardField,
                                mouseField,
                                (k, m) => k == Field.None ? m : k)
                            .BindTo(this.ViewModel, x => x.FocusedField)
                            .DisposeWith(disposables);
                    });
        }
    }
}