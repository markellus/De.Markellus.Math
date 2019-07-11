using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.NodeTransformation.PowTransformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.NodeTransformation.PowTransformation
{
    [TestClass]
    public class PowDivisionTests
    {

        [TestMethod]
        public void Test01()
        {
            NodeTransformationTestMethods.Check("2^32 / 2^10", "2^(32 - 10)", typeof(PowDivision));
        }

        [TestMethod]
        public void Test02()
        {
            NodeTransformationTestMethods.Check("1^2 / 1^3", "1^(2 - 3)", typeof(PowDivision));
        }
    }
}
