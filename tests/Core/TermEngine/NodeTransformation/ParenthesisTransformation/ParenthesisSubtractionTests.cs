using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.TermEngine.NodeTransformation.ParenthesisTransformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.NodeTransformation.ParenthesisTransformation
{
    [TestClass]
    public class ParenthesisSubtractionTests
    {
        [TestMethod]
        public void Test01()
        {
            NodeTransformationTestMethods.Check("(12 - 10) - 13", "12 - (10 + 13)", typeof(ParenthesisSubtractionV1));
            NodeTransformationTestMethods.Check("12 - (10 - 13)", "(12 - 10) + 13", typeof(ParenthesisSubtractionV1));
        }

        [TestMethod]
        public void Test02()
        {
            NodeTransformationTestMethods.Check("(- 21 - 4) - 10", "- 21 - (4 + 10)", typeof(ParenthesisSubtractionV1));
            NodeTransformationTestMethods.Check("- 21 - (4 - 10)", "(- 21 - 4) + 10", typeof(ParenthesisSubtractionV1));
        }

        [TestMethod]
        public void Test03()
        {
            NodeTransformationTestMethods.Check("(12 + 10) - 13", "12 + (10 - 13)", typeof(ParenthesisSubtractionV2));
        }

        [TestMethod]
        public void Test04()
        {
            NodeTransformationTestMethods.Check("(- 21 + 4) - 10", "- 21 + (4 - 10)", typeof(ParenthesisSubtractionV2));
        }

        [TestMethod]
        public void Test05()
        {
            NodeTransformationTestMethods.Check("((21 + 12 + 14) + 4) - 10", "(21 + 12 + 14) + (4 - 10)", typeof(ParenthesisSubtractionV2));
        }

        [TestMethod]
        public void Test06()
        {
            NodeTransformationTestMethods.Check("(- 21 * 52 + 4 * 7) - 10", "- 21 * 52 + ( 4 * 7 - 10)", typeof(ParenthesisSubtractionV2));
        }

        [TestMethod]
        public void Test07()
        {
            NodeTransformationTestMethods.Check("12 - (10 + 13)", "12 - 10 - 13", typeof(ParenthesisSubtractionV3));
        }

        [TestMethod]
        public void Test08()
        {
            NodeTransformationTestMethods.Check("- 21 - (4 + 10)", "(- 21 - 4) - 10", typeof(ParenthesisSubtractionV3));
        }

        [TestMethod]
        public void Test09()
        {
            NodeTransformationTestMethods.Check("(21 + 12 + 14) - (4 + 10)", "(21 + 12 + 14) - 4 - 10", typeof(ParenthesisSubtractionV3));
        }

        [TestMethod]
        public void Test10()
        {
            NodeTransformationTestMethods.Check("- 21 * 52 - (4 * 7 + 10)", "- 21 * 52 - 4 * 7 - 10", typeof(ParenthesisSubtractionV3));
        }
    }
}
