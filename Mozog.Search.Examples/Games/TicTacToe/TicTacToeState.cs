using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.TicTacToe
{
    public class TicTacToeState : State, IState
    {
        private string[,] board;

        public TicTacToeState(string[,] board, string playerToMove)
            : base(playerToMove)
        {
            this.board = board;
        }

        public static TicTacToeState CreateInitial()
            => new TicTacToeState(InitialBoard, TicTacToe.PlayerX);

        private static string[,] InitialBoard
            => new string[3, 3].Initialize((r, c) => TicTacToe.Empty);

        #region Move

        public override IEnumerable<IAction> GetLegalMoves()
        {
            return Squares.Where(s => s.square == TicTacToe.Empty)
                .Select(s => new TicTacToeMove(s.row, s.col));
        }

        private IEnumerable<(int row, int col, string square)> Squares
        {
            get
            {
                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 3; c++)
                        yield return (r, c, board[r, c]);
            }
        }

        public override IState MakeMove(IAction action)
            => new TicTacToeState(NewBoard(action as TicTacToeMove), Opponent);

        private string[,] NewBoard(TicTacToeMove action)
        {
            var newBoard = (string[,])board.Clone();
            newBoard[action.Row, action.Col] = PlayerToMove;
            return newBoard;
        }

        private string Opponent
            => PlayerToMove == TicTacToe.PlayerX ? TicTacToe.PlayerO : TicTacToe.PlayerX;

        #endregion // Move

        #region Evaluate

        protected override double? Evaluate()
        {
            if (IsGameWon(TicTacToe.PlayerX)) return +1.0;
            else if (IsGameWon(TicTacToe.PlayerO)) return -1.0;
            else if (IsGameDrawn) return 0.0;
            else return null;
        }

        private bool IsGameWon(string player)
            => IsAnyRowComplete(player) || IsAnyColComplete(player) || IsAnyDiagonalComplete(player);

        private bool IsAnyRowComplete(string player)
        {
            bool IsRowComplete(int row) => Row(row).All(p => p == player);

            for (int row = 0; row < 3; row++)
                if (IsRowComplete(row)) return true;
            return false;
        }

        private string[] Row(int r) => new string[] { board[r, 0], board[r, 1], board[r, 2] };

        private bool IsAnyColComplete(string player)
        {
            bool IsColComplete(int col) => Col(col).All(p => p == player);

            for (int col = 0; col < 3; col++)
                if (IsColComplete(col)) return true;
            return false;
        }

        private string[] Col(int c) => new string[] { board[0, c], board[1, c], board[2, c] };

        private bool IsAnyDiagonalComplete(string player)
            => PrimaryDiagonal.All(p => p == player) || SecondaryDiagonal.All(p => p == player);

        private string[] PrimaryDiagonal => new string[] { board[0, 0], board[1, 1], board[2, 2] };

        private string[] SecondaryDiagonal => new string[] { board[0, 2], board[1, 1], board[2, 0] };

        private bool IsGameDrawn => board.Cast<string>().All(c => c != TicTacToe.Empty);

        #endregion // Evaluate

        public override string ToString()
        {
            string PrintRow(int row) => $"{board[row, 0]}│{board[row, 1]}│{board[row, 2]}{Environment.NewLine}";
            var Bar = $"─────{Environment.NewLine}";

            return new StringBuilder()
                .Append(PrintRow(0)).Append(Bar)
                .Append(PrintRow(1)).Append(Bar)
                .Append(PrintRow(2)).ToString();
        }

        public override string Debug => String.Concat(board.Cast<string>());
    }
}
