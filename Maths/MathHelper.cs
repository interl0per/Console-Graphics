namespace ConsoleGraphics.Maths
{
    /// <summary>
    /// A helper class that contains useful function for doing maths
    /// </summary>
    public class MathHelper
    {
        /// <summary>
        /// Swaps the values of the 2 given floats
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Swap(ref float x, ref float y)
        {
            float temp = x;
            x = y;
            y = temp;
        }

        /// <summary>
        /// Swaps the values of the 2 given integers
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Swap(ref int x, ref int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }

        /// <summary>
        /// Clamps the given value between the given min and max value.
        /// <para>
        /// e.g. Clamp(420, 50, 500) return 420, Clamp(100, 5, 50) return 50, Clamp(
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
                return max;
            if (value < min)
                return min;
            return value;
        }
    }
}
