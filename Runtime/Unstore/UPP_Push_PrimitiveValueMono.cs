using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPP_Push_PrimitiveValueMono : MonoBehaviour
{
    public UserPermaPrefRegisterMono m_usersRegister;
    public string m_keyId;

    
    AbstractUserPermaPrefRegister register;
    UserPermaPref userInfo;
    public void Push(in string userId, in bool value)
    {

        GetOrCreateUserInfo(in userId, out register, out userInfo);

        userInfo.SetPrimitive(m_keyId, value);

        SendBackUserUserInfo();
    }
    public void Push(in string userId, in float value)
    {

         GetOrCreateUserInfo(in userId, out register, out userInfo);

        userInfo.SetPrimitive(m_keyId, value);

        SendBackUserUserInfo();
    }
    public void Push(in string userId, in int value)
    {

         GetOrCreateUserInfo(in userId, out register, out userInfo);

        userInfo.SetPrimitive(m_keyId, value);

        SendBackUserUserInfo();
    }

    private void SendBackUserUserInfo()
    {
        register.OverrideOrAdd(in userInfo);
    }

    private void GetOrCreateUserInfo( in string userId, out AbstractUserPermaPrefRegister register, out UserPermaPref userInfo)
    {
        register = m_usersRegister.GetRegisterRef();
        if (!register.IsUserExist(in userId))
        {
            register.AddUser(in userId);
        }

        register.SearchFor(in userId, out bool found, out userInfo);
    }
}
