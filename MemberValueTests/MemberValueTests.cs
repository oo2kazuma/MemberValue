using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OHLibrary.Tests
{
    [TestClass()]
    [System.Runtime.InteropServices.Guid("C93C4818-6F48-41DB-BB27-D28A0E09A8DC")]
    public class MemberValueTests
    {
        MemberValue D1 = new MemberValue("int1", 1);
        MemberValue D2 = new MemberValue("int1", 1, MemberValue.DefaultSearchingConditions, typeof(OHLibrary.MemberValue));
        MemberValue D3 = new MemberValue("null", null, MemberValue.DefaultSearchingConditions, null);
        MemberValue D4 = new MemberValue("null", null, MemberValue.DefaultSearchingConditions, null, 2);
        MemberValue D5
        {
            get
            {
                var v = new MemberValue("null", null, MemberValue.DefaultSearchingConditions, null, 2);
                v.DepthChar = '*';
                return v;
            }
        }
        MemberValue D6 = new MemberValue("TestClass", new TestClass(1));
        MemberValue D7 = new MemberValue("TestClass", new TestClass(1),
            MemberValue.DefaultSearchingConditions | SearchCondition.DeclaredOnly);
        MemberValue D8 = new MemberValue("TestClass", new TestClass(1), 
            SearchCondition.Field | SearchCondition.Property | SearchCondition.Public | SearchCondition.Static | SearchCondition.DeclaredOnly);
        MemberValue D9 = new MemberValue("TestClass", new TestClass(1),
            SearchCondition.Field | SearchCondition.Property | SearchCondition.NonPublic | SearchCondition.Static | SearchCondition.FlattenHierarchy);
        MemberValue D10 = new MemberValue("TestClass", new TestClass(1),
            SearchCondition.Method | SearchCondition.NonPublic | SearchCondition.Public | SearchCondition.Instance | SearchCondition.Static | SearchCondition.FlattenHierarchy);
        MemberValue D11 = new MemberValue("TestClass", new TestClass(1),SearchCondition.MaxOutPut);
        MemberValue D12 = new MemberValue("TestClass", new TestClass(1), SearchCondition.None);

        [TestMethod()]
        public void ConstructorDefaultValueTest()
        {
            Assert.AreEqual(D1.Name, "int1");
            Assert.AreEqual(D1.Value, 1);
            Assert.AreEqual(D1.Depth, MemberValue.DefaultDepth);
            Assert.AreEqual(D1.DepthChar, MemberValue.DefaultDepthChar);
            Assert.AreEqual(D1.SearchingConditionsValue, MemberValue.DefaultSearchingConditions);
            Assert.AreEqual(D1.ThrewException, MemberValue.DefaultThrewException);
        }


        [TestMethod()]
        public void ConstructorTest()
        {
            Assert.AreEqual(D1.TypeOfValue, typeof(int));
            Assert.AreEqual(D1.IsSystem, true);

            Assert.AreEqual(D2.TypeOfValue, typeof(OHLibrary.MemberValue));
            Assert.AreEqual(D2.IsSystem, false);
            
            Assert.AreEqual(D3.TypeOfValue, null);
            Assert.AreEqual(D3.IsSystem, false);
        }
        

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.AreEqual(D1.ToString(), "Name[int1] Value[1] Type[System.Int32] ThrewException[False]");
            Assert.AreEqual(D4.ToString(), "Name[null] Value[null] Type[null] ThrewException[False]");
            Assert.AreEqual(D5.ToString(), "Name[null] Value[null] Type[null] ThrewException[False]");

            Assert.AreEqual(D1.ToString(true), "Name[int1] Value[1] Type[System.Int32] ThrewException[False]");
            Assert.AreEqual(D4.ToString(true), "Name[null] Value[null] Type[null] ThrewException[False]");
            Assert.AreEqual(D5.ToString(true), "Name[null] Value[null] Type[null] ThrewException[False]");


            Assert.AreEqual(D1.ToString(false), "Name[int1] Value[1] Type[System.Int32] ThrewException[False]");
            Assert.AreEqual(D4.ToString(false), "--Name[null] Value[null] Type[null] ThrewException[False]");
            Assert.AreEqual(D5.ToString(false), "**Name[null] Value[null] Type[null] ThrewException[False]");
        }

        [TestMethod()]
        public void DumpCollectionTest()
        {
            var expecteds = new List<string>();

            // D1
            expecteds.Add("Name[int1] Value[1] Type[System.Int32] ThrewException[False]");
            Assert.IsTrue(expecteds.SequenceEqual(D1.DumpCollection()));

            // D2 例外が発生しているコレクションが１２個中１１個
            foreach (var tmp in D2.DumpCollection().Where(v => v.IndexOf("ThrewException[True]") != -1)) { 
                Console.WriteLine(tmp);
            }

            Assert.AreEqual(8, D2.DumpCollection().Count(v => v.IndexOf("ThrewException[True]") != -1));

            // D3
            expecteds.Clear();
            expecteds.Add("Name[null] Value[null] Type[null] ThrewException[False]");
            Assert.IsTrue(expecteds.SequenceEqual(D3.DumpCollection()));

            // D6
            expecteds.Clear();
            expecteds.AddRange(GetTestClassDumpCollection());
            expecteds.AddRange(GetTestClassBaseDumpCollection());
            expecteds.Sort();

            var list = D6.DumpCollection().ToList();
            list.Sort();

            // PropertyThrowExceptionBaseだけ部分一致　値が環境に依存するため。
            var index = list
                .Where(v => v.StartsWith(PropertyThrowExceptionBasePre) && v.EndsWith(PropertyThrowExceptionBaseSuf))
                .First();
            Assert.IsTrue(list.Remove(index));
            Assert.IsTrue(expecteds.Remove((PropertyThrowExceptionBasePre + (PropertyThrowExceptionBaseSuf))));

            Assert.IsTrue(expecteds.SequenceEqual(list));

            // D7
            expecteds.Clear();
            expecteds.AddRange(GetTestClassDumpCollection());
            expecteds.Sort();
            var D7temp = D7.DumpCollection().ToList();
            D7temp.Sort();
            Assert.IsTrue(expecteds.SequenceEqual(D7temp));

            // D8
            expecteds.Clear();
            expecteds.Add("Name[TestClass] Value[OHLibrary.Tests.TestClass] Type[OHLibrary.Tests.TestClass] ThrewException[False]");
            expecteds.Add("-Name[FieldStringPublicStatic] Value[Static] Type[System.String] ThrewException[False]");
            Assert.IsTrue(expecteds.SequenceEqual(D8.DumpCollection().ToList()));

            // D9
            expecteds.Clear();
            expecteds.AddRange(GetTestClassDumpCollectionNoPublic());
            Assert.IsTrue(expecteds.SequenceEqual(D9.DumpCollection().ToList()));
            
            // D10
            Console.WriteLine(D11.Dump());
        }

        [TestMethod()]
        public void ValuesTest()
        {
            
            Assert.AreEqual(0, D1.GetCollections().ToList().Count());


            var expected = new List<MemberValue>();
            //expected.Add(new MemberValue())
            var D7aa = D7.GetCollections();
        }

        private static List<string> GetTestClassDumpCollection()
        {
            var returnValue = new List<string>();
            returnValue.Add("Name[TestClass] Value[OHLibrary.Tests.TestClass] Type[OHLibrary.Tests.TestClass] ThrewException[False]");
            returnValue.Add("-Name[PropertyPublic] Value[1] Type[System.Int32] ThrewException[False]");
            returnValue.Add("-Name[Property2Public] Value[System.Int32[]] Type[System.Int32[]] ThrewException[False]");
            returnValue.Add("--Name[Property2Public[0]] Value[1] Type[System.Int32] ThrewException[False]");
            returnValue.Add("--Name[Property2Public[1]] Value[2] Type[System.Int32] ThrewException[False]");
            returnValue.Add("--Name[Property2Public[2]] Value[3] Type[System.Int32] ThrewException[False]");
            returnValue.Add("-Name[FieldArrayPublic] Value[System.Int32[]] Type[System.Int32[]] ThrewException[False]");
            returnValue.Add("--Name[FieldArrayPublic[0]] Value[1] Type[System.Int32] ThrewException[False]");
            returnValue.Add("--Name[FieldArrayPublic[1]] Value[2] Type[System.Int32] ThrewException[False]");
            returnValue.Add("--Name[FieldArrayPublic[2]] Value[3] Type[System.Int32] ThrewException[False]");
            returnValue.Add("-Name[FieldListPublic] Value[System.Collections.Generic.List`1[System.Object]] Type[System.Collections.Generic.List`1[System.Object]] ThrewException[False]");
            returnValue.Add("--Name[FieldListPublic[0]] Value[FieldListPublic] Type[System.String] ThrewException[False]");
            returnValue.Add("--Name[FieldListPublic[1]] Value[null] Type[null] ThrewException[False]");
            returnValue.Add("-Name[FieldStringPublic] Value[null] Type[System.String] ThrewException[False]");
            return returnValue;
        }

        private static List<string> GetTestClassDumpCollectionNoPublic()
        {
            var returnValue = new List<string>();
            returnValue.Add("Name[TestClass] Value[OHLibrary.Tests.TestClass] Type[OHLibrary.Tests.TestClass] ThrewException[False]");
            returnValue.Add("-Name[PropertyProtectedStaticBase] Value[0] Type[System.Int32] ThrewException[False]");
            returnValue.Add("-Name[FieldPrivateStatic] Value[0] Type[System.Int32] ThrewException[False]");
            return returnValue;
        }


        static readonly string PropertyThrowExceptionBasePre = "-Name[PropertyThrowExceptionBase] Value[";
        static readonly string PropertyThrowExceptionBaseSuf = "] Type[System.Reflection.TargetInvocationException] ThrewException[True]";

        private static List<string> GetTestClassBaseDumpCollection()
        {
            var returnValue = new List<string>();
            returnValue.Add("-Name[PropertyPublicBase] Value[1] Type[System.Int32] ThrewException[False]");
            returnValue.Add("-Name[Property2PublicBase] Value[System.Int32[]] Type[System.Int32[]] ThrewException[False]");
            returnValue.Add("--Name[Property2PublicBase[0]] Value[1] Type[System.Int32] ThrewException[False]");
            returnValue.Add("--Name[Property2PublicBase[1]] Value[2] Type[System.Int32] ThrewException[False]");
            returnValue.Add("--Name[Property2PublicBase[2]] Value[3] Type[System.Int32] ThrewException[False]");
            returnValue.Add(PropertyThrowExceptionBasePre + PropertyThrowExceptionBaseSuf);
            returnValue.Add("-Name[PropertyReturnException2Base] Value[System.Exception: test] Type[System.Exception] ThrewException[False]");
            returnValue.Add("-Name[FieldArrayPublicBase] Value[System.Int32[]] Type[System.Int32[]] ThrewException[False]");
            returnValue.Add("--Name[FieldArrayPublicBase[0]] Value[1] Type[System.Int32] ThrewException[False]");
            returnValue.Add("--Name[FieldArrayPublicBase[1]] Value[2] Type[System.Int32] ThrewException[False]");
            returnValue.Add("--Name[FieldArrayPublicBase[2]] Value[3] Type[System.Int32] ThrewException[False]");
            returnValue.Add("-Name[FieldListPublicBase] Value[System.Collections.Generic.List`1[System.Object]] Type[System.Collections.Generic.List`1[System.Object]] ThrewException[False]");
            returnValue.Add("--Name[FieldListPublicBase[0]] Value[FieldListPublic] Type[System.String] ThrewException[False]");
            returnValue.Add("--Name[FieldListPublicBase[1]] Value[null] Type[null] ThrewException[False]");
            returnValue.Add("--Name[FieldListPublicBase[2]] Value[3] Type[System.Int32] ThrewException[False]");
            returnValue.Add("-Name[FieldStringPublicBase] Value[null] Type[System.String] ThrewException[False]");
            return returnValue;
        }
    }
}