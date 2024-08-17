using System.Reflection;
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
using Bingo.Controls;
using Bingo.Models;
using Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bingo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<int> bingoGridNumbers = new List<int>();
    private List<int> drawnNumbersGridNumbers = new List<int>();
    private const int GridSize = 5;
    private Button[,] buttons = new Button[GridSize, GridSize];
    private DrawnNumbersManager drawnNumbersManager;
    private readonly Button[,] _buttons = new Button[5, 5];
 

    public HashSet<int> UsedNumbers { get; private set; }

    public MainWindow()
    {
        InitializeComponent();
     

        drawnNumbersManager = new DrawnNumbersManager();
        MyDrawnNumbersTable.InitializeDrawnNumbersGrid(drawnNumbersManager);
        InitializeBingoGrid();
        
    }

    private void InitializeBingoGrid()
    {
        BingoGrid.Children.Clear();
        BingoGrid.RowDefinitions.Clear();
        BingoGrid.ColumnDefinitions.Clear();

        bingoGridNumbers = drawnNumbersManager.GenerateUniqueNumbers(GridSize * GridSize);


        for (int i = 0; i < GridSize; i++)
        {
            BingoGrid.RowDefinitions.Add(new RowDefinition());
            BingoGrid.ColumnDefinitions.Add(new ColumnDefinition());
        }

        int numberIndex = 0;
        for (int row = 0; row < GridSize; row++)
        {
            for (int col = 0; col < GridSize; col++)
            {
                
                Button button = new Button
                {
                    Content = bingoGridNumbers[numberIndex++].ToString(),
                    Margin = new Thickness(5),
                    FontSize = 24,
                 

                };
                button.Click += MatchingNumber;
                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
                BingoGrid.Children.Add(button);
                buttons[row, col] = button;
                _buttons[row, col] = button;

            }
        }
        DrawNumberText.Text = "";
    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            button.IsEnabled = false;
            button.Background = SystemColors.ControlDarkBrush;
            button.Content = "BINGO";
            if (CheckForWinner())
            {
                EndGame();
            }
        }
    }
    private void MatchingNumber(object sender, RoutedEventArgs e)

    {
        if(sender is Button button)
        {
            int clickedNumber;
            if (int.TryParse(button.Content.ToString(), out clickedNumber))
            {
                if (drawnNumbersGridNumbers.Contains(clickedNumber))
                {
                    Button_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Sorry, this number doesn't match the drawn number.", "Incorrect Number", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            
             else
            {
                MessageBox.Show("Invalid button content.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
   

    private void NewGame_Click(object sender, RoutedEventArgs e)
    {
        drawnNumbersManager.Reset();
        InitializeBingoGrid();
        MyDrawnNumbersTable.InitializeDrawnNumbersGrid(drawnNumbersManager);
        drawnNumbersGridNumbers.Clear();

    }

    //public void  ResetGame_Click(object sender, RoutedEventArgs e)
    //{
    //    foreach (var button in buttons)
    //    {
    //        button.IsEnabled = true;
    //        button.Background = SystemColors.ControlDarkBrush;
    //    }
    //}
    private void DrawNumber_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int drawnNumber = drawnNumbersManager.DrawRandomNumber();
            DrawNumberText.Text = drawnNumber.ToString();
            drawnNumbersGridNumbers.Add(drawnNumber);
            MyDrawnNumbersTable.MarkNumber(drawnNumber);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public void EndGame()
    {
        if (CheckForWinner())
        {
            MessageBox.Show("Congratulations! You won the game");
            var newGameButton = FindName("NewGameButton") as Button; 
            if (newGameButton != null) 
            {
                {
                    RoutedEventArgs args = new RoutedEventArgs(Button.ClickEvent);
                    newGameButton.RaiseEvent(args);
                }
            }


        }
    }
    private bool CheckForWinner()
    {
        for (int i = 0; i < 5; i++)
        {
         
            if (AreButtonsEqual(_buttons[i, 0], _buttons[i, 1], _buttons[i, 2], _buttons[i, 3], _buttons[i, 4]))
            {
                return true;
            }


            if (AreButtonsEqual(_buttons[0, i], _buttons[1, i], _buttons[2, i], _buttons[3, i], _buttons[4, i]))
            {
                return true;
            }
        }

        if (AreButtonsEqual(_buttons[0, 0], _buttons[1, 1], _buttons[2, 2], _buttons[3, 3], _buttons[4, 4]))
        {
            return true;
        }
        if (AreButtonsEqual(_buttons[0, 4], _buttons[1, 3], _buttons[2, 2], _buttons[3, 1], _buttons[4, 0]))
        {
            return true;
        }

        return false;
    }


    private bool AreButtonsEqual(params Button[] buttons)
    {
       
        if (buttons == null || buttons.Length == 0 || buttons.Any(button => button == null || button.Content == null))
        {
            return false;
        }

        var firstContent = buttons[0].Content;
        return buttons.All(button => button.Content.Equals(firstContent));
    }

    public string one;
}