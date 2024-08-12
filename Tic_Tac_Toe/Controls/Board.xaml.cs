using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Tic_Tac_Toe.Enums;
using Tic_Tac_Toe;
using System.Numerics;
using System.ComponentModel;

namespace Tic_Tac_Toe.Controls;
/// <summary>
/// Interaction logic for Board.xaml
/// </summary>
public partial class Board : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<GameEndEventArgs> GameEnded;

    private const string PlayerOneContent = "X";
    private const string PlayerTwoContent = "O";

    private readonly Button[,] _buttons = new Button[3, 3];
    private readonly Random _rnd = new Random();
    private bool _moveMade;
    private DispatcherTimer _playerMoveTimer;
    private bool _isPlayerOneTurn = true;
    public string _playerOneNameDisplay;
    public string _playerTwoNameDisplay;




    private bool _gameIsActive = true;
    private GameType _gameType;
    public GameType GameMode
    {
        get => _gameType;
        set
        {
            _gameType = value;
            OnPropertyChanged(nameof(GameMode));
        }
    }

    private PlayerTurn _currentPlayerTurn;
    public PlayerTurn CurrentPlayerTurn
    {
        get => _currentPlayerTurn;
        set
        {
            _currentPlayerTurn = value;
            OnPropertyChanged(nameof(CurrentPlayerTurn));
            OnPropertyChanged(nameof(CurrentPlayerNameTurn));
        }
    }
    public string PlayerOneNameDisplay
    {
        get => _playerOneNameDisplay;
        set
        {
            _playerOneNameDisplay = value;
            OnPropertyChanged(nameof(PlayerOneNameDisplay));
            OnPropertyChanged(nameof(CurrentPlayerNameTurn));
        }
    }

    public string PlayerTwoNameDisplay
    {
        get => _playerTwoNameDisplay;
        set
        {
            _playerTwoNameDisplay = value;
            OnPropertyChanged(nameof(PlayerTwoNameDisplay));
            OnPropertyChanged(nameof(CurrentPlayerNameTurn));
        }
    }
    public string CurrentPlayerNameTurn
    {
        get => CurrentPlayerTurn == PlayerTurn.PlayerOneTurn ? PlayerOneNameDisplay : PlayerTwoNameDisplay;
    }
    public Board()
    {
        InitializeComponent();
        InitializeGameGrid();
        DataContext = this;
        UpdateCurrentPlayerTurn();
    
        InitializePlayerMoveTimer();
    }

    private void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private void InitializePlayerMoveTimer()
    {
        _playerMoveTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(7)
        };
        _playerMoveTimer.Tick += PlayerMoveTimer_Tick;
    }
    private void PlayerMoveTimer_Tick(object sender, EventArgs e)
    {
        if (!_moveMade)
        {
            MessageBox.Show("Did you fall asleep? Come on and play!", "wake up call");
        }
        _playerMoveTimer.Stop();
    }

    private void UpdateCurrentPlayerTurn()
    {
        CurrentPlayerTurn = _isPlayerOneTurn ? PlayerTurn.PlayerOneTurn : PlayerTurn.PlayerTwoTurn;
   
    }
        private void InitializeGameGrid()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button btn = new Button()
                    {
                        FontSize = 40,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(5)
                    };

                    btn.Click += Button_Click;

                    Grid.SetRow(btn, i);
                    Grid.SetColumn(btn, j);

                    _buttons[i, j] = btn;

                    GameGrid.Children.Add(btn);
                }
            }
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!_gameIsActive)
            {
                return;
            }
            Button? btn = sender as Button;
            if (btn == null || btn.Content != null)
            {
                return;
            }
            int row = Grid.GetRow(btn);
            int column = Grid.GetColumn(btn);
            btn.Content = _isPlayerOneTurn ? PlayerOneContent : PlayerTwoContent;
            if (ProcessEndGame(_isPlayerOneTurn))
            {
                return;
            }

            _isPlayerOneTurn = !_isPlayerOneTurn;
            UpdateCurrentPlayerTurn();


            if (_gameType == GameType.PvC && !_isPlayerOneTurn)
            {
                ComputerMove();
            }
        }

        private void ComputerMove()
        {
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(_rnd.Next(10) / 10.0)
            };
            timer.Tick += (sender, e) =>
            {

                timer.Stop();

                Button btn;
                do
                {
                    int row = _rnd.Next(3);
                    int col = _rnd.Next(3);
                    btn = _buttons[row, col];
                } while (btn.Content != null);

                btn.Content = PlayerTwoContent;

                if (ProcessEndGame(!_isPlayerOneTurn))
                {
                    return;
                }

                _isPlayerOneTurn = !_isPlayerOneTurn;
                UpdateCurrentPlayerTurn();
                _moveMade = false;
                ResetAndStartPlayerMoveTimer();
            };
            timer.Start();
        }
        private void ResetAndStartPlayerMoveTimer()
        {
            _playerMoveTimer.Stop();
            _moveMade = false;
            _playerMoveTimer.Start();
        }
        private void OnGameEnd(GameResult result)
        {
            GameEnded?.Invoke(this, new GameEndEventArgs(result));

        }

        private bool ProcessEndGame(bool isPlayerOneTurn)
        {
            if (CheckForWinner())
            {
                GameResult result = isPlayerOneTurn ? GameResult.PlayerOneWins : GameResult.PlayerTwoWins;
                string winnerMessage = isPlayerOneTurn ? PlayerOneNameDisplay : PlayerTwoNameDisplay;

                if (string.IsNullOrEmpty(winnerMessage))
                {
                    MessageBox.Show("Winner name is not set.");
                }
                else
                {
                    MessageBox.Show($"{winnerMessage} wins!");
                }
                _gameIsActive = false;
                OnGameEnd(result);
                StartNewGame(GameMode);
                return true;

            }

            if (IsBoardFull())
            {
                GameResult result = GameResult.Draw;
                MessageBox.Show(result.ToString());
                _gameIsActive = false;
                OnGameEnd(result);
                StartNewGame(GameMode);
                return true;
            }

            return false;
        }

        public void StartNewGame(GameType gameType)
        {
            if (_gameIsActive)
            {
                return;
            }
            GameMode = gameType;
            //_gameType = gameType;
            _isPlayerOneTurn = true;
            _gameIsActive = true;

            foreach (Button btn in _buttons)
            {
                btn.Content = null;
            }
        }

        //private void HandleGameEnded(object sender, GameEndEventArgs e)
        //{

        //    MessageBox.Show($"Game ended");


        //    _gameIsActive = false;
        //    _isPlayerOneTurn = true;
        //    _gameType = GameType.PvP;

        //    foreach (Button btn in _buttons)
        //    {
        //        btn.Content = null;
        //    }
        //}
        private bool IsBoardFull()
        {
            foreach (Button button in _buttons)
            {
                if (button.Content == null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckForWinner()
        {
            for (int i = 0; i < 3; i++)
            {

                if (AreButtonsEqual(_buttons[i, 0], _buttons[i, 1], _buttons[i, 2]))
                {
                    return true;
                }

                if (AreButtonsEqual(_buttons[0, i], _buttons[1, i], _buttons[2, i]))
                {
                    return true;
                }
            }


            if (AreButtonsEqual(_buttons[0, 0], _buttons[1, 1], _buttons[2, 2]))
            {
                return true;
            }
            if (AreButtonsEqual(_buttons[0, 2], _buttons[1, 1], _buttons[2, 0]))
            {
                return true;
            }

            return false;

        }

        private bool AreButtonsEqual(Button b1, Button b2, Button b3) =>
             b1.Content != null && b1.Content == b2.Content && b2.Content == b3.Content;

    }