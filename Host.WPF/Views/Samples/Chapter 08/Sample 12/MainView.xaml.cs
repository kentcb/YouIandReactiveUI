namespace Book.Views.Samples.Chapter08.Sample12
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using MahApps.Metro.Controls.Dialogs;
    using ReactiveUI;
    using ViewModels.Samples.Chapter08.Sample12;

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
                            .Bind(this.ViewModel, x => x.User, x => x.userTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.LoginCommand, x => x.loginButton)
                            .DisposeWith(disposables);
                        this
                            .BindCommand(this.ViewModel, x => x.CancelCommand, x => x.cancelButton)
                            .DisposeWith(disposables);

                        // we marshal changes to password manually because WPF's PasswordBox.Password property doesn't support change notifications
                        this
                            .passwordBox
                            .Events()
                            .PasswordChanged
                            .Select(_ => this.passwordBox.Password)
                            .Subscribe(x => this.ViewModel.Password = x)
                            .DisposeWith(disposables);

                        this
                            .ViewModel
                            .LoginCommand
                            .SelectMany(
                                result =>
                                {
                                    if (!result.HasValue)
                                    {
                                        return Observable.Empty<MessageDialogResult>();
                                    }

                                    if (result.Value)
                                    {
                                        return this.ShowMessage("Login Successful", "Welcome!");
                                    }
                                    else
                                    {
                                        return this.ShowMessage("Login Failed", "Ah, ah, ah, you didn't say the magic word!");
                                    }
                                })
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }
    }
}