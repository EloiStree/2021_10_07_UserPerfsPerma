using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ContextTerminalToUserPermaPref : MonoBehaviour
{
    public UserPermaPrefMono m_userTarget;
    public InputField m_eventAlias;
    public InputField m_computerAlias;
    public InputField m_applicationAlias;
    public InputField m_versionAlias;

    public void PushUI2UserInfo()
    {

        m_userTarget.User.m_primitivesStorage.SetAsAlphaNumericString("eventalias", m_eventAlias.text);
        m_userTarget.User.m_primitivesStorage.SetAsAlphaNumericString("computeralias", m_computerAlias.text);
        m_userTarget.User.m_primitivesStorage.SetAsAlphaNumericString("applicationalias", m_applicationAlias.text);
        m_userTarget.User.m_primitivesStorage.SetAsAlphaNumericString("applicationversionalias", m_versionAlias.text);


    }
    public void PushUserInfo2UI()
    {
        bool found = false; 
        string value="";
        m_userTarget.User.m_primitivesStorage.m_unprotectedString.GetValue("eventalias", out  found, out value);
        m_eventAlias.text = value;
        m_userTarget.User.m_primitivesStorage.m_unprotectedString.GetValue("computeralias", out  found, out value);
        m_computerAlias.text = value;
        m_userTarget.User.m_primitivesStorage.m_unprotectedString.GetValue("applicationalias", out  found, out value);
        m_applicationAlias.text = value;
        m_userTarget.User.m_primitivesStorage.m_unprotectedString.GetValue("applicationversionalias", out found, out value);
        m_versionAlias.text = value;
    }
}
