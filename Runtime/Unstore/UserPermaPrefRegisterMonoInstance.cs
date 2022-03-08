using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPermaPrefRegisterMonoInstance : MonoBehaviour
{
    public UserPermaPrefRegisterMono m_target;
    public static UserPermaPrefRegisterMono m_instanceInScene;
    public static UserPermaPrefRegisterMono GetRegister() { return m_instanceInScene; }

    private void Awake()
    {
        m_instanceInScene = m_target;
    }
}
