using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDD_InjectPermaPrefToRegisterToFileMono : MonoBehaviour
{
    public UserPermaPref m_dataSourceToInject;
    public UserPermaPrefRegisterMono m_register;
    public ExportUserPermaFocusAsFile m_exportUser;


    [ContextMenu("Inject & Export data")]
    public void InjectInRegisterAndSaveRegisterToFile()
    {
        UserPermaPrefInjectUtility.InjectAllFieldsIn(
           m_dataSourceToInject,
           m_exportUser.m_focusUser.GetUserInfo());

        AbstractUserPermaPrefRegister reg = m_register.GetRegisterRef();
        UserPermaPref user = m_exportUser.m_focusUser.GetUserInfo();
        reg.SearchFor(in user.m_userInfo.m_userStringId,out bool found, out UserPermaPref inRegister);
        if (!found) {
            reg.OverrideOrAdd(in user);
            m_exportUser.Export(user);
        }
        else {
            UserPermaPrefInjectUtility.InjectAllFieldsIn(user, inRegister);
            reg.OverrideOrAdd( inRegister);
            m_exportUser.Export(inRegister);
        }

    }
}