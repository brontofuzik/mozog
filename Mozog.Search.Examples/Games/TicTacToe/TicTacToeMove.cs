using System;
using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.TicTacToe
{
    public class TicTacToeMove : IAction
    {
        public static TicTacToeMove Parse(string moveStr)
        {
            int moveNumber = Int32.Parse(moveStr);
            switch (moveNumber)
            {
                case 1: return new TicTacToeMove(0, 0);
                case 2: return new TicTacToeMove(0, 1);
                case 3: return new TicTacToeMove(0, 2);

                case 4: return new TicTacToeMove(1, 0);
                case 5: return new TicTacToeMove(1, 1);
                case 6: return new TicTacToeMove(1, 2);

                case 7: return new TicTacToeMove(2, 0);
                case 8: return new TicTacToeMove(2, 1);
                case 9: return new TicTacToeMove(2, 2);

                default: return null;
            }
        }

        public TicTacToeMove(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; }

        public int Col { get; }

        public bool Equals(IAction other)
        {
            var otherMove = (TicTacToeMove)other;
            return Row == otherMove.Row && Col == otherMove.Col;
        }
    }
}
