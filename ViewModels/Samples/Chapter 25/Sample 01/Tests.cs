namespace Book.ViewModels.Samples.Chapter25.Sample01
{
    using System;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Microsoft.Reactive.Testing;
    using PCLMock;
    using ReactiveUI;
    using ReactiveUI.Testing;
    using Xunit;

    [Sample(
        "Re-structuring for improved readability",
        @"This sample is a refactoring of [Sample 24.02](24.02) to improve the readability and maintainability of the code.")]
    public sealed class Tests : TestSampleViewModel
    {
        [Fact]
        public void data_is_not_retrieved_upon_construction()
        {
            var api = new ApiMock();
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                CurrentThreadScheduler.Instance);

            api
                .Verify(x => x.GetDinosaur(It.IsAny<int>()))
                .WasNotCalled();
        }

        [Fact]
        public void data_is_retrieved_upon_activation()
        {
            var api = new ApiMock();
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                CurrentThreadScheduler.Instance);

            sut
                .Activator
                .Activate();

            api
                .Verify(x => x.GetDinosaur(42))
                .WasCalledExactlyOnce();
        }

        [Fact]
        public void retrieved_data_is_used_to_populate_the_initial_values()
        {
            var scheduler = new TestScheduler();
            var dinosaur = new Dinosaur("Barney", 13, new byte[] { 1, 2, 3 });
            var api = new ApiMock();
            api
                .When(x => x.GetDinosaur(42))
                .Return(Observable.Return(dinosaur));
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                scheduler);

            sut
                .Activator
                .Activate();
            scheduler.AdvanceByMs(1);

            Assert.Equal("Barney", sut.Name);
            Assert.Equal("13", sut.Weight);
            Assert.Equal(13, sut.ValidatedWeight.Value);
            Assert.NotNull(sut.Image);
        }

        [Fact]
        public void valid_weights_are_accepted()
        {
            var scheduler = new TestScheduler();
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                new ApiMock(),
                scheduler);
            sut
                .Activator
                .Activate();
            scheduler.AdvanceByMs(1);

            sut.Weight = "42";
            scheduler.AdvanceByMs(1);

            Assert.True(sut.ValidatedWeight.IsValid);
            Assert.Equal(42, sut.ValidatedWeight.Value);
        }

        [Fact]
        public void invalid_weights_are_rejected()
        {
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                new ApiMock(),
                CurrentThreadScheduler.Instance);
            sut
                .Activator
                .Activate();

            sut.Weight = "42a";

            Assert.False(sut.ValidatedWeight.IsValid);
            Assert.Equal("'42a' is not a valid weight. Please enter whole numbers only.", sut.ValidatedWeight.Error);
        }

        [Fact]
        public void changing_the_image_data_updates_the_image()
        {
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                new ApiMock(),
                CurrentThreadScheduler.Instance);
            sut
                .Activator
                .Activate();
            var images = sut
                .WhenAnyValue(x => x.Image)
                .CreateCollection(ImmediateScheduler.Instance);

            sut.ImageData = new byte[] { 1, 2, 3 };
            sut.ImageData = new byte[] { 4, 5, 6 };

            Assert.Equal(3, images.Count);
        }

        [Fact]
        public void data_is_saved_two_seconds_after_name_is_modified()
        {
            var scheduler = new TestScheduler();
            var api = new ApiMock();
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                scheduler);
            sut
                .Activator
                .Activate();
            scheduler.AdvanceByMs(2000);

            sut.Name = "Barney";
            sut.Weight = "42";
            scheduler.AdvanceByMs(1999);
            api
                .Verify(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .WasNotCalled();

            scheduler.AdvanceByMs(1);
            api
                .Verify(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .WasCalledExactlyOnce();
        }

        [Fact]
        public void data_is_saved_two_seconds_after_weight_is_modified()
        {
            var scheduler = new TestScheduler();
            var api = new ApiMock();
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                scheduler);
            sut
                .Activator
                .Activate();
            scheduler.AdvanceByMs(2000);

            sut.Weight = "42";
            scheduler.AdvanceByMs(1999);
            api
                .Verify(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .WasNotCalled();

            scheduler.AdvanceByMs(1);
            api
                .Verify(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .WasCalledExactlyOnce();
        }

        [Fact]
        public void data_is_saved_two_seconds_after_image_data_is_modified()
        {
            var scheduler = new TestScheduler();
            var api = new ApiMock();
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                scheduler);
            sut
                .Activator
                .Activate();
            scheduler.AdvanceByMs(2000);

            sut.Weight = "42";
            sut.ImageData = new byte[] { 1, 2, 3 };
            scheduler.AdvanceByMs(1999);
            api
                .Verify(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .WasNotCalled();

            scheduler.AdvanceByMs(1);
            api
                .Verify(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .WasCalledExactlyOnce();
        }

        [Fact]
        public void data_is_saved_immediately_upon_deactivation_even_if_two_seconds_has_not_elapsed_since_last_change()
        {
            var scheduler = new TestScheduler();
            var api = new ApiMock();
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                scheduler);
            sut
                .Activator
                .Activate();

            sut.Name = "Barney";
            scheduler.AdvanceByMs(1);
            api
                .Verify(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .WasNotCalled();

            sut
                .Activator
                .Deactivate();
            scheduler.AdvanceByMs(1);
            api
                .Verify(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .WasCalledExactlyOnce();
        }

        [Fact]
        public void saves_are_throttled_to_two_seconds()
        {
            var scheduler = new TestScheduler();
            var api = new ApiMock();
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                scheduler);
            sut
                .Activator
                .Activate();
            scheduler.AdvanceByMs(2000);

            sut.Name = "Barney";
            scheduler.AdvanceByMs(500);
            sut.Name = "Barney the Dinosaur";
            scheduler.AdvanceByMs(500);
            sut.Weight = "42";

            scheduler.AdvanceByMs(1999);
            api
                .Verify(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .WasNotCalled();

            scheduler.AdvanceByMs(1);
            api
                .Verify(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .WasCalledExactlyOnce();
        }

        [Fact]
        public void data_is_not_saved_if_it_is_invalid()
        {
            var api = new ApiMock();
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                CurrentThreadScheduler.Instance);
            sut
                .Activator
                .Activate();

            sut.Weight = "42a";

            api
                .Verify(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .WasNotCalled();
        }

        [Fact]
        public void data_cannot_be_deleted_if_the_auditing_system_is_unavailable()
        {
            var api = new ApiMock();
            var isAuditingAvailable = new BehaviorSubject<bool>(true);
            api
                .When(x => x.IsAuditingAvailable)
                .Return(isAuditingAvailable);
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                CurrentThreadScheduler.Instance);
            sut
                .Activator
                .Activate();

            Assert.True(sut.DeleteCommand.CanExecute.FirstAsync().Wait());

            isAuditingAvailable.OnNext(false);
            Assert.False(sut.DeleteCommand.CanExecute.FirstAsync().Wait());

            isAuditingAvailable.OnNext(true);
            Assert.True(sut.DeleteCommand.CanExecute.FirstAsync().Wait());
        }

        [Fact]
        public void data_is_not_deleted_if_user_cancels()
        {
            var api = new ApiMock();
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                CurrentThreadScheduler.Instance);

            using (sut.ConfirmDeleteInteraction.RegisterHandler(context => context.SetOutput(false)))
            {
                sut
                    .DeleteCommand
                    .Execute()
                    .Subscribe();
            }

            api
                .Verify(x => x.DeleteDinosaur(It.IsAny<int>()))
                .WasNotCalled();
        }

        [Fact]
        public void data_is_deleted_if_user_confirms()
        {
            var api = new ApiMock();
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                CurrentThreadScheduler.Instance);

            using (sut.ConfirmDeleteInteraction.RegisterHandler(context => context.SetOutput(true)))
            {
                sut
                    .DeleteCommand
                    .Execute()
                    .Subscribe();
            }

            api
                .Verify(x => x.DeleteDinosaur(It.IsAny<int>()))
                .WasCalledExactlyOnce();
        }

        [Fact]
        public void busy_flag_remains_true_whilst_retrieving_data()
        {
            var api = new ApiMock();
            var getDinosaur = new Subject<Dinosaur>();
            api
                .When(x => x.GetDinosaur(It.IsAny<int>()))
                .Return(getDinosaur);
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                CurrentThreadScheduler.Instance);

            sut
                .Activator
                .Activate();
            Assert.True(sut.IsBusy);

            getDinosaur.OnCompleted();
            Assert.False(sut.IsBusy);
        }

        [Fact]
        public void busy_flag_remains_true_whilst_saving_data()
        {
            var scheduler = new TestScheduler();
            var api = new ApiMock();
            var saveDinosaur = new Subject<Unit>();
            api
                .When(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .Return(saveDinosaur);
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                scheduler);

            sut
                .Activator
                .Activate();

            sut.Name = "Barney";
            sut.Weight = "42";
            scheduler.AdvanceByMs(2001);
            Assert.True(sut.IsBusy);

            saveDinosaur.OnCompleted();
            scheduler.AdvanceByMs(1);
            Assert.False(sut.IsBusy);
        }

        [Fact]
        public void busy_flag_remains_true_whilst_deleting_data()
        {
            var api = new ApiMock();
            var deleteDinosaur = new Subject<Unit>();
            api
                .When(x => x.DeleteDinosaur(It.IsAny<int>()))
                .Return(deleteDinosaur);
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                CurrentThreadScheduler.Instance);

            sut
                .Activator
                .Activate();

            using (sut.ConfirmDeleteInteraction.RegisterHandler(context => context.SetOutput(true)))
            {
                sut
                    .DeleteCommand
                    .Execute()
                    .Subscribe();
                Assert.True(sut.IsBusy);
            }

            deleteDinosaur.OnCompleted();
            Assert.False(sut.IsBusy);
        }

        [Fact]
        public void errors_retrieving_data_are_surfaced()
        {
            var api = new ApiMock();
            api
                .When(x => x.GetDinosaur(It.IsAny<int>()))
                .Return(Observable.Throw<Dinosaur>(new InvalidOperationException("foo")));
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                CurrentThreadScheduler.Instance);

            sut
                .Activator
                .Activate();

            Assert.NotNull(sut.Error);
            Assert.Equal("foo", sut.Error.Exception.Message);

            api
                .When(x => x.GetDinosaur(It.IsAny<int>()))
                .Return(Observable.Return(default(Dinosaur)));
            sut
                .Error
                .RetryCommand
                .Execute(null);
            Assert.Null(sut.Error);
        }

        [Fact]
        public void errors_saving_data_are_surfaced()
        {
            var scheduler = new TestScheduler();
            var api = new ApiMock();
            api
                .When(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .Return(Observable.Throw<Unit>(new InvalidOperationException("foo")));
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                scheduler);

            sut
                .Activator
                .Activate();
            sut.Name = "Barney";
            sut.Weight = "42";
            scheduler.AdvanceByMs(2001);

            Assert.NotNull(sut.Error);
            Assert.Equal("foo", sut.Error.Exception.Message);

            api
                .When(x => x.SaveDinosaur(It.IsAny<Dinosaur>()))
                .Return(Observable.Return(Unit.Default));
            sut
                .Error
                .RetryCommand
                .Execute(null);
            scheduler.AdvanceByMs(1);
            Assert.Null(sut.Error);
        }

        [Fact]
        public void errors_deleting_data_are_surfaced()
        {
            var api = new ApiMock();
            api
                .When(x => x.DeleteDinosaur(It.IsAny<int>()))
                .Return(Observable.Throw<Unit>(new InvalidOperationException("foo")));
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                CurrentThreadScheduler.Instance);

            sut
                .Activator
                .Activate();

            using (sut.ConfirmDeleteInteraction.RegisterHandler(context => context.SetOutput(true)))
            {
                sut
                    .DeleteCommand
                    .Execute()
                    .Subscribe(
                        _ => { },
                        _ => { });

                Assert.NotNull(sut.Error);
                Assert.Equal("foo", sut.Error.Exception.Message);

                api
                    .When(x => x.DeleteDinosaur(It.IsAny<int>()))
                    .Return(Observable.Return(Unit.Default));
                sut
                    .Error
                    .RetryCommand
                    .Execute(null);
                Assert.Null(sut.Error);
            }
        }

        [Fact]
        public void memory_is_reclaimed_after_deactivation()
        {
            //
            // IMPORTANT: if you're observing failures in this test, try running it in release mode without a debugger attached.
            //

            // The SUT will subscribe to this subject, so we hold a reference to it so that it can't be GC'd.
            var isAuditingAvailable = new Subject<bool>();
            var api = new ApiMock();
            api
                .When(x => x.IsAuditingAvailable)
                .Return(isAuditingAvailable);
            var sut = new DinosaurDetailsViewModel(
                42,
                new BitmapLoaderMock(),
                api,
                CurrentThreadScheduler.Instance);
            sut
                .Activator
                .Activate();

            var weakSut = new WeakReference(sut);
            sut = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Assert.True(weakSut.IsAlive);

            // Deactivation is when the SUT should detach subscriptions and therefore become eligible for collection.
            ((DinosaurDetailsViewModel)weakSut.Target)
                .Activator
                .Deactivate();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Assert.False(weakSut.IsAlive);
        }
    }
}