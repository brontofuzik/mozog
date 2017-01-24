using CuttingEdge.Conditions;

namespace Mozog.Utils
{
    public class Require
    {
        /// <summary>
        /// Requires that an object is not <c>null</c>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="objName">The name of the object.</param>
        /// <exception cref="ArgumentNullException">
        /// Condition: <c>obj</c> is <c>null</c>.
        /// </exception>
        public static void IsNotNull(object obj, string objName)
        {
            Condition.Requires(obj, objName).IsNotNull();
        }

        /// <summary>
        /// Requires that a number is non-negative.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="numberName">The name of the number.</param>
        /// <exception cref="ArgumentException">
        /// Condition: <c>number</c> is negative.
        /// </exception>
        public static void IsNonNegative(double number, string numberName)
        {
            Condition.Requires(number, numberName).IsGreaterOrEqual(0.0);
        }

        /// <summary>
        /// Requires that a number is positive.
        /// <param name="number">The number.</param>
        /// <param name="numberName">The name of the number.</param>
        /// <exception cref="ArgumentException">
        /// Condition: <c> number </c> is non-positive.
        /// </exception>
        public static void IsPositive(double number, string numberName)
        {
            Condition.Requires(number, numberName).IsGreaterThan(0.0);
        }

        /// <summary>
        /// Requires that a number is within range.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="numberName">The name of the number.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static void IsWithinRange(double number, string numberName, double minValue, double maxValue)
        {
            Condition.Requires(number, numberName).IsInRange(minValue, maxValue);
        }
    }
}
