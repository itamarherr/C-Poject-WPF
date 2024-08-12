using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Common;

namespace WeatherApp

{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "WeatherApp";
        public string IsRecommended { get; set; } = "RECOMMENDED";
        public void Open()
        {

        }

        public BitmapImage Icon
        {
            get
            {
                try
                {
                    // Ensure that the assembly name of the WeatherApp project is used
                    string weatherAppAssemblyName = "WeatherApp"; // Explicitly set the assembly name
                    Uri uri = new Uri($"pack://application:,,,/{weatherAppAssemblyName};component/Resources/Main.png", UriKind.Absolute);
                    return new BitmapImage(uri);
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    MessageBox.Show($"Failed to load image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }
        public void Run()
        {
            ExplanationWindow explanationWindow = new ExplanationWindow(OpenWeatherApp);
            explanationWindow.Show();
           
        }
        public void OpenWeatherApp()
        {
            MainWindow window = new MainWindow();
            window.Show();
        }

    }
}
