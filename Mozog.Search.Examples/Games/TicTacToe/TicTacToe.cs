using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.TicTacToe
{
    public class TicTacToe : Game, IGame
    {
        public static void Play_Minimax()
        {
            var ticTacToe = new TicTacToe();
            var engine = GameEngine.Minimax(ticTacToe);
            engine.Play();
        }

        public static void Play_AlphaBeta()
        {
            var ticTacToe = new TicTacToe();
            var engine = GameEngine.AlphaBeta(ticTacToe);
            engine.Play();
        }

        public const string PlayerX = "X";
        public const string PlayerO = "O";
        public const string Empty = " ";

        public override string[] Players { get; } = new string[] { PlayerX, PlayerO };

        public override IState InitialState
            => TicTacToeState.CreateInitial();

        public override Objective GetObjective(string player)
            => player == PlayerX ? Objective.Max : Objective.Min;

        public override IAction ParseMove(string moveStr)
            => TicTacToeAction.Parse(moveStr);
    }
}
