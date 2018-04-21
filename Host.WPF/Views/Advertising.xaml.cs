using ReactiveUI;
using System.Diagnostics;
using System.Windows.Controls;

namespace Book.Views
{
    public partial class Advertising : UserControl
    {
        public Advertising()
        {
            InitializeComponent();

            var link = "https://kent-boogaart.com/you-i-and-reactiveui/";

            var buyCommand = ReactiveCommand.Create(() => Process.Start(link));
            buyButton.Command = buyCommand;
            buyHyperlink.Command = buyCommand;
        }
    }
}
