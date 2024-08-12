using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tic_Tac_Toe.Enums;

namespace Tic_Tac_Toe
{
    public class Move
    { public int Row {  get; set; }
        public int Column { get; set; }
        public PlayerTurn Player { get; set; }

        public Move(int row, int column, PlayerTurn player)
        {
            Row = row;
            Column = column;
            Player = player;
        }

    }
}
