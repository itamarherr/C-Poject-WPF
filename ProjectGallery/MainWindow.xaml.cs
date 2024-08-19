using ProjectGallery.Controls;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tic_Tac_Toe;
using Bingo;
using MemoryGame;
using Pong;
using ToDoList;
using WeatherApp;
using System.Net.NetworkInformation;
using Common;




namespace ProjectGallery

{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private IProjectMeta[] projects = new IProjectMeta[]
        {
            new MemoryGame.Project(),
            new Tic_Tac_Toe.Project(),
            new Pong.Project(),
            new WeatherApp.Project(),
            new ToDoList.Project(),
            new Bingo.Project()

        };
        public MainWindow()
        {
            InitializeComponent();
            InitializeProjectButtons();
        }
        private void InitializeProjectButtons()
        {
            int index = 0;
            foreach (var project in projects)
            {

                ProjectButton button = new ProjectButton(project);
                
                    Margin = new Thickness(20);
                    Width = 150;
                    Height = 180;
                

                if (index % 2 == 0)
                {
                    FirstRowPanel.Children.Add(button);
                }
                else
                {
                    SecondRowPanel.Children.Add(button);
                }

                index++;

            }
        }
    }
}