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
                var fromRow = player == Hexapawn.White ? toRow - 1 : toRow + 1;
                return new HexapawnMove(moveStr[0], fromRow, moveStr[0], toRow);
            }
            else if (moveStr.Length == 4)
            {
                // Capture move
                var toRow = (int)Char.GetNumericValue(moveStr[3]);
                var fromRow = player == Hexapawn.White ? toRow - 1 : toRow + 1;
                return new HexapawnMove(moveStr[0], fromRow, moveStr[2], toRow);
            }
            else
                return null;
        }

        public HexapawnMove(Square from, Square to)
        {
            From = from;
            To = to;
        }

        public HexapawnMove(char fromCol, int fromRow, char toCol, int toRow)
            : this(new Square(fromCol, fromRow), new Square(toCol, toRow))
        {
        }

        public Square From { get; }

        public Square To { get; }

        private bool IsCapture => From.ColInt != To.ColInt;

        public bool Equals(IAction other)
        {
            var otherMove = (HexapawnMove)other;
            return From.Equals(otherMove.From) && To.Equals(otherMove.To);
        }

        public override string ToString()
            => IsCapture ? $"{From.ColChar}x{To.ToString()}" : To.ToString();
    }
}
