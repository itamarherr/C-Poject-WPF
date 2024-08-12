using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MemoryGame.Controls;
using MemoryGame;
using System.IO;
using MemoryGame.Enums;
using System.ComponentModel;

namespace MemoryGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public readonly string imagesDirectory = @"C:\Users\itama\Desktop\איתמר\HackerU\C#\C#Project\MemoryGame\Resources\Animals Images";
        private Random random = new Random();

        private PlayerTurn _currentPlayerTurn;
        public PlayerTurn CurrentPlayerTurn
        {
            get => _currentPlayerTurn;
            set
            {
                _currentPlayerTurn = value;
                OnPropertyChanged(nameof(CurrentPlayerTurn));
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void StartNewGame_ButtonClick(object sender, RoutedEventArgs e)
        {
            string[] imageFiles = Directory.GetFiles(imagesDirectory, "*.jpg");

            List<string> duplicatedImagePaths = new List<string>();

            foreach (string imagePath in imageFiles)
            {
                duplicatedImagePaths.Add(imagePath);
                duplicatedImagePaths.Add(imagePath);

            }

            List<string> shuffledImagePaths = Shuffle(duplicatedImagePaths, random);
            Board.buttonImagePathMap.Clear();
            int index = 0;
            foreach (Button cardsButton in MyBoard.GameGrid.Children.OfType<Button>())
            {
                if (index < shuffledImagePaths.Count)
                {
                    string imagePath = shuffledImagePaths[index];

                    Board.buttonImagePathMap[cardsButton] = imagePath;
                    index++;
                    cardsButton.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/MemoryGame;component/Resources/Background.jpg")),
                        Width = 200,
                        Height = 200 
                    };
                }
                else
                {
                    cardsButton.Content = null;
                }

                

            }
        }


        private List<T> Shuffle<T>(List<T> list, Random random)
        {
            
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
            return list;
        }


    }
}