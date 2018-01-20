using System.Collections.Generic;

namespace Mozog.Search.Adversarial
{
    public interface IGame
    {
        string[] Players { get; }

        IState InitialState { get; }

        Objective GetObjective(string player);

        // Minimax
        string GetPlayer(IState state);

        // Minimax
        IList<IAction> GetActions(IState state);

        // Minimax
        bool IsTerminal(IState state);

        // Minimax
        IState GetResult(IState state, IAction action);

        double? GetUtility(IState state);

        string PrintState(IState state);

        IAction ParseMove(string moveStr);
    }

    public interface IState
    {
        bool IsTerminal { get; }

        string PlayerToMove { get; }

        double? Evaluation { get; }

        IList<IAction> GetLegalMoves();

        IState MakeMove(IAction action);

        string Debug();
    }

    public interface IAction
    {
    }
}
