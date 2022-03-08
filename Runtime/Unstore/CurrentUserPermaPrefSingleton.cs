using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUserPermaPrefSingleton : MonoBehaviour
{
    public UserPermaPrefMono m_whereToStoreInfo;
    public static UserPermaPrefMono User { get {
            if (m_instance == null)
                return null;
            return m_instance.m_whereToStoreInfo; 
        } }
    public static void GetUserReference(out UserPermaPrefMono userInfo)
    {
        userInfo = m_instance.m_whereToStoreInfo;
    }
    public static UserPermaPrefMono GetUserReference()
    {
        return m_instance.m_whereToStoreInfo;
    }
    private static CurrentUserPermaPrefSingleton m_instance;

    public static Action m_onDataNotifyAsChanged;
    public static void NotifyUserInfoChanged()
    {
        if(m_onDataNotifyAsChanged!=null)
            m_onDataNotifyAsChanged.Invoke();
    }
    public static void AddChangeNotified(System.Action toDo)
    {
        m_onDataNotifyAsChanged += toDo;
    }
    public static void RemoveChangeNotified(System.Action toDo)
    {
        m_onDataNotifyAsChanged -= toDo;
    }

    void Awake()
    {
        if (m_instance != null)
            Debug.Log("There two instance of the singleton.", this.gameObject);
        m_instance = this; 
    }

    public static void SetUnthrustedText(in string key, in string value)
    {
        UserPermaPrefMono user = User;
        if (user != null) {
            user.User.SetUnthrustedText(in key, in value);
        }
    }
}
