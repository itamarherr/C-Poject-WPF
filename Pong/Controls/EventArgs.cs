using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pong.Enums;

namespace Pong.Controls;

public class GameEndEventArgs : EventArgs
{
    public GameEndEventArgs(GameResult result)
    {
        GameResult = result;

    }

    public GameResult GameResult { get; set; }
}





