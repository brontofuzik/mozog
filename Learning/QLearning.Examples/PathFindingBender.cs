using System;
using System.Diagnostics;

namespace QLearning.Examples
{
    class PathFindingBender
    {
        internal enum S
        {
            Bedroom,
            Hallway,
            Stairwell,
            Kitchen
        }
 
        static void Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            RunInternal();

            stopwatch.Stop();

            Console.WriteLine($"\n{stopwatch.Elapsed.TotalSeconds.Pretty()} sec. press a key ...");
            Console.ReadKey();
        }         
 
        static void RunInternal()
        {
            //
            // Setup
            //

            var q = new QLearning();

            // "Bedroom" state
            var bedroomState = new State(S.Bedroom);
            q.AddState(bedroomState);

            bedroomState.AddAction(new Action("up"))
                .AddResult(S.Bedroom);
            bedroomState.AddAction(new Action("right"))
                .AddResult(S.Hallway);
            bedroomState.AddAction(new Action("down"))
                .AddResult(S.Bedroom);
            bedroomState.AddAction(new Action("left"))
                .AddResult(S.Bedroom);

            // "Hallway" state  
            var hallwayState = new State(S.Hallway);
            q.AddState(hallwayState);

            hallwayState.AddAction(new Action("up"))
                .AddResult(S.Hallway);
            hallwayState.AddAction(new Action("right"))
                .AddResult(S.Hallway);
            hallwayState.AddAction(new Action("down"))
                .AddResult(S.Stairwell);
            hallwayState.AddAction(new Action("left"))
                .AddResult(S.Bedroom);

            // "Stairway" state
            var stairwellState = new State(S.Stairwell);
            q.AddState(stairwellState);

            stairwellState.AddAction(new Action("up"))
                .AddResult(S.Hallway);
            stairwellState.AddAction(new Action("right"))
                .AddResult(S.Stairwell);
            stairwellState.AddAction(new Action("down"))
                .AddResult(S.Stairwell);
            stairwellState.AddAction(new Action("left"))
                .AddResult(S.Kitchen, 1.0, 100);

            var kitchenState = new State(S.Kitchen);
            q.AddState(kitchenState, isEndState: true);

            //
            // Learning
            //

            q.RunTraining();
            q.PrintQLearningStructure();
            q.ShowPolicy();
        }
    }
}