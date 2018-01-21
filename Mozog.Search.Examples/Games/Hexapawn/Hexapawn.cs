using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.Hexapawn
{
    public class Hexapawn : Game, IGame
    {
        public static void Play_Minimax()
        {
            var hexapawn = new Hexapawn(rows: 3, cols: 3);
            var engine = GameEngine.Minimax(hexapawn);
            engine.Play();
        }

        internal static void Play_AlphaBeta()
        {
            var hexapawn = new Hexapawn(rows: 4, cols: 4);
            var engine = GameEngine.AlphaBeta(hexapawn);
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

        public override IState InitialState => HexapawnState.CreateInitial(rows, cols);

        public override Objective GetObjective(string player)
            => player == PlayerW ? Objective.Max : Objective.Min;

        public override IAction ParseMove(string moveStr, string player)
            => HexapawnMove.Parse(moveStr, player);
    }
}
