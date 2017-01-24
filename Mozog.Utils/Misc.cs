namespace Mozog.Utils
{
    public class Misc
    {
        public static void Swap<T>(ref T var1, ref T var2)
        {
            T temp = var1;
            var1 = var2;
            var2 = temp;
        }
    }
}
