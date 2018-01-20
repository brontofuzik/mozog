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
            => new GameEngine(game, g => new MinimaxSearch(g), humanBegins);

        public static GameEngine AlphaBeta(IGame game, bool humanBegins = true)
            => new GameEngine(game, g => new AlphaBetaSearch(g), humanBegins);

        public void Play()
        {
            var currentState = game.InitialState;

            while (!game.IsTerminal(currentState))
            {
                var move = game.GetPlayer(currentState) == humanPlayer
                    ? GetHumanMove(currentState)
                    : GetEngineMove(currentState);
                currentState = game.GetResult(currentState, move);
            }

            PrintResult(currentState);
        }

        private IAction GetHumanMove(IState currentState)
        {
            Console.WriteLine(currentState.ToString());
            return game.ParseMove(Console.ReadLine());
        }

        private IAction GetEngineMove(IState currentState)
        {
            var move = search.MakeDecision(currentState);
            Console.WriteLine($"NodesExpanded_Move: {search.Metrics.Get<int>("NodesExpanded_Move")}");
            return move;
        }

        private void PrintResult(IState currentState)
        {
            Console.WriteLine($"NodesExpanded_Game: {search.Metrics.Get<int>("NodesExpanded_Game")}");
            Console.WriteLine("Game over");
        }
    }

    public interface IGameEngine
    {
        void Play();
    }
}
