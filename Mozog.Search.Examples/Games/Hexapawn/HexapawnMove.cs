using System;
using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.Hexapawn
{
    public class HexapawnMove : IAction
    {
        // "b2" or "axb2"
        public static HexapawnMove Parse(string moveStr, string player)
        {
            if (moveStr.Length == 2)
            {
                // Normal move
                var toRow = (int)Char.GetNumericValue(moveStr[1]);
                var fromRow = player == Hexapawn.PlayerW ? toRow - 1 : toRow + 1;
                return new HexapawnMove(moveStr[0], fromRow, moveStr[0], toRow);
            }
            else if (moveStr.Length == 4)
            {
                // Capture move
                var toRow = (int)Char.GetNumericValue(moveStr[3]);
                var fromRow = player == Hexapawn.PlayerW ? toRow - 1 : toRow + 1;
                return new HexapawnMove(moveStr[0], fromRow, moveStr[2], toRow);
            }
            else
                return null;
        }

        public HexapawnMove(HexapawnSquare from, HexapawnSquare to)
        {
            From = from;
            To = to;
        }

        public HexapawnMove(char fromCol, int fromRow, char toCol, int toRow)
            : this(new HexapawnSquare(fromCol, fromRow), new HexapawnSquare(toCol, toRow))
        {
        }

        public HexapawnSquare From { get; }

        public HexapawnSquare To { get; }

        private bool IsCapture => From.ColInt != To.ColInt;

        public bool Equals(IAction other)
        {
            var otherMove = (HexapawnMove)other;
            return From.Equals(otherMove.From) && To.Equals(otherMove.To);
        }

        public override string ToString()
            => IsCapture ? $"{From.ColChar}x{To.ToString()}" : To.ToString();
    }

    public struct HexapawnSquare : IEquatable<HexapawnSquare>
    {
        public HexapawnSquare(int col, int row0)
        {
            ColInt = col;
            Row0 = row0;
        }

        public HexapawnSquare(char col, int row1)
            : this(BoardUtils.CharToInt(col), row1 - 1)
        {
        }

        public int ColInt { get; }

        public char ColChar => BoardUtils.IntToChar(ColInt);

        public int Row0 { get; }

        public int Row1 => Row0 + 1;

        public bool Equals(HexapawnSquare other)
            => ColInt == other.ColInt && Row0 == other.Row0;

        public override string ToString() => $"{ColChar}{Row1}";
    }
}
