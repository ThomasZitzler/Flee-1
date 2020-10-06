﻿using Flee.PublicTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Flee.Test.ExpressionTests
{
    public class Base
    {
        public double Value { get; set; }
        public static Base operator +(Base left, Base right)
        {
            return new Base { Value = left.Value + right.Value };
        }
        public static Base operator -(Base left)
        {
            return new Base { Value = -left.Value };
        }
    }

    public class Derived : Base
    {
    }

    public class OtherDerived : Base
    {
    }

    [TestClass]
    public class CustomOperators
    {
        [TestMethod]
        public void LeftBaseRightBase()
        {
            var m1 = new Base { Value = 2 };
            var m2 = new Base { Value = 5 };

            ExpressionContext context = new ExpressionContext();
            context.Variables.Add("m1", m1);
            context.Variables.Add("m2", m2);
            IDynamicExpression e1 = context.CompileDynamic("m1 + m2");

            Base added = (Base) e1.Evaluate();
            Assert.AreEqual(7, added.Value);
        }

        [TestMethod]
        public void LeftBaseRightDerived()
        {
            var m1 = new Base { Value = 2 };
            var m2 = new Derived { Value = 5 };

            ExpressionContext context = new ExpressionContext();
            context.Variables.Add("m1", m1);
            context.Variables.Add("m2", m2);
            IDynamicExpression e1 = context.CompileDynamic("m1 + m2");

            Base added = (Base)e1.Evaluate();
            Assert.AreEqual(7, added.Value);
        }

        [TestMethod]
        public void LeftDerivedRightBase()
        {
            var m1 = new Derived { Value = 2 };
            var m2 = new Base { Value = 5 };

            ExpressionContext context = new ExpressionContext();
            context.Variables.Add("m1", m1);
            context.Variables.Add("m2", m2);
            IDynamicExpression e1 = context.CompileDynamic("m1 + m2");

            Base added = (Base)e1.Evaluate();
            Assert.AreEqual(7, added.Value);
        }

        [TestMethod]
        public void LeftDerivedRightDerived()
        {
            var m1 = new Derived { Value = 2 };
            var m2 = new Derived { Value = 5 };

            ExpressionContext context = new ExpressionContext();
            context.Variables.Add("m1", m1);
            context.Variables.Add("m2", m2);
            IDynamicExpression e1 = context.CompileDynamic("m1 + m2");

            Base added = (Base)e1.Evaluate();
            Assert.AreEqual(7, added.Value);
        }

        [TestMethod]
        public void LeftDerivedRightOtherDerived()
        {
            var m1 = new Derived { Value = 2 };
            var m2 = new OtherDerived { Value = 5 };

            ExpressionContext context = new ExpressionContext();
            context.Variables.Add("m1", m1);
            context.Variables.Add("m2", m2);
            IDynamicExpression e1 = context.CompileDynamic("m1 + m2");

            Base added = (Base)e1.Evaluate();
            Assert.AreEqual(7, added.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionCompileException))]
        public void MissingOperator()
        {
            var m1 = new Derived { Value = 2 };
            var m2 = new OtherDerived { Value = 5 };

            ExpressionContext context = new ExpressionContext();
            context.Variables.Add("m1", m1);
            context.Variables.Add("m2", m2);

            //// var message = "ArithmeticElement: Operation 'Subtract' is not defined for types 'Derived' and 'OtherDerived'";
            context.CompileDynamic("m1 - m2");
        }

        [TestMethod]
        public void BaseUnaryOperator()
        {
            var m1 = new Base { Value = 2 };

            ExpressionContext context = new ExpressionContext();
            context.Variables.Add("m1", m1);
            IDynamicExpression e1 = context.CompileDynamic("-m1");

            Base negated = (Base)e1.Evaluate();
            Assert.AreEqual(-2, negated.Value);
        }

        [TestMethod]
        public void DerivedUnaryOperator()
        {
            var m1 = new Derived { Value = 2 };

            ExpressionContext context = new ExpressionContext();
            context.Variables.Add("m1", m1);
            IDynamicExpression e1 = context.CompileDynamic("-m1");

            Base negated = (Base)e1.Evaluate();
            Assert.AreEqual(-2, negated.Value);
        }

        [TestMethod]
        public void DerivedUnaryOperatorPlusOperator()
        {
            var m1 = new Derived { Value = 2 };

            ExpressionContext context = new ExpressionContext();
            context.Variables.Add("m1", m1);
            IDynamicExpression e1 = context.CompileDynamic("-m1 + m1");

            Base negated = (Base)e1.Evaluate();
            Assert.AreEqual(0, negated.Value);
        }
    }
}
