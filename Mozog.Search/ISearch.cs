using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozog.Search
{
    public interface ISearch
    {
        IAction MakeDecision(IState state);
    }
}
