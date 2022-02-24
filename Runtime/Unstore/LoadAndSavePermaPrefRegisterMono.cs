using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadAndSavePermaPrefRegisterMono : MonoBehaviour
{
    public UserPermaPrefRegisterMono m_register;
    public ExportUserPermaRegisterAsCSV m_csvExported;
    public AbstractMetaAbsolutePathDirectoryMono m_whereToStore;
    public SearchOption m_importType = SearchOption.AllDirectories;
  

    [ContextMenu("Import")]
    public void ImportAll()
    {

        string[] paths = Directory.GetFiles(m_whereToStore.GetPath()
            , "*" + UserPermaPrefImportExport.FileExtensionName, m_importType);

        for (int i = 0; i < paths.Length; i++)
        {
            UserPermaPref user = new UserPermaPref();
            UserPermaPrefImport.ImportUserPermaPrefFromPath(in paths[i],
                in user, out bool wasConvertedWithoutError);
            SaveUserInCacheRegister(user);
        }
    }

    [ContextMenu("Export")]
    public void ExportCollectedData()
    {
        m_csvExported.ExportCSVWithAllDataInIt(out string csv);
        MetaFileNameWithExtension all =  new  MetaFileNameWithExtension("collectedall", "csv");
       IMetaAbsolutePathFileGet file = E_FileAndFolderUtility.Combine(m_whereToStore, all);
        E_FileAndFolderUtility.ExportByOverriding(in file, csv);
    }

    [ContextMenu("Create random user for testing: cache")]
    public void AddRandomUserForTestingInCache()
    {
        UserPermaPref user = GetRandomFakeUser();
        SaveUserInCacheRegister(user);
    }
    [ContextMenu("Create random user for testing: cache file")]
    public void AddRandomUserForTestingInCacheAndFile()
    {
        UserPermaPref user = GetRandomFakeUser();
        SaveUserInCacheRegister(user);
        SaveUserInDirectory(user);
    }

    private static UserPermaPref GetRandomFakeUser()
    {
        Eloi.E_GeneralUtility.GetTimeULongId(DateTime.Now, out ulong id);
        string userId = id.ToString() + "@fakemail.com";
        UserPermaPref user = new UserPermaPref(userId);
        Eloi.E_RandomTextUtility.GetRandomName_DisnayCharacter(out string name);
        user.SetUnthrustedText("name", name);
        user.SetUnthrustedText("mail", userId);
        return user;
    }

    [ContextMenu("Open project root")]
    public void OpenProjectRoot()
    {
        Application.OpenURL(Eloi.E_GeneralUtility.GetProjectRoot());
    }
    [ContextMenu("Open storage root")]
    public void OpenStorageRoot()
    {
        Application.OpenURL(m_whereToStore.GetPath());
    }

    [ContextMenu("Flush All")]
    public void FlushRegisterAll()
    {
        m_register.Flush();
    }

    public void SaveUserInCacheAndDirectory(UserPermaPrefMono user)
    {
        SaveUserInCacheRegister(user);
        SaveUserInDirectory(user);
    }
    public void SaveUserInCacheAndDirectory(UserPermaPref user)
    {
        SaveUserInCacheRegister(user);
        SaveUserInDirectory(user);
    }

    public void SaveUserInCacheRegister(UserPermaPrefMono user)
    {
        SaveUserInCacheRegister(user.GetPlayerReference());
    }

    public void SaveUserInCacheRegister(UserPermaPref user) {
        user.GetUserId(out string userId);
        user.TryToGuestKeyValueAsString(in userId, out string id);
        m_register.Push(in id, in user);
    }

    public void SaveUserInDirectory(UserPermaPrefMono user)
    {

        SaveUserInDirectory(user.GetPlayerReference());

    }
    public void SaveUserInDirectory(UserPermaPref user)
    {
        UserPermaPrefImportExport.SaveAsFile( m_whereToStore, user);

    }




}
