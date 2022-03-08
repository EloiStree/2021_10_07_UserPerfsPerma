using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAndUnloadCurrentUserPref : MonoBehaviour
{
    public UserPermaPrefMono m_currentUser;
    public LoadAndSavePermaPrefRegisterMono m_usersStorage;


    public void LoadFromCache() {
        m_currentUser.User.GetUserId(out string userId);
        m_usersStorage.GetUserFromCache(in  userId, out UserPermaPref currentUser);
        m_currentUser.User = currentUser;
    }
    public void LoadFromDisk() {

        m_currentUser.User.GetUserId(out string userId);
        m_usersStorage.LoadUserFromDiskInCache(in userId);
        LoadFromCache();
    }

    public void SaveOnDisk() {
        m_usersStorage.SaveUserInDirectory(m_currentUser);
    }
    public void SaveInCache() {
        m_usersStorage.SaveUserInCacheRegister(m_currentUser);
    }
}
