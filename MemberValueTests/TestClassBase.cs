using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OHLibrary.Tests
{

    public class TestClassBase
    {
        //フィールド
        public static int FieldPublicBase;
        public int[] FieldArrayPublicBase = new int[] { 1, 2, 3 };
        public List<object> FieldListPublicBase = new List<object>();
        public string FieldStringPublicBase = null;
        public static string FieldStringPublicStaticBase = "StaticBase";

        //プロパティ
        public int PropertyPublicBase { get { return FieldPublicBase; } }
        public int[] Property2PublicBase { get { return FieldArrayPublicBase; } }
        public int PropertyThrowExceptionBase
        {
            get
            {
                throw new System.Exception("test");
                return 1;
            }
        }
        public Exception PropertyReturnException2Base { get { return new Exception("test"); } }
        protected static int PropertyProtectedStaticBase { get; set; }
        private static int PropertyPrivateStaticBase { get; set; }


        //コンストラクタ
        public TestClassBase(int val)
        {
            FieldPublicBase = val;
            FieldListPublicBase.Add("FieldListPublic");
            FieldListPublicBase.Add(null);
            FieldListPublicBase.Add(3);
        }
    }
}
