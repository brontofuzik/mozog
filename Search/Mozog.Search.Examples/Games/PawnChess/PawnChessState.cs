using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.PawnChess
{
    public class PawnChessState : State
    {
        private readonly Board board;
        private readonly int movesPlayed;
        private /*readonly*/ PawnChessState game; // TODO Make readonly

        public PawnChessState(Board board, string playerToMove, int movesPlayed, PawnChessState game)
            : base(playerToMove)
        {
            this.board = board;
            this.movesPlayed = movesPlayed;
            this.game = game;
        }

        public static IState CreateInitial(PawnChess game)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IAction> GetLegalMoves()
        {
            throw new NotImplementedException();
        }

        public override IState MakeMove(IAction action)
        {
            throw new NotImplementedException();
        }

        protected override GameResult GetResult()
        {
            throw new NotImplementedException();
        }

        protected override double? EvaluateTerminal()
        {
            throw new NotImplementedException();
        }

        protected override double Evaluate()
        {
            throw new NotImplementedException();
        }

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
