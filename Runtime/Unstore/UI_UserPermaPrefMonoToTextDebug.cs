using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UserPermaPrefMonoToTextDebug : MonoBehaviour
{
    public UserPermaPrefMono m_user;
    public InputField m_text;

    public void PushUserInfo2TextUI()
    {
     

        UserPermaPref user = m_user.User;
        UserPermaPrefImportExport.ConvertToExportableText(in user, out string text);
        m_text.text = text;


    }
    public void PushTextUI2UserInfo()
    {
        string text = m_text.text;
        UserPermaPref user = m_user.User;
        UserPermaPrefImport.ImportUserPermaPrefFromText(in text, out bool converted, ref user);
        m_user.Override(user);

    }
}
