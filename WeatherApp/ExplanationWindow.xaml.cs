using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for ExplanationWindow.xaml
    /// </summary>
    public partial class ExplanationWindow : Window
    {
        public readonly Action _onStartApp;
        public ExplanationWindow(Action onStartApp)
        {
            InitializeComponent();
            _onStartApp = onStartApp;
        }
        public void StartApp_Click(object sender, RoutedEventArgs e)
        {
            _onStartApp.Invoke();
            this.Close();
        }
    }
}
