using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.Hexapawn
{
    public class HexapawnState : State, IState
    {
        private string[,] board;

        public HexapawnState(string[,] board, string playerToMove)
            : base(playerToMove)
        {
            this.board = board;
        }

        private string Opponent
            => PlayerToMove == Hexapawn.PlayerW ? Hexapawn.PlayerB : Hexapawn.PlayerW;

        public static IState CreateInitial(int rows, int cols)
            => new HexapawnState(InitialBoard(rows, cols), Hexapawn.PlayerW);

        private static string[,] InitialBoard(int rows, int cols)
            => new string[rows, cols].Initialize((r, c) => Utils.Switch(r, Hexapawn.Empty,
                (r1 => r1 == 0, Hexapawn.PlayerW),
                (r2 => r2 == rows - 1, Hexapawn.PlayerB)));

        #region Move

        public override IEnumerable<IAction> GetLegalMoves()
            => board.Squares().Where(s => s.square == PlayerToMove)
                .SelectMany(s => GetLegalMovesForPiece(s.row, s.col));

        private IEnumerable<IAction> GetLegalMovesForPiece(int row, int col)
        {
            var current = new HexapawnSquare(col, row);
            int newRow = PlayerToMove == Hexapawn.PlayerW ? row + 1 : row - 1;

            var moveForward = new HexapawnSquare(col, newRow);
            if (board.GetSquare(moveForward) == Hexapawn.Empty)
                yield return new HexapawnMove(current, moveForward);

            var captureRight = new HexapawnSquare(PlayerToMove == Hexapawn.PlayerW ? col + 1 : col - 1, newRow);
            if (board.GetSquare(captureRight) == Opponent)
                yield return new HexapawnMove(current, captureRight);

            var captureLeft = new HexapawnSquare(PlayerToMove == Hexapawn.PlayerW ? col - 1 : col + 1, newRow);
            if (board.GetSquare(captureLeft) == Opponent)
                yield return new HexapawnMove(current, captureLeft);
        }

        public override IState MakeMove(IAction action)
            => new HexapawnState(NewBoard((HexapawnMove)action), Opponent);

        private string[,] NewBoard(HexapawnMove action)
        {
            var newBoard = (string[,])board.Clone();
            newBoard.SetSquare(action.From, Hexapawn.Empty);
            newBoard.SetSquare(action.To, PlayerToMove);
            return newBoard;
        }

        #endregion // Move

        #region Evaluate

        // No draws?
        protected override double? Evaluate()
        {
            if (WhiteWon) return +1.0;
            else if (BlackWon) return -1.0;
            else return null;
        }

        public bool WhiteWon
            => board.Squares().Where(s => s.row == board.Rows() - 1).Any(s => s.square == Hexapawn.PlayerW) // White has queened
            || PlayerToMove == Hexapawn.PlayerB && !GetLegalMoves().Any(); // Black has no legal moves

        public bool BlackWon
            => board.Squares().Where(s => s.row == 0).Any(s => s.square == Hexapawn.PlayerB) // Black has queened
            || PlayerToMove == Hexapawn.PlayerW && !GetLegalMoves().Any(); // White has no legal moves

        #endregion // Evaluate

        // 3x3 only
        public override string ToString()
        {
            string PrintRow(int row) => $"│{board[row, 0]}│{board[row, 1]}│{board[row, 2]}│{Environment.NewLine}";
            var Bar = $"───────{Environment.NewLine}";

            return new StringBuilder()
                .Append(Bar)
                .Append(PrintRow(2)).Append(Bar)
                .Append(PrintRow(1)).Append(Bar)
                .Append(PrintRow(0)).Append(Bar)
                .ToString();
        }

        public override string Debug => String.Concat(board.Cast<string>());
    }

    internal static class BoardUtils
    {
        public static int Rows(this string[,] board) => board.GetLength(0);

        public static int Cols(this string[,] board) => board.GetLength(1);

        //// Current board by default
        //public static string GetSquare(this string[,] board, char col, int row)
        //    => board[row, CharToInt(col)];

        // Current board by default
        public static string GetSquare(this string[,] board, HexapawnSquare square)
            => board.IsWithinBoard(square.Row, square.ColInt) ? board[square.Row, square.ColInt] : null;

        //// Current board by default
        //public static void SetSquare(this string[,] board, char col, int row, string value)
        //    => board[row, CharToInt(col)] = value;

        // Current board by default
        public static void SetSquare(this string[,] board, HexapawnSquare square, string value)
        {
            if (board.IsWithinBoard(square.Row, square.ColInt))
                board[square.Row, square.ColInt] = value;
            else
                throw new ArgumentException(nameof(square));
        }

        //// Square or null if not within board.
        //public static string GetSquare(this string[,] board, int row, int col)
        //    => board.IsWithinBoard(row, col) ? board[row, col] : null;

        private static bool IsWithinBoard(this string[,] board, int row, int col)
            => 0 <= row && row < board.Rows() && 0 <= col && col < board.Cols();

        public static IEnumerable<(int row, int col, string square)> Squares(this string[,] board)
        {
            for (int r = 0; r < board.Rows(); r++)
                for (int c = 0; c < board.Cols(); c++)
                    yield return (r, c, board[r, c]);
        }

        public static char IntToChar(int i) => (char)('a' + i);

        public static int CharToInt(char c) => c - 'a';
    }
}
