using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvDeadlockRepro
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void Button_OnClick(object sender, RoutedEventArgs e) {
            var dlg = new MyDialog();
            await dlg.ShowDialog<bool>(this);
        }
    }
}