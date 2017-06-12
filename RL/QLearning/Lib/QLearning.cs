using System;
using System.Collections.Generic;
using System.Linq;

namespace QLearning.Lib
{
    class QLearning
    {
        private readonly Random random = new Random();

        //public List<QState> States { get; } = new List<QState>();
        public IDictionary<string, State> States { get; } = new Dictionary<string, State>();

        public HashSet<string> EndStates { get; } = new HashSet<string>();

        //public Dictionary<string, QState> StateLookup { get; } = new Dictionary<string, QState>();

        public double Alpha { get; internal set; } = 0.1;

        public double Gamma { get; internal set; } = 0.9;

        public int Episodes { get; internal set; } = 1000;

        // avoid infinite loop
        public int MaxExploreStepsWithinOneEpisode { get; internal set; } = 1000;

        // show runtime warnings regarding q-learning
        public bool ShowWarning { get; internal set; } = true;


        public void AddState(State state, bool isEndState = false)
        {
            States.Add(state.Name, state);
            EndStates.Add(state.Name);
        }
 
        public void RunTraining()
        {
            ValidateProbabilities();
 
            /*       
            For each episode: Select random initial state 
            Do while not reach goal state
                Select one among all possible actions for the current state 
                Using this possible action, consider to go to the next state 
                Get maximum Q value of this next state based on all possible actions                
                Set the next state as the current state
            */
   
            long maxloopEventCount = 0;

            for (long i = 0; i < Episodes; i++)
            {
                long maxloop = 0;

                var state = GetRandomState();
                Action action = null;

                do
                {
                    if (maxloop++ > MaxExploreStepsWithinOneEpisode)
                    {
                        maxloopEventCount++;
                        if (ShowWarning)
                        {
                            Console.WriteLine($"{maxloopEventCount} !! MAXLOOP state: {state} action: {action}, maybe your path setup is wrong or the  endstate is to difficult to reach?");
                        }
                         
                        break;
                    }
 
                    if (state.Actions.Count == 0)
                        break;

                    action = state.Actions[random.Next(state.Actions.Count)];
 
                    var actionResult = action.Execute();

                    double q = actionResult.QEstimated;
                    double r = actionResult.Reward;
                    double maxQ = MaxQ(actionResult.NextStateName);
 
                    // Q(s,a)= Q(s,a) + alpha * (R(s,a) + gamma * Max(next state, all actions) - Q(s,a))
                    actionResult.QValue = q + Alpha * (r + Gamma * maxQ - q);
 
                    if (EndStates.Contains(actionResult.NextStateName))
                        break;
                    
                    state = States[actionResult.NextStateName];
 
                } while (true);
            }
        }

        private void ValidateProbabilities()
        {
            foreach (var state in States.Values)
            {
                foreach (var action in state.Actions)
                {
                    action.ValidateProbabilities();
                }
            }
        }

        private State GetRandomState()
            => States[States.Keys.ToList()[random.Next(States.Count)]];

        private double MaxQ(string stateName)
        {
            if (!States.ContainsKey(stateName))            
                return 0;                            
 
            var state = States[stateName];

            double? maxValue = null;
            foreach (var action in state.Actions)
            {
                foreach (var nextState in action.ActionsResult)
                {
                    double value = nextState.QEstimated;
                    if (value > maxValue || !maxValue.HasValue)
                        maxValue = value;
                }
            }
 
            // no update
            if (!maxValue.HasValue && ShowWarning)
            {
                Console.WriteLine($"Warning: No MaxQ value for stateName {stateName}");
            }

            return maxValue ?? 0;
        }

        #region Printing

        public void PrintQLearningStructure()
        {
            Console.WriteLine("** Q-Learning structure **");
            foreach (var state in States.Values)
            {
                Console.WriteLine("State {0}", state.Name);
                foreach (var action in state.Actions)
                {
                    Console.WriteLine("  Action " + action.Name);
                    Console.Write(action.GetActionResults());
                }
            }
            Console.WriteLine();
        }
 
        public void ShowPolicy()
        {
            Console.WriteLine("** Show Policy **");
            foreach (var state in States.Values)
            {
                double max = Double.MinValue;
                string actionName = "nothing";
                foreach (var action in state.Actions)
                {
                    foreach (var actionResult in action.ActionsResult)
                    {
                        if (actionResult.QEstimated > max)
                        {
                            max = actionResult.QEstimated;
                            actionName = action.Name;
                        }
                    }
                }
 
                Console.WriteLine($"From state {state.Name} do action {actionName}, max QEstimated is {max.Pretty()}");
            }
        }

        #endregion // Printing
    }
}