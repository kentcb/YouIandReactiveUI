namespace Book
{
    using System.Windows;
    using Book.ViewModels;
    using ReactiveUI;
    using Splat;

    public partial class App : Application
    {
        private readonly AutoSuspendHelper autoSuspendHelper;

        public App()
        {
            this.autoSuspendHelper = new AutoSuspendHelper(this);
            Registrations.Register(Splat.Locator.CurrentMutable);
        }
    }
}