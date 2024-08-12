using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Common;

namespace Bingo

{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "Bingo";
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
                   
                    string BingoAssemblyName = "Bingo"; 
                    Uri uri = new Uri($"pack://application:,,,/{BingoAssemblyName};component/Resources/Main.png", UriKind.Absolute);
                    return new BitmapImage(uri);
                }
                catch (Exception ex)
                {
                   
                    MessageBox.Show($"Failed to load image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }
        public void Run()
        {

            ExplanationWindow explanationWindow = new ExplanationWindow(OpenBingoGame);
            explanationWindow.Show();

           
        }

        public void OpenBingoGame()
        {
            MainWindow window = new MainWindow();
            window.Show();
        }

    }
}
