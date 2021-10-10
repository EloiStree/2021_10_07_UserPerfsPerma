using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class UserPerfsPermaLoaderDemoMono : MonoBehaviour
{

    public string m_whatFile;
    public UserPermaPref m_playerWithContext;
    public KeyPropertiesUntrustedText m_unsafeText;
    public KeyPropertiesVector3Stringable m_positions = new KeyPropertiesVector3Stringable();
    public KP_EventTerminalContextText m_playerLocationContext;
    public UserPermaPrefArchetype m_userArchetype;
    private void Start()
    {
        m_playerWithContext.m_dynamiqueStorage.m_dynamiqueStorage.Add(m_positions);
        m_playerWithContext.m_dynamiqueStorage.m_dynamiqueStorage.Add(m_unsafeText);
    }
}

public class UserPermaPrefArchetype {
    public UserPermaPref m_userArchetypeModel;

    public void SetArchetype(UserPermaPref model) {
        m_userArchetypeModel = model;   
    }
    public void GetDuplicationOfArchetype(out UserPermaPref model) {
        UserPermaPref created = new UserPermaPref();

        E_CodeTag.NotTimeNowButUrgent.Info("Copy the archetype as instance");
        model = created;
    }
}

public class UserPermaPrefArchetypeUtility
{
    public List<KeyPropertiesBuilder> m_builders = new List<KeyPropertiesBuilder>();
    public void AddBuilder(KeyPropertiesBuilder builder) {
        m_builders.Add(builder);
    }
    public void RemoveBuilder(KeyPropertiesBuilder builder) {
        m_builders.Remove(builder);
    }

    public UserPermaPrefArchetypeUtility(IEnumerable<KeyPropertiesBuilder> builders)
    {
        m_builders.AddRange(builders);
    }

    public void CreateInstanceOfUser(out UserPermaPref userCreated) {
        userCreated = new UserPermaPref();
        for (int i = 0; i < m_builders.Count; i++)
        {
            m_builders[i].CreateEmptyCollection(out IKeyPropertiesAsStringFullInteraction collection);
            userCreated.m_dynamiqueStorage.m_dynamiqueStorage.Add(collection);
        }
        
    }
}

public abstract class KeyPropertiesBuilder {

    public abstract void CreateEmptyCollection(out IKeyPropertiesAsStringFullInteraction created);
    
}
