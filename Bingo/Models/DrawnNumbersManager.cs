using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingo.Models;

public class DrawnNumbersManager
{
    public Random random;
    public List<int> DrawNumbers {  get; private set; }
    public HashSet<int> UsedNumbers { get; private set; }
    

    public DrawnNumbersManager()
    {
        random = new Random();
        InitializeDrawNumbers();
    }

    private void InitializeDrawNumbers()
    {
        DrawNumbers = Enumerable.Range(1, 100).OrderBy(n => random.Next()).ToList();
        UsedNumbers = new HashSet<int>();
    }
    public List<int> GenerateUniqueNumbers(int count)
    {
        return DrawNumbers.OrderBy(n => random.Next()).Take(count).ToList();
    }


    public int DrawRandomNumber()
    {
        if (UsedNumbers.Count < 100)
        {
            int number;
            do
            {
                number = DrawNumbers[random.Next(DrawNumbers.Count)];
            } while (UsedNumbers.Contains(number));

            UsedNumbers.Add(number);
            return number;
        }
        throw new InvalidOperationException("No more numbers to draw");
    }
    public void MarkNumberAsUsed(int number)
    {
        UsedNumbers.Add(number);
    }

    public bool IsNumberUsed(int number)
    {
        return UsedNumbers.Contains(number);
    }

    public void Reset()
    {
        InitializeDrawNumbers();
    }
}
