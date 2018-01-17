using System.Collections.Generic;

namespace Mozog.Search.Adversarial
{
    public interface IGame
    {
        IState InitialState { get; }

        IList<string> Players { get; }

        Objective GetObjective(string player);

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

        double? GetUtility_NEW(IState state);

        string PrintState(IState state);

        IAction ParseMove(string moveStr);
    }

    public interface IState
    {
        bool IsTerminal { get; }

        string PlayerToMove { get; }

        double? Evaluation { get; }

        double? Evaluation_NEW { get; }

        IList<IAction> GetLegalMoves();

        IState MakeMove(IAction action);
    }

    public interface IAction
    {
    }
}
