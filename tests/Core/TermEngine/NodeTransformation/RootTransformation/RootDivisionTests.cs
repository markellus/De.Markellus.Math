using De.Markellus.Math.Tests.Core.TermEngine.NodeTransformation;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.NodeTransformation.RootTransformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.NodeTransformation.RootTransformation
{
    [TestClass]
    public class RootDivisionTests
    {

        [TestMethod]
        public void Test01()
        {
            NodeTransformationTestMethods.Check("sqrt(5) / sqrt(12)", "sqrt(5 / 12)", typeof(RootDivision));
        }

        [TestMethod]
        public void Test02()
        {
            NodeTransformationTestMethods.Check("sqrt(4,3) / sqrt(3, 4)", "sqrt(4 / 3, 3 * 4)", typeof(RootDivision));
        }

        [TestMethod]
        public void Test03()
        {
            NodeTransformationTestMethods.Check("sqrt(4 / 342 * 40 ^3 , 3) / sqrt(43252, 3)",
                "sqrt((4 / 342 * 40 ^3) / 43252, 3)", typeof(RootDivision));
        }
    }
}
