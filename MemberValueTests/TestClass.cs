using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OHLibrary.Tests
{
    public class TestClass : TestClassBase
    {
        //フィールド
        private int FieldPrivate;
        private static int FieldPrivateStatic;
        public int[] FieldArrayPublic = new int[] { 1, 2, 3 };
        public List<object> FieldListPublic = new List<object>();
        public string FieldStringPublic = null;
        public static string FieldStringPublicStatic = "Static";

        //プロパティ
        public int PropertyPublic { get { return FieldPrivate; } }
        public int[] Property2Public { get { return FieldArrayPublic; } }
        
        //コンストラクタ
        public TestClass(int val) : base(val)
        {
            FieldPrivate = val;
            FieldListPublic.Add("FieldListPublic");
            FieldListPublic.Add(null);
        }

    }

}
