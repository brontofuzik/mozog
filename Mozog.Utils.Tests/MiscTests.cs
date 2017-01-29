using NUnit.Framework;

namespace Mozog.Utils.Tests
{
    [TestFixture]
    class MiscTests
    {
        private int[] array1;
        private int[] array2;

        [SetUp]
        public void SetUp()
        {
            array1 = new[] { 1, 2, 3, 4, 5 };
            array2 = new[] { 6, 7, 8, 9, 10 };
        }

        [Test]
        public void SwapArrays_SubArray()
        {
            Misc.SwapArrays(array1, 1, array2, 1, 3);
            Assert.AreEqual(new[] { 1, 7, 8, 9, 5 }, array1);
            Assert.AreEqual(new[] { 6, 2, 3, 4, 10 }, array2);
        }

        [Test]
        public void SwapArrays_SingleElement()
        {
            Misc.SwapArrays(array1, 2, array2, 2, 1);
            Assert.AreEqual(new[] { 1, 2, 8, 4, 5 }, array1);
            Assert.AreEqual(new[] { 6, 7, 3, 9, 10 }, array2);
        }

        [Test]
        public void SwapArrays_FullLength()
        {
            Misc.SwapArrays(array1, 0, array2, 0, 5);
            Assert.AreEqual(new[] { 6, 7, 8, 9, 10 }, array1);
            Assert.AreEqual(new[] { 1, 2, 3, 4, 5 }, array2);
        }

        [Test]
        public void SwapArrays_ZeroLength()
        {
            Misc.SwapArrays(array1, 0, array2, 0, 0);
            Assert.AreEqual(new[] { 1, 2, 3, 4, 5 }, array1);
            Assert.AreEqual(new[] { 6, 7, 8, 9, 10 }, array2);
        }
    }
}
