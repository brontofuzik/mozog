﻿using System.Linq;
using System.Collections.Generic;
using Mozog.Search.Adversarial;
using System;
using System.Text;

namespace Mozog.Search.Examples.Adversarial
{
    public class TicTacToe : IGame
    {
        internal static void Play_Minimax()
        {
            var ticTacToe = new TicTacToe();
            var engine = GameEngine.Minimax(ticTacToe);
            engine.Play();
        }

        internal static void Play_AlphaBeta()
        {
            var ticTacToe = new TicTacToe();
            var engine = GameEngine.AlphaBeta(ticTacToe);
            engine.Play();
        }

        public const string PlayerX = "X";
        public const string PlayerO = "O";
        public const string Empty = " ";

        public IState InitialState { get; } = TicTacToeState.CreateInitial();

        public IList<string> Players => new List<string>() { PlayerX, PlayerO };

        public Objective GetObjective(string player)
            => player == PlayerX ? Objective.Max : Objective.Min;

        public IList<IAction> GetActions(IState state) => state.GetLegalMoves();

        public string GetPlayer(IState state) => state.PlayerToMove;

        public IState GetResult(IState state, IAction action) => state.MakeMove(action);

        public double? GetUtility(IState state) => state.Evaluation;

        public bool IsTerminal(IState state) => state.IsTerminal;

        public string PrintState(IState state)
        {
            return state.ToString();
        }

        public IAction ParseMove(string moveStr)
        {
            int moveNumber = Int32.Parse(moveStr);
            switch (moveNumber)
            {
                case 1: return new TicTacToeAction(0, 0);
                case 2: return new TicTacToeAction(0, 1);
                case 3: return new TicTacToeAction(0, 2);

                case 4: return new TicTacToeAction(1, 0);
                case 5: return new TicTacToeAction(1, 1);
                case 6: return new TicTacToeAction(1, 2);

                case 7: return new TicTacToeAction(2, 0);
                case 8: return new TicTacToeAction(2, 1);
                case 9: return new TicTacToeAction(2, 2);

                default: throw new ArgumentException(nameof(moveStr));
            }
        }
    }

    public class TicTacToeState : IState
    {
        private string[,] board;

        private double? evaluation = null;

        public TicTacToeState(string[,] board, string playerToMove)
        {
            this.board = board;
            PlayerToMove = playerToMove;
        }

        public static TicTacToeState CreateInitial()
            => new TicTacToeState(EmptyBoard, TicTacToe.PlayerX);

        private static string[,] EmptyBoard => new string[,]
        {
            { TicTacToe.Empty, TicTacToe.Empty, TicTacToe.Empty },
            { TicTacToe.Empty, TicTacToe.Empty, TicTacToe.Empty },
            { TicTacToe.Empty, TicTacToe.Empty, TicTacToe.Empty }
        };

        public string PlayerToMove { get; private set; }

        public bool IsTerminal => Evaluation.HasValue;

        public IList<IAction> GetLegalMoves()
        {
            var result = new List<IAction>();

            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    if (board[row, col] == TicTacToe.Empty)
                        result.Add(new TicTacToeAction(row, col));

            return result;
        }

        public IState MakeMove(IAction action)
        {
            return new TicTacToeState(NewBoard(action as TicTacToeAction),
                PlayerToMove == TicTacToe.PlayerX ? TicTacToe.PlayerO : TicTacToe.PlayerX);
        }

        private string[,] NewBoard(TicTacToeAction action)
        {
            var newBoard = (string[,])board.Clone();
            newBoard[action.Row, action.Col] = PlayerToMove;
            return newBoard;
        }

        public double? Evaluation
        {
            get
            {
                if (evaluation.HasValue)
                    return evaluation;

                if (IsGameWon(TicTacToe.PlayerX))
                    evaluation = 1.0;
                else if (IsGameWon(TicTacToe.PlayerO))
                    evaluation = -1.0;
                else if (IsGameDrawn)
                    evaluation = 0.0;

                return evaluation;
            }
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

        public override string ToString()
        {
            const string Bar = "-----";

            var sb = new StringBuilder();

            sb.Append($"{board[0, 0]}|{board[0, 1]}|{board[0, 2]}").Append(Environment.NewLine)
                .Append(Bar).Append(Environment.NewLine)
                .Append($"{board[1, 0]}|{board[1, 1]}|{board[1, 2]}").Append(Environment.NewLine)
                .Append(Bar).Append(Environment.NewLine)
                .Append($"{board[2, 0]}|{board[2, 1]}|{board[2, 2]}");

            return sb.ToString();
        }

        public string Debug()
        {
            return board[0,0] + board[0,1] + board[0,2] +
                board[1,0] + board[1,1] + board[1,2] +
                board[2,0] + board[2,1] + board[2,2];
        }
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
