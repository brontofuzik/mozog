using Mozog.Search.Adversarial;

namespace Mozog.Search.Examples.Games.Hexapawn
{
    public class HexapawnAction : IAction
    {
        // "b2" or "axb2"
        public static HexapawnAction Parse(string moveStr)
        {
            return moveStr.Length == 2
                ? new HexapawnAction(null, moveStr[0], moveStr[1])
                : new HexapawnAction(moveStr[0], moveStr[2], moveStr[3]);
        }

        public char? From { get; }

        public char Col { get; }

        public int Row { get; }

        public HexapawnAction(char? from, char col, int row)
        {
            Col = col;
            Row = row;
        }
    }
}
