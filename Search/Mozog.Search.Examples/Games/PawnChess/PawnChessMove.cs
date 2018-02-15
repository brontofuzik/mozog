using System;
using System.Linq;
using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.PawnChess
{
    public class PawnChessMove : IChessboardMove
    {
        // TODO Add check and mate symbols.
        // Pawn: "b2", "axb2"
        // King: "Kb2", "Kxb2"
        public static IAction Parse(string moveStr, PawnChessState currentState)
        {
            var legalMoves = currentState.GetLegalMoves().ToDictionary(m => m.ToString());
            legalMoves.TryGetValue(moveStr, out var move);
            return move;
        }

        public static PawnChessMove King(Square from, Square to, bool isCapture)
            => new PawnChessMove(PawnChess.King, from, to, isCapture);

        public static PawnChessMove Pawn(Square from, Square to, bool isCapture)
            => new PawnChessMove(PawnChess.Pawn, from, to, isCapture);

        public PawnChessMove(char piece, Square from, Square to, bool isCapture)
        {
            Piece = piece;
            From = from;
            To = to;
            this.isCapture = isCapture;
        }

        public char Piece { get; }

        public string Empty => PawnChess.Empty;

        public Square From { get; }

        public Square To { get; }

        private readonly bool isCapture;

        public bool Equals(IAction other)
        {
            var otherMove = (PawnChessMove)other;
            return Piece.Equals(otherMove.Piece) && From.Equals(otherMove.From) && To.Equals(otherMove.To);
        }

        // TODO Add check and mate symbols.
        public override string ToString()
        {
            string piece = Piece == PawnChess.King ? PawnChess.King.ToString() : String.Empty;
            string pawnFrom = Piece == PawnChess.Pawn ? From.ColChar.ToString() : String.Empty;
            string capture = isCapture ? "x" : String.Empty;
            return $"{piece}{pawnFrom}{capture}{To}";
        }
    }
}
