
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Eloi { 
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
    public UserPermaPref m_oneUserTest;
    public KeyPropertiesUntrustedText m_unsafeText;
    public KeyPropertiesVector3Stringable m_positions = new KeyPropertiesVector3Stringable();
    public bool m_useBase64;

    [Header("Save and load")]
    public bool m_saved;
    public string m_pathUsed;


        [Space(5)]
        [Header("Save and load")]
        public UserPermaPref m_userWithContext;

    void Start()
    {

        m_positions.SetValue("head", Vector3.up * 1.85f);
        m_positions.SetValue("forward", Vector3.forward);
        m_oneUserTest.m_dynamiqueStorage.m_dynamiqueStorage.Add(m_positions);
        m_oneUserTest.m_dynamiqueStorage.m_dynamiqueStorage.Add(m_unsafeText);
        UserPermaPref user = m_oneUserTest;
        UserPermaPrefImportExport.ConvertToExportableText(in user, 
            out m_subFolderProposition,
            out m_fileNameProposition,
            out m_textToStore );
        UserPermaPrefImportExport.TryToSave(out m_saved, out m_pathUsed, in m_textToStore, in m_whereToStorePath, in m_subFolderProposition, in m_fileNameProposition) ;
    }
}
}