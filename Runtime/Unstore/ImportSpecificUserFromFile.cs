using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Eloi;
using System.IO;

public class ImportSpecificUserFromFile : MonoBehaviour
{
    public Eloi.AbstractMetaAbsolutePathDirectoryMono m_whereToStore;
    public UserPermaPrefMono m_focusUser;

    public void ImportIfExistingInFocus(string id)
    {
        ImportByOverriding(in id, in m_focusUser);
    }

    public void IsUserExist(in string userId, out bool exist)
    {
        UserPermaPrefImportExport.GetFileNameFromUserId(in userId, out string fileName);
        Eloi.IMetaFileNameWithExtensionGet file = new MetaFileNameWithExtension(fileName);
        IMetaAbsolutePathFileGet filePath = E_FileAndFolderUtility.Combine(m_whereToStore, file);
        exist = File.Exists(filePath.GetPath());
    }
    public void Import(in string userId, in UserPermaPrefRegisterMono register, out bool exist, out UserPermaPref user)
    {
        Import(in userId, out exist, out user);
        if (exist)
        {
            register.m_register.OverrideOrAdd(user);
        }
    }
    public void ImportByOverriding(in string userId, in UserPermaPrefMono target)
    {
        Import(in userId, out bool exist, out UserPermaPref user);
        if (exist)
        {
            target.User = user;
        }
    }
    public void ImportByOverriding(in string userId, in UserPermaPrefMono target, out bool exist, out UserPermaPref user)
    {
        Import(in userId, out exist, out user);
        if (exist)
        {
            target.User= user;
        }
    }

    public void Import(in string userId, out bool exist, out UserPermaPref user)
    {
        user = new UserPermaPref();
        UserPermaPrefImportExport.GetFileNameFromUserId(in userId, out string fileName);
        Eloi.IMetaFileNameWithExtensionGet file = new MetaFileNameWithExtension(fileName);
        IMetaAbsolutePathFileGet filePath = E_FileAndFolderUtility.Combine(m_whereToStore, file);
        exist = File.Exists(filePath.GetPath());
        if (exist)
        {
            UserPermaPrefImport.ImportUserPermaPrefFromPathIn(filePath.GetPath(), ref user, out exist);
        }
    }

}
