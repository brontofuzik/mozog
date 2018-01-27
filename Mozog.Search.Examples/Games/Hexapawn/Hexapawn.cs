using Mozog.Search.Adversarial;
using Mozog.Utils;
using Mozog.Utils.Math;

namespace Mozog.Search.Examples.Games.Hexapawn
{
    public class Hexapawn : Game
    {
        public static void Play_Minimax()
        {
            var hexapawn = new Hexapawn(rows: 3, cols: 3);
            var engine = GameEngine.Minimax(hexapawn);
            engine.Play();
        }

        internal static void Play_AlphaBeta()
        {
            var hexapawn = new Hexapawn(rows: 5, cols: 5);
            var engine = GameEngine.AlphaBeta(hexapawn);
            engine.Play();
        }

        public const string PlayerW = "W";
        public const string PlayerB = "B";
        public const string Empty = " ";

        private readonly int rows;
        private readonly int cols;

        // Transposition table
        internal int[,] Table { get; }

        public Hexapawn(int rows = 3, int cols = 3)
        {
            this.rows = rows;
            this.cols = cols;

            // Transposition table
            Table = new int[cols * rows, 2];
            Table.Initialize2D((i1, i2) => StaticRandom.Int());
        }

        public override string[] Players { get; } = { PlayerW, PlayerB };

        public override IState InitialState => HexapawnState.CreateInitial(rows, cols, this);

        public override Objective GetObjective(string player)
            => player == PlayerW ? Objective.Max : Objective.Min;

        public override IAction ParseMove(string moveStr, string player)
            => HexapawnMove.Parse(moveStr, player);
    }
}
