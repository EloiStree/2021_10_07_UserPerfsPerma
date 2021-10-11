using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserPermaPrefRegisterMono : MonoBehaviour
{
    public AbstractUserPermaPrefRegister m_register = new DefaultUserPermaPrefRegister();

    public void GetRegister(out AbstractUserPermaPrefRegister register) => register = m_register;

    public AbstractUserPermaPrefRegister GetRegisterRef()
    {
        return m_register;
    }

    public void Flush()
    {
        m_register.Flush();
    }
    
    public void AppendKeysAsDistinctIn(ref Dictionary<string, string> columnDico)
    {
        m_register.AppendKeysAsDistinctIn(ref columnDico);
    }
}
public abstract class AbstractUserPermaPrefRegister
{

    public abstract bool IsUserExist(in string stringId);

    public abstract void AddUser(in string stringId);

    public abstract void OverrideOrAdd(in UserPermaPref user);
    public abstract void RemoveUser(in string userId);
    public abstract void RemoveUser(in UserPermaPref user);

    public abstract void SearchFor(in string stringId, out bool found, out UserPermaPref usersInfo);
    public abstract void SearchWithStringId(in string stringId, out bool found, out UserPermaPref[] usersInfo);
    public abstract void GetAllUserPermaPref(out UserPermaPref[] usersInfo);
    public abstract void Flush();
    public abstract void AppendKeysAsDistinctIn(ref Dictionary<string,string> containerDico);
}

public class DefaultUserPermaPrefRegister : AbstractUserPermaPrefRegister
{
    public Dictionary<string, UserPermaPref> m_registeredUser = new Dictionary<string, UserPermaPref>();
    public override void Flush()
    {
        m_registeredUser.Clear();
    }
    public override void OverrideOrAdd(in UserPermaPref user)
    {
        user.GetUserId(out string mail);
        mail = mail.Trim();
        if (!m_registeredUser.ContainsKey(mail))
        {
            m_registeredUser.Add(mail, user);
        }
        else m_registeredUser[mail] = user;
    }

    public override void AddUser(in string mail)
    {
        
            UserPermaPref user = new UserPermaPref(mail);
            OverrideOrAdd(user);
        
    }

    public override void GetAllUserPermaPref(out UserPermaPref[] usersInfo)
    {
        usersInfo = m_registeredUser.Values.ToArray();
    }

    public override bool IsUserExist(in string stringId)
    {
        return m_registeredUser.ContainsKey(stringId.Trim());
    }

    public override void RemoveUser(in string stringId)
    {
        string mail = stringId.Trim();
        if (!m_registeredUser.ContainsKey(mail))
        {
            m_registeredUser.Remove(mail);
        }
    }

    public override void SearchFor(in string stringId, out bool found, out UserPermaPref userInfo)
    {
        string stringIdTrim = stringId.Trim();
        found = m_registeredUser.ContainsKey(stringIdTrim);
        if (found)
        {
            userInfo = m_registeredUser[stringIdTrim];
        }
        else
        {
            found = false;
            userInfo = new UserPermaPref();
        }
    }

    public override void SearchWithStringId(in string stringId, out bool found, out UserPermaPref[] usersInfo)
    {
        string id = stringId;
        usersInfo = m_registeredUser.Values.Where(k => k.UserId.IndexOf(id) >= 0).ToArray();
        found = usersInfo.Length > 0;
    }

    public override void RemoveUser(in UserPermaPref user)
    {
        user.GetUserId(out string id);
        RemoveUser(in id);
    }

    
    public override void AppendKeysAsDistinctIn(ref Dictionary<string, string> containerDico)
    {
        GetAllUserPermaPref(out UserPermaPref[] all);
        for (int i = 0; i < all.Length; i++)
        {
            all[i].AppendKeysAsDistinctIn(ref containerDico);
        }
    }
}