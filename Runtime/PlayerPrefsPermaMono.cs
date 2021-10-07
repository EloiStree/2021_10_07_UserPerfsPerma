using System;
using System.Collections;
using System.Collections.Generic;
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

public class PlayerPrefsPerma {

    public Dictionary<string, int> m_ints = new Dictionary<string, int>();
    public void Set(string key, int value) { }

}

[System.Serializable]
public class KeyProperty <T> {
    public string m_key="";
    public T m_properity;
    public KeyProperty()
    {    }
    public KeyProperty(string key, T properity)
    {
        m_key = key;
        m_properity = properity;
    }

    public void IsEqualAndNotEmpty(in string key, out bool areEqual, bool ignoreCase=true, bool trim=true)
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
[System.Serializable]
public class KeyProperties<T>
{
    public List<KeyProperty<T>> m_properties = new List<KeyProperty<T>>();

}
[System.Serializable]
public abstract class KeyStirngableProperties<T> : IKeyProperitiesAsString, IKeyProperitiesPushableStringValueToConverte
{
    public List<KeyProperty<T>> m_properties = new List<KeyProperty<T>>();

    public abstract void GetValue(in string key, out bool found, out string value);
    public abstract void SetValue(in string key, in string value, out bool wasConverted);
}


[System.Serializable]
public class KeyPropertiesInt : KeyProperties<int>
{
  
}
[System.Serializable]
public class KeyPropertiesUInt : KeyProperties<uint>
{

}
[System.Serializable]
public class KeyPropertiesFloat : KeyProperties<float>
{

}
[System.Serializable]
public class KeyPropertiesByte : KeyProperties<byte>
{

}
[System.Serializable]
public class KeyPropertiesShort : KeyProperties<short>
{

}
[System.Serializable]
public class KeyPropertiesDouble : KeyProperties<double>
{

}
[System.Serializable]
public class KeyPropertiesLong : KeyProperties<long>
{

}
[System.Serializable]
public class KeyPropertiesVector3 : KeyProperties<Vector3>
{

}

[System.Serializable]
public class KeyPropertiesVector3Stringable : KeyStirngableProperties<Vector3>
{
    public string m_valuesFormat = "[{0:0.00}:{1:0.00}:{2:0.00}]";
    public override void GetValue(in string key, out bool found, out string value)
    {
        for (int i = 0; i < m_properties.Count; i++)
        {
            m_properties[i].IsEqualAndNotEmpty(in key, out bool areEqual);
            if (areEqual) {
                found = true;
                value = string.Format(m_valuesFormat, m_properties[i].m_properity.x, m_properties[i].m_properity.y, m_properties[i].m_properity.z);
                return;
            }
        }
        found = false;
        value = "";
    }


    public override void SetValue(in string key, in string value, out bool wasConverted)
    {
        string toConvert = value.Trim().ToLower();
        if (toConvert.Length < 4) {
            wasConverted = false;
            return;
        }
        string[] tokens = toConvert.Substring(1, toConvert.Length - 2).Split(':');
        if (float.TryParse(tokens[0], out float x) &&
         float.TryParse(tokens[1], out float y) &&
         float.TryParse(tokens[2], out float z) )
        {
            wasConverted = true;
            Vector3 v = new Vector3(x, y, z);
            for (int i = 0; i < m_properties.Count; i++)
            {
                m_properties[i].IsEqualAndNotEmpty(in key, out bool areEqual);
                if (areEqual)
                {
                    m_properties[i].m_properity = v;
                    return;
                }
            }
            KeyProperty<Vector3> v3 = new KeyProperty<Vector3>(key, v);
            m_properties.Add(v3);
        }
        else wasConverted = false;
    }
}

public interface IKeyProperitiesAsString
{
    public void GetValue(in string key, out bool found, out string value);
}
public interface IKeyProperitiesPushableStringValueToConverte
{
    public void SetValue(in string key, in string value, out bool wasConverted);
}

public interface IKeyProperityAsString
{
    public void GetKey(out string key);
    public void GetValue(out string value);
}

public interface IKeyProperityAsGeneric<T>
{
    public void GetKey(out string key);
    public void GetValue(out T value);
}