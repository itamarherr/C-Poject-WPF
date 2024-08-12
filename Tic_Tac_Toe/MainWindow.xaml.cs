using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Tic_Tac_Toe.Controls;
using Tic_Tac_Toe.Enums;

namespace Tic_Tac_Toe
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int player1Score;
        private int player2Score;
        private string playerOneName;
        private string playerTwoName;

        public string playerOneNameDisplay;
        public string playerTwoNameDisplay;


        public string PlayerOneName
        {
            get => playerOneName;
            set
            {
                if (playerOneName != value)
                {
                    playerOneName = value;
                    OnPropertyChanged(nameof(PlayerOneName));
                
                }
            }
        }
        public string PlayerTwoName
        {
            get => playerTwoName;
            set
            {
                if (playerTwoName != value)
                {
                    playerTwoName = value;
                    OnPropertyChanged(nameof(PlayerTwoName));
                }
            }
        }

        public string PlayerOneNameDisplay
        {
            get => playerOneNameDisplay;
            set
            {
                if (playerOneNameDisplay != value)
                {
                    playerOneNameDisplay = value;
                    OnPropertyChanged(nameof(PlayerOneNameDisplay));
                }
            }
        }
  
        public string PlayerTwoNameDisplay
        {
            get => playerTwoNameDisplay;
            set
            {
                if (playerTwoNameDisplay != value)
                {
                    playerTwoNameDisplay = value;
                    OnPropertyChanged(nameof(PlayerTwoNameDisplay));
                }
            }
        }

        public int Player1Score
        {
            get => player1Score;
            set
            {
                if (player1Score != value)
                {
                    player1Score = value;
                    OnPropertyChanged(nameof(Player1Score));
                }
            }
        }

        public int Player2Score
        {
            get => player2Score;
            set
            {
                if (player2Score != value)
                {
                    player2Score = value;
                    OnPropertyChanged(nameof(Player2Score));
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            MyBoard.GameEnded += HandleGameEnded;
            MyBoard.PlayerOneNameDisplay = playerOneNameDisplay;
            MyBoard.PlayerTwoNameDisplay = playerTwoNameDisplay;

        }

        private void HandleGameEnded(object sender, GameEndEventArgs e)
        {
            switch (e.Result)
            {
                case GameResult.PlayerOneWins:
                    Player1Score++;
                    break;
                case GameResult.PlayerTwoWins:
                    Player2Score++;
                    break;
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            GameType gameType;
            if (sender == Btn_PvP)
            {
                gameType = GameType.PvP;
                Player1Score = 0;
                Player2Score = 0;
                PlayerTwoNameDisplay = "";
            }
            else if (sender == Btn_PvC)
            {
                gameType = GameType.PvC;
                Player1Score = 0;
                Player2Score = 0;
                PlayerTwoNameDisplay = "Computer"; 

            }
            else
            {
                return;
            }

            MyBoard.StartNewGame(gameType);
            MyBoard.GameMode = gameType;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ClearText(object sender, RoutedEventArgs e)
        {
            if (sender == PlayerOneNameTextBox)
            {
                MyBoard.PlayerOneNameDisplay = PlayerOneNameTextBox.Text;
                PlayerOneNameDisplay = PlayerOneNameTextBox.Text;
                PlayerOneName = string.Empty;
            }

            else if (sender == PlayerTwoNameTextBox)
            {
                MyBoard.PlayerTwoNameDisplay = PlayerTwoNameTextBox.Text;
                PlayerTwoNameDisplay = PlayerTwoNameTextBox.Text;
                PlayerTwoName = string.Empty;
            }
        }
    }
}
