using System;
using System.Diagnostics;

namespace QLearning.Examples
{
    class PathFindingAdvanced
    {
        internal enum S
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
            var q = new QLearning
            {
                Episodes = 1000,
                Alpha = 0.1,
                Gamma = 0.9, 
                MaxExploreStepsWithinOneEpisode = 1000
            };

            //
            // Setup
            //

            q.EndStates.Add(S.C.EnumToString());
 
            // A
            var state = new State(S.A);
            q.AddState(state);
            state.AddAction(new Action(S.B)).AddResult(S.B);
            state.AddAction(new Action(S.D)).AddResult(S.D);
 
            // B
            state = new State(S.B);
            q.AddState(state);

            state.AddAction(new Action(S.A)).AddResult(S.A);
            state.AddAction(new Action(S.C))
                .AddResult(S.C, 0.1, 100)
                .AddResult(S.A, 0.9);
            state.AddAction(new Action(S.E)).AddResult(S.E);

            // C
            state = new State(S.C);
            q.AddState(state, isEndState: true);
            state.AddAction(new Action(S.C)).AddResult(S.C);

            // D
            state = new State(S.D);
            q.AddState(state);
            state.AddAction(new Action(S.A)).AddResult(S.A);
            state.AddAction(new Action(S.E)).AddResult(S.E);

            // E
            state = new State(S.E);
            q.AddState(state);
            state.AddAction(new Action(S.B)).AddResult(S.B);
            state.AddAction(new Action(S.D)).AddResult(S.D);
            state.AddAction(new Action(S.F)).AddResult(S.F);

            // F
            state = new State(S.F);
            q.AddState(state);
            state.AddAction(new Action(S.C)).AddResult(S.C, 1.0, 100);
            state.AddAction(new Action(S.E)).AddResult(S.E);

            //
            // Learning
            //

            q.RunTraining();
            q.PrintQLearningStructure();
            q.ShowPolicy();
        }
    }
}