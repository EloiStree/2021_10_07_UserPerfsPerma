using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImportUserPermaToRegisterFromFiles : MonoBehaviour
{
    public Eloi.AbstractMetaAbsolutePathDirectoryMono m_whereToStore;
    public UserPermaPrefRegisterMono m_register;

    [ContextMenu("Import")]
    public void Import()
    {
        string dirPath = m_whereToStore.GetPath();
        if (!Directory.Exists(dirPath))
            return;
        string [] filePaths =Directory.GetFiles(dirPath,"*"+ UserPermaPrefImportExport.FileExtensionName);

        for (int i = 0; i < filePaths.Length; i++)
        {
            UserPermaPref user;
            UserPermaPrefImport.ImportUserPermaPrefFromPath(in filePaths[i], out user, out bool converted);
            if (converted) {
                user.GetUserId(out string id);
                if(Eloi.E_StringUtility.IsFilled(id))
                    m_register.Push(in id, in user);
            }
        }
        

    }

}
