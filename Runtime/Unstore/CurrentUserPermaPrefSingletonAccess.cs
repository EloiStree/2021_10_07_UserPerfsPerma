using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUserPermaPrefSingletonAccess : MonoBehaviour
{
    public bool m_isDefined;
    public UserPermaPref m_userInfo;

    [ContextMenu("Refresh")]
    public void Refresh() {
        m_isDefined = IsUserInScene();
        if (m_isDefined)
            m_userInfo = GetCurrentUserInScene().User;
        else
            m_userInfo = new UserPermaPref();
    }
    public void Awake()
    {
        CurrentUserPermaPrefSingleton.AddChangeNotified(Refresh);
    }
    public void OnDestroy()
    {
        CurrentUserPermaPrefSingleton.RemoveChangeNotified(Refresh);

    }

    public static void NotifyUserInfoChanged() {
        CurrentUserPermaPrefSingleton.NotifyUserInfoChanged();
    }

    public static void SetUnthrustedText(in string key, in string value)
    {
        UserPermaPrefMono user = CurrentUserPermaPrefSingleton.User;
        if (user != null)
        {
            user.User.SetUnthrustedText(in key, in value);
        }
    }

    public static void GetSetUnthrustedText(in string key, out string value, string defaultValue="") {
        UserPermaPrefMono user = CurrentUserPermaPrefSingleton.User;
        if (user != null)
        {
            user.User.GetUnthrustedTextValue(in key,out bool found, out value);
            if(found)
                return;
        }
        value = defaultValue;

    }
    public static bool IsUserInScene() {
       return  GetCurrentUserInScene()!=null;
    }

    public static void GetCurrentUserInScene(out UserPermaPrefMono userRef )
    {
        userRef= CurrentUserPermaPrefSingleton.User;
    }
    public static UserPermaPrefMono GetCurrentUserInScene( )
    {
        return CurrentUserPermaPrefSingleton.User;
    }
}
