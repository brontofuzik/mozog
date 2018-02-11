using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mozog.Search.Adversarial;
using Mozog.Utils;
using Mozog.Utils.Math;

namespace Mozog.Search.Examples.Games.PawnChess
{
    public class PawnChess : Game
    {
        // Example
        public static void Play_PawnChess(bool iterativeDeepening = true)
        {
            var pawnChess = new PawnChess();
            var engine = new GameEngine(pawnChess, humanBegins: true, iterativeDeepening: iterativeDeepening, prune: true, tt: true);
            engine.Play();
        }

        // Example
        public static void Analyze_PawnChess(bool iterativeDeepening = true)
        {
            var pawnChess = new PawnChess();
            var engine = new GameEngine(pawnChess, humanBegins: true, iterativeDeepening: iterativeDeepening, prune: true, tt: true);
            engine.Analyze();
        }

        private const int rows = 8;
        private const int cols = 8;

        // Players
        public const string White = "W";
        public const string Black = "B";

        // Pieces
        public const string WhitePawn = "P";
        public const string WhiteKing = "K";
        public const string BlackPawn = "p";
        public const string BlackKing = "k";
        public const string Empty = " ";

        public PawnChess()
        {
            InitializeTranspositionTable();
        }

        private void InitializeTranspositionTable()
        {
            Table = new int[rows * cols, 4]; // Wp, Wk, Bp, Bk 
            Table.Initialize2D((i1, i2) => StaticRandom.Int());
            Table_WhiteToMove = StaticRandom.Int();
            Table_BlackToMove = StaticRandom.Int();
        }

        // Transposition table
        internal int[,] Table { get; private set; }
        internal int Table_WhiteToMove { get; private set; }
        internal int Table_BlackToMove { get; private set; }

        public override string[] Players { get; } = { White, Black };

        public override IState InitialState => PawnChessState.CreateInitial(this);

        public override Objective GetObjective(string player)
            => player == White ? Objective.Max : Objective.Min;

        public override IAction ParseMove(string moveStr, string player)
            => PawnChessMove.Parse(moveStr, player);
    }
}
