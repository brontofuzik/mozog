using System;
using System.Diagnostics;
using QLearning.Lib;
using Action = QLearning.Lib.Action;

namespace QLearning.Examples
{
    class PathFindingAdvanced
    {
        internal enum States
        {
            A, B, C, D, E, F
        }
 
        static void Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            RunInternal();

            stopwatch.Stop();

            Console.WriteLine("\n{0} sec. press a key ...", stopwatch.Elapsed.TotalSeconds.Pretty()); Console.ReadKey();
        }         
 
        static void RunInternal()
        {
            var q = new Lib.QLearning
            {
                Episodes = 1000,
                Alpha = 0.1,
                Gamma = 0.9, 
                MaxExploreStepsWithinOneEpisode = 1000
            };

            //
            // Setup
            //

            q.EndStates.Add(States.C.EnumToString());
 
            // A
            var state = new State(States.A);
            q.AddState(state);
            state.AddAction(new Action(States.B)).AddResult(States.B);
            state.AddAction(new Action(States.D)).AddResult(States.D);
 
            // B
            state = new State(States.B);
            q.AddState(state);

            state.AddAction(new Action(States.A)).AddResult(States.A);
            state.AddAction(new Action(States.C))
                .AddResult(States.C, 0.1, 100)
                .AddResult(States.A, 0.9);
            state.AddAction(new Action(States.E)).AddResult(States.E);

            // C
            state = new State(States.C);
            q.AddState(state, isEndState: true);
            state.AddAction(new Action(States.C)).AddResult(States.C);

            // D
            state = new State(States.D);
            q.AddState(state);
            state.AddAction(new Action(States.A)).AddResult(States.A);
            state.AddAction(new Action(States.E)).AddResult(States.E);

            // E
            state = new State(States.E);
            q.AddState(state);
            state.AddAction(new Action(States.B)).AddResult(States.B);
            state.AddAction(new Action(States.D)).AddResult(States.D);
            state.AddAction(new Action(States.F)).AddResult(States.F);

            // F
            state = new State(States.F);
            q.AddState(state);
            state.AddAction(new Action(States.C)).AddResult(States.C, 1.0, 100);
            state.AddAction(new Action(States.E)).AddResult(States.E);

            //
            // Learning
            //

            q.RunTraining();
            q.PrintQLearningStructure();
            q.ShowPolicy();
        }
    }
}