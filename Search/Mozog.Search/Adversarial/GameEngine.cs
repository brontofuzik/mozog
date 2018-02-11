using System;

namespace Mozog.Search.Adversarial
{
    public class GameEngine : IGameEngine
    {
        private readonly IGame game;
        private readonly IAdversarialSearch search;

        private readonly string humanPlayer;
        private readonly string enginePlayer;

        public GameEngine(IGame game, bool humanBegins = true, bool iterativeDeepening = true, bool prune = true, bool tt = true)
        {
            this.game = game;
            this.search = iterativeDeepening
                ? IterativeDeepeningSearch.New(game, prune, tt)
                : new MinimaxSearch(game, prune, tt);

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

        public void Play(IState initialState = null)
        {
            var currentState = initialState ?? game.InitialState;
            while (!game.IsTerminal(currentState))
            {
                PrintState(currentState);
                var move = game.GetPlayer(currentState) == humanPlayer
                    ? GetHumanMove(currentState)
                    : GetEngineMove(currentState);
                currentState = game.GetResult(currentState, move);
            }

            PrintResult(currentState);
        }

        public void Analyze(IState initialState = null)
        {
            var currentState = initialState ?? game.InitialState;
            while (!game.IsTerminal(currentState))
            {
                PrintState(currentState);
                var engineMove = GetEngineMove_DEBUG(currentState);
                PrintMove(engineMove.move, engineMove.eval, engineMove.nodes);

                var humanMove = GetHumanMove(currentState);
                currentState = game.GetResult(currentState, humanMove);
            }
        }

        private IAction GetHumanMove(IState currentState)
        {
            IAction move;
            do
            {
                Console.WriteLine("Your move?");
                move = game.ParseMove(Console.ReadLine(), currentState);
            }
            while (move == null);
            return move;
        }

        private IAction GetEngineMove(IState currentState)
            => search.MakeDecision(currentState).move;

        private (IAction move, double eval, int nodes) GetEngineMove_DEBUG(IState currentState)
            => search.MakeDecision_DEBUG(currentState);

        #region Printing

        private void PrintState(IState state)
        {
            Console.Clear();
            Console.WriteLine(state);
            //Console.WriteLine($"{nameof(MinimaxSearch.NodesExpanded_Move)}: {search.Metrics.Get<int>(MinimaxSearch.NodesExpanded_Move)}");
        }

        private void PrintResult(IState state)
        {
            PrintState(state);
            Console.WriteLine("Game over");
            //Console.WriteLine($"{nameof(MinimaxSearch.NodesExpanded_Game)}: {search.Metrics.Get<int>(MinimaxSearch.NodesExpanded_Game)}");
        }

        private void PrintMove(IAction move, double eval, int nodes)
        {
            Console.WriteLine($"{move}: {eval} ({nodes})");
        }

        #endregion // Printing
    }

    public interface IGameEngine
    {
        void Play(IState initialState = null);

        void Analyze(IState initialState = null);
    }
}
