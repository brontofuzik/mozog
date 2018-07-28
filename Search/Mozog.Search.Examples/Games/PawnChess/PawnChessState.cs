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
        private readonly PawnChess game;

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

        public override IEnumerable<IAction> GetLegalMoves()
        {
            var pawnMoves = board.FindPieces(PlayersPawn(PlayerToMove)).SelectMany(GetPawnMoves);
            var kingMoves = GetKingMoves(board.FindPiece(PlayersKing(PlayerToMove)).Value);
            return pawnMoves.Concat(kingMoves);
        }

        private IEnumerable<IAction> GetPawnMoves(Square pawn)
        {
            int newRow = PlayerToMove == PawnChess.White ? pawn.Row0 + 1 : pawn.Row0 - 1;

            // Move forward
            var to = new Square(row0: newRow, col0: pawn.Col0);
            if (CanMoveToSquare(pawn, to))
            {
                var move = PawnChessMove.Pawn(pawn, to, isCapture: false);
                if (!KingChecked(PlayerToMove, board.MakeMove(move)))
                    yield return move;
            }

            // Capture left
            to = new Square(row0: newRow, col0: pawn.Col0 - 1);
            if (CanMoveToSquare(pawn, to) && PlayerFromSquare(board[to]) == Opponent)
            {
                var move = PawnChessMove.Pawn(pawn, to, isCapture: true);
                if (!KingChecked(PlayerToMove, board.MakeMove(move)))
                    yield return move;
            }

            // Capture right
            to = new Square(row0: newRow, col0: pawn.Col0 + 1);
            if (CanMoveToSquare(pawn, to) && PlayerFromSquare(board[to]) == Opponent)
            {
                var move = PawnChessMove.Pawn(pawn, to, isCapture: true);
                if (!KingChecked(PlayerToMove, board.MakeMove(move)))
                    yield return move;
            }
        }

        private IEnumerable<IAction> GetKingMoves(Square king)
        {
            var toSquares = new List<Square>
            {
                new Square(row0: king.Row0 + 1, col0: king.Col0 - 1), // Up & left
                new Square(row0: king.Row0 + 1, col0: king.Col0    ), // Up
                new Square(row0: king.Row0 + 1, col0: king.Col0 + 1), // Up & right
                new Square(row0: king.Row0,     col0: king.Col0 + 1), // Right
                new Square(row0: king.Row0 - 1, col0: king.Col0 + 1), // Down & right
                new Square(row0: king.Row0 - 1, col0: king.Col0    ), // Down
                new Square(row0: king.Row0 - 1, col0: king.Col0 - 1), // Down & left
                new Square(row0: king.Row0,     col0: king.Col0 - 1)  // Left
            };

            return toSquares.Where(s => CanMoveToSquare(king, s))
                .Select(s => PawnChessMove.King(king, s, isCapture: PlayerFromSquare(board[s]) == Opponent))
                .Where(IsLegalMove);
        }

        // Target square is within board and doesn't contain own piece
        private bool CanMoveToSquare(Square from, Square to)
            => board.IsWithinBoard(row: to.Row0, col: to.Col0) && PlayerFromSquare(board[to.Row0, to.Col0]) != PlayerFromSquare(from.Piece);

        private bool IsLegalMove(PawnChessMove move)
            => !KingChecked(PlayerToMove, board.MakeMove(move));

        public override IState MakeMove(IAction move)
            => new PawnChessState(board.MakeMove((IChessboardMove)move), Opponent, movesPlayed + 1, game);

        #endregion // Move

        #region Result

        protected override GameResult GetResult()
        {
            // Only check for White win if it's Black's turn
            if (BlackToMove && WhiteWon) return GameResult.Win1;

            // Only check for Black win if it's White's turn
            if (WhiteToMove && BlackWon) return GameResult.Win2;

            if (Drawn) return GameResult.Draw;

            return GameResult.InProgress;
        }

        private bool WhiteWon => PawnPromoted(PawnChess.White) || KingMated(PawnChess.Black);

        private bool BlackWon => PawnPromoted(PawnChess.Black) || KingMated(PawnChess.White);

        private bool Drawn => !KingChecked(PlayerToMove) && GetLegalMoves().None();

        private bool PawnPromoted(string player)
            => player == PawnChess.WhitePawn
                ? board.Squares.Where(s => s.Row1 == board.Rows).Any(s => s.Piece == PawnChess.WhitePawn)
                : board.Squares.Where(s => s.Row1 == 1).Any(s => s.Piece == PawnChess.BlackPawn);

        private bool KingMated(string player) => KingChecked(player) && GetLegalMoves().None();

        private bool KingChecked(string player, Board newBoard = null)
        {
            var kingSquare = (newBoard ?? board).FindPiece(PlayersKing(player)).Value; // A king is always on the board
            var threatenedSqaures = (newBoard ?? board).FindPieces(PlayersPawn(player)).SelectMany(s => GetThreatenedSquares(s, player));
            return threatenedSqaures.Contains(kingSquare);
        }

        private IEnumerable<Square> GetThreatenedSquares(Square pawn, string player)
        {
            if (player == PawnChess.White)
            {
                if (board.IsWithinBoard(row: pawn.Row0 + 1, col: pawn.Col0 - 1))
                    yield return new Square(row0: pawn.Row0 + 1, col0: pawn.Col0 - 1);

                if (board.IsWithinBoard(row: pawn.Row0 + 1, col: pawn.Col0 + 1))
                    yield return new Square( row0: pawn.Row0 + 1, col0: pawn.Col0 + 1);
            }
            else
            {
                if (board.IsWithinBoard(row: pawn.Row0 - 1, col: pawn.Col0 - 1))
                    yield return new Square(row0: pawn.Row0 - 1, col0: pawn.Col0 - 1);

                if (board.IsWithinBoard(row: pawn.Row0 - 1, col: pawn.Col0 + 1))
                    yield return new Square(row0: pawn.Row0 - 1, col0: pawn.Col0 + 1);
            }
        }

        // Colour to piece
        private static string PlayersKing(string player)
            => player == PawnChess.White ? PawnChess.WhiteKing : PawnChess.BlackKing;

        // Colour to piece
        private static string PlayersPawn(string player)
            => player == PawnChess.White ? PawnChess.WhitePawn : PawnChess.BlackPawn;

        // Piece to colour
        private static string PlayerFromSquare(string piece)
        {
            switch (piece)
            {
                case PawnChess.WhiteKing:
                case PawnChess.WhitePawn:
                    return PawnChess.White;
                case PawnChess.BlackKing:
                case PawnChess.BlackPawn:
                    return PawnChess.Black;
                default:
                    return null;
            }
        }

        #endregion // Result

        #region Evaluate

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

        // TODO Add more features.
        protected override double EvaluateNonTerminal()
        {
            return MaterialBalance();
        }

        private double MaterialBalance()
        {
            int whitePawns = board.FindPieces(PawnChess.WhitePawn).Count();
            int blackPawns = board.FindPieces(PawnChess.BlackPawn).Count();
            return whitePawns - blackPawns;
        }

        #endregion // Evaluate

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
