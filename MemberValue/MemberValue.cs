using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

[assembly: CLSCompliant(true)]
namespace OHLibrary
{
    /// <summary> 
    /// オブジェクトが持つメンバー情報[フィールド・プロパティ・メソッド（引数なし・返り値あり）]を取得するクラスです。
    /// <see cref="Value"/>が基本的な型（ネームスペースがSystemで始まる）のメンバーは取得できません。
    /// <see cref="Value"/>がnullでなく、<see cref="TypeOfValue"/>がnullの場合、<see cref="Value"/>からTypeを取得します。
    /// <see cref="Value"/>の型と<see cref="TypeOfValue"/>で指定された型が違っても例外としません。
    /// </summary>
    public sealed class MemberValue
    {
        #region constructor
        /// <summary>新しいインスタンスを初期化します。</summary>
        /// <param name="name">値の名前</param>
        /// <param name="value">値</param>
        public MemberValue(string name, object value) : this(name, value, DefaultSearchingConditions) { }
        /// <summary>新しいインスタンスを初期化します。</summary>
        /// <param name="name">値の名前</param>
        /// <param name="value">値</param>
        /// <param name="searchingConditions">値のメンバーを検索条件</param>
        public MemberValue(string name, object value, SearchCondition searchingConditions) : this(name, value, searchingConditions, DefaultTypeOfValue) { }
        /// <summary>新しいインスタンスを初期化します。</summary>
        /// <param name="name">値の名前</param>
        /// <param name="value">値</param>
        /// <param name="typeOfValue">値の型</param>
        public MemberValue(string name, object value, Type typeOfValue) : this(name, value, DefaultSearchingConditions, typeOfValue) { }
        /// <summary>新しいインスタンスを初期化します。</summary>
        /// <param name="name">値の名前</param>
        /// <param name="value">値</param>
        /// <param name="searchingConditions">値のメンバーを検索条件</param>
        /// <param name="typeOfValue">値の型</param>
        public MemberValue(string name, object value, SearchCondition searchingConditions, Type typeOfValue) : this(name, value, searchingConditions, typeOfValue, DefaultDepth) { }
        /// <summary>新しいインスタンスを初期化します。</summary>
        /// <param name="name">値の名前</param>
        /// <param name="value">値</param>
        /// <param name="searchingConditions">値のメンバーを検索条件</param>
        /// <param name="typeOfValue">値の型</param>
        /// <param name="depth">深さ</param>
        public MemberValue(string name, object value, SearchCondition searchingConditions, Type typeOfValue, int depth) : this(name, value, searchingConditions, typeOfValue, depth, DefaultThrewException) { }
        /// <summary>新しいインスタンスを初期化します。</summary>
        /// <param name="name">値の名前</param>
        /// <param name="value">値</param>
        /// <param name="searchingConditions">値のメンバーを検索条件</param>
        /// <param name="typeOfValue">値の型</param>
        /// <param name="depth">深さ</param>
        /// <param name="threwException">値の取得時に例外が発生</param>
        public MemberValue(string name, object value, SearchCondition searchingConditions, Type typeOfValue, int depth, bool threwException)
        {
            this.Name = name;
            this.Value = value;
            this.Depth = depth;
            if (typeOfValue == null && value != null)
            {
                this.TypeOfValue = value.GetType();
            }
            else
            {
                //Delete if (this.Value != null && this.Value.GetType() != TypeOfValue) throw new ArgumentException("引数の型と値のから取得した型が一致しません。", "TypeOfValue");
                this.TypeOfValue = typeOfValue;
            }
            this.SearchingConditionsValue = searchingConditions;
            this.ThrewException = threwException;
            this.DepthChar = DefaultDepthChar;

            if (this.TypeOfValue != null) this.IsSystem = this.TypeOfValue.Namespace.StartsWith("System", StringComparison.Ordinal);
        }
        #endregion

        #region field

        #region DefaultValue
        /// <summary>既定値　深さ</summary>
        public static readonly int DefaultDepth = 0;
        /// <summary>既定値　値の型</summary>
        public static readonly Type DefaultTypeOfValue = null;
        /// <summary>既定値　メンバーの検索条件</summary>
        public static readonly SearchCondition DefaultSearchingConditions = SearchCondition.Instance | SearchCondition.Public | SearchCondition.Field | SearchCondition.Property;
        /// <summary>既定値　値の取得時に例外が発生</summary> 
        public static readonly bool DefaultThrewException = false;
        /// <summary>既定値　深さを示す記号</summary>
        public static readonly char DefaultDepthChar = '-';
        #endregion

        /// <summary>値の名前</summary>
        public string Name { private set; get; }
        /// <summary>値</summary>
        public object Value { private set; get; }
        /// <summary>深さ</summary>
        public int Depth { private set; get; }
        /// <summary>値の型</summary>
        public Type TypeOfValue { private set; get; }
        /// <summary>メンバーの検索条件</summary>
        public SearchCondition SearchingConditionsValue { private set; get; }
        /// <summary>値取得時に例外が発生</summary>
        public bool ThrewException { private set; get; }
        /// <summary>深さを示す記号</summary>
        public char DepthChar { set; get; }
        /// <summary>TypeOfValueの型がSystem名前空間に所属しているか。</summary>
        public bool IsSystem { private set; get; }
        /// <summary><see cref="ToString"/>で使用するフォーマット</summary>
        public static readonly string ToStringFormat = "Name[{0}] Value[{1}] Type[{2}] ThrewException[{3}]";
        /// <summary>フィールドを検索する</summary>
        private bool SearchField { get { return this.SearchingConditionsValue.HasFlag(SearchCondition.Field); } }
        /// <summary>プロパティを検索する</summary>
        private bool SearchProperty { get { return this.SearchingConditionsValue.HasFlag(SearchCondition.Property); } }
        /// <summary>メソッドを検索する</summary>
        private bool SearchMethod { get { return this.SearchingConditionsValue.HasFlag(SearchCondition.Method); } }
        #endregion      

        /// <summary>SearchConditionをSearchConditionに変換します。</summary>
        /// <param name="value">変換対象の値</param>
        /// <returns>変換後の値</returns>
        private static BindingFlags SearchingConditionsToBindingFlags(SearchCondition value)
        {
            var returnValue = BindingFlags.Default;
            if (value != SearchCondition.None)
            {
                if (value.HasFlag(SearchCondition.Instance)) returnValue |= BindingFlags.Instance;
                if (value.HasFlag(SearchCondition.Static)) returnValue |= BindingFlags.Static;
                if (value.HasFlag(SearchCondition.Public)) returnValue |= BindingFlags.Public;
                if (value.HasFlag(SearchCondition.NonPublic)) returnValue |= BindingFlags.NonPublic;
                if (value.HasFlag(SearchCondition.DeclaredOnly)) returnValue |= BindingFlags.DeclaredOnly;
                if (value.HasFlag(SearchCondition.FlattenHierarchy)) returnValue |= BindingFlags.FlattenHierarchy;
            }
            return returnValue;
        }

        #region Dump
        /// <summary>ダンプ文字列のコレクションを取得します 。</summary>
        public IEnumerable<string> DumpCollection()
        {
            yield return this.ToString(false);
            foreach (var v in this.GetCollections())
                foreach (var vv in v.DumpCollection()) yield return vv;
        }

        /// <summary>ダンプ文字列を取得します。</summary>
        public string Dump() { return string.Join(Environment.NewLine, this.DumpCollection().ToArray()); }
        #endregion

        #region ToString
        /// <summary>現在のオブジェクトを表す文字列を返します。</summary>
        /// <returns>現在のオブジェクトを表す文字列。</returns>
        public override string ToString() { return this.ToString(true); }

        /// <summary>現在のオブジェクトを表す文字列を返します。</summary>
        /// <param name="ignoreDepth">深さを示す記号を文字列に含めません。</param>
        /// <returns>現在のオブジェクトを表す文字列。</returns>
        public string ToString(bool ignoreDepth)
        {
            string Name, Value, TypeOfValue = string.Empty;
            try
            {
                Name = this.Name == null ? "null" : this.Name.ToString();
                Value = this.Value == null ? "null" : this.Value.ToString();
                TypeOfValue = this.TypeOfValue == null ? "null" : this.TypeOfValue.ToString();
            }
            finally { }
            var Formated = string.Format(CultureInfo.CurrentCulture, ToStringFormat, Name, Value, TypeOfValue, this.ThrewException, this.Depth);
            if (!ignoreDepth) Formated = new string(DepthChar, this.Depth) + Formated;
            return Formated;
        }
        #endregion

        #region Get
        /// <summary><see cref="Value"/>を配列値として取得します。</summary>
        private IEnumerable<MemberValue> GetArrayValues()
        {
            Array ArrayValue = (Array)this.Value;
            foreach (var i in Enumerable.Range(0, ArrayValue.Length))
            {
                var v = ArrayValue.GetValue(i);
                var t = v != null ? v.GetType() : null;
                yield return new MemberValue(string.Format("{0}[{1}]", this.Name, i), v, this.SearchingConditionsValue, t, this.Depth + 1);
            }
        }

        /// <summary><see cref="Value"/>をリスト値として取得します。</summary>
        private IEnumerable<MemberValue> GetListValues()
        {
            dynamic ListValue = this.Value;
            foreach (var i in Enumerable.Range(0, ListValue.Count))
            {
                var v = ListValue[i];
                var t = v != null ? v.GetType() : null;
                yield return new MemberValue(string.Format("{0}[{1}]", this.Name, i), v, this.SearchingConditionsValue, t, this.Depth + 1);
            }
        }

        /// <summary>
        /// オブジェクトが持つメンバー情報[フィールド・プロパティ・メソッド（引数なし・返り値あり）]を取得します。
        /// </summary>
        public IEnumerable<MemberValue> GetCollections()
        {
            if (this.Value is Array)
            {
                // Array Value
                foreach (var tmp in GetArrayValues()) yield return tmp;
            }
            else if (this.Value != null && this.Value.GetType().IsGenericType && this.Value.GetType().GetGenericTypeDefinition() == typeof(List<>))
            {
                // List Value
                foreach (var tmp in GetListValues()) yield return tmp;
            }
            else
            {
                // Single Value
                if (!this.IsSystem && this.TypeOfValue != null)
                {
                    var bindingFlags = SearchingConditionsToBindingFlags(this.SearchingConditionsValue);
                    MemberInfo[] members = this.TypeOfValue.GetMembers(bindingFlags);
                    foreach (var m in members)
                    {
                        MemberValue tmp = null;
                        Exception catchedException = null;

                        try
                        {
                            if (this.SearchProperty && m.MemberType == MemberTypes.Property)
                                {
                                PropertyInfo info = this.TypeOfValue.GetProperty(m.Name, bindingFlags);
                                tmp = new MemberValue(m.Name, info.GetValue(this.Value, null), this.SearchingConditionsValue, info.PropertyType, this.Depth + 1);
                            }
                            else if (this.SearchField && m.MemberType == MemberTypes.Field)
                            {
                                FieldInfo info = this.TypeOfValue.GetField(m.Name, bindingFlags);
                                tmp = new MemberValue(m.Name, info.GetValue(this.Value), this.SearchingConditionsValue, info.FieldType, this.Depth + 1);
                            }
                            else if (this.SearchMethod && m.MemberType == MemberTypes.Method)
                            {
                                MethodInfo info = this.TypeOfValue.GetMethod(m.Name, bindingFlags);
                                ParameterInfo[] prminfo = info.GetParameters();
                                if (prminfo.Length == 0 && info.ReturnType != typeof(void))
                                {
                                    var v = info.Invoke(this.Value, bindingFlags, null, null, null);
                                    tmp = new MemberValue(m.Name, v, this.SearchingConditionsValue, info.ReturnType, this.Depth + 1);
                                }
                            }
                        }
                        catch (Exception ex) { catchedException = ex; }

                        if (catchedException != null) tmp = new MemberValue(m.Name, catchedException, this.SearchingConditionsValue, catchedException.GetType(), this.Depth + 1, true);
                        if (tmp != null) yield return tmp;
                    }
                }

            }
        }
        #endregion
    }

}
