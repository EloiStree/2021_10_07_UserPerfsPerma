using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupOfUserPermaPrefMono : MonoBehaviour
{
    [SerializeField] GroupOfuserPermaPref m_usersInfo;
   

    public GroupOfuserPermaPref Users
    {
        get { return m_usersInfo; }
        set { m_usersInfo = value; }
    }

}



[System.Serializable]
public class GroupOfuserPermaPref {

    public List<UserPermaPref> m_users= new List<UserPermaPref>();


    public void AddOrOverrideByUserId(in UserPermaPref toAdd) {

        string userInListId;
        string userId;
        toAdd.GetUserId(out userId);
        for (int i = 0; i < m_users.Count; i++)
        {
            m_users[i].GetUserId(out userInListId);
            if (Eloi.E_StringUtility.AreEquals(in userId, in userInListId,false, true)) {
                m_users[i] = toAdd;
                return;
            }
        }
        m_users.Add(toAdd);
    
    }
}