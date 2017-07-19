namespace QLearning
{
    public class ActionResult
    {
        public ActionResult(string nextState = null, double probability = 1, double reward = 0)
        {
            NextStateName = nextState;
            Probability = probability;
            Reward = reward;
        }

        public string PreviousStateName { get; internal set; }

        public string NextStateName { get; internal set; }  

        public double Probability { get; internal set; }

        public double Reward { get; internal set; }

        public double QValue { get; internal set; }

        public double QEstimated => QValue * Probability;

        public override string ToString()
            => $"State {NextStateName}, Prob. {Probability.Pretty()}, Reward {Reward}, PrevState {PreviousStateName}, QE {QEstimated.Pretty()}";
    }
}