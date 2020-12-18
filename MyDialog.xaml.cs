using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace AvDeadlockRepro
{
    public class MyDialog : Window {
        private readonly BusyIndicator m_BusyIndicator;
        
        public MyDialog()
        {
            InitializeComponent();
            m_BusyIndicator = this.FindControl<BusyIndicator>("busy");
            Console.WriteLine("Dialog start");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void Store_Click(object sender, RoutedEventArgs e) {
            m_BusyIndicator.StartSpinner();
            await Task.Delay(500);  // Simulate storage
            Console.WriteLine("Closing");
            m_BusyIndicator.StopSpinner();
            //await Task.Delay(100);  // Heuristic workaround
            this.Close(true);
        }
    }
    
}