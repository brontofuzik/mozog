using System;
using System.Collections.Generic;
using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Adversarial
{
    public class Hexapawn : GameBase, IGame
    {
        public static void Play_Minimax()
        {
            var hexapawn = new Hexapawn();
            var engine = GameEngine.Minimax(hexapawn);
            engine.Play();
        }

        public const string PlayerW = "W";
        public const string PlayerB = "B";
        public const string Empty = " ";

        private readonly int rows;
        private readonly int cols;

        public Hexapawn(int rows = 3, int cols = 3)
        {
            this.rows = rows;
            this.cols = cols;
        }

        public override string[] Players { get; } = new string[] { PlayerW, PlayerB };

        public override IState InitialState => HexapawnState.CreateInitial(this);

        public override Objective GetObjective(string player)
            => player == PlayerW ? Objective.Max : Objective.Min;

        // TODO
        public override IAction ParseMove(string moveStr)
        {
            throw new NotImplementedException();
        }
    }

    public class HexapawnState : IState
    {
        // TODO
        public static IState CreateInitial(Hexapawn hexapawn)
        {
            throw new NotImplementedException();
        }

        public bool IsTerminal => throw new NotImplementedException();

        public string PlayerToMove => throw new NotImplementedException();

        public double? Evaluation => throw new NotImplementedException();



        public string Debug()
        {
            throw new NotImplementedException();
        }

        public IList<IAction> GetLegalMoves()
        {
            throw new NotImplementedException();
        }

        public IState MakeMove(IAction action)
        {
            throw new NotImplementedException();
        }
    }

    public class HexapawnAction : IAction
    {
        // "b2" or "axb2"
        public HexapawnAction Parse(string moveStr)
        {
            return moveStr.Length == 2
                ? new HexapawnAction(null, moveStr[0], moveStr[1])
                : new HexapawnAction(moveStr[0], moveStr[2], moveStr[3]);
        }

        public char? From { get; }

        public char Col { get; }

        public int Row { get; }

        public HexapawnAction(char? from, char col, int row)
        {
            Col = col;
            Row = row;
        }
    }
}
