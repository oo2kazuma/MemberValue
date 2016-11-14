using System;
using System.Runtime.InteropServices;

namespace OHLibrary
{
    /// <summary>値の検索方法を制御するフラグを指定します。</summary>
    [ComVisible(true)]
    [Flags]
    public enum SearchCondition
    {
        /// <summary>何も指定されていない状態です。</summary>
        None = 0,
        /// <summary>インスタンス メンバーを検索に含めるように指定します。</summary>
        Instance = 1,
        /// <summary>静的メンバーを検索に含めるように指定します。</summary>
        Static = 2,
        /// <summary>パブリック メンバーを検索に含めるように指定します。</summary>
        Public = 4,
        /// <summary>非パブリック メンバーを検索に含めるように指定します。</summary>
        NonPublic = 8,
        /// <summary>フィールドを検索に含めるように指定します。</summary>
        Field = 16,
        /// <summary>プロパティを検索に含めるように指定します。</summary>
        Property = 32,
        /// <summary>メソッドを検索に含めるように指定します。引数ありまたは返り値なしのメソッドは対象になりません。</summary>
        Method = 64,
        /// <summary>指定した型の階層のレベルで宣言されたメンバーだけが対象になるように指定します。継承されたメンバーは対象になりません。</summary>
        DeclaredOnly = 128,
        /// <summary>階層上位のパブリックおよびプロテクトの静的メンバーを返す場合に指定します。継承クラスのプライベートな静的メンバーは返されません。静的メンバーには、フィールド、メソッド、イベント、プロパティなどがあります。入れ子にされた型は返されません。</summary>
        FlattenHierarchy = 256,
        /// <summary>取得する情報が最大になるような条件に指定します。</summary>
        MaxOutPut = 383
    }
}
