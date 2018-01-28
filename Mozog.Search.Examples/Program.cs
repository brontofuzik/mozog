using Mozog.Search.Examples.Games.Hexapawn;
using Mozog.Search.Examples.Games.TicTacToe;
using Mozog.Utils.Math;

namespace Mozog.Search.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            //StaticRandom.Seed = 42;

            //TicTacToe.Play_Minimax();
            //TicTacToe.Play_AlphaBeta();

            //Hexapawn.Play_Minimax(cols: 3, rows: 3, tt: true); // Black wins
            //Hexapawn.Play_Minimax(cols: 3, rows: 4, tt: true); // Black wins

            //Hexapawn.Play_AlphaBeta(cols: 4, rows: 4, tt: true); // White wins
            Hexapawn.Play_AlphaBeta(cols: 4, rows: 5, tt: true); // Black wins

            //Hexapawn.Play_AlphaBeta(cols: 5, rows: 5, tt: true); // White wins
        }
    }
}
