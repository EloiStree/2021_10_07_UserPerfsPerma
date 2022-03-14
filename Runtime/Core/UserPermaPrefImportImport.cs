using Eloi;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class UserPermaPrefImport
{
    public enum ReadingCollection { Primitive, Dynamique }


    public static void ImportUsersPermaFromPathToRegister(in Eloi.IMetaAbsolutePathDirectoryGet directory,
        ref AbstractUserPermaPrefRegister register) {
        string dirPath = directory.GetPath();
        if (!Directory.Exists(dirPath))
            return;
        string[] filePaths = Directory.GetFiles(dirPath, "*" + UserPermaPrefImportExport.FileExtensionName, SearchOption.AllDirectories);

        for (int i = 0; i < filePaths.Length; i++)
        {
            UserPermaPref user;
            UserPermaPrefImport.ImportUserPermaPrefFromPath(in filePaths[i], out user, out bool converted);
            if (converted)
            {
                user.GetUserId(out string id);
                if (Eloi.E_StringUtility.IsFilled(id)) {
                    register.OverrideOrAdd(in user);
                }
            }
        }

    }




    public static void ImportUserPermaPrefFromPath(in string path,
     out UserPermaPref userWithContext, out bool converted)
    {
        userWithContext = new UserPermaPref();
        ImportUserPermaPrefFromPathIn(in path, ref userWithContext, out converted);
    }
        public static void ImportUserPermaPrefFromPathIn(in string path,
        ref UserPermaPref userWithContext, out bool converted ) {

        if (File.Exists(path))
        {
            string fileText = File.ReadAllText(path);
            ImportUserPermaPrefFromText(in fileText,out converted, ref userWithContext);
         
            return;
        }

        converted = false;
    }


    public static void ImportUserPermaPrefFromText(in string text, out bool convertedUser,
        ref UserPermaPref playerWithContext)
    {
       
        convertedUser = false;



         ReadingCollection collectionTypeEnum = ReadingCollection.Primitive;
        string collectionAlias = "", collectionType = "", collectionAssemblyName = "";
        string[] lines = text.Split('\n');
        Regex keyvalueRegex = new Regex("^\\s*\".*\"\\s*:\\s*\".*\"\\s*$");





        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.Length <= 0) continue;
            if (line.IndexOf(">>") == 0)
            {
                //Start of a dynamique collection;
                collectionTypeEnum = ReadingCollection.Dynamique;
                CatchCollectionInfo(in line, out bool converted, out collectionAlias, out collectionType, out collectionAssemblyName);
                //Debug.Log(string.Format(
                //"Dynamique|{4}|{0} ({1}, {2})  >{3}",
                //collectionAlias, collectionType, collectionAssemblyName,  line, converted) );

            }
            else if (line.IndexOf(">>") != 0 && line.IndexOf(">") == 0)
            {
                //Start of a primitive collection;
                collectionTypeEnum = ReadingCollection.Primitive;
                CatchCollectionInfo(in line, out bool converted, out collectionAlias, out collectionType, out collectionAssemblyName);
                // Debug.Log(string.Format(
                // "Primitive|{0} ({1}, {2})  >{3}",
                //collectionAlias, collectionType, collectionAssemblyName, line));

            }
            else if (keyvalueRegex.IsMatch(line))
            {
                //Is a value;
                ExtractKeyValueOf(in line, out bool converted, out string key, out string value);
                if (converted)
                {
                    UserPermaPref pref =playerWithContext;
                    if (collectionTypeEnum == ReadingCollection.Primitive)
                    {
                        PushPrimitive(in collectionAlias, in key, in value, in pref);
                    }
                    else
                    if (collectionTypeEnum == ReadingCollection.Dynamique)
                    {

                        PushDynamique(in collectionAlias, in collectionType, in collectionAssemblyName, out bool convertedDynamique, in key, in value, in pref);
                    }
                }
            }
            else if (line.ToLower().IndexOf("useralias:") == 0)
            {
                string value = line.Substring("useralias:".Length).Trim();
                playerWithContext.SetUserNameAlias(in value);
            }
            else if (line.ToLower().IndexOf("userid:") == 0)
            {

                string value = line.Substring("userid:".Length).Trim();
                playerWithContext.SetUserId(in value);
            }

            convertedUser = true;
        }

    }

    private static void PushDynamique(in string collectionAlias, in string collectionType, in string collectionAssembly, out bool converted, in string key, in string value, in UserPermaPref userPref)
    {
        converted = false;
        List<IKeyPropertiesAsStringFullInteraction> collections = userPref.m_dynamiqueStorage.m_dynamiqueStorage;
        for (int i = 0; i < collections.Count; i++)
        {
            collections[i].GetAliasTypeName(out string cAlias);
            collections[i].GetTypeAsString(out string cType, out string cAssembly);
            //Debug.Log("Try to parse:" + string.Join("|", cAlias, cType, cAssembly) + " >To> " +
            // string.Join("|", collectionAlias, collectionType, collectionAssembly));
            if (E_StringUtility.AreEquals(in cAlias, in collectionAlias, true, true)
                && E_StringUtility.AreEquals(in cType, in collectionType, true, true)
                && E_StringUtility.AreEquals(in cAssembly, in collectionAssembly, true, true)
                )
            {
                // Debug.Log("Parsed :) :" + string.Join("|", cAlias, cType, cAssembly) + " >To> " +
                //          string.Join("|", collectionAlias, collectionType, collectionAssembly));

                string valueToSend = value;
                if (collections[i].HasDangerousCharacter())
                {
                    E_StringByte64Utility.GetTextFromTextB64(in valueToSend, out bool b64Converted, out valueToSend);

                }
                collections[i].SetValue(in key, in valueToSend, out bool convertedAsValue);
                if (convertedAsValue)
                {
                    converted = true;
                    return;
                }
                // Try to parse

            }
        }


    }

    private static void PushPrimitive(in string collectionAlias, in string key, in string value, in UserPermaPref userPref)
    {
        string tempAlias = collectionAlias.ToLower().Trim();
        if (tempAlias == "bool")
        {
            bool.TryParse(value, out bool pValue);
            userPref.m_primitivesStorage.m_bool.SetValue(in key, in pValue);

        }
        else if (tempAlias == "byte")
        {
            byte.TryParse(value, out byte pValue);
            userPref.m_primitivesStorage.m_byte.SetValue(in key, in pValue);

        }
        else if (tempAlias == "sbyte")
        {
            sbyte.TryParse(value, out sbyte pValue);
            userPref.m_primitivesStorage.m_signByte.SetValue(in key, in pValue);

        }
        else if (tempAlias == "char")
        {

            char.TryParse(value, out char pValue);
            userPref.m_primitivesStorage.m_char.SetValue(in key, in pValue);
        }
        else if (tempAlias == "decimal")
        {

            decimal.TryParse(value, out decimal pValue);
            userPref.m_primitivesStorage.m_decimal.SetValue(in key, in pValue);
        }
        else if (tempAlias == "double")
        {

            double.TryParse(value, out double pValue);
            userPref.m_primitivesStorage.m_double.SetValue(in key, in pValue);
        }
        else if (tempAlias == "float")
        {

            float.TryParse(value, out float pValue);
            userPref.m_primitivesStorage.m_float.SetValue(in key, in pValue);
        }
        else if (tempAlias == "int")
        {

            int.TryParse(value, out int pValue);
            userPref.m_primitivesStorage.m_int.SetValue(in key, in pValue);
        }
        else if (tempAlias == "uint")
        {

            uint.TryParse(value, out uint pValue);
            userPref.m_primitivesStorage.m_uint.SetValue(in key, in pValue);
        }
        else if (tempAlias == "long")
        {

            long.TryParse(value, out long pValue);
            userPref.m_primitivesStorage.m_long.SetValue(in key, in pValue);
        }
        else if (tempAlias == "ulong")
        {

            ulong.TryParse(value, out ulong pValue);
            userPref.m_primitivesStorage.m_ulong.SetValue(in key, in pValue);
        }
        else if (tempAlias == "short")
        {

            short.TryParse(value, out short pValue);
            userPref.m_primitivesStorage.m_short.SetValue(in key, in pValue);
        }
        else if (tempAlias == "ushort")
        {

            ushort.TryParse(value, out ushort pValue);
            userPref.m_primitivesStorage.m_ushot.SetValue(in key, in pValue);
        }
        else if (tempAlias == "string")
        {
            string pvalue = value;//.ToLower();
            userPref.m_primitivesStorage.m_unprotectedString.SetValue(in key, in pvalue);
        }
    }

    private static void ExtractKeyValueOf(in string line, out bool converted, out string key, out string value)
    {
        string lineTemp = line;
        int indexOfDoubleComma = lineTemp.IndexOf(":");
        if (indexOfDoubleComma >= 0)
        {

            E_StringUtility.SplitInTwo(in lineTemp, in indexOfDoubleComma, out string leftPart, out string rightPart);

            key = leftPart.Trim().Substring(1, leftPart.Length - 2);
            value = rightPart.Trim().Substring(1, rightPart.Length - 2);
            converted = true;
            return;

        }
        converted = false;
        key = "";
        value = "";
    }

    private static void CatchCollectionInfo(in string line, out bool convertionProbleme, out string collectionAlias, out string collectonType, out string collectionAssemblyName)
    {
        string lineTemp = line.Replace(">", "");
        int indexStartBracket = lineTemp.IndexOf("(");
        int indexComma = lineTemp.IndexOf(",");
        int indexEndBracket = lineTemp.IndexOf(")");
        convertionProbleme = (indexStartBracket < 0 || indexEndBracket < 0 || indexComma < 0);
        if (!convertionProbleme)
        {
            collectionAlias = lineTemp.Substring(0, indexStartBracket);
            collectonType = lineTemp.Substring(indexStartBracket + 1, indexComma - indexStartBracket - 1);
            collectionAssemblyName = lineTemp.Substring(indexComma + 1, indexEndBracket - indexComma - 1);
        }
        else collectionAlias = collectonType = collectionAssemblyName = "";
    }
}
