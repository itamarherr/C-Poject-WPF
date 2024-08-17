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
using System.Windows.Threading;
using Pong.Enums;
using Pong.Controls;
using System.ComponentModel;
using Common;
using System.Runtime.CompilerServices;


namespace Pong;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private double paddleSpeed = 10;

    private Rectangle paddle1;

    private Rectangle paddle2;

    private Rectangle ball;

    private DispatcherTimer gameTimer;

    private double ballSpeedY = 10;
    private double ballSpeedX = 5;

    public int leftPlayerScore = 0;
    public int rightPlayerScore = 0;

    public GameResult gameResult;
    public GameLevel gameLevel = GameLevel.Beginner;
    private GameLevel currentGameLevel = GameLevel.Beginner;
    private double LargeBallSize;
    private double MediumBallSize;
    private double SmallBallSize;


    public event PropertyChangedEventHandler? PropertyChanged;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
        KeyDown += new KeyEventHandler(HandleKeyDown);

        gameTimer = new DispatcherTimer();
        gameTimer.Interval = TimeSpan.FromMilliseconds(16);
        gameTimer.Tick += new EventHandler(GameLoop);


        DataContext = this;


    }
    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        InitializeGameElements();
        SetInitialPositions();
        GameCanvas.SizeChanged += GameCanvas_SizeChanged;
    }

    private void GameCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        SetInitialPositions();
    }

    private void UpdateGameElementSizes()
    {
        if (GameCanvas == null) return;
        double paddleWidth = GameCanvas.ActualWidth * 0.015;
        double paddleHeight = GameCanvas.ActualHeight * 0.2;
        double ballSize = Math.Min(GameCanvas.ActualWidth, GameCanvas.ActualHeight) * 0.20;


        if (paddle1 != null)
        {
            paddle1.Width = paddleWidth;
            paddle1.Height = paddleHeight;
        }
        if (paddle2 != null)
        {
            paddle2.Width = paddleWidth;
            paddle2.Height = paddleHeight;
        }
        if (ball != null)
        {
            ball.Width = ballSize;
            ball.Height = ballSize;
        }
    }
    public void Start_Click(object sender, RoutedEventArgs e)
    {
        gameTimer.Start();
    }

    public void Pause_Click(object sender, RoutedEventArgs e)
    {
        gameTimer.Stop();

    }
    private void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private void GameLoop(object? sender, EventArgs e)
    {
        Canvas.SetLeft(ball, Canvas.GetLeft(ball) + ballSpeedX);
        Canvas.SetTop(ball, Canvas.GetTop(ball) + ballSpeedY);

        // Ball collision with top and bottom walls
        if (Canvas.GetTop(ball) <= 0 || Canvas.GetTop(ball) >= GameCanvas.ActualHeight - ball.Height)
        {
            ballSpeedY = -ballSpeedY;
        }

        // Ball collision with paddles
        if (Canvas.GetLeft(ball) <= Canvas.GetLeft(paddle1) + paddle1.Width &&
            Canvas.GetTop(ball) + ball.Height >= Canvas.GetTop(paddle1) &&
            Canvas.GetTop(ball) <= Canvas.GetTop(paddle1) + paddle1.Height)
        {
            ballSpeedX = -ballSpeedX;
        }

        if (Canvas.GetLeft(ball) >= Canvas.GetLeft(paddle2) - ball.Width &&
            Canvas.GetTop(ball) + ball.Height >= Canvas.GetTop(paddle2) &&
            Canvas.GetTop(ball) <= Canvas.GetTop(paddle2) + paddle2.Height)
        {
            ballSpeedX = -ballSpeedX;
        }

        // Ball out of bounds (left or right side)
        if (Canvas.GetLeft(ball) <= 0 || Canvas.GetLeft(ball) >= GameCanvas.ActualWidth - ball.Width)
        {
            ResetBall();
            SetInitialPositions();
            
        }

    }

    private void ResetBall()
    {
        gameTimer.Stop();
        if (Canvas.GetLeft(ball) <= 0)
        {
            gameResult = GameResult.LeftPlayerWin;
        }
        else if (Canvas.GetLeft(ball) >= GameCanvas.ActualWidth - ball.Width)
        {
            gameResult = GameResult.RightPlayerWin;
        }
        HandleGameEnded(this, new GameEndEventArgs(gameResult));
    }

    public int LeftPlayerScore
    {
        get => leftPlayerScore;
        set
        {
            leftPlayerScore = value;
            OnPropertyChanged(nameof(LeftPlayerScore));
        }
    }

    public int RightPlayerScore
    {
        get => rightPlayerScore;
        set
        {
            rightPlayerScore = value;
            OnPropertyChanged(nameof(RightPlayerScore));
        }
    }
    private void HandleGameEnded(object? sender, GameEndEventArgs e)
    {
        switch (e.GameResult)
        {
            case GameResult.LeftPlayerWin:
                LeftPlayerScore++;
                break;

            case GameResult.RightPlayerWin:
                RightPlayerScore++;
                break;

        }
    }

    private void SetInitialPositions()
    {
        if (GameCanvas == null || paddle1 == null || paddle2 == null || ball == null)
        {
            Console.WriteLine("Error: Game elements not initialized");
            return;
        }
        UpdatePaddleSizes(GameLevel.Beginner);
        if (GameCanvas.ActualWidth == 0 || GameCanvas.ActualHeight == 0)
        {
            Console.WriteLine("Warning: GameCanvas size is 0. Deferring position setting.");
            return;
        }
        Canvas.SetLeft(paddle1, 20);
        double paddle1Top = Math.Max(0, Math.Min(GameCanvas.ActualHeight - paddle1.ActualHeight,
            (GameCanvas.ActualHeight / 2) - (paddle1.ActualHeight / 2)));
        Canvas.SetTop(paddle1, paddle1Top);

        // Paddle 2 (right paddle)
        Canvas.SetLeft(paddle2, GameCanvas.ActualWidth - paddle2.ActualWidth - 20);
        double paddle2Top = Math.Max(0, Math.Min(GameCanvas.ActualHeight - paddle2.ActualHeight,
            (GameCanvas.ActualHeight / 2) - (paddle2.ActualHeight / 2)));
        Canvas.SetTop(paddle2, paddle2Top);

        // Ball
        Canvas.SetLeft(ball, (GameCanvas.ActualWidth - ball.ActualWidth) / 2);
        Canvas.SetTop(ball, (GameCanvas.ActualHeight - ball.ActualHeight) / 2);
    }

    private void HandleWindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
        SetInitialPositions();
    }

    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
        SetInitialPositions();
    }

    private void HandleKeyDown(object sender, KeyEventArgs e)
    {

        double p1Top = Canvas.GetTop(paddle1);
        double p2Top = Canvas.GetTop(paddle2);


        if (e.Key == Key.W && p1Top > 0)
        {
            Canvas.SetTop(paddle1, p1Top - paddleSpeed);
        }
        if (e.Key == Key.S && p1Top < (GameCanvas.ActualHeight - paddle1.ActualHeight))
        {
            Canvas.SetTop(paddle1, p1Top + paddleSpeed);
        }
        if (e.Key == Key.Up && p2Top > 0)
        {
            Canvas.SetTop(paddle2, p2Top - paddleSpeed);
        }
        if (e.Key == Key.Down && p2Top < (GameCanvas.ActualHeight - paddle2.ActualHeight))
        {
            Canvas.SetTop(paddle2, p2Top + paddleSpeed);
        }
    }

    private void Level_ClickHandler(object sender, EventArgs e)
    {
        GameLevel gameLevel;
        if (sender == Beginner_Btn)
        {
            gameLevel = GameLevel.Beginner;
        }
        else if (sender == Intermediate_Btn)
        {
            gameLevel = GameLevel.Intermediate;
        }
        else if (sender == Professional_Btn)
        {
            gameLevel = GameLevel.Professional;
        }
        else
        {
            return;
        }

        StartNewGame(gameLevel);
    }

    private void StartNewGame(GameLevel gameLevel)
    {

        switch (gameLevel)
        {
            case GameLevel.Intermediate:
                paddleSpeed = 12;
                ballSpeedY = 9;
                ballSpeedX = 7;
               
                break;
            case GameLevel.Professional:
                paddleSpeed = 15;
                ballSpeedY = 15;
                ballSpeedX = 10;
              
                break;
            case GameLevel.Beginner:
            default:
                paddleSpeed = 10;
                ballSpeedY = 5;
                ballSpeedX = 5;
                break;
        }

        UpdatePaddleSizes(gameLevel);
        //SetInitialPositions();
    }

    public void InitializeGameElements()
    {

        double paddleWidth = GameCanvas.ActualWidth * 0.015; // 1.5% of canvas width
        double paddleHeight = GameCanvas.ActualHeight * 0.2;
        LargeBallSize = Math.Min(GameCanvas.ActualWidth, GameCanvas.ActualHeight) * 0.04;
        MediumBallSize = Math.Min(GameCanvas.ActualWidth, GameCanvas.ActualHeight) * 0.03;
        SmallBallSize = Math.Min(GameCanvas.ActualWidth, GameCanvas.ActualHeight) * 0.02;
        paddle1 = new Rectangle
        {
            Width = paddleWidth,
            Height = paddleHeight,
            Fill = Brushes.Brown
        };
        paddle2 = new Rectangle
        {
            Width = paddleWidth,
            Height = paddleHeight,
            Fill = Brushes.Brown
        };
     
        ball = new Rectangle
        {
            Width = LargeBallSize,
            Height = LargeBallSize,
            Fill = Brushes.White
        };

        GameCanvas.Children.Add(paddle1);
        GameCanvas.Children.Add(paddle2);
        GameCanvas.Children.Add(ball);
    }

    public void UpdatePaddleSizes(GameLevel level)
    {
        if (paddle1 == null || paddle2 == null || GameCanvas == null || ball == null)
        {
            Console.WriteLine("Error: Paddles or GameCanvas not initialized");
            return;
        }

        double paddleHeightPercentage;
        Brush paddleColor;
        double ballSize;
        switch (level)
        {
            case GameLevel.Intermediate:
                paddleHeightPercentage = 0.25; 
                ballSize = MediumBallSize;

                paddleColor = Brushes.Green;
                break;
            case GameLevel.Professional:
                paddleHeightPercentage = 0.15; 
                ballSize = SmallBallSize;
                paddleColor = Brushes.Blue;
                break;
            case GameLevel.Beginner:
            default:
                paddleHeightPercentage = 0.40;

                ballSize = LargeBallSize; 
               ;
                paddleColor = Brushes.Brown;
                break;
        }

        double paddleHeight = GameCanvas.ActualHeight * paddleHeightPercentage;
        double paddleWidth = GameCanvas.ActualWidth * 0.015; 

        paddle1.Height = paddleHeight;
        paddle1.Width = paddleWidth;
        paddle1.Fill = paddleColor;

        paddle2.Height = paddleHeight;
        paddle2.Width = paddleWidth;
        paddle2.Fill = paddleColor;

        ball.Width = ballSize;
        ball.Height = ballSize;

        gameLevel = level;

        OnPropertyChanged(nameof(gameLevel));
    }

  
}