using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QLearning.Lib
{
    class Action
    {
        private static readonly Random random = new Random();

        private readonly string fromState;
        private readonly string toState;

        public Action(string fromState, string toState = null)
        {
            this.fromState = fromState;
            this.toState = toState;
        }

        public Action(Enum fromState, Enum toState = null)
            : this(fromState.ToString(), toState.ToString())
        {
        }

        public string Name => toState != null ? $"{fromState}->{toState}" : fromState;

        public string CurrentState { get; set; }

        public List<ActionResult> ActionsResult { get; } = new List<ActionResult>();

        public Action AddResult(ActionResult result)
        {
            ActionsResult.Add(result);
            result.PreviousStateName = CurrentState;
            return this;
        }

        public Action AddResult(string nextState = null, double probability = 1, double reward = 0)
        {
            return AddResult(new ActionResult(nextState, probability, reward));
        }

        public Action AddResult(Enum nextState = null, double probability = 1, double reward = 0)
        {
            return AddResult(nextState.ToString(), probability, reward);
        }

        public string GetActionResults()
        {
            var sb = new StringBuilder();
            foreach (var result in ActionsResult)
                sb.AppendLine("     ActionResult " + result);
            return sb.ToString();
        }
 
        // The sum of action outcomes must be close to 1
        public void ValidateProbabilities()
        {
            const double epsilon = 0.1;
 
            if (ActionsResult.Count == 0)
                throw new ApplicationException($"ValidateActionsResultProbability is invalid, no action results:\n{this}");
 
            double sum = ActionsResult.Sum(a => a.Probability);
            if (Math.Abs(1 - sum) > epsilon)
                throw new ApplicationException($"ValidateActionsResultProbability is invalid:\n{this}");
        }
 
        public ActionResult Execute()
        {
            double d = random.NextDouble();
            double sum = 0;
            foreach (var actionResult in ActionsResult)
            {
                sum += actionResult.Probability;
                if (d <= sum)
                    return actionResult;
            }
             
            // we might get here if sum probability is below 1.0 e.g. 0.99 
            // and the d random value is 0.999
            if (ActionsResult.Count > 0) 
                return ActionsResult.Last();
 
            throw new ApplicationException($"No PickAction result: {this}");
        }
 
        public override string ToString()
        {
            double sum = ActionsResult.Sum(a => a.Probability);
            return $"Name: {Name}, Probability sum: {sum}, Results: {ActionsResult.Count}";
        }
    }
}