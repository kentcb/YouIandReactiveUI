namespace Book
{
    using System;
    using System.IO;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Runtime.Serialization;
    using Book.Views.Samples.Chapter18.Sample05;
    using ReactiveUI;
    using Splat;
    using ViewModels;
    using Views;

    public static class Registrations
    {
        static Registrations()
        {
            // override Splat's mode detector with our own. This allows us to by-pass the performance hit that
            // we would incur using the default Splat implementation
            ModeDetector.OverrideModeDetector(CustomModeDetector.Instance);

            // seems stupid, but this forces the RxApp static constructor to run
            // without this, our registrations below might be overwritten by RxUI when it eventually initializes
            RxApp.DefaultExceptionHandler = RxApp.DefaultExceptionHandler;
        }

        public static void Register(IMutableDependencyResolver container)
        {
            ConfigureSuspensionHost(container);

            var defaultViewLocator = container.GetService<IViewLocator>();
            var viewLocator = new ConventionBasedViewLocator(defaultViewLocator);

            // register a custom view locator that uses convention to resolve views based on view model names, but also defers
            // to the default view locator for those samples that require it for demonstration purposes
            container.RegisterConstant(viewLocator, typeof(IViewLocator));

            var defaultActivationForViewFetcher = container.GetService<IActivationForViewFetcher>();
            var activationForViewFetcher = new ForcibleActivationForViewFetcher(defaultActivationForViewFetcher);

            // register a custom activation for view fetcher purely to facilitate taking screenshots for the book. I need to
            // be able to force activation so that views are correctly initialized prior to taking the screenshot
            container.RegisterConstant(activationForViewFetcher, typeof(IActivationForViewFetcher));
            container.RegisterConstant(activationForViewFetcher, typeof(IForcibleActivationForViewFetcher));
        }

        private static void ConfigureSuspensionHost(IMutableDependencyResolver container)
        {
            var suspensionDriver = new SuspensionDriver();
            container.RegisterConstant<ISuspensionDriver>(suspensionDriver);

            var suspensionHost = RxApp.SuspensionHost;
            suspensionHost.CreateNewAppState = () => new AppState();
            suspensionHost.SetupDefaultSuspendResume();
        }

        private sealed class ConventionBasedViewLocator : IViewLocator
        {
            private readonly IViewLocator deferTo;

            public ConventionBasedViewLocator(IViewLocator deferTo)
            {
                this.deferTo = deferTo;
            }

            public IViewFor ResolveView<T>(T viewModel, string contract = null)
                where T : class
            {
                if (viewModel is SampleWithChapterViewModel)
                {
                    return new SampleHostView();
                }

                if (viewModel is ChapterViewModel)
                {
                    return new ChapterView();
                }
                else if (viewModel is ConsoleSampleViewModel)
                {
                    return new ConsoleSampleView();
                }
                else if (viewModel is TestSampleViewModel)
                {
                    return new TestSampleView();
                }

                if (contract != null)
                {
                    return this.deferTo.ResolveView<T>(viewModel, contract);
                }

                var viewTypeName = viewModel
                    .GetType()
                    .FullName
                    .Replace("ViewModels.", "Views.");

                if (viewTypeName.EndsWith("ViewModel"))
                {
                    viewTypeName = viewTypeName.Substring(0, viewTypeName.Length - 5);
                }

                var viewType = Type.GetType(viewTypeName);

                if (viewType != null)
                {
                    return (IViewFor)Activator.CreateInstance(viewType);
                }

                return this.deferTo.ResolveView<T>(viewModel, contract);
            }
        }

        private sealed class CustomModeDetector : IModeDetector
        {
            public static readonly CustomModeDetector Instance = new CustomModeDetector();

            private CustomModeDetector()
            {
            }

            public bool? InDesignMode() =>
                false;

            public bool? InUnitTestRunner() =>
                false;
        }

        private sealed class SuspensionDriver : ISuspensionDriver
        {
            private readonly string fileName;
            private readonly DataContractSerializer serializer;

            public SuspensionDriver()
            {
                this.fileName = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ReactiveUIBookSamples.data");
                this.serializer = new DataContractSerializer(typeof(AppState));
            }

            public IObservable<Unit> InvalidateState() =>
                Observable
                    .Create<Unit>(
                        observer =>
                        {
                            try
                            {
                                File.Delete(this.fileName);
                            }
                            catch
                            {
                            }

                            observer.OnNext(Unit.Default);
                            observer.OnCompleted();

                            return Disposable.Empty;
                        });

            public IObservable<object> LoadState() =>
                Observable
                    .Create<object>(
                        observer =>
                        {
                            if (!File.Exists(this.fileName))
                            {
                                observer.OnError(new Exception("No state file."));
                                return Disposable.Empty;
                            }

                            try
                            {
                                using (var stream = File.OpenRead(this.fileName))
                                {
                                    var data = this.serializer.ReadObject(stream);
                                    observer.OnNext(data);
                                    observer.OnCompleted();
                                }
                            }
                            catch (Exception ex)
                            {
                                observer.OnError(ex);
                            }

                            return Disposable.Empty;
                        });

            public IObservable<Unit> SaveState(object state) =>
                Observable
                    .Create<Unit>(
                        observer =>
                        {
                            try
                            {
                                using (var stream = File.Create(this.fileName))
                                {
                                    this.serializer.WriteObject(stream, state);
                                    observer.OnNext(Unit.Default);
                                    observer.OnCompleted();
                                }
                            }
                            catch (Exception ex)
                            {
                                observer.OnError(ex);
                            }

                            return Disposable.Empty;
                        });
        }
    }
}