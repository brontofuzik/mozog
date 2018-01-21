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

        public bool Equals(HexapawnSquare other)
            => ColInt == other.ColInt && Row == other.Row;

        public override string ToString() => $"{ColChar}{Row}";
    }
}
