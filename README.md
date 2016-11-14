# MemberValue

====

##Overview

It is a library to dump on C #.  
Displays the value of each field of the object as a string.

(これはC＃でダンプするライブラリです。  
オブジェクトの各フィールドの値を文字列で表示します。)

## Description
C # does not have a way to dump objects by string.  
This library makes this possible.  
You do not have to override the ToString method in your own class.    
Please use it in the debug log.

(C#にはオブジェクトを文字列でダンプする方法が備わっていません。  
このライブラリはこれを可能にするものです。  
一々自作のクラスでToStringメソッドをオーバーライドする必要もありません。  
デバッグログでご利用ください。)

## Requirement
.NET Framework 4 Client Profile 

## Usage

1.Add MemberValue.dll to the project reference.(プロジェクトの参照に [MemberValue.dll](https://github.com/oo2kazuma/MemberValue/releases/tag/0.9.7) を追加してください。)  

2.Create an instance of the MemberValue class with the object you want to dump as an argument.(
ダンプしたいオブジェクトを引数にMemberValueクラスのインスタンスを作成してください。)    

3.Please dump.(ダンプしてください。)  

## Example

###Code


        private void NewMethod()
        {
            var dumper = new OHLibrary.MemberValue("TestClassObject", new TestClass());
            Console.WriteLine(dumper.Dump());
        }

        public class TestClass
        {
            public string StringValue="StringValue";
            public int IntValue = 1;
        }

### Result
Name[TestClassObject] Value[WindowsFormsApplication1.Form1+TestClass] Type[WindowsFormsApplication1.Form1+TestClass] ThrewException[False]  
-Name[StringValue] Value[StringValue] Type[System.String] ThrewException[False]  
-Name[IntValue] Value[1] Type[System.Int32] ThrewException[False]

## Licence

[MIT](https://github.com/oo2kazuma/MemberValue/blob/master/LICENCE)

## Author

[oo2.kazuma](https://github.com/oo2kazuma)
