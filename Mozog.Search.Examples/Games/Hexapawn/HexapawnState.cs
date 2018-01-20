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

        private int Rows => board.GetLength(0);

        private int Cols => board.GetLength(1);

        public static IState CreateInitial(int rows, int cols)
            => new HexapawnState(InitialBoard(rows, cols), Hexapawn.PlayerW);

        private static string[,] InitialBoard(int rows, int cols)
            => new string[rows, cols].Initialize((r, c) => Utils.Switch(r, Hexapawn.Empty,
                (r1 => r1 == 0, Hexapawn.PlayerW),
                (r2 => r2 == rows - 1, Hexapawn.PlayerB)));

        #region Move

        public override IEnumerable<IAction> GetLegalMoves()
            => Squares.Where(s => s.square == PlayerToMove)
                .SelectMany(s => GetLegalMovesForPiece(s.row, s.col));

        private IEnumerable<IAction> GetLegalMovesForPiece(int row, int col)
        {
            // Move forward
            int newRow = PlayerToMove == Hexapawn.PlayerW ? row + 1 : row - 1;
            if (GetSquare(newRow, col) == Hexapawn.Empty)
                yield return new HexapawnAction(null, IntToChar(col), newRow);

            // Capture right
            int newCol = PlayerToMove == Hexapawn.PlayerW ? col + 1 : col - 1;
            if (GetSquare(newRow, newCol) == Opponent)
                yield return new HexapawnAction(IntToChar(col), IntToChar(newCol), newRow);

            // Capture left
            newCol = PlayerToMove == Hexapawn.PlayerW ? col - 1 : col + 1;
            if (GetSquare(newRow, newCol) == Opponent)
                yield return new HexapawnAction(IntToChar(col), IntToChar(newCol), newRow);
        }

        // Square or null if not within board.
        private string GetSquare(int row, int col)
            => IsWithinBoard(row, col) ? board[row, col] : null;

        private bool IsWithinBoard(int row, int col)
            => 0 <= row && row < Rows && 0 <= col && col < Cols;

        private char IntToChar(int i) => (char)('a' + i);

        private IEnumerable<(int row, int col, string square)> Squares
        {
            get
            {
                for (int r = 0; r < Rows; r++)
                    for (int c = 0; c < Cols; c++)
                        yield return (r, c, board[r, c]);
            }
        }

        public override IState MakeMove(IAction action)
            => new HexapawnState(NewBoard((HexapawnAction)action), Opponent);

        // TODO
        private string[,] NewBoard(HexapawnAction action)
        {
            throw new NotImplementedException();
        }

        private string Opponent
            => PlayerToMove == Hexapawn.PlayerW ? Hexapawn.PlayerB : Hexapawn.PlayerW;

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
            => Squares.Where(s => s.row == Rows - 1).Any(s => s.square == Hexapawn.PlayerW) // White has queened
            || PlayerToMove == Hexapawn.PlayerB && !GetLegalMoves().Any(); // Black has no legal moves

        public bool BlackWon
            => Squares.Where(s => s.row == 0).Any(s => s.square == Hexapawn.PlayerB) // Black has queened
            || PlayerToMove == Hexapawn.PlayerW && !GetLegalMoves().Any(); // White has no legal moves

        #endregion // Evaluate

        // 3x3 only
        public override string ToString()
        {
            string PrintRow(int row) => $"{board[row, 0]}|{board[row, 1]}|{board[row, 2]}{Environment.NewLine}";
            var Bar = $"-----{Environment.NewLine}";

            return new StringBuilder()
                .Append(PrintRow(0)).Append(Bar)
                .Append(PrintRow(1)).Append(Bar)
                .Append(PrintRow(2)).ToString();
        }

        public override string Debug => String.Concat(board.Cast<string>());
    }
}
