using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.NodeTransformation.PowTransformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.NodeTransformation.PowTransformation
{
    [TestClass]
    public class PowAdditionTests
    {
        [TestMethod]
        public void Test01()
        {
            NodeTransformationTestMethods.Check("31^21 + 31^21", "2 * 31^21", typeof(PowAdditionV1));
        }

        [TestMethod]
        public void Test02()
        {
            NodeTransformationTestMethods.Check("(342 * 13 + 543289 / 546^2)^2 + (342 * 13 + 543289 / 546^2)^2", "2 * (342 * 13 + 543289 / 546 ^2)^2", typeof(PowAdditionV1));
        }

        [TestMethod]
        public void Test03()
        {
            NodeTransformationTestMethods.Check("31^21 + 4 * 31^21", "(1 + 4) * 31^21", typeof(PowAdditionV2));
        }

        [TestMethod]
        public void Test04()
        {
            NodeTransformationTestMethods.Check("31^21 + 31^21 * 4", "(1 + 4) * 31^21", typeof(PowAdditionV2));
        }

        [TestMethod]
        public void Test05()
        {
            NodeTransformationTestMethods.Check("4 * 31^21 + 31^21", "(4 + 1) * 31^21", typeof(PowAdditionV2));
        }

        [TestMethod]
        public void Test06()
        {
            NodeTransformationTestMethods.Check("31^21 * 4 + 31^21", "(4 + 1) * 31^21", typeof(PowAdditionV2));
        }

        [TestMethod]
        public void Test07()
        {
            NodeTransformationTestMethods.Check("3 * 31^21 + 4 * 31^21", "(3 + 4) * 31^21", typeof(PowAdditionV3));
        }

        [TestMethod]
        public void Test08()
        {
            NodeTransformationTestMethods.Check("21 * 31^21 + 31^21 * 4", "(21 + 4) * 31^21", typeof(PowAdditionV3));
        }

        [TestMethod]
        public void Test09()
        {
            NodeTransformationTestMethods.Check("31^21 * 4 + 31^21 * 8", "(4 + 8) * 31^21", typeof(PowAdditionV3));
        }

        [TestMethod]
        public void Test10()
        {
            NodeTransformationTestMethods.Check("31^21 * 4 + 12 * 31^21", "(4 + 12) * 31^21", typeof(PowAdditionV3));
        }

        [TestMethod]
        public void Test11()
        {
            NodeTransformationTestMethods.Check(" 3 * (342 * 13 + 543289 / 321 ^2)^2 + (342 * 13 + 543289 / 321 ^2)^2", "(3 + 1) * (342 * 13 + 543289 / 321 ^2)^2", typeof(PowAdditionV2));
        }
    }
}
