namespace Book.ViewModels.Samples.Chapter14.Sample15
{
    using ReactiveUI;

    [Sample(
        "BindTo",
        @"This sample demonstrates the use of `BindTo`, which can be used to bind the output of any observable to any other property. In this case, the controls in the view are bound directly together rather than going through the view model (which is empty).")]
    public sealed class MainViewModel : ReactiveObject
    {
    }
}