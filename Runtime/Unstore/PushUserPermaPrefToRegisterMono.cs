using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushUserPermaPrefToRegisterMono : MonoBehaviour
{
    public UserPermaPrefMono m_userInScene;
    public UserPermaPrefRegisterMono m_register;

    public void OverridePushUserToRegister()
    {
        m_userInScene.User.GetUserId(out string id);
        m_register.Push( id, m_userInScene.User);
    }
}
