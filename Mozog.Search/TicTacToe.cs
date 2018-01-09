using System;
using System.Linq;
using System.Collections.Generic;

namespace Mozog.Search
{
    public class TicTacToe : IGame<TicTacToeState, TicTacToeAction, char>
    {
        public const char PlayerX = 'X';
        public const char PlayerO = 'O';

        private TicTacToeState initialState = new TicTacToeState();

        public TicTacToeState InitialState => initialState;

        public IList<char> Players => new List<char>() { PlayerX, PlayerO };

        public IList<TicTacToeAction> GetActions(TicTacToeState state) => state.GetLegalMoves();

        public char GetPlayer(TicTacToeState state) => state.PlayerToMove;

        public TicTacToeState GetReult(TicTacToeState state, TicTacToeAction action) => state.MakeMove(action);

        public double? GetUtility(TicTacToeState state, char player) => state.Utility;

        public bool IsTerminal(TicTacToeState state) => state.IsTerminal;
    }

    public class TicTacToeState
    {
        private Char[,] board = new Char[,]
        {
            { '_', '_', '_' },
            { '_', '_', '_' },
            { '_', '_', '_' }
        };

        private char playerToMove = TicTacToe.PlayerX;

        private double? utility = null;

        public bool IsTerminal => IsGameWon || IsGameDrawn;

        public char PlayerToMove => playerToMove;

        public IList<TicTacToeAction> GetLegalMoves()
        {
            var result = new List<TicTacToeAction>();

            for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++)
                if (board[row, col] == '_')
                    result.Add(new TicTacToeAction(row, col));

            return result;
        }

        public TicTacToeState MakeMove(TicTacToeAction action)
        {
            var newBoard = (Char[,])board.Clone();
            newBoard[action.Row, action.Col] = playerToMove;
            return new TicTacToeState()
            {
                board = newBoard,
                playerToMove = playerToMove == TicTacToe.PlayerX ? TicTacToe.PlayerO : TicTacToe.PlayerX
            };
        }

        public double? Utility
        {
            get
            {
                if (IsGameWon)
                    utility = playerToMove == TicTacToe.PlayerX ? 1.0 : 0.0;
                else if (IsGameDrawn)
                    utility = 0.0;

                return utility;
            }
        }

        private bool IsGameWon
        {
            get
            {
                // Any row?
                for (int row = 0; row < 3; row++)
                    if (IsRowComplete(row)) return true;

                // Any col?
                for (int col = 0; col < 3; col++)
                    if (IsColComplete(col)) return true;

                // Any diag?
                return IsAnyDiagonalComplete;
            }
        }

        // TODO
        private bool IsRowComplete(int row)
        {
            throw new NotImplementedException();
        }

        // TODO
        private bool IsColComplete(int col)
        {
            throw new NotImplementedException();
        }

        // TODO
        private bool IsAnyDiagonalComplete { get; }

        private bool IsGameDrawn => board.Cast<char>().All(c => c != '_');
    }

    public class TicTacToeAction
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
