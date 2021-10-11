using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPP_Push_UnthrustedString : MonoBehaviour
{
    public UserPermaPrefRegisterMono m_usersRegister;
    public string m_keyId;
   // public bool m_overrideByDefault=true;


    public void Push(in string userId, in string textValue) {

        Push(in userId, in m_keyId, in textValue);
    }

    public void Push(in string userId, in string fieldKey, in string textValue)
    {
        AbstractUserPermaPrefRegister register = m_usersRegister.GetRegisterRef();

        if (!register.IsUserExist(in userId))
        {
            register.AddUser(in userId);
        }

        register.SearchFor(in userId, out bool found, out UserPermaPref userInfo);
        //if (m_overrideByDefault) {
        userInfo.SetUnthrustedText(fieldKey, textValue);
        //}
        register.OverrideOrAdd(in userInfo);
    }

}
