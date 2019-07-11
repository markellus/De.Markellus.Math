using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.NodeTransformation.PowTransformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.NodeTransformation.PowTransformation
{
    [TestClass]
    public class PowToRootTests
    {
        [TestMethod]
        public void Test01()
        {
            NodeTransformationTestMethods.Check("1^(1/2)", "sqrt(1^1)", typeof(PowToRoot));
        }

        [TestMethod]
        public void Test02()
        {
            NodeTransformationTestMethods.Check("321^(3/7)", "sqrt(321^3, 7)", typeof(PowToRoot));
        }
    }
}
