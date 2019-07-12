using De.Markellus.Maths.Core.TermEngine.NodeTransformation.ParenthesisTransformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.NodeTransformation.ParenthesisTransformation
{
    [TestClass]
    public class ParenthesisAdditionTests
    {
        [TestMethod]
        public void Test01()
        {
            NodeTransformationTestMethods.Check("(12 + 10) + 13", "12 + (10 + 13)", typeof(ParenthesisAdditionV1));
            NodeTransformationTestMethods.Check("12 + (10 + 13)", "(12 + 10) + 13", typeof(ParenthesisAdditionV1));
        }

        [TestMethod]
        public void Test02()
        {
            NodeTransformationTestMethods.Check("(- 21 + 4) + 10", "- 21 + (4 + 10)", typeof(ParenthesisAdditionV1));
            NodeTransformationTestMethods.Check("- 21 + (4 + 10)", "(- 21 + 4) + 10", typeof(ParenthesisAdditionV1));
        }

        [TestMethod]
        public void Test03()
        {
            NodeTransformationTestMethods.Check("(21 - 4) + 10", "21 - (4 - 10)", typeof(ParenthesisAdditionV2));
        }

        [TestMethod]
        public void Test04()
        {
            NodeTransformationTestMethods.Check("(21 - -4) + -10", "21 - (-4 - -10)", typeof(ParenthesisAdditionV2));
        }

        [TestMethod]
        public void Test05()
        {
            NodeTransformationTestMethods.Check("10 + (21 - 4)", "(10 + 21) - 4", typeof(ParenthesisAdditionV3));
        }

        [TestMethod]
        public void Test06()
        {
            NodeTransformationTestMethods.Check("-10 + (21 - -4)", "(-10 + 21) - -4", typeof(ParenthesisAdditionV3));
        }
    }
}
