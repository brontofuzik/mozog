using System;
using System.Collections.Generic;
using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.Hexapawn
{

    public class HexapawnState : IState
    {
        // TODO
        public static IState CreateInitial(Hexapawn hexapawn)
        {
            throw new NotImplementedException();
        }

        public bool IsTerminal => throw new NotImplementedException();

        public string PlayerToMove => throw new NotImplementedException();

        public double? Evaluation => throw new NotImplementedException();

        public string Debug()
        {
            throw new NotImplementedException();
        }

        public IList<IAction> GetLegalMoves()
        {
            throw new NotImplementedException();
        }

        public IState MakeMove(IAction action)
        {
            throw new NotImplementedException();
        }
    }
}
