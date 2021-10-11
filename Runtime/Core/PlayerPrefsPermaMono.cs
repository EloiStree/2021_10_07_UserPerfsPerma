using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using UnityEngine;

public class PlayerPrefsPermaMono : MonoBehaviour
{
    public KeyPropertiesInt m_testInt;
    public KeyPropertiesVector3 m_v3;
    public KeyPropertiesLong m_long;

    public KeyPropertiesVector3Stringable m_vector3StringAble;
    public string m_key;
    public bool m_found;
    public string m_valueOfVector3;
    public string m_newKey;
    public bool m_wasPushedIn;
    public string m_valueOfVector3NewKey;

    private void Awake()
    {
        m_vector3StringAble.GetValue(m_key, out m_found, out m_valueOfVector3);
        m_vector3StringAble.SetValue(m_newKey, in m_valueOfVector3 , out m_wasPushedIn);
        m_vector3StringAble.GetValue(m_newKey, out m_found, out m_valueOfVector3NewKey);
    }
}

[System.Serializable]
public class UserContext {
    public string m_userAlias;
    public string m_userStringId;
}

[System.Serializable]
public class UserPermaPref
{
    public UserContext m_userInfo = new UserContext();

    public string UserId { get { return m_userInfo.m_userStringId; } set { m_userInfo.m_userStringId = value; } }

    public void GetUnthrustedTextValue(in string key, out string value)
    {
        CheckThatUnthurstedTextExist();
        m_unthrustedText.GetValue(key, out value);

    }

    public void TryToGuestKeyValueAsString(in string key, out string value)
    {
        m_primitivesStorage.GetAllAsObject(out object[] collections);
      
        bool found;
        for (int i = 0; i < collections.Length; i++)
        {
            IKeyProperitiesAsStringGet collectionTarget = collections[i] as IKeyProperitiesAsStringGet;
            if (collectionTarget != null) {
                collectionTarget.GetValue(in key, out found, out value);
                if (found) {
                    return;
                }
            }
        }
        List<IKeyPropertiesAsStringFullInteraction> dynamicCollections = m_dynamiqueStorage.m_dynamiqueStorage;
        for (int i = 0; i < dynamicCollections.Count; i++)
        {
            IKeyPropertiesGenericGet collectionTarget = dynamicCollections[i] as IKeyPropertiesGenericGet; ;
            if (collectionTarget != null)
            {
                collectionTarget.GetValueObject(in key, out found, out object objRecovered);
                if (found)
                {
                    value = objRecovered.ToString();
                    return;
                }
            }
        }
        value = "";
    }

    public  void AppendKeysAsDistinctIn(ref Dictionary<string,string> keys)
    {
        if (keys == null)
            keys = new Dictionary<string, string>();
        m_primitivesStorage.GetAllAsObject(out object[] collections);
        
        for (int i = 0; i < collections.Length; i++)
        {
            IKeyProperitiesKeysContainer collectionTarget = collections[i] as IKeyProperitiesKeysContainer;
            if (collectionTarget != null)
            {
                collectionTarget.GetAllKey(out string [] k);
                for (int j = 0; j < k.Length; j++)
                {
                    if (!keys.ContainsKey(k[j]))
                        keys.Add(k[j], null);

                }
            }
        }
        List<IKeyPropertiesAsStringFullInteraction> dynamicCollections = m_dynamiqueStorage.m_dynamiqueStorage;
        for (int i = 0; i < dynamicCollections.Count; i++)
        {
            IKeyProperitiesKeysContainer collectionTarget = dynamicCollections[i] as IKeyProperitiesKeysContainer; ;
            if (collectionTarget != null)
            {
                collectionTarget.GetAllKey(out string[] k);
                for (int j = 0; j < k.Length; j++)
                {
                    if (!keys.ContainsKey(k[j]))
                        keys.Add(k[j], null);
                }
            }
        }
    }

    public void GetUnthrustedTextValue(in string key,out bool found, out string value)
    {
        CheckThatUnthurstedTextExist();
        m_unthrustedText.Exists(in key, out found);
        if (found)
            m_unthrustedText.GetValue(key, out value);
        else value = "";

    }

    public AllSharpPrimitiveKeyValueStorageCollection m_primitivesStorage = new AllSharpPrimitiveKeyValueStorageCollection();
    public DynamiqueKeyValueStorageCollection m_dynamiqueStorage = new DynamiqueKeyValueStorageCollection();

    public void GetPrimtiveStringValue(in string key, out bool found, out string value)
    {
        m_primitivesStorage.m_unprotectedString.Exists(in key, out found);
        if (found)
            m_primitivesStorage.m_unprotectedString.GetValue(in key, out value);
        else value = "";
    }

    public KeyPropertiesUntrustedText m_unthrustedText = new KeyPropertiesUntrustedText();

    public UserPermaPref()
    {
        CheckThatUnthurstedTextExist();
    }
    public UserPermaPref(string id)
    {
        CheckThatUnthurstedTextExist();
        SetUserId(id);
    }

  

    public void SetUnthrustedText(in string key, in string value)
    {
        CheckThatUnthurstedTextExist();
        m_unthrustedText.SetValue(in key, in value);
    }

    private void CheckThatUnthurstedTextExist()
    {
        if (m_unthrustedText == null)
        {
            m_unthrustedText = new KeyPropertiesUntrustedText();
        }
        m_dynamiqueStorage.AddIfNotContaining(m_unthrustedText);
    }

    public void SetUserId(in string value)
    {
        m_userInfo.m_userStringId = value;
    }

    public void SetUserIdAndAliasWithMailB64(in string mail)
    {
        E_StringByte64Utility.GetText64FromText(in mail, out string b64Mail);
        SetUserId(b64Mail);
        SetUserNameAlias(mail);
    }

    public void GetPrimtiveStringFloat(in string key, out bool exist, out float valueRecovert, float ifNotExisting=0)
    {
        m_primitivesStorage.m_float.Exists(key, out exist);
        if (exist)
            m_primitivesStorage.m_float.GetValue(in key, out valueRecovert);
        else valueRecovert = ifNotExisting;
    }

    public void GetUserIdFromAsB64(out string b64IdUsed, out string mail) {
        b64IdUsed = m_userInfo.m_userStringId;
        E_StringByte64Utility.GetTextFromTextB64(in b64IdUsed, out bool converted, out mail); 
    }

    public void SetPrimitive(in string key, in float value)
    {
        m_primitivesStorage.m_float.SetValue(in key, in value);
    }

    public void SetPrimitive(in string key, in bool value)
    {
        m_primitivesStorage.m_bool.SetValue(in key, in value);
    }
    public void SetPrimitive(in string key, in int value)
    {
        m_primitivesStorage.m_int.SetValue(in key, in value);
    }

    public void PrimitiveExistBool(in string key, out bool exist)
    {
        m_primitivesStorage.m_bool.Exists(in key, out exist);
    }

    public void PrimitiveExistString(in string key, out bool exist)
    {
        m_primitivesStorage.m_unprotectedString.Exists(in key, out exist);
    }




    public void SetUserNameAlias(in string value)
    {
        m_userInfo.m_userAlias = value;
    }

    public void GetUserId(out string userId)
    {
        userId = m_userInfo.m_userStringId;
    }
}

[System.Serializable]
public class AllSharpPrimitiveKeyValueStorageCollection
{
   public KeyPropertiesBool           m_bool= new KeyPropertiesBool();
   public KeyPropertiesByte           m_byte = new KeyPropertiesByte();
   public KeyPropertiesSignByte       m_signByte = new KeyPropertiesSignByte();
   public KeyPropertiesChar           m_char = new KeyPropertiesChar();
   public KeyPropertiesDecimal        m_decimal = new KeyPropertiesDecimal();
   public KeyPropertiesDouble         m_double = new KeyPropertiesDouble();
   public KeyPropertiesFloat          m_float = new KeyPropertiesFloat();
   public KeyPropertiesInt            m_int = new KeyPropertiesInt();
   public KeyPropertiesUInt           m_uint = new KeyPropertiesUInt();
   public KeyPropertiesLong           m_long = new KeyPropertiesLong();
   public KeyPropertiesULong          m_ulong = new KeyPropertiesULong();
   public KeyPropertiesShort          m_short = new KeyPropertiesShort();
   public KeyPropertiesUShort           m_ushot = new KeyPropertiesUShort();
   public KeyPropertiesUnprotectedString m_unprotectedString = new KeyPropertiesUnprotectedString();
    private object[] m_lightWayAccess=null;
    public void GetAllAsObject(out object [] all) {
        if (m_lightWayAccess != null) { 
            all = m_lightWayAccess;
            return;
        }
        all = new object[] {

        m_bool,
        m_byte,
        m_signByte,
        m_char,
        m_decimal,
        m_double,
        m_float,
        m_int,
        m_uint,
        m_long,
        m_ulong,
        m_short,
        m_ushot,
        m_unprotectedString
    };
    }

    public void SetAsAlphaNumericString(string key, string value)
    {
        Eloi.E_StringUtility.ConvertToAlphaNumByReplacing(in value, out string protectedEventAlias);
        SetString(key, protectedEventAlias);
    }

    public void SetString(string key, string value)
    {
        m_unprotectedString.SetValue(in key, in value);
    }
}
public class DynamiqueKeyValueStorageCollection
{
    public List<IKeyPropertiesAsStringFullInteraction> m_dynamiqueStorage = new List<IKeyPropertiesAsStringFullInteraction>();

    public void AddIfNotContaining(IKeyPropertiesAsStringFullInteraction keyValueCollection) {
        if (!m_dynamiqueStorage.Contains(keyValueCollection)) {
            m_dynamiqueStorage.Add(keyValueCollection);
        }
    }
}



[System.Serializable]
public class KeyProperty <T> {
    public string m_key="";
    public T m_value;
    public KeyProperty()
    {    }
    public KeyProperty(string key, T properity)
    {
        m_key = key;
        m_value = properity;
    }

    public void IsKeyEqualAndNotEmpty(in string key, out bool areEqual, bool ignoreCase=true, bool trim=true)
    {
       
        if (key == null || m_key == null)
        {
            areEqual = false;
            return;
        }
        string aKey= m_key, bKey= key;
        if (trim)
        {
            aKey = aKey.Trim();
            bKey = bKey.Trim();
        }

        if (aKey.Length == 0 || bKey.Length == 0) { 
            areEqual= false;
            return;
        }
        if (ignoreCase)
        {
            aKey = aKey.ToLower();
            bKey = bKey.ToLower();
        }
        areEqual = aKey.Length == bKey.Length && aKey.IndexOf(bKey) == 0;
    }
}

//public class KeyPropertiesPrimitive<bool> : KeyStringableProperties<>
//{
//    public PrimitiveType m_storedType;

//    public KeyPropertiesPrimitive(PrimitiveType storedType)
//    {
//        m_storedType = storedType;
//    }

//    public enum PrimitiveType {_bool, _byte, _sbyte, _char, _decimal, _double, _float, _int, _uint, _long, _ulong, _short, _ushort, }
//    public override void GetValue(in string key, out bool found, out string value)
//    {
//        for (int i = 0; i < m_properties.Count; i++)
//        {
//            m_properties[i].IsKeyEqualAndNotEmpty(in key, out bool areEqual);
//            if (areEqual)
//            {
//                found = true;
//                value = m_properties[i].m_value.ToString();
//                return;
//            }
//        }
//        found = false;
//        value = "";
//    }


//    public override void SetValue(in string key, in string value, out bool wasConverted)
//    {
//        string toConvert = value.Trim().ToLower();

//        switch (m_storedType)
//        {
//            case PrimitiveType._bool:
//                TryToConvertTo(in value, out bool converted, out bool boolean);
//                if (converted) {
//                    T value = (T) boolean;
//                    SetValue()
//                }
//                break;
//            case PrimitiveType._byte:
//                break;
//            case PrimitiveType._sbyte:
//                break;
//            case PrimitiveType._char:
//                break;
//            case PrimitiveType._decimal:
//                break;
//            case PrimitiveType._double:
//                break;
//            case PrimitiveType._float:
//                break;
//            case PrimitiveType._int:
//                break;
//            case PrimitiveType._uint:
//                break;
//            case PrimitiveType._long:
//                break;
//            case PrimitiveType._ulong:
//                break;
//            case PrimitiveType._short:
//                break;
//            case PrimitiveType._ushort:
//                break;
//            default:
//                break;
//        }


//        throw new System.NotImplementedException();
//        //string[] tokens = toConvert.Substring(1, toConvert.Length - 2).Split(':');
//        //if (float.TryParse(tokens[0], out float x) &&
//        // float.TryParse(tokens[1], out float y) &&
//        // float.TryParse(tokens[2], out float z))
//        //{
//        //    wasConverted = true;
//        //    UnityEngine.Vector3 v = new UnityEngine.Vector3(x, y, z);
//        //    for (int i = 0; i < m_properties.Count; i++)
//        //    {
//        //        m_properties[i].IsKeyEqualAndNotEmpty(in key, out bool areEqual);
//        //        if (areEqual)
//        //        {
//        //            m_properties[i].m_value = v;
//        //            return;
//        //        }
//        //    }
//        //    KeyProperty<UnityEngine.Vector3> v3 = new KeyProperty<UnityEngine.Vector3>(key, v);
//        //    m_properties.Add(v3);
//        //}
//        //else wasConverted = false;
//    }
//}

public interface IKeyPropertiesAliasTypeName {
    void GetAliasTypeName(out string aliasTypeName);
}

[System.Serializable]
public class KeyProperties<T> : IKeyProperitiesAsGenericContainer<T>, IKeyProperitiesAsStringGet, IValueContainDangerousCharacter, IKeyPropertiesGenericGet
{
    public List<KeyProperty<T>> m_properties = new List<KeyProperty<T>>();

    public void Exists(in string key, out bool exist) {
        for (int i = m_properties.Count - 1; i >= 0; i--)
        {
            if (IsEquals(in key, in m_properties[i].m_key))
            {
                exist = true;
                return;
            }
        }
        exist = false;
        return;
    }

    public void GetAllKey(out string[] key)
    {
      key=  m_properties.Select(k => k.m_key).ToArray();
    }

    public void GetAllValue(out T[] value)
    {
        value = m_properties.Select(k => k.m_value).ToArray();
    }

    public void GetCount(out uint count)
    {
        count =(uint) m_properties.Count;
    }

    public void GetTypeOfAsFullStringDescription(out string typeOfStorage)
    {
        GetTypeOfStorage(out Type type, out Assembly assembly);
        typeOfStorage = string.Format("{0}, {1}", type, assembly);
    }

    public void GetTypeOfObjectStored(out Type typeOfStorage, out Assembly assembly)
    {
        assembly=typeof(T).Assembly;
        typeOfStorage= typeof(T);
    }

    public void GetValueObject(in string key, out bool found, out object obj)
    {
        Exists(in key, out found);
        if (found)
        {
            GetValue(in key, out T foundObject);
            obj = foundObject;
        }
        else obj = null;
    }

    public void GetValueObjectWithType(in string key, out Type typeOfStorage, out Assembly assembly, out bool found, out object obj)
    {
        GetTypeOfObjectStored(out typeOfStorage, out assembly);
        GetValueObject(in key, out found, out obj);

    }

    public void GetTypeOfStorage(out Type type, out Assembly assembly )
    {
        assembly = typeof(T).Assembly;
        type = typeof(T);
    }

    public void GetValue(in string key, out T value)
    {

        for (int i = m_properties.Count - 1; i >= 0; i--)
        {
            if (IsEquals(in key, in m_properties[i].m_key))
            {
                value = m_properties[i].m_value;
                return;
            }
        }
        throw new Exception("Value not found, please check that it exists. Or catch the exception");
    }
    public void GetValue(in string key, out bool found, out T value, T defaultValue)
    {

        for (int i = m_properties.Count - 1; i >= 0; i--)
        {
            if (IsEquals(in key, in m_properties[i].m_key))
            {
                value = m_properties[i].m_value;
                found = true;
                return;
            }
        }
        value = defaultValue;
        found = false;
    }

    
    public void GetValue(in string key, out bool found, out string value)
    {
        for (int i = 0; i < m_properties.Count; i++)
        {
            m_properties[i].IsKeyEqualAndNotEmpty(in key, out bool areEqual);
            if (areEqual)
            {
                found = true;
                value = m_properties[i].m_value.ToString();
                return;
            }
        }
        found = false;
        value = "";
    
    }


    public virtual bool HasDangerousCharacter() {
        return false;
    }

    public  void SetValue(in string key, in T value)
    {

        if (m_properties.Count == 0)
        {
            m_properties.Add(new KeyProperty<T>(key, value));
        }
        else { 
            bool found = false;
            for (int i = m_properties.Count-1; i >=0 ; i--)
            {
                if (IsEquals(in key, in m_properties[i].m_key)) {
                    if (!found)
                    {
                        found = true;
                        m_properties[i].m_value = value;
                    }
                    else if (found) {
                        m_properties.RemoveAt(i);
                    }
                }
            }
            if (!found)
            {
                m_properties.Add(new KeyProperty<T>(key, value));
            }
        }  
    }

    private bool IsEquals( in string key, in  string keyB)
    {
        return key.Length == keyB.Length && key.IndexOf(keyB) == 0;
    }
}
[System.Serializable]
public abstract class KeyStringableProperties<T> : KeyProperties<T> , IKeyPropertiesAliasTypeName, IKeyProperitiesAsStringGet, IKeyProperitiesAsStringSet, IKeyProperitiesAsStringContainer, IKeyPropertiesAsStringFullInteraction
{
    public void GetAllValue(out string[] values)
    {
        GetAllKey(out string []  keys);
         values = new string[keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            GetValue(keys[i], out bool found, out string v);
            if (found)
                values[i] = v;
            else values[i] = "";
        }
    }

    public void GetTypeAsString(out string typeFullname, out string assemblyName)
    {
        typeFullname = typeof(T).FullName;
        assemblyName = typeof(T).Assembly.GetName().Name;
    }

    public void GetTypeAliasNameAsString(out string typeName)
    {
        typeName = typeof(T).Name;
    }

    public new abstract void GetValue(in string key, out bool found, out string value);
    public abstract void SetValue(in string key, in string value, out bool wasConverted);

    public abstract new bool HasDangerousCharacter();
    public abstract void GetAliasTypeName(out string aliasTypeName);
}

[System.Serializable]
public class KeyPropertiesUnprotectedString : KeyProperties<string> { }
[System.Serializable]
public class KeyPropertiesBool : KeyProperties<bool>{}
[System.Serializable]
public class KeyPropertiesByte : KeyProperties<byte> { }
[System.Serializable]
public class KeyPropertiesSignByte : KeyProperties<sbyte> { }
[System.Serializable]
public class KeyPropertiesChar : KeyProperties<char> { }
[System.Serializable]
public class KeyPropertiesDecimal : KeyProperties<decimal> { }
[System.Serializable]
public class KeyPropertiesDouble : KeyProperties<double> { }
[System.Serializable]
public class KeyPropertiesFloat : KeyProperties<float> { }
[System.Serializable]
public class KeyPropertiesInt : KeyProperties<int> { }
[System.Serializable]
public class KeyPropertiesUInt : KeyProperties<uint> { }
[System.Serializable]
public class KeyPropertiesLong : KeyProperties<long> { }
[System.Serializable]
public class KeyPropertiesULong : KeyProperties<ulong> { }
[System.Serializable]
public class KeyPropertiesShort : KeyProperties<short> { }
[System.Serializable]
public class KeyPropertiesUShort : KeyProperties<ushort> { }



[System.Serializable]
public class KeyPropertiesVector3 : KeyProperties<UnityEngine.Vector3>
{

}

[System.Serializable]
public class KeyPropertiesVector3Stringable : KeyStringableProperties<UnityEngine.Vector3>
{
    public string m_valuesFormat = "[{0:0.00}:{1:0.00}:{2:0.00}]";

    public override void GetAliasTypeName(out string aliasTypeName)
    {
        aliasTypeName = "Vector3";
    }

    public override void GetValue(in string key, out bool found, out string value)
    {
        for (int i = 0; i < m_properties.Count; i++)
        {
            m_properties[i].IsKeyEqualAndNotEmpty(in key, out bool areEqual);
            if (areEqual)
            {
                found = true;
                value = string.Format(m_valuesFormat, m_properties[i].m_value.x, m_properties[i].m_value.y, m_properties[i].m_value.z);
                return;
            }
        }
        found = false;
        value = "";
    }

    public override bool HasDangerousCharacter()
    {
        return false;
    }

    public override void SetValue(in string key, in string value, out bool wasConverted)
    {
        string toConvert = value.Trim().ToLower();
        if (toConvert.Length < 4)
        {
            wasConverted = false;
            return;
        }
        string[] tokens = toConvert.Substring(1, toConvert.Length - 2).Split(':');
        if (float.TryParse(tokens[0], out float x) &&
         float.TryParse(tokens[1], out float y) &&
         float.TryParse(tokens[2], out float z))
        {
            wasConverted = true;
            UnityEngine.Vector3 v = new UnityEngine.Vector3(x, y, z);
            for (int i = 0; i < m_properties.Count; i++)
            {
                m_properties[i].IsKeyEqualAndNotEmpty(in key, out bool areEqual);
                if (areEqual)
                {
                    m_properties[i].m_value = v;
                    return;
                }
            }
            KeyProperty<UnityEngine.Vector3> v3 = new KeyProperty<UnityEngine.Vector3>(key, v);
            m_properties.Add(v3);
        }
        else wasConverted = false;
    }
}



[System.Serializable]
public class KeyPropertiesBigInteger : KeyStringableProperties<BigInteger>
{
    public override void GetAliasTypeName(out string aliasTypeName)
    {
        aliasTypeName = "Big Integer";
    }
    public string m_valuesFormat = "[BI:{0}]";
    public override void GetValue(in string key, out bool found, out string value)
    {
        for (int i = 0; i < m_properties.Count; i++)
        {
            m_properties[i].IsKeyEqualAndNotEmpty(in key, out bool areEqual);
            if (areEqual)
            {
                found = true;
                value = string.Format(m_valuesFormat,m_properties[i].m_value.ToString());
                return;
            }
        }
        found = false;
        value = "";
    }

    public override bool HasDangerousCharacter()
    {
        return false;
    }

    public override void SetValue(in string key, in string value, out bool wasConverted)
    {
        string toConvert = value.Trim().ToLower();
        if (toConvert.Length < 4)
        {
            wasConverted = false;
            return;
        }
        string valueBI = toConvert.Substring(3, toConvert.Length - 4);
        if (BigInteger.TryParse(valueBI, out BigInteger convertValue))
        {
            wasConverted = true;
            for (int i = 0; i < m_properties.Count; i++)
            {
                m_properties[i].IsKeyEqualAndNotEmpty(in key, out bool areEqual);
                if (areEqual)
                {
                    m_properties[i].m_value = convertValue;
                    return;
                }
            }
            KeyProperty<BigInteger> v3 = new KeyProperty<BigInteger>(key, convertValue);
            m_properties.Add(v3);
        }
        else wasConverted = false;
    }
}
public interface IKeyPropertiesAsStringFullInteraction
: IKeyProperitiesAsStringContainer, IKeyPropertiesAliasTypeName,  IKeyProperitiesAsStringGet, IKeyProperitiesAsStringSet, IKeyPropertiesAsStringTypeConvertion, IValueContainDangerousCharacter
{ }
public interface IKeyPropertiesFullInteraction<T> : IKeyPropertiesAsStringFullInteraction, IKeyProperitiesAsGenericContainer<T>
{ }

public interface IValueContainDangerousCharacter {
     bool HasDangerousCharacter();
}

public interface IKeyPropertiesAsStringTypeConvertion
{
     void GetTypeAliasNameAsString(out string typeName);
     void GetTypeAsString(out string typeFullname, out string assemblyName);
}

public interface IKeyProperitiesKeysContainer {

     void GetCount(out uint count);
     void GetAllKey(out string[] key);
}
public interface IKeyProperitiesAsStringContainer : IKeyProperitiesKeysContainer
{
     void GetAllValue(out string[] value);
}
public interface IKeyProperitiesAsGenericContainer<T> : IKeyProperitiesKeysContainer
{
     void GetAllValue(out T[] value);
     void GetTypeOfStorage(out System.Type typeOfStorage, out System.Reflection.Assembly assembly);
     void GetTypeOfAsFullStringDescription(out string typeOfStorage);
}

public interface IKeyPropertiesGenericGet {

    void GetValueObjectWithType(in string key, out System.Type typeOfStorage, out System.Reflection.Assembly assembly,out bool found, out object obj);
    void GetValueObject(in string key, out bool found, out object obj);
    void GetTypeOfObjectStored(out System.Type typeOfStorage, out System.Reflection.Assembly assembly);

}

public interface IKeyProperitiesAsStringGet
{
     void GetValue(in string key, out bool found, out string value);
}
public interface IKeyProperitiesAsStringSet
{
     void SetValue(in string key, in string value, out bool wasConverted);
}

public interface IKeyProperityAsString
{
     void GetKey(out string key);
     void GetValue(out string value);
}

public interface IKeyProperityAsGeneric<T>
{
     void GetKey(out string key);
     void GetValue(out T value);
}


[System.Serializable]
public class KeyPropertiesUntrustedText : KeyStringableProperties<string>
{
    public override void GetAliasTypeName(out string aliasTypeName)
    {
        aliasTypeName = "Untrusted Text";
    }
    public string m_valuesFormat = "[TEXT:{0}]";
    public override void GetValue(in string key, out bool found, out string value)
    {
        for (int i = 0; i < m_properties.Count; i++)
        {
            m_properties[i].IsKeyEqualAndNotEmpty(in key, out bool areEqual);
            if (areEqual)
            {
                found = true;
                value = string.Format(m_valuesFormat, m_properties[i].m_value.ToString());
                return;
            }
        }
        found = false;
        value = "";
    }

    public override bool HasDangerousCharacter()
    {
        return true;
    }

    public override void SetValue(in string key, in string value, out bool wasConverted)
    {
        string toConvert = value.Trim().ToLower();
        if (toConvert.Length < "[TEXT:".Length+1)
        {
            wasConverted = false;
            return;
        }
        string valueUnsaveText = toConvert.Substring("[TEXT:".Length, toConvert.Length - "[TEXT:".Length - 1);
       
            wasConverted = true;
            for (int i = 0; i < m_properties.Count; i++)
            {
                m_properties[i].IsKeyEqualAndNotEmpty(in key, out bool areEqual);
                if (areEqual)
                {
                    m_properties[i].m_value = valueUnsaveText;
                    return;
                }
            }
            KeyProperty<string> textKey = new KeyProperty<string>(key, valueUnsaveText);
            m_properties.Add(textKey);
        
    }
}


[System.Serializable]
public class RecordContext
{
    public string m_applicationAliasID = "Undefined";
    public string m_realLifeEventAliasID = "Undefined";
    public string m_computerAliasID = "Default";
    public string m_applicatoinVersion = "0.0.0";

}



[System.Serializable]
public class KP_EventTerminalContextText : KeyStringableProperties<RecordContext>
{
    public override void GetAliasTypeName(out string aliasTypeName)
    {
        aliasTypeName = "Event Terminal Context";
    }
    public string m_valuesFormat = "[CONTEXT:{0}|{1}|{2}|{3}]";
    public override void GetValue(in string key, out bool found, out string value)
    {
        for (int i = 0; i < m_properties.Count; i++)
        {
            m_properties[i].IsKeyEqualAndNotEmpty(in key, out bool areEqual);
            if (areEqual)
            {
                found = true;
                value = string.Format(m_valuesFormat,
                    m_properties[i].m_value.m_realLifeEventAliasID,
                    m_properties[i].m_value.m_computerAliasID,
                    m_properties[i].m_value.m_applicationAliasID,
                    m_properties[i].m_value.m_applicatoinVersion);
                return;
            }
        }
        found = false;
        value = "";
    }

    public override bool HasDangerousCharacter()
    {
        return true;
    }

    public override void SetValue(in string key, in string value, out bool wasConverted)
    {
        string toConvert = value.Trim().ToLower();
        if (toConvert.Length < "[CONTEXT:".Length + 1)
        {
            wasConverted = false;
            return;
        }
        string valueUnsaveText = toConvert.Substring("[CONTEXT:".Length, toConvert.Length - "[CONTEXT:".Length - 1);
        string[] tokens = valueUnsaveText.Split('|');
        string realEvent="", computer = "", application = "", version = "";
        if (tokens.Length >= 1)
            realEvent = tokens[0];
        if (tokens.Length >= 2)
            computer = tokens[1];
        if (tokens.Length >= 3)
            application = tokens[2];
        if (tokens.Length >= 4)
            version = tokens[3];

        wasConverted = true;
        for (int i = 0; i < m_properties.Count; i++)
        {
            m_properties[i].IsKeyEqualAndNotEmpty(in key, out bool areEqual);
            if (areEqual)
            {
                m_properties[i].m_value.m_realLifeEventAliasID = realEvent;
                m_properties[i].m_value.m_computerAliasID = computer;
                m_properties[i].m_value.m_applicationAliasID = application;
                m_properties[i].m_value.m_applicatoinVersion = version;
                return;
            }
        }
        RecordContext newValue = new RecordContext();
        newValue.m_realLifeEventAliasID = realEvent;
        newValue.m_computerAliasID = computer;
        newValue.m_applicationAliasID = application;
        newValue.m_applicatoinVersion = version;
        KeyProperty<RecordContext> textKey = new KeyProperty<RecordContext>(key, newValue);
  
        m_properties.Add(textKey);

    }
}
