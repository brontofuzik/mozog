using System;
using System.Linq;
using System.Collections.Generic;

namespace Mozog.Search
{
    public class TicTacToe : IGame
    {
        public const string PlayerX = "X";
        public const string PlayerO = "O";

        private TicTacToeState initialState = new TicTacToeState();

        public IState InitialState => initialState;

        public IList<string> Players => new List<string>() { PlayerX, PlayerO };

        public IList<IAction> GetActions(IState state) => state.GetLegalMoves();

        public string GetPlayer(IState state) => state.PlayerToMove;

        public IState GetResult(IState state, IAction action) => state.MakeMove(action);

        public double? GetUtility(IState state, string player)
        {
            var u = state.Evaluation;
            if (!u.HasValue) return null;
            return player == PlayerX ? u : 1 - u;
        }

        public bool IsTerminal(IState state) => state.IsTerminal;
    }

    public class TicTacToeState : IState
    {
        private string[,] board = new string[,]
        {
            { "_", "_", "_" },
            { "_", "_", "_" },
            { "_", "_", "_" }
        };

        private string playerToMove = TicTacToe.PlayerX;

        private double? evaluation = null;

        public bool IsTerminal => IsGameWon || IsGameDrawn;

        public string PlayerToMove => playerToMove;

        public IList<IAction> GetLegalMoves()
        {
            var result = new List<IAction>();

            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    if (board[row, col] == "_")
                        result.Add(new TicTacToeAction(row, col));

            return result;
        }

        public IState MakeMove(IAction action)
        {
            return new TicTacToeState()
            {
                board = NewBoard(action as TicTacToeAction),
                playerToMove = playerToMove == TicTacToe.PlayerX ? TicTacToe.PlayerO : TicTacToe.PlayerX
            };
        }

        private string[,] NewBoard(TicTacToeAction action)
        {
            var newBoard = (string[,])board.Clone();
            newBoard[action.Row, action.Col] = playerToMove;
            return newBoard;
        }

        public double? Evaluation
        {
            get
            {
                if (IsGameWon)
                    evaluation = playerToMove == TicTacToe.PlayerX ? 1.0 : 0.0;
                else if (IsGameDrawn)
                    evaluation = 0.5;

                return evaluation;
            }
        }

        private bool IsGameWon
            => IsAnyRowComplete() || IsAnyColComplete() || IsAnyDiagonalComplete;

        private bool IsAnyRowComplete()
        {
            bool IsRowComplete(int row)
                => board[row, 0] != "_" && board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2];

            for (int row = 0; row < 3; row++)
                if (IsRowComplete(row)) return true;
            return false;
        }

        private bool IsAnyColComplete()
        {
            bool IsColComplete(int col)
                => board[0, col] != "_" && board[0, col] == board[1, col] && board[1, col] == board[2, col];

            for (int col = 0; col < 3; col++)
                if (IsColComplete(col)) return true;
            return false;
        }

        private bool IsAnyDiagonalComplete
            => board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]
            || board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]; 

        private bool IsGameDrawn => board.Cast<string>().All(c => c != "_");
    }

    public class TicTacToeAction : IAction
    {
        public TicTacToeAction(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; }

        public int Col { get; }
    }
}
