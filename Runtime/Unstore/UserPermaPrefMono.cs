using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPermaPrefMono : MonoBehaviour
{
    [SerializeField] UserPermaPref m_userInfo;

    public UserPermaPref GetPlayerReference() {

        return m_userInfo;
    }

    public UserPermaPref User
    {
        get { return m_userInfo; }
        set { m_userInfo = value; }
    }

    public void Override(UserPermaPref user)
    {
        m_userInfo = user;
    }

    public void PushIn(UserPermaPref user)
    {
        UserPermaPrefUtility.PushIn(in m_userInfo , in user);
    }
}
public class UserPermaPrefUtility {

    public static void PushIn(in UserPermaPref from, in UserPermaPref to)
    {
        E_ThrowException.ThrowNotImplemented();
    }
    public static void PushInNewValue(in UserPermaPref from, in UserPermaPref to)
    {

        E_ThrowException.ThrowNotImplemented();
    }
    public static void OverrideIn(in UserPermaPref from, in UserPermaPref to)
    {

        E_ThrowException.ThrowNotImplemented();
    }

}