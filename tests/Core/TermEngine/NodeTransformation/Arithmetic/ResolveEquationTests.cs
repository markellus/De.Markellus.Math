using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.NodeTransformation.Arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.NodeTransformation.Arithmetic
{
    [TestClass]
    public class ResolveEquationTests
    {

        [TestMethod]
        public void Test01()
        {
            NodeTransformationTestMethods.Check("10+10", "20", typeof(ResolveEquation));
        }

        [TestMethod]
        public void Test02()
        {
            NodeTransformationTestMethods.Check("132 * 2 - 21", "243", typeof(ResolveEquation));
        }
    }
}
