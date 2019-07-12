using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.TermEngine.NodeTransformation.ParenthesisTransformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.NodeTransformation.ParenthesisTransformation
{
    [TestClass]
    public class ParenthesisMultiplicationTests
    {
        [TestMethod]
        public void Test01()
        {
            NodeTransformationTestMethods.Check("(12 * 10) * 13", "12 * (10 * 13)", typeof(ParenthesisMultiplication));
            NodeTransformationTestMethods.Check("12 * (10 * 13)", "(12 * 10) * 13", typeof(ParenthesisMultiplication));
        }

        [TestMethod]
        public void Test02()
        {
            NodeTransformationTestMethods.Check("((4 * 5) * 6) * 10", "(4 * 5) * (6 * 10)", typeof(ParenthesisMultiplication));
            NodeTransformationTestMethods.Check("(4 * (5 * 6)) * 10", "4 * ((5 * 6) * 10)", typeof(ParenthesisMultiplication));
            NodeTransformationTestMethods.Check("4 * (5 * 6 * 10)", "(4 * (5 * 6)) * 10", typeof(ParenthesisMultiplication));
        }
    }
}
