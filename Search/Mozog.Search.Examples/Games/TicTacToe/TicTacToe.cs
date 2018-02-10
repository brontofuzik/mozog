using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.TicTacToe
{
    public class TicTacToe : Game
    {
        public static void Play_Minimax()
        {
            var ticTacToe = new TicTacToe();
            var engine = new GameEngine(ticTacToe, humanBegins: true, iterativeDeepening: false, prune: false, tt: false);
            engine.Play();
        }

        public const string PlayerX = "X";
        public const string PlayerO = "O";
        public const string Empty = " ";

        public override string[] Players { get; } = { PlayerX, PlayerO };

        public override IState InitialState
            => TicTacToeState.CreateInitial();

        public override Objective GetObjective(string player)
            => player == PlayerX ? Objective.Max : Objective.Min;

        public override IAction ParseMove(string moveStr, string player)
            => TicTacToeMove.Parse(moveStr);
    }
}
