using Eloi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UserInfoToPlayerPermaPref : MonoBehaviour
{
    public UserPermaPrefMono m_userTarget;
    public InputField m_firstName;
    public InputField m_lastName;
    public InputField m_gameAlias;
    public InputField m_mailId;
    public bool m_useMailAsId;

    public void PushUI2UserInfo()
    {
        
        m_userTarget.User.SetUnthrustedText("firstname", m_firstName.text);
        m_userTarget.User.SetUnthrustedText("lastname", m_lastName.text);
        m_userTarget.User.SetUnthrustedText("gamealias", m_gameAlias.text);
        m_userTarget.User.SetUnthrustedText("mail", m_mailId.text);


        if (m_useMailAsId) { 
            string mail = m_mailId.text;
            m_userTarget.User.SetUserIdAndAliasWithMailB64(in mail);
        }

    }
    public void PushUserInfo2UI()
    {
        bool found = false;
        string value = "";
        m_userTarget.User.GetUnthrustedTextValue("firstname", out value);
        m_firstName.text = value;
        m_userTarget.User.GetUnthrustedTextValue("lastname",  out value);
        m_lastName.text = value;
        m_userTarget.User.GetUnthrustedTextValue("gamealias",  out value);
        m_gameAlias.text = value;
        m_userTarget.User.GetUnthrustedTextValue("mail",  out value);
        m_mailId.text = value;
    }
}
