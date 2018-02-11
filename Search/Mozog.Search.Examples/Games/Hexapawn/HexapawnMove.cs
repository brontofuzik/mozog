using System;
using System.Linq;
using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.Hexapawn
{
    public class HexapawnMove : IChessboardMove
    {
        // "b2" or "axb2"
        public static IAction Parse(string moveStr, HexapawnState currentState)
        {
            var legalMoves = currentState.GetLegalMoves().ToDictionary(m => m.ToString());
            legalMoves.TryGetValue(moveStr, out var move);
            return move;
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

        public char Piece => throw new NotImplementedException();

        public string Empty => Hexapawn.Empty;

        public Square From { get; }

        public Square To { get; }

        private bool IsCapture => From.Col0 != To.Col0;

        public bool Equals(IAction other)
        {
            var otherMove = (HexapawnMove)other;
            return From.Equals(otherMove.From) && To.Equals(otherMove.To);
        }

        public override string ToString()
            => IsCapture ? $"{From.ColChar}x{To}" : To.ToString();
    }
}
