using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class UserPropertiesRegisterMono : MonoBehaviour
{
    [Header("Proposition")]
    public string m_subFolderProposition;
    public string m_fileNameProposition;
    [TextArea(0, 10)]
    public string m_textToStore;

    [Header("Where to store")]
    public string m_whereToStorePath = "";

    [Header("Value")]
    public UserPermaPrefWithContext m_oneUserTest;
    public KeyPropertiesUnsaveText m_unsafeText;
    public KeyPropertiesVector3Stringable m_positions = new KeyPropertiesVector3Stringable();
    public bool m_useBase64;

    [Header("Save and load")]
    public bool m_saved;
    public string m_pathUsed;
    void Start()
    {
        UserPermaPrefImportExport.SetAsUsingBase64(m_useBase64);
        m_positions.SetValue("head", Vector3.up * 1.85f);
        m_positions.SetValue("forward", Vector3.forward);
        m_oneUserTest.m_dynamiqueStorage.m_dynamiqueStorage.Add(m_positions);
        m_oneUserTest.m_dynamiqueStorage.m_dynamiqueStorage.Add(m_unsafeText);
        UserPermaPrefImportExport.GetSaveProposition(in m_oneUserTest, 
            out m_subFolderProposition,
            out m_fileNameProposition,
            out m_textToStore );

        UserPermaPrefImportExport.TryToSave(out m_saved, out m_pathUsed, in m_textToStore, in m_whereToStorePath, in m_subFolderProposition, in m_fileNameProposition) ;
    }



}
public class UserPermaPrefImportExport
{
    public static bool m_userBase64 = true;
    public static bool m_putCommentary = true;
    public static string m_defaultReplaceAlphaNum = "_";
    public static uint m_defaultMaxFileSize =200;
    public static void GetSaveProposition(in UserPermaPrefWithContext user, out string subFolderProposition, out string fileNameProposition, out string textInFile)
    {
        string nameFileAlpha = string.Format("{0}_{1}", user.m_userInfo.m_userAlias, user.m_userInfo.m_userStringId);

        E_StringUtility.ConvertToAlphaNumByReplacing(in nameFileAlpha, out nameFileAlpha, in m_defaultReplaceAlphaNum);
        E_StringUtility.Clamp(in nameFileAlpha, out nameFileAlpha, in m_defaultMaxFileSize);

        fileNameProposition = string.Format("{0}.userperfperma", nameFileAlpha);
        GetContextAlphaClamp(in user.m_context, out  subFolderProposition,in m_defaultMaxFileSize, in m_defaultReplaceAlphaNum);


        GetSaveProposition(in user.m_userInfo, out string userInfo);
        GetSaveProposition(in user.m_primitivesStorage, out string textPrimitive );
        GetSaveProposition(in user.m_dynamiqueStorage, out string textDynamique);
        StringBuilder metaInfo = new StringBuilder();
        if (m_putCommentary) { 
            metaInfo.AppendLine("//This file is build with https://github.com/EloiStree/2021_10_07_UserPerfsPerma");
            metaInfo.AppendLine("//Feel free to go watch the source and use it in Unity if you need to import and use this file.");
            metaInfo.AppendLine("//Base64 to Text: https://www.base64decode.org/");
        }

        textInFile = string.Join("\n\n", metaInfo.ToString(), userInfo, textPrimitive, textDynamique);
    }

    public static void SetAsUsingBase64(bool useBase64) {
        m_userBase64 = useBase64;
    }
    private static void GetSaveProposition(in UserContext userInfo, out string userInfoAsText)
    {
        string alias= userInfo.m_userAlias, userString = userInfo.m_userStringId;

        if (m_userBase64) { 
            E_StringByte64Utility.GetText64FromText(in userInfo.m_userAlias, out alias);
            E_StringByte64Utility.GetText64FromText(in userInfo.m_userStringId, out userString);
        }
        userInfoAsText = string.Format("useralias:{0}\nuserid:{1}\n", alias, userString);
    }
    private static void GetSaveProposition(in AllSharpPrimitiveKeyValueStorageCollection primitiveStorage, out string primitiveStorageAsText)
    {
        StringBuilder sb = new StringBuilder();
        primitiveStorage.GetAllAsObject(out object[] primitive);
        for (int i = 0; i < primitive.Length; i++)
        {
            AppendKeyProperties(in primitive[i], in sb);
        }
        primitiveStorageAsText = sb.ToString();
    }
    private static void AppendKeyProperties(in object collection, in StringBuilder sb)
    {
        if (collection is KeyPropertiesBool)
        {
            KeyProperties<bool> value = (KeyProperties<bool>)collection;
            AppendKeyProperties<bool>(in value, in sb);
        }
        else if (collection is KeyPropertiesByte)
        {
            KeyProperties<byte> value = (KeyProperties<byte>)collection;
            AppendKeyProperties<byte>(in value, in sb);
        }
        else if (collection is KeyPropertiesSignByte)
        {
            KeyProperties<sbyte> value = (KeyProperties<sbyte>)collection;
            AppendKeyProperties<sbyte>(in value, in sb);
        }
        else if (collection is KeyPropertiesShort)
        {
            KeyProperties<short> value = (KeyProperties<short>)collection;
            AppendKeyProperties<short>(in value, in sb);
        }
        else if (collection is KeyPropertiesUShort)
        {
            KeyProperties<ushort> value = (KeyProperties<ushort>)collection;
            AppendKeyProperties<ushort>(in value, in sb);
        }
        else if (collection is KeyPropertiesChar)
        {
            KeyProperties<char> value = (KeyProperties<char>)collection;
            AppendKeyProperties<char>(in value, in sb);
        }
        else if (collection is KeyPropertiesDecimal)
        {
            KeyProperties<decimal> value = (KeyProperties<decimal>)collection;
            AppendKeyProperties<decimal>(in value, in sb);
        }
        else if (collection is KeyPropertiesDouble)
        {
            KeyProperties<double> value = (KeyProperties<double>)collection;
            AppendKeyProperties<double>(in value, in sb);
        }
        else if (collection is KeyPropertiesFloat)
        {
            KeyProperties<float> value = (KeyProperties<float>)collection;
            AppendKeyProperties<float>(in value, in sb);
        }
        else if (collection is KeyPropertiesInt)
        {
            KeyProperties<int> value = (KeyProperties<int>)collection;
            AppendKeyProperties<int>(in value, in sb);
        }
        else if (collection is KeyPropertiesUInt)
        {
            KeyProperties<uint> value = (KeyProperties<uint>)collection;
            AppendKeyProperties<uint>(in value, in sb);
        }
        else if (collection is KeyPropertiesLong)
        {
            KeyProperties<long> value = (KeyProperties<long>)collection;
            AppendKeyProperties<long>(in value, in sb);
        }
        else if (collection is KeyPropertiesULong)
        {
            KeyProperties<ulong> value = (KeyProperties<ulong>)collection;
            AppendKeyProperties<ulong>(in value, in sb);
        }
        else if (collection is KeyPropertiesShort)
        {
            KeyProperties<short> value = (KeyProperties<short>)collection;
            AppendKeyProperties<short>(in value, in sb);
        }
        else if (collection is KeyPropertiesUShort)
        {
            KeyProperties<ushort> value = (KeyProperties<ushort>)collection;
            AppendKeyProperties<ushort>(in value, in sb);
        }
        else if (collection is KeyPropertiesUnprotectedString)
        {
            KeyProperties<string> value = (KeyProperties<string>)collection;
            AppendKeyProperties<string>(in value, in sb);
        }
    }
    private static void AppendKeyProperties<T>(in KeyProperties<T> collection, in StringBuilder sb)
    {
        string[] keys;
        collection.GetAllKey(out keys);
        collection.GetTypeOfStorage(out Type type, out System.Reflection.Assembly assembly);

        string name = "";
        if (type == typeof(bool)) name = "bool";
        else if (type == typeof(byte)) name = "byte";
        else if (type == typeof(sbyte)) name = "sbyte";
        else if (type == typeof(char)) name = "char";
        else if (type == typeof(decimal)) name = "decimal";
        else if (type == typeof(double)) name = "double";
        else if (type == typeof(float)) name = "float";
        else if (type == typeof(int)) name = "int";
        else if (type == typeof(uint)) name = "uint";
        else if (type == typeof(long)) name = "long";
        else if (type == typeof(ulong)) name = "ulong";
        else if (type == typeof(short)) name = "short";
        else if (type == typeof(ushort)) name = "ushort";
        else name = typeof(T).Name;


        if (keys.Length > 0)
        {
            sb.Append(string.Format("\n>{0} ({1}, {2})\n", name, typeof(T).FullName, assembly.GetName().Name ));
            foreach (string key in keys)
            {


                collection.GetValue(in key,out bool found, out string value);
                string tk =key, tv= value;

                if (m_userBase64)
                {
                    E_StringByte64Utility.GetText64FromText(in tv, out tv);
                }

                if (found)
                    sb.Append(string.Format("\"{0}\":\"{1}\"\n", tk, tv));
            }
        }
    }
    private static void Append(in IKeyPropertiesAsStringFullInteraction collection  , in StringBuilder sb)
    {
        string[] keys;
        collection.GetAllKey(out keys);
        collection.GetTypeAliasNameAsString(out string typeName);
        collection.GetTypeAsString(out string typeFullname, out string assembly);
        //Type type;
        //System.Reflection.Assembly assembly;



        if (keys.Length > 0)
        {
            sb.Append(string.Format("\n>> {0} ({1}, {2})\n", typeName, typeFullname, assembly));
            foreach (string key in keys)
            {
                collection.GetValue(in key, out bool found, out string value);

                string tk = key, tv = value;
                if (m_userBase64)
                {
                    E_StringByte64Utility.GetText64FromText(in tv, out tv);
                }
                if (found)
                    sb.Append(string.Format("\"{0}\":\"{1}\"\n", tk, tv));
            }
        }
    }


    private static void GetSaveProposition(in DynamiqueKeyValueStorageCollection dynamiqueStorage, out string dynmaiqueStorageAsText)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < dynamiqueStorage.m_dynamiqueStorage.Count; i++)
        {
            IKeyPropertiesAsStringFullInteraction collection= dynamiqueStorage.m_dynamiqueStorage[i];
            Append(in collection,in sb);
        }
        dynmaiqueStorageAsText = sb.ToString();
    }

    private static void GetContextAlphaClamp(in RecordContext context, out string folderName, in uint maxFileCharacter=32, in string replaceBy="_")
    {
        string nameFileAlpha = string.Format("{0}_{1}_{2}_{3}", context.m_realLifeEventAliasID, context.m_computerAliasID,context.m_applicationAliasID, context.m_applicatoinVersion);
        E_StringUtility.ConvertToAlphaNumByReplacing (nameFileAlpha, out string compressedName, in replaceBy);
        E_StringUtility.Clamp(in compressedName, out folderName, in maxFileCharacter);
    }

    public static void TryToSave(in string textToStore, in string subFolderProposition, in string fileNameProposition, in string whereToStorePath)
    {
        TryToSave(out bool savedTmp, out string pathTmp, in textToStore, in subFolderProposition, in fileNameProposition, in whereToStorePath);
    }

    public static void TryToSave(out bool saved, out string pathUsed, in string textToStore, in string whereToStorePath, in string subFolderProposition, in string fileNameProposition)
    {
        saved = false;
        pathUsed = "";
        if (!Directory.Exists(whereToStorePath))
        { return; }
        string subFolderPath = whereToStorePath + "/" + subFolderProposition ;
        string subFilePath = whereToStorePath + "/" + subFolderProposition + "/" + fileNameProposition;
        if (!Directory.Exists(subFolderPath))
            Directory.CreateDirectory(subFolderPath);
        File.WriteAllText(subFilePath, textToStore);


        saved = true;
        pathUsed = subFilePath;
    }
}
