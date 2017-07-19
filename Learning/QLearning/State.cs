using System;
using System.Collections.Generic;

namespace QLearning
{
    public class State
    {
        public State(string name)
        {
            Name = name;
        }

        public State(Enum state)
            : this(state.ToString())
        {
        }

        public string Name { get; }

        public List<Action> Actions { get; } = new List<Action>();

        public Action AddAction(Action action)
        {
            Actions.Add(action);
            action.CurrentState = Name;
            return action;
        }
 
        public override string ToString() => $"Name: {Name}";
    }
}