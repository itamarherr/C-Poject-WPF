using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Common;

namespace MemoryGame

{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "MemoryGame";
        public string IsRecommended { get; set; } = "RECOMMENDED";
        public void Open()
        {
        
        }
        
        public BitmapImage Icon
        {
            get
            {
                //return new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}/Resources/tic_tac_toe.png", UriKind.Absolute));

                string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                Uri uri = new Uri($"pack://application:,,,/{assemblyName};component/Resources/Main.png", UriKind.Absolute);
                return new BitmapImage(uri);
            }

        }
        public void Run()
        {

            ExplanationWindow explanationWindow = new ExplanationWindow(OpenMemoryGame);
            explanationWindow.Show();

            //MessageBox.Show("High level game. are you sure you are redy for this?");
            //MainWindow window = new MainWindow();
            //window.Show();
        }

        public void OpenMemoryGame()
        {
            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
