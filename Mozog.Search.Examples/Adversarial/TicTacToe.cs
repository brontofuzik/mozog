using System.Linq;
using System.Collections.Generic;
using Mozog.Search.Adversarial;
using System;
using System.Text;

namespace Mozog.Search.Examples.Adversarial
{
    public class TicTacToe : IGame
    {
        public static void Run()
        {
            var ticTacToe = new TicTacToe();
            var engine = new GameEngine(ticTacToe);
            engine.Run();
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

        public double? GetUtility(IState state, string player)
        {
            var u = state.Evaluation;
            if (!u.HasValue) return null;
            return player == PlayerX ? u : 1 - u;
        }

        public double? GetUtility_NEW(IState state) => state.Evaluation_NEW;

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

        public bool IsTerminal => IsGameWon || IsGameDrawn;

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
                if (IsGameWon)
                    evaluation = PlayerToMove == TicTacToe.PlayerX ? 1.0 : 0.0;
                else if (IsGameDrawn)
                    evaluation = 0.5;

                return evaluation;
            }
        }

        public double? Evaluation_NEW
        {
            get
            {
                if (IsGameWon)
                    evaluation = PlayerToMove == TicTacToe.PlayerX ? 1.0 : -1.0;
                else if (IsGameDrawn)
                    evaluation = 0.0;

                return evaluation;
            }
        }

        private bool IsGameWon
            => IsAnyRowComplete || IsAnyColComplete || IsAnyDiagonalComplete;

        private bool IsAnyRowComplete
        {
            get
            {
                bool IsRowComplete(int row)
                    => board[row, 0] != TicTacToe.Empty && board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2];

                for (int row = 0; row < 3; row++)
                    if (IsRowComplete(row)) return true;
                return false;
            }
        }

        private bool IsAnyColComplete
        {
            get
            {
                bool IsColComplete(int col)
                    => board[0, col] != TicTacToe.Empty && board[0, col] == board[1, col] && board[1, col] == board[2, col];

                for (int col = 0; col < 3; col++)
                    if (IsColComplete(col)) return true;
                return false;
            }
        }

        private bool IsAnyDiagonalComplete
            => board[0, 0] != TicTacToe.Empty && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]
            || board[0, 2] != TicTacToe.Empty && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]; 

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
