using System.Collections.Generic;

namespace Mozog.Search
{
    public interface IGame
    {
        IState InitialState { get; }

        IList<string> Players { get; }

        // Minimax
        string GetPlayer(IState state);

        // Minimax
        IList<IAction> GetActions(IState state);

        // Minimax
        bool IsTerminal(IState state);

        // Minimax
        IState GetResult(IState state, IAction action);

        // Minimax
        double? GetUtility(IState state, string player);
    }

    public interface IState
    {
        bool IsTerminal { get; }

        string PlayerToMove { get; }

        double? Evaluation { get; }

        IList<IAction> GetLegalMoves();

        IState MakeMove(IAction action);
    }

    public interface IAction
    {
    }
}
