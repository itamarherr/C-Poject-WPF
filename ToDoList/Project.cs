using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Common;

namespace ToDoList
{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "ToDoList";

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
            //MessageBox.Show("High level game. are you sure you are redy for this?");
            ExplanationWindow explanationWindow = new ExplanationWindow(OpenToDoList);
            explanationWindow.Show();
        }

        public void OpenToDoList()
        {
            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
