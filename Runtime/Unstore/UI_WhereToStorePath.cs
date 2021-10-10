using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WhereToStorePath : MonoBehaviour
{
    public InputField m_whereToStore;
    public string m_id= "userlocation";
    
    void Start()
    {

        string inMemory  = PlayerPrefs.GetString(m_id);
        if (inMemory != null && inMemory.Length > 0)
            m_whereToStore.text = inMemory;
    }

    internal void GetPath(out string path)
    {
        path = m_whereToStore.text;
    }

    [ContextMenu("New id")]
    public void GenerateRandomId() {
       E_UnityRandomUtility.GetRandomStringFrom(in E_StringUtility.AlphaNum, 50, out m_id);
    }
    private void Reset()
    {
        GenerateRandomId();
    }
    void OnDestroy()
    {
       PlayerPrefs.SetString(m_id, m_whereToStore.text);
    }

    internal void OpenFolder()
    {
        Application.OpenURL(m_whereToStore.text);
    }
}
