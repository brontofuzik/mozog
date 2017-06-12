using System;
using System.Diagnostics;
using QLearning.Lib;
using Action = QLearning.Lib.Action;

namespace QLearning.Examples
{
    class PathFindingBender
    {
        internal enum States
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

            var q = new Lib.QLearning();

            // "Bedroom" state
            var bedroomState = new State(States.Bedroom);
            q.AddState(bedroomState);

            bedroomState.AddAction(new Action("up"))
                .AddResult(States.Bedroom);
            bedroomState.AddAction(new Action("right"))
                .AddResult(States.Hallway);
            bedroomState.AddAction(new Action("down"))
                .AddResult(States.Bedroom);
            bedroomState.AddAction(new Action("left"))
                .AddResult(States.Bedroom);

            // "Hallway" state  
            var hallwayState = new State(States.Hallway);
            q.AddState(hallwayState);

            hallwayState.AddAction(new Action("up"))
                .AddResult(States.Hallway);
            hallwayState.AddAction(new Action("right"))
                .AddResult(States.Hallway);
            hallwayState.AddAction(new Action("down"))
                .AddResult(States.Stairwell);
            hallwayState.AddAction(new Action("left"))
                .AddResult(States.Bedroom);

            // "Stairway" state
            var stairwellState = new State(States.Stairwell);
            q.AddState(stairwellState);

            stairwellState.AddAction(new Action("up"))
                .AddResult(States.Hallway);
            stairwellState.AddAction(new Action("right"))
                .AddResult(States.Stairwell);
            stairwellState.AddAction(new Action("down"))
                .AddResult(States.Stairwell);
            stairwellState.AddAction(new Action("left"))
                .AddResult(States.Kitchen, 1.0, 100);

            var kitchenState = new State(States.Kitchen);
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