using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FormToPlayerPrefsStorageMono : MonoBehaviour
{
    public UserPermaPrefMono m_currentUser;
    public UserPermaPrefRegisterMono m_register;
    public LoadAndSavePermaPrefRegisterMono m_allUsersStorage;

    public InputField m_userIdInputField;
    public InputFieldToUnthrusted[] m_inputfields;
    public ToogleToUnthrusted[] m_toggles;

    [System.Serializable]
    public class InputFieldToUnthrusted
    {
        public string m_unthrustedFieldName;
        public InputField m_inputField;
    }
    [System.Serializable]
    public class ToogleToUnthrusted
    {
        public string m_unthrustedFieldName;
        public Toggle m_toggle;
    }

    [ContextMenu("Import and Save")]
    public void SaveUserInCurrentAndStorage()
    {
        string userId = m_userIdInputField.text;
        UserPermaPref user;
        m_register.GetRegisterRef().SearchFor(in userId, out bool found, out user);
        if (!found)
            user = new UserPermaPref(m_userIdInputField.text);

        for (int i = 0; i < m_inputfields.Length; i++)
        {
            if (m_inputfields[i].m_inputField != null)
            {
                user.SetUnthrustedText(in m_inputfields[i].m_unthrustedFieldName
                    , m_inputfields[i].m_inputField.text);
            }
        }
        for (int i = 0; i < m_toggles.Length; i++)
        {
            if (m_toggles[i].m_toggle != null)
            {
                user.SetPrimitive(in m_toggles[i].m_unthrustedFieldName
                    , m_toggles[i].m_toggle.isOn);
            }
        }
        m_allUsersStorage.SaveUserInCacheAndDirectory(user);
        m_currentUser.Override(user);
    }
}
