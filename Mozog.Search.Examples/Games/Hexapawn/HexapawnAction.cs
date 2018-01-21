using System;
using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.Hexapawn
{
    public class HexapawnAction : IAction
    {
        // "b2" or "axb2"
        public static HexapawnAction Parse(string moveStr)
        {
            if (moveStr.Length == 2)
            {
                // Normal move
                return new HexapawnAction(new HexapawnSquare(moveStr[0], moveStr[1]));
            }
            else if (moveStr.Length == 4)
            {
                // Capture move
                var to = new HexapawnSquare(moveStr[2], moveStr[3]);
                var from = new HexapawnSquare(moveStr[0], to.Row - 1);
                return new HexapawnAction(to, from);
            }
            else
                throw new ArgumentException(nameof(moveStr));
        }

        public HexapawnAction(HexapawnSquare to, HexapawnSquare? from = null)
        {
            To = to;
            From = from ?? new HexapawnSquare(to.ColInt, to.Row - 1);
        }

        public HexapawnSquare From { get; }

        public HexapawnSquare To { get; }
    }

    public struct HexapawnSquare
    {
        public HexapawnSquare(int col, int row)
        {
            ColInt = col;
            Row = row;
        }

        public HexapawnSquare(char col, int row)
            : this(BoardUtils.CharToInt(col), row)
        {
        }

        public int ColInt { get; }

        public char ColChar => BoardUtils.IntToChar(ColInt);

        public int Row { get; }
    }
}
