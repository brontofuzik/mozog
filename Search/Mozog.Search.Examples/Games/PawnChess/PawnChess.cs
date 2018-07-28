using Mozog.Search.Adversarial;
using Mozog.Utils;
using Mozog.Utils.Math;

namespace Mozog.Search.Examples.Games.PawnChess
{
    public class PawnChess : Game
    {
        // Example
        public static void Play(bool iterativeDeepening = true)
        {
            var pawnChess = new PawnChess();
            var engine = new GameEngine(pawnChess, humanBegins: true, iterativeDeepening: iterativeDeepening, prune: true, tt: true);
            engine.Play();
        }

        // Example
        public static void Analyze(bool iterativeDeepening = true)
        {
            var pawnChess = new PawnChess();
            var engine = new GameEngine(pawnChess, humanBegins: true, iterativeDeepening: iterativeDeepening, prune: true, tt: true);
            engine.Analyze();
        }

        public const int Rows = 8;
        public const int Cols = 8;

        // Players
        public const string White = "W";
        public const string Black = "B";

        // Pieces
        public const char King = 'K';
        public const char Pawn = 'P';

        // Board
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
            Table = new int[Rows * Cols, 4]; // P, K, p, k 
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

        public override IAction ParseMove(string moveStr, IState currentState)
            => PawnChessMove.Parse(moveStr, (PawnChessState)currentState);
    }
}
