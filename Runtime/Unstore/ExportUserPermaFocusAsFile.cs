using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportUserPermaFocusAsFile : MonoBehaviour
{
    public Eloi.AbstractMetaAbsolutePathDirectoryMono m_whereToStore;
    public UserPermaPrefMono m_focusUser;
    [ContextMenu("Export")]

    public void Export()
    {
        if (m_focusUser == null)
            return;
        UserPermaPref pref = m_focusUser.User;
        Export(pref);
    }

    internal void Export(UserPermaPref user)
    {
        string dirPath = m_whereToStore.GetPath();
        Directory.CreateDirectory(dirPath);

        string filePath = m_whereToStore.GetPath();
        UserPermaPrefImportExport.ConvertToExportableText(user, out string text);
        user.GetUserId(out string id);
        UserPermaPrefImportExport.GetFileNameFromUserId(id, out string fileId);
        Eloi.MetaFileNameWithExtension fileRelPath = new Eloi.MetaFileNameWithExtension(fileId);
        string p = Eloi.E_FileAndFolderUtility.Combine(m_whereToStore, fileRelPath).GetPath();
        File.WriteAllText(p, text);
    }
}
