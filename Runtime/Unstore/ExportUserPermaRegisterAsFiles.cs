using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportUserPermaRegisterAsFiles : MonoBehaviour
{
    public Eloi.AbstractMetaAbsolutePathDirectoryMono m_whereToStore;
    public UserPermaPrefRegisterMono m_register;
    [ContextMenu("Export")]
    public void Export() {
        m_register.m_register.GetAllUserPermaPref(out UserPermaPref[] users);
        string dirPath = m_whereToStore.GetPath();
        Directory.CreateDirectory(dirPath);
        string text="";
        for (int i = 0; i < users.Length; i++)
        {
            string filePath = m_whereToStore.GetPath();
            UserPermaPrefImportExport.ConvertToExportableText(in users[i], out  text);
            users[i].GetUserId(out string id);
            UserPermaPrefImportExport.GetFileNameFromUserId(id, out string fileId);
            Eloi.MetaFileNameWithExtension fileRelPath =new  Eloi.MetaFileNameWithExtension(fileId);
            string p =Eloi.E_FileAndFolderUtility.Combine(m_whereToStore, fileRelPath).GetPath();
            File.WriteAllText(p, text);
        }

    }
}
