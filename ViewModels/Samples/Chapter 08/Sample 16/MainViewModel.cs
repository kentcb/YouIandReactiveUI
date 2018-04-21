namespace Book.ViewModels.Samples.Chapter08.Sample16
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Text;
    using ReactiveUI;

    [Sample(
        "Encapsulating Asynchronous Work",
        @"This sample demonstrates how an asynchronous task (loading ""roars"", in this case) can be encapsulated in a command even though the user doesn't directly instigate execution of that command.

Every 10 seconds, the `refreshRoarsCommand` is automatically executed without user intervention. Any errors are surfaced with a generic error message.")]
    public sealed class MainViewModel : ReactiveObject, ISupportsActivation
    {
        private static readonly Random random = new Random();
        private readonly ViewModelActivator activator;
        private readonly ReactiveCommand<Unit, IList<RoarViewModel>> refreshRoarsCommand;
        private readonly ObservableAsPropertyHelper<IList<RoarViewModel>> roars;
        private readonly ObservableAsPropertyHelper<bool> isInError;

        public MainViewModel()
        {
            this.activator = new ViewModelActivator();
            this.refreshRoarsCommand = ReactiveCommand.CreateFromObservable(
                this.RefreshRoars,
                outputScheduler: RxApp.MainThreadScheduler);
            this.roars = this
                .refreshRoarsCommand
                .StartWith(new List<RoarViewModel>())
                .ToProperty(this, x => x.Roars);
            this.isInError = Observable
                .Merge(
                    this.refreshRoarsCommand.ThrownExceptions.Select(_ => true),
                    this.refreshRoarsCommand.Select(_ => false))
                .ToProperty(this, x => x.IsInError);

            this
                .WhenActivated(
                    disposables =>
                    {
                        Observable
                            .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(10))
                            .Select(_ => Unit.Default)
                            .InvokeCommand(this.refreshRoarsCommand)
                            .DisposeWith(disposables);
                    });
        }

        public ViewModelActivator Activator => this.activator;

        public IList<RoarViewModel> Roars => this.roars.Value;

        public bool IsInError => this.isInError.Value;

        private IObservable<IList<RoarViewModel>> RefreshRoars()
        {
            var error = random.Next(0, 5) == 0;

            if (error)
            {
                return Observable.Throw<IList<RoarViewModel>>(new InvalidOperationException());
            }

            var count = random.Next(10, 15);
            var dinosaurs = Data
                .Dinosaurs
                .All;
            var roars = Enumerable
                .Range(0, count)
                .Select(
                    _ =>
                    {
                        var user = "@" + dinosaurs[random.Next(0, dinosaurs.Count)].Name;
                        var userGender = random.Next(0, 2) == 0 ? "women" : "men";
                        var userNumber = random.Next(0, 100);
                        var image = $"https://randomuser.me/api/portraits/{userGender}/{userNumber:00}.jpg";
                        var text = GenerateText();
                        return new RoarViewModel(
                            user,
                            image,
                            text);
                    })
                .ToList();

            return Observable
                .Return(roars)
                .Delay(TimeSpan.FromMilliseconds(random.Next(100, 1000)));
        }

        private static string GenerateText()
        {
            var wordCount = random.Next(5, 20);
            var words = Enumerable
                .Range(0, wordCount)
                .Select(_ => loremIpsum[random.Next(0, loremIpsum.Length)])
                .ToList();
            words.Insert(random.Next(0, words.Count), "#dinosaur");
            return words
                .Aggregate(new StringBuilder(), (acc, next) => acc.Append(next).Append(" "), acc => acc.ToString());
        }

        private static readonly string[] loremIpsum = "Donec auctor ligula eget malesuada placerat. Nulla interdum quam posuere leo pretium, a elementum metus ornare. Morbi tellus libero, eleifend ut nunc sed, tincidunt elementum dolor. Donec laoreet, arcu ut laoreet cursus, magna tortor faucibus metus, in elementum mi felis tincidunt lorem. Phasellus at volutpat lorem. Proin id venenatis nisl. Nullam tristique cursus orci at lacinia. Donec eu commodo libero. Sed ut est vel velit eleifend posuere. Vivamus eleifend, tellus sed laoreet ornare, mi ipsum porttitor nibh, in volutpat orci nisl sed tortor. Donec aliquam et lorem a viverra. Sed ac quam neque. Vestibulum varius purus vel mi semper, mattis rutrum risus bibendum. Praesent vitae leo luctus, mattis augue sit amet, feugiat sem. Morbi nunc neque, tincidunt at ante at, congue rutrum nisi. In malesuada eros ac mauris fermentum tincidunt. Phasellus nec commodo nisl. Duis vehicula varius ligula vel suscipit. Aliquam accumsan nulla magna, vehicula lacinia libero aliquam sit amet. Aenean nulla purus, elementum nec tortor quis, pellentesque ullamcorper dolor. Etiam semper dignissim tincidunt. Etiam blandit laoreet lacus non auctor. Sed id lobortis neque, eu interdum elit. Sed euismod iaculis lobortis. Praesent id arcu commodo urna lacinia dapibus sit amet nec arcu. Nunc luctus mauris mi, at ullamcorper nisl aliquet id. Praesent sed nunc pellentesque, commodo velit id, rhoncus quam. Pellentesque vel dolor accumsan, porta lectus eget, commodo nibh. Donec id mi viverra, malesuada nibh a, dapibus nunc. Quisque in finibus leo. Praesent commodo nibh et consectetur lacinia. Duis vitae eros ante. In hac habitasse platea dictumst. Phasellus nec nibh et erat congue dapibus vitae eu lorem. Integer ac lectus mauris. Aenean et ante sodales, tempus massa quis, tempor risus. Sed felis urna, pharetra sed tellus vel, hendrerit malesuada ligula. Praesent eleifend pharetra pulvinar. Integer et erat ut massa venenatis tristique et id neque. Phasellus massa libero, feugiat sit amet tortor quis, consectetur sagittis erat. Nunc sapien dolor, commodo nec pulvinar commodo, feugiat ut ante. Mauris ac purus ac lacus consequat laoreet sed fermentum sapien. Aenean blandit arcu quis laoreet tincidunt. Nulla mauris erat, porta sit amet viverra a, dictum ac dolor. Ut hendrerit nunc libero, faucibus pulvinar dui egestas finibus. In eu tristique justo. Vestibulum ligula felis, fermentum sed turpis ac, semper mattis sem. Fusce sodales dolor metus, ut blandit sem dapibus sed. Ut eu risus nec diam feugiat imperdiet ut non ligula. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla vehicula nisi ut viverra finibus."
            .Split(new char[] { '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }
}