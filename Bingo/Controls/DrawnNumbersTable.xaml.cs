using System;
using System.Collections.Generic;
using System.Linq;
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
using Bingo;
using Bingo.Models;

namespace Bingo.Controls;

/// <summary>
/// Interaction logic for DrawnNumbersTable.xaml
/// </summary>
public partial class DrawnNumbersTable : UserControl
{
    
    private const int Rows = 20;
    private const int Columns = 5;
    private DrawnNumbersManager drawnNumbersManager;
     private Dictionary<int, TextBlock>? numberCells;
    private int numberIndex;
    public DrawnNumbersTable()
    {
        InitializeComponent();
        drawnNumbersManager = new DrawnNumbersManager();
        numberCells = new Dictionary<int, TextBlock>();
       
    }

    public void InitializeDrawnNumbersGrid(DrawnNumbersManager manager)
    {
        DrawnNumbersGrid.Children.Clear();
        DrawnNumbersGrid.RowDefinitions.Clear();
        DrawnNumbersGrid.ColumnDefinitions.Clear();
        numberCells.Clear();

        int number = 1;


        for (int row = 0; row<Rows; row++)
        {
            DrawnNumbersGrid.RowDefinitions.Add(new RowDefinition());

            for (int col = 0; col<Columns; col++)
            {
                DrawnNumbersGrid.ColumnDefinitions.Add(new ColumnDefinition());

                    TextBlock textBlock = new TextBlock
                    {
                        Text = number.ToString(),
                        Margin = new Thickness(5),
                      
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Background = new SolidColorBrush(Colors.LightGray)
                    };
                    Grid.SetRow(textBlock, row);
                    Grid.SetColumn(textBlock, col);
                    DrawnNumbersGrid.Children.Add(textBlock);

                    numberCells[number] = textBlock;
                    number++;
                
               
                
            }
        }
       
    }
    private int GetNumberForGrid(int row, int col)
    {

        
            return drawnNumbersManager.DrawRandomNumber(); 
        
    }

    public void MarkNumber(int number)
    {
        if (numberCells.TryGetValue(number, out TextBlock cell))
        {
            cell.Background = Brushes.Gray;
            cell.TextDecorations = TextDecorations.Strikethrough;
        }
    }


}
