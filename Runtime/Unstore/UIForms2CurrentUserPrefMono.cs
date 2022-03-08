using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIForms2CurrentUserPrefMono : MonoBehaviour
{
    public UserPermaPrefMono m_currentUser;
    public InputField m_userIdInputField;
    public InputFieldToUnthrusted[] m_inputfields;
    public ToogleToUnthrusted[] m_toggles;

    public bool m_addCurrentDateWhenSaved=true;
    public string m_dateLabel = "Last Date";
    public string m_dateFormat = "yyyy MM dd hh:mm";
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
    public void OverrideWithFormInfo()
    {
        string userId = m_userIdInputField.text;
        UserPermaPref user = m_currentUser.User;
        user.SetUserId(m_userIdInputField.text);

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
        if (m_addCurrentDateWhenSaved)
            user.SetUnthrustedText(in m_dateLabel, DateTime.Now.ToString(m_dateFormat));
        m_currentUser.Override(user);
    }
}
