using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UI_ImportToExcelNoOptiDemo : MonoBehaviour
{
    public string m_fileCsvName="userpermaprefdemo.csv";
    public GroupOfUserPermaPrefMono m_usersLoaded;
    public UI_WhereToStorePath m_whereToLook;
    public UI_WhereToStorePath m_whereToStoreExcel;

    public string m_filesFound;
    public StringBuilder m_excelBuilder;
    public string m_excelGenerated;

    public InputField m_fileFoundDebug;
    public InputField m_excelGeneratedDebug;

    [ContextMenu("Import all")]
    public void ImportAllAtLocation()
    {
        m_whereToLook.GetPath(out string path);
        ImportAllAtLocation(path);
    }
    public void ImportAllAtLocation(string path)
    {
        string [] paths= Directory.GetFiles(path, "*" + UserPermaPrefImportExport.FileExtensionName, SearchOption.AllDirectories);
        m_filesFound =string.Join("\n", paths);
        m_fileFoundDebug.text = m_filesFound;

        m_excelBuilder = new StringBuilder();
        m_excelBuilder.Append("id;firstname;lastname;aliasName;mail;event;computer\n");

        for (int i = 0; i < paths.Length; i++)
        {
            UserPermaPref user = new UserPermaPref();
            UserPermaPrefImportImport.ImportUserPermaPrefFromPath(in paths[i],
                in user, out bool wasConvertedWithoutError);
            if (wasConvertedWithoutError) {
                m_usersLoaded.Users.AddOrOverrideByUserId(in user);
                bool found=false;
                user.GetPrimtiveStringValue("eventalias", out found, out string eventAlias);
                user.GetPrimtiveStringValue("computeralias", out found, out string computeAlias);
                user.GetUnthrustedTextValue("firstname",  out string firstNamealias);
                user.GetUnthrustedTextValue("firstname",  out string lastNameAlias);
                user.GetUnthrustedTextValue("gamealias",  out string playerNameAlias);
                user.GetUnthrustedTextValue("mail",  out string mail);
                user.GetUserId(out string id);


                //("id ; firstname; lastname; aliasName ; mail ; event; computer");
                m_excelBuilder.Append(string.Format("{0};{1};{2};{3};{4};{5};{6}\n",
                    id, firstNamealias, lastNameAlias, playerNameAlias , mail, eventAlias, computeAlias
                    ));
            }
        }
        m_excelGenerated = m_excelBuilder.ToString();
        if(m_excelGeneratedDebug!=null)
            m_excelGeneratedDebug.text = m_excelGenerated;
    }


    public void OpenSourceLocation() {
        m_whereToLook.OpenFolder();
    }
    public void OpenSourceExcelDestination()
    {
        m_whereToStoreExcel.OpenFolder();

    }
    public void GenerateExcel() {

        m_whereToStoreExcel.GetPath(out string path);
        string rootPath = Path.GetPathRoot(path);
        if (!Directory.Exists(rootPath)) {
            Directory.CreateDirectory(rootPath);
        }
        File.WriteAllText(path+"/"+ m_fileCsvName, m_excelGenerated);
    }

}
