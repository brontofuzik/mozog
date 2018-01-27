using System;

namespace Mozog.Search.Adversarial
{
    public class GameEngine : IGameEngine
    {
        private readonly IGame game;
        private readonly IAdversarialSearch search;

        private readonly string humanPlayer;
        private readonly string enginePlayer;

        public GameEngine(IGame game, Func<IGame, IAdversarialSearch> search, bool humanBegins = true)
        {
            this.game = game;
            this.search = search(game);

            // Determine players.
            if (humanBegins)
            {
                humanPlayer = game.Players[0];
                enginePlayer = game.Players[1];
            }
            else
            {
                enginePlayer = game.Players[0];
                humanPlayer = game.Players[1];
            }
        }

        public static GameEngine Minimax(IGame game, bool humanBegins = true)
            => new GameEngine(game, MinimaxSearch.Default, humanBegins);

        public static GameEngine AlphaBeta(IGame game, bool humanBegins = true)
            => new GameEngine(game, MinimaxSearch.AlphaBeta, humanBegins);

        public void Play()
        {
            var currentState = game.InitialState;
            PrintState(currentState);

            while (!game.IsTerminal(currentState))
            {
                var move = game.GetPlayer(currentState) == humanPlayer
                    ? GetHumanMove(currentState)
                    : GetEngineMove(currentState);
                currentState = game.GetResult(currentState, move);
                PrintState(currentState);
            }

            PrintResult(currentState);
        }

        private IAction GetHumanMove(IState currentState)
        {
            IAction move = null;
            do
            {
                Console.WriteLine("Your move?");
                move = game.ParseMove(Console.ReadLine(), currentState.PlayerToMove);
            }
            while (move == null || !game.IsLegalMove(currentState, move));
            return move;
        }

        private IAction GetEngineMove(IState currentState)
            => search.MakeDecision(currentState);

        private void PrintState(IState state)
        {
            Console.Clear();
            Console.WriteLine(state);
        }

        private void PrintResult(IState currentState)
        {
            Console.WriteLine("Game over");
            Console.WriteLine($"NodesExpanded_Game: {search.Metrics.Get<int>("NodesExpanded_Game")}");
        }
    }

    public interface IGameEngine
    {
        void Play();
    }
}
