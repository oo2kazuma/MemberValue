# MemberValue

====

##Overview

It is a library to dump on C #.  
Displays the value of each field of the object as a string.

(�����C���Ń_���v���郉�C�u�����ł��B  
�I�u�W�F�N�g�̊e�t�B�[���h�̒l�𕶎���ŕ\�����܂��B)

## Description
C # does not have a way to dump objects by string.  
This library makes this possible.  
You do not have to override the ToString method in your own class.    
Please use it in the debug log.

(C#�ɂ̓I�u�W�F�N�g�𕶎���Ń_���v������@��������Ă��܂���B  
���̃��C�u�����͂�����\�ɂ�����̂ł��B  
��X����̃N���X��ToString���\�b�h���I�[�o�[���C�h����K�v������܂���B  
�f�o�b�O���O�ł����p���������B)

## Requirement
.NET Framework 4 Client Profile 

## Usage

1.Add MemberValue.dll to the project reference.(�v���W�F�N�g�̎Q�Ƃ� [MemberValue.dll](https://github.com/oo2kazuma/MemberValue/releases/tag/0.9.7) ��ǉ����Ă��������B)  

2.Create an instance of the MemberValue class with the object you want to dump as an argument.(
�_���v�������I�u�W�F�N�g��������MemberValue�N���X�̃C���X�^���X���쐬���Ă��������B)    

3.Please dump.(�_���v���Ă��������B)  

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
