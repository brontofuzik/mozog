using NeuralNetwork.Utils;
using NUnit.Framework;

namespace NeuralNetwork.Tests
{
    [TestFixture]
    public class VectorTests
    {
        [Test]
        public void Subtract_IntVector()
        {
            var result = Vector.Subtract(new[] {4, 4, 4}, new[] {1, 2, 3});
            Assert.AreEqual(new[] {3, 2, 1}, result);
        }

        [Test]
        public void Subtract_FloatVector()
        {
            var result = Vector.Subtract(new[] {4.0, 4.0, 4.0}, new[] {1.0, 2.0, 3.0});
            Assert.AreEqual(new[] { 3.0, 2.0, 1.0 }, result);
        }

        [Test]
        public void Multiply_IntVector()
        {
            var result = Vector.Multiply(new[] {1, 2, 3}, new[] {4, 5, 6});
            Assert.AreEqual(32, result);
        }

        [Test]
        public void Multiply_FloatVector()
        {
            var result = Vector.Multiply(new[] { 1.0, 2.0, 3.0 }, new[] { 4.0, 5.0, 6.0 });
            Assert.AreEqual(32.0, result);
        }

        [Test]
        public void Magnitude_IntVector()
        {
            var result = Vector.Magnitude(new[] { 3, 4 });
            Assert.AreEqual(5, result);
        }

        [Test]
        public void Magnitude_FloatVector()
        {
            var result = Vector.Magnitude(new[] { 3.0, 4.0 });
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void Distance_IntVector()
        {
            var result = Vector.Distance(new[] {10, 10}, new[] {7, 6});
            Assert.AreEqual(5, result);
        }

        [Test]
        public void Distance_FloatDistance()
        {
            var result = Vector.Distance(new[] { 10.0, 10.0 }, new[] { 7.0, 6.0 });
            Assert.AreEqual(5.0, result);
        }
    }
}
