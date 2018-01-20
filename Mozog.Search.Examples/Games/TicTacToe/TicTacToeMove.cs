using System;
using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.TicTacToe
{
    public class TicTacToeAction : IAction
    {
        public static TicTacToeAction Parse(string moveStr)
        {
            int moveNumber = Int32.Parse(moveStr);
            switch (moveNumber)
            {
                case 1: return new TicTacToeAction(0, 0);
                case 2: return new TicTacToeAction(0, 1);
                case 3: return new TicTacToeAction(0, 2);

                case 4: return new TicTacToeAction(1, 0);
                case 5: return new TicTacToeAction(1, 1);
                case 6: return new TicTacToeAction(1, 2);

                case 7: return new TicTacToeAction(2, 0);
                case 8: return new TicTacToeAction(2, 1);
                case 9: return new TicTacToeAction(2, 2);

                default: throw new ArgumentException(nameof(moveStr));
            }
        }

        public TicTacToeAction(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; }

        public int Col { get; }
    }
}
