namespace Book.Views.Samples.Chapter02.Sample02
{
    using System;
    using System.IO;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using ReactiveUI;
    using ToastNotifications;
    using ToastNotifications.Lifetime;
    using ToastNotifications.Messages;
    using ToastNotifications.Position;
    using ViewModels.Samples.Chapter02.Sample02;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this.DataContext = this.ViewModel;

                        this
                            .Bind(this.ViewModel, x => x.Query, x => x.queryTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Tweets, x => x.tweetsItemsControl.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.IsLoadingTweets, x => x.progressRing.IsActive)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.IsTweeting, x => x.tweetTextTextBox.IsEnabled, isTweeting => !isTweeting)
                            .DisposeWith(disposables);
                        this
                            .Bind(this.ViewModel, x => x.TweetText, x => x.tweetTextTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.TweetTextCharactersRemaining, x => x.tweetTextCharactersRemainingLabel.Content, GetMessageForCharactersRemaining)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.TweetTextCharactersRemaining, x => x.tweetTextCharactersRemainingLabel.Foreground, GetForegroundForCharactersRemaining)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Attachments, x => x.attachmentsItemsControl.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.TweetCommand, x => x.tweetButton)
                            .DisposeWith(disposables);

                        this
                            .ViewModel
                            .ConfirmTweetInteraction
                            .RegisterHandler(
                                context =>
                                {
                                    cancelView.ViewModel = context.Input;

                                    var cancel = this
                                        .cancelView
                                        .Cancel
                                        .Do(
                                            _ =>
                                            {
                                                context.SetOutput(false);
                                                cancelView.ViewModel = null;
                                            })
                                        .Select(_ => Unit.Default);

                                    var timedOut = context
                                        .Input
                                        .WhenAnyValue(x => x.TimeRemaining)
                                        .Where(timeRemaining => timeRemaining == 0)
                                        .FirstAsync()
                                        .Do(
                                            _ =>
                                            {
                                                context.SetOutput(true);
                                                cancelView.ViewModel = null;
                                            })
                                        .Select(_ => Unit.Default);

                                    return Observable
                                        .Merge(
                                            cancel,
                                            timedOut);
                                })
                            .DisposeWith(disposables);

                        var notifier = CreateNotifier()
                            .DisposeWith(disposables);

                        this
                            .ViewModel
                            .TweetCommand
                            .Do(_ => notifier.ShowInformation("Tweet published!"))
                            .Subscribe();

                        this
                            .queryTextBox
                            .Events()
                            .KeyDown
                            .Select(keyEventArgs => keyEventArgs.Key)
                            .Where(key => key == Key.Enter)
                            .Select(_ => Unit.Default)
                            .InvokeCommand(this.ViewModel.ImmediateSearchCommand)
                            .DisposeWith(disposables);

                        CommandManager.AddPreviewCanExecuteHandler(this.tweetTextTextBox, this.OnTweetTextBoxPreviewCanExecute);
                        CommandManager.AddPreviewExecutedHandler(this.tweetTextTextBox, this.OnTweetTextBoxPreviewExecuted);

                        Disposable
                            .Create(
                                () =>
                                {
                                    CommandManager.RemovePreviewCanExecuteHandler(this.tweetTextTextBox, this.OnTweetTextBoxPreviewCanExecute);
                                    CommandManager.RemovePreviewExecutedHandler(this.tweetTextTextBox, this.OnTweetTextBoxPreviewExecuted);
                                })
                            .DisposeWith(disposables);

                        this
                            .ViewModel
                            .Errors
                            .SelectMany(error => this.ShowMessage("Sorry, something has gone wrong", error))
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }

        private void OnTweetTextBoxPreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.CanExecute = Clipboard.ContainsText() || (Clipboard.ContainsImage() && this.ViewModel.AddAttachmentCommand.CanExecute.FirstAsync().Wait());
                e.Handled = true;
            }
        }

        private void OnTweetTextBoxPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (Clipboard.ContainsImage())
                {
                    var image = Clipboard.GetImage();
                    var encoder = new PngBitmapEncoder();

                    using (var stream = new MemoryStream())
                    {
                        encoder.Frames.Add(BitmapFrame.Create(image));
                        encoder.Save(stream);

                        this
                            .ViewModel
                            .AddAttachmentCommand
                            .Execute(stream.ToArray())
                            .Subscribe();
                    }

                    e.Handled = true;
                }
            }
        }

        private static Notifier CreateNotifier() =>
            new Notifier(
                configuration =>
                {
                    configuration.PositionProvider = new WindowPositionProvider(
                        parentWindow: Application.Current.MainWindow,
                        corner: Corner.TopRight,
                        offsetX: 10,
                        offsetY: 50);

                    configuration.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                        notificationLifetime: TimeSpan.FromSeconds(5),
                        maximumNotificationCount: MaximumNotificationCount.FromCount(3));

                    configuration.Dispatcher = Application.Current.Dispatcher;
                });

        private static string GetMessageForCharactersRemaining(int charactersRemaining)
        {
            if (charactersRemaining < 0)
            {
                return $"You've used {Math.Abs(charactersRemaining)} too many characters.";
            }

            if (charactersRemaining == 0)
            {
                return "No characters remaining.";
            }

            if (charactersRemaining <= 10)
            {
                return $"{charactersRemaining} characters remaining.";
            }

            if (charactersRemaining <= 50)
            {
                return "Only a few characters remaining.";
            }

            if (charactersRemaining <= 100)
            {
                return "Plenty of characters remaining.";
            }

            return null;
        }

        private static Brush GetForegroundForCharactersRemaining(int charactersRemaining)
        {
            if (charactersRemaining < 0)
            {
                return Brushes.Red;
            }

            if (charactersRemaining <= 50)
            {
                return Brushes.Orange;
            }

            return Brushes.Black;
        }
    }
}