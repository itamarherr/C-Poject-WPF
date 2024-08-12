using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MemoryGame.Enums;

namespace MemoryGame.Controls;

/// <summary>
/// Interaction logic for UserControl1.xaml
/// </summary>
public partial class Board : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private int revealedCardsCounter = 0;
    public static Dictionary<Button, string> buttonImagePathMap = new Dictionary<Button, string>();
    private Dictionary<Button, bool> buttonRevealedMap = new Dictionary<Button, bool>();

    private SerializationInfo imagesDirectory;
    private bool _isPlayerOneTurn = true;
    public int MatchedCardsCounter = 0;
    public int totalPairs = 8;
    public bool IsEndGame = false;
    public int playerOneMatches = 0;
    public int playerTwoMatches = 0;


    private PlayerTurn _currentPlayerTurn;
    public PlayerTurn CurrentPlayerTurn
    {
        get => _currentPlayerTurn;
        set
        {
            _currentPlayerTurn = value;
            OnPropertyChanged(nameof(CurrentPlayerTurn));
            if (DataContext is MainWindow mainWindow)
            {
                mainWindow.CurrentPlayerTurn = value;
            }
        }
    }


    public Board()
    {
        InitializeComponent();
        DataContext = this;
        UpdateCurrentPlayerTurn();
        foreach (Button button in clickedButtons) 
        {
            buttonRevealedMap.Add(button, false);
        }
    }

    private void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


    private void UpdateCurrentPlayerTurn()
    {
        CurrentPlayerTurn = _isPlayerOneTurn ? PlayerTurn.PlayerOneTurn : PlayerTurn.PlayerTwoTurn;
    }


    private List<Button> clickedButtons = new List<Button>();
    public void Card_Click(object sender, RoutedEventArgs e)
    {

        Button cardsButton = (Button)sender;
        if (buttonImagePathMap.TryGetValue(cardsButton, out string imagePath))
        {
            if (revealedCardsCounter < 2 && !clickedButtons.Contains(cardsButton))
            {
                cardsButton.Content = new Image { Source = new BitmapImage(new Uri(imagePath)) };
                clickedButtons.Add(cardsButton); 
                revealedCardsCounter++;
                if (revealedCardsCounter == 2)
                {
                    if (buttonImagePathMap[clickedButtons[0]] == buttonImagePathMap[clickedButtons[1]])
                    {
                        Task.Delay(3000).ContinueWith(_ =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            foreach (Button clickedButton in clickedButtons)
                            {
                                clickedButton.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri("pack://application:,,,/MemoryGame;component/Resources/Solved.jpg")),
                                    Width = 50, 
                                    Height = 50
                                };
                            }
                            _isPlayerOneTurn = !_isPlayerOneTurn;

                            UpdateCurrentPlayerTurn();
                            MatchedCardsCounter++;
                            ProcessMatchCards();
                           
                            
                  

                            clickedButtons.Clear();

                            revealedCardsCounter = 0;
                        });
                    });
                    }
                    else
                    {
                     
                        Task.Delay(2000).ContinueWith(_ =>
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                foreach (Button clickedButton in clickedButtons)
                                {
                                    clickedButton.Content = new Image
                                    {
                                        Source = new BitmapImage(new Uri("pack://application:,,,/MemoryGame;component/Resources/Background.jpg")),
                                        Width = 200, // Set desired image width
                                        Height = 200 // Set desired image height
                                    };
                                }
                                _isPlayerOneTurn = !_isPlayerOneTurn;
                                UpdateCurrentPlayerTurn();
                               


                                clickedButtons.Clear();
                                revealedCardsCounter = 0;
                            });
                        });
                    }
                }

            }
        }
    }
    public void ProcessMatchCards()
    {
        CardsMatch result = _isPlayerOneTurn ? CardsMatch.PlayerOneCardsMatch : CardsMatch.PlayerTwoCardsMatch;
        if(result == CardsMatch.PlayerOneCardsMatch)
        {
            playerOneMatches++;
        }
        else
        {
            playerTwoMatches++;
        }

        if (MatchedCardsCounter == totalPairs)
        {
            IsEndGame = true;
            if (playerOneMatches > playerTwoMatches)

            {
                GameResult FinalResult = GameResult.PlayerOneWin;
                MessageBox.Show(FinalResult.ToString());

            }
            else if (playerOneMatches < playerTwoMatches)
            {
                GameResult FinalResult = GameResult.PlayerTwoWin;
                MessageBox.Show(FinalResult.ToString()); ;
            }
            else
            {
                GameResult FinalResult = GameResult.Draw;
                MessageBox.Show(FinalResult.ToString());
            }

        }
        else
        {
          
            _isPlayerOneTurn = !_isPlayerOneTurn;
            UpdateCurrentPlayerTurn();
        }



    }

}
