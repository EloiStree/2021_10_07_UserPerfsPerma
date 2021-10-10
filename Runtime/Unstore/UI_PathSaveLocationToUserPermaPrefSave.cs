using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UI_PathSaveLocationToUserPermaPrefSave : MonoBehaviour
{
    public UI_WhereToStorePath m_whereToSave;
    public UserPermaPrefMono m_user;
    public bool m_useMailAsId;
    public bool m_useNameAsAlias;
    public UI_UserInfoToPlayerPermaPref m_userInfo;

    public void PushSave()
    {
        m_whereToSave.GetPath(out string path);
        PushSaveAt(path);
    }
    public void PushSaveAt(string path)
    {
        if (path == null || path.Length == 0)
            path = Directory.GetCurrentDirectory();

        if ( (m_useMailAsId || m_useNameAsAlias) && m_userInfo != null) {
            if (m_useMailAsId)
            {
                string mail = m_userInfo.m_mailId.text;
                m_user.User.SetUserId(in mail);
            }
            if (m_useNameAsAlias)
            {
                string name = m_userInfo.m_lastName.text;
                string firstname = m_userInfo.m_firstName.text;
                m_user.User.SetUserNameAlias(name+" "+firstname);
            }
        }

        UserPermaPref user = m_user.User;
        UserPermaPrefImportExport.SaveAsFile(in path, in user);
    }
}
