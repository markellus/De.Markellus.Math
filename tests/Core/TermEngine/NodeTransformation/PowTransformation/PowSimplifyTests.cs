using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.NodeTransformation.PowTransformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.NodeTransformation.PowTransformation
{
    [TestClass]
    public class PowSimplifyTests
    {
        [TestMethod]
        public void Test01()
        {
            NodeTransformationTestMethods.Check("31241^1", "31241", typeof(PowSimplify));
        }

        [TestMethod]
        public void Test02()
        {
            NodeTransformationTestMethods.Check("(34729 + 3249 / 3421)^1", "34729 + 3249 / 3421", typeof(PowSimplify));
        }
    }
}