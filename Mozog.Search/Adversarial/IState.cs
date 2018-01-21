using System.Collections.Generic;

namespace Mozog.Search.Adversarial
{
    public interface IState
    {
        bool IsTerminal { get; }

        string PlayerToMove { get; }

        double? Evaluation { get; }

        IEnumerable<IAction> GetLegalMoves();

        bool IsLegalMove(IAction move);

        IState MakeMove(IAction action);

        string Debug { get; }
    }
}
