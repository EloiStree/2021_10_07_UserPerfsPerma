using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Eloi;
using System;
using UnityEngine.Events;

public class MakeCSVPerDayPerProject : MonoBehaviour
{
    public AbstractUserPermaPrefRegister m_register;
    public Eloi.AbstractMetaAbsolutePathDirectoryMono m_userDirectory;
    public Eloi.MetaFileNameWithExtension m_fileName;

    public InnverEvents m_events;
    [System.Serializable]
    public class InnverEvents { 
        public UnityEvent m_start;
        public UnityEvent m_stepPing;
        public UnityEvent m_end;
    }


    public string m_directory;
    public string[] m_perDayDirectories;

    public UserPermaPref[] m_debugUsers;

    public void ImportAndExportProcess()
    {
        StartCoroutine(Coroutine_ImportAndExportProcess());
    }

    public float m_timeBetween = 1f;
    public float m_timeBeforeEndEvent = 5f;
    IEnumerator Coroutine_ImportAndExportProcess()
    {
        m_events.m_start.Invoke();
        m_directory = m_userDirectory.GetPath();
        m_perDayDirectories = Directory.GetDirectories(m_directory, "*", SearchOption.TopDirectoryOnly);


        fullRegisterPerDay.Flush();
        for (int i = 0; i < m_perDayDirectories.Length; i++)
        {
            IMetaAbsolutePathDirectoryGet d = new MetaAbsolutePathDirectory(m_perDayDirectories[i]);
            ExportDirectory(d);
            yield return new WaitForSeconds(m_timeBetween);
            m_events.m_stepPing.Invoke();
        }
        ExportDirectoryFullRegister(m_userDirectory);
        yield return new WaitForSeconds(m_timeBeforeEndEvent);
        m_events.m_end.Invoke();

    }

    private void ExportDirectoryFullRegister(IMetaAbsolutePathDirectoryGet directory)
    {
        UserPermaPrefExportToCSV.ExportCSVWithAllDataInIt(in fullRegisterPerDay, out string text);
        IMetaAbsolutePathFileGet file = Eloi.E_FileAndFolderUtility.Combine(directory, m_fileName);
        File.WriteAllText(file.GetPath(), text);
    }

    public string m_dateIdLabel="date";
    AbstractUserPermaPrefRegister fullRegisterPerDay = new DefaultUserPermaPrefRegister();
    private void ExportDirectory(IMetaAbsolutePathDirectoryGet directory)
    {
       
        E_FilePathUnityUtility.GetJustDirectoryName( directory.GetPath(), out string folderName);
        AbstractUserPermaPrefRegister register = new DefaultUserPermaPrefRegister();
        UserPermaPrefImport.ImportUsersPermaFromPathToRegister(directory,
            ref register);
        UserPermaPrefExportToCSV.ExportCSVWithAllDataInIt(in register, out string text);
        IMetaAbsolutePathFileGet file = Eloi.E_FileAndFolderUtility.Combine(directory, m_fileName);
        File.WriteAllText(file.GetPath(), text);

        register.GetAllUserPermaPref(out m_debugUsers);
        for (int i = 0; i < m_debugUsers.Length; i++)
        {
            m_debugUsers[i].m_primitivesStorage.m_unprotectedString.SetValue(in m_dateIdLabel, folderName);
            m_debugUsers[i].m_userInfo.m_userStringId = folderName +"_"+ m_debugUsers[i].m_userInfo.m_userStringId;
            fullRegisterPerDay.OverrideOrAdd(m_debugUsers[i]);
        }

    }
}
