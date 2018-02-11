using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mozog.Search.Adversarial;
using Mozog.Utils;

namespace Mozog.Search.Examples.Games.PawnChess
{
    public class PawnChessState : State
    {
        private readonly Board board;
        private readonly int movesPlayed;
        private /*readonly*/ PawnChess game; // TODO Make readonly

        public PawnChessState(Board board, string playerToMove, int movesPlayed, PawnChess game)
            : base(playerToMove)
        {
            this.board = board;
            this.movesPlayed = movesPlayed;
            this.game = game;
        }

        // Convenience
        public PawnChessState(string[,] board, string playerToMove, int movesPlayed, PawnChess game)
            : this(new Board(board), playerToMove, movesPlayed, game)
        {
        }

        private bool WhiteToMove => PlayerToMove == PawnChess.White;

        private bool BlackToMove => PlayerToMove == PawnChess.Black;

        private string Opponent => WhiteToMove ? PawnChess.Black : PawnChess.White;

        public static IState CreateInitial(PawnChess game)
            => new PawnChessState(InitialBoard(), PawnChess.White, 0, game);

        private static Board InitialBoard()
            => new Board(PawnChess.Rows, PawnChess.Cols).Initialize(square => square.Switch(PawnChess.Empty,
                (s => s.Is("e1"), PawnChess.WhiteKing),
                (s => s.Is("*2"), PawnChess.WhitePawn),
                (s => s.Is("*7"), PawnChess.BlackPawn),
                (s => s.Is("e8"), PawnChess.BlackKing)));

        #region Move

        // TODO
        public override IEnumerable<IAction> GetLegalMoves()
        {
            throw new NotImplementedException();
        }

        public override IState MakeMove(IAction action)
            => new PawnChessState(NewBoard((PawnChessMove)action), Opponent, movesPlayed + 1, game);

        private Board NewBoard(PawnChessMove move)
            => board.Clone()
                .SetSquare(move.From, PawnChess.Empty)
                .SetSquare(move.To, PlayerToMove);

        #endregion // Move

        protected override GameResult GetResult()
        {
            if (WhiteWon) return GameResult.Win1;
            if (BlackWon) return GameResult.Win2;
            if (Drawn) return GameResult.Draw;
            return GameResult.InProgress;
        }

        // TODO
        private bool WhiteWon { get; }

        // TODO
        private bool BlackWon { get; }

        // TODO
        private bool Drawn { get; }

        protected override double? EvaluateTerminal()
        {
            switch (Result)
            {
                case GameResult.Win1: return +10.0 + (10.0 / movesPlayed);
                case GameResult.Win2: return -10.0 - (10.0 / movesPlayed);
                case GameResult.Draw: return 0.0;
                default: return null; // GameResult.InProgress
            }
        }

        // TODO
        protected override double EvaluateNonTerminal()
        {
            throw new NotImplementedException();
        }

        #region Hashing

        // Zobrist hashing
        protected override int CalculateHash()
        {
            int PieceToIndex(string piece)
            {
                switch (piece)
                {
                    case PawnChess.WhitePawn: return 0;
                    case PawnChess.WhiteKing: return 1;
                    case PawnChess.BlackPawn: return 2;
                    case PawnChess.BlackKing: return 3;
                    default: throw new ArgumentException(nameof(piece));
                }
            }

            int SquareHash(Square square)
                => game.Table[board.SquareToIndex(square), PieceToIndex(board.GetSquare(square))];

            var boardHash = board.Squares.Where(s => s.Piece != PawnChess.Empty)
                .Aggregate(0, (h, s) => h ^ SquareHash(s));

            var playerHash = WhiteToMove ? game.Table_WhiteToMove : game.Table_BlackToMove;

            return boardHash ^ playerHash;
        }

        #endregion //Hashing

        public override string ToString()
        {
            string PrintRow(int row)
                => $"│{string.Join("│", Enumerable.Range(0, board.Cols).Select(c => board[row, c]))}│";

            var Bar = $"{new String('─', 2 * board.Cols + 1)}";

            // Board
            var sb = new StringBuilder();
            sb.Append(Bar).Append(Environment.NewLine);
            for (int r = board.Rows - 1; r >= 0; r--)
                sb.Append(PrintRow(r)).Append(Environment.NewLine)
                    .Append(Bar).Append(Environment.NewLine);

            // Player to move
            var player = PlayerToMove == PawnChess.White ? "White" : "Black";
            sb.Append($"{player} to move").Append(Environment.NewLine);

            sb.Append(Bar);

            return sb.ToString();
        }

        public override string Debug => $"{board.Debug}|{PlayerToMove}";
    }
}
