using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;
using SimulatedAnnealing.Functions.Cooling;
using SimulatedAnnealing.Functions.Initialization;
using SimulatedAnnealing.Functions.Objective;
using SimulatedAnnealing.Functions.Perturbation;

namespace SimulatedAnnealing.Examples
{
    static class TravellingSalesmanProblem
    {
        public static SimulatedAnnealing<char> Algorithm()
        {
            var tsp = Mozog.Examples.TravellingSalesmanProblem.Tsp;
            return new SimulatedAnnealing<char>(tsp.CityCount)
            {
                Objective = ObjectiveFunction<char>.Minimize(state => tsp.TotalDistance(state)),
                Initialization = new LambdaInitialization<char>(_ => StaticRandom.Shuffle(new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' })),
                Perturbation = new LambdaPerturbation<char>(state =>
                {
                    var candidateState = (char[])state.Clone();

                    // SwapArrays two consecutive (tour-wisely) cities.
                    int from = StaticRandom.Int(0, state.Length);
                    int to = (from + 1) % state.Length;
                    Misc.Swap(ref candidateState[from], ref candidateState[to]);

                    return candidateState;
                }),
                Cooling = LambdaCooling.Exponential
            };
        }
    }
}
