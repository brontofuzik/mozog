using System;

namespace Mozog.Search.Adversarial
{
    public class GameEngine : IGameEngine
    {
        private readonly IGame game;
        private readonly IAdversarialSearch search;

        private readonly string humanPlayer;
        private readonly string enginePlayer;

        public GameEngine(IGame game, Func<IGame, bool, IAdversarialSearch> searchFactory, bool humanBegins = true, bool tt = true)
        {
            this.game = game;
            this.search = searchFactory(game, tt);

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

        public static GameEngine Minimax(IGame game, bool humanBegins = true, bool tt = true)
            => new GameEngine(game, MinimaxSearch.Default, humanBegins, tt: tt);

        public static GameEngine AlphaBeta(IGame game, bool humanBegins = true, bool tt = true)
            => new GameEngine(game, MinimaxSearch.AlphaBeta, humanBegins, tt: tt);

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
