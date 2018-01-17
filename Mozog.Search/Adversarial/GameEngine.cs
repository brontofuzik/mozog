using System;

namespace Mozog.Search.Adversarial
{
    public class GameEngine : IGameEngine
    {
        private readonly IGame game;
        private readonly IAdversarialSearch search;

        private readonly string humanPlayer;
        private readonly string enginePlayer;

        //private IState currentState;

        public GameEngine(IGame game, bool humanBegins = true)
        {
            this.game = game;
            this.search = new MinimaxSearch(game);

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

        public void Run()
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
            return move;
        }

        private void PrintResult(IState currentState)
        {
            Console.WriteLine("Game over");
        }
    }

    public interface IGameEngine
    {
        void Run();
    }
}
