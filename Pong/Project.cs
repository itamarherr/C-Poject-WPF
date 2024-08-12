using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Common;

namespace Pong

{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "Pong";
        public void Open()
        {

        }

        public BitmapImage Icon
        {
            get
            {

                string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                Uri uri = new Uri($"pack://application:,,,/{assemblyName};component/Resources/Main.png", UriKind.Absolute);
                return new BitmapImage(uri);
            }

        }
        public void Run()
        {
         
            ExplanationWindow explanationwindow = new ExplanationWindow(OpenPongGame);
            explanationwindow.Show();
        }

        public void OpenPongGame()
        {
            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
