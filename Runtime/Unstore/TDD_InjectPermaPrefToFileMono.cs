using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDD_InjectPermaPrefToFileMono : MonoBehaviour
{
    public UserPermaPref m_dataSourceToInject;
    public ExportUserPermaFocusAsFile m_exportUser;


    [ContextMenu("Inject data")]
    public void Inject()
    {
        UserPermaPrefInjectUtility.InjectAllFieldsIn(
            m_dataSourceToInject ,
            m_exportUser.m_focusUser.GetUserInfo());

    }

    [ContextMenu("Export data")]
    public void Export()
    {
        m_exportUser.Export();

    }
    [ContextMenu("Inject & Export data")]
    public void InjectAndExport()
    {
        Inject();
        Export();

    }
}

public class UserPermaPrefInjectUtility {

    public static void InjectAllFieldsIn(in UserPermaPref source, in UserPermaPref destination) {

        if (Eloi.E_StringUtility.IsFilled(source.m_userInfo.m_userStringId))
            destination.m_userInfo.m_userStringId = source.m_userInfo.m_userStringId;
        if (Eloi.E_StringUtility.IsFilled(source.m_userInfo.m_userAlias))
            destination.m_userInfo.m_userAlias = source.m_userInfo.m_userAlias;

        string[] ids;
        source.m_unthrustedText.GetAllKey(out ids);
        foreach (var item in ids)
        {
            source.GetUnthrustedTextValue(in item, out string value);
            destination.SetUnthrustedText(in item, value);
        }

        InjectFromToInUnkownStruct(
            source.m_dynamiqueStorage.m_dynamiqueStorage,
            destination.m_dynamiqueStorage.m_dynamiqueStorage);
        InjectFromToIfFill(
            source.m_unthrustedText,
            destination.m_unthrustedText);

        InjectFromToIn(
            source.m_primitivesStorage.m_bool,
            destination.m_primitivesStorage.m_bool);

        InjectFromToIn(
          source.m_primitivesStorage.m_byte,
          destination.m_primitivesStorage.m_byte);

        InjectFromToIn(
          source.m_primitivesStorage.m_char,
          destination.m_primitivesStorage.m_char);

        InjectFromToIn(
          source.m_primitivesStorage.m_decimal,
          destination.m_primitivesStorage.m_decimal);

        InjectFromToIn(
          source.m_primitivesStorage.m_double,
          destination.m_primitivesStorage.m_double);

        InjectFromToIn(
          source.m_primitivesStorage.m_float,
          destination.m_primitivesStorage.m_float);

        InjectFromToIn(
          source.m_primitivesStorage.m_int,
          destination.m_primitivesStorage.m_int);

        InjectFromToIn(
          source.m_primitivesStorage.m_long,
          destination.m_primitivesStorage.m_long);

        InjectFromToIn(
          source.m_primitivesStorage.m_short,
          destination.m_primitivesStorage.m_short);

        InjectFromToIn(
          source.m_primitivesStorage.m_signByte,
          destination.m_primitivesStorage.m_signByte);

        InjectFromToIn(
          source.m_primitivesStorage.m_uint,
          destination.m_primitivesStorage.m_uint);

        InjectFromToIn(
          source.m_primitivesStorage.m_ulong,
          destination.m_primitivesStorage.m_ulong);

        InjectFromToIfFill(
          source.m_primitivesStorage.m_unprotectedString,
          destination.m_primitivesStorage.m_unprotectedString);

        InjectFromToIn(
          source.m_primitivesStorage.m_ushot,
          destination.m_primitivesStorage.m_ushot);
    }

    private static void InjectFromToInUnkownStruct(List<IKeyPropertiesAsStringFullInteraction> source, List<IKeyPropertiesAsStringFullInteraction> destination)
    {

        foreach (var s in source)
        {
            foreach (var d in destination)
            {
                Eloi.E_CodeTag.NotTimeNowButUrgent.Info("Not tested.");
                if (s.GetType() == d.GetType()) {
                    Eloi.E_CodeTag.NotTimeNowButUrgent.Info(
                        "Found similiar type of collection " + s.GetType() + "-" + d.GetType());
                }
            }
        }
    }
    private static void InjectFromToInUnkownStruct(IKeyPropertiesAsStringFullInteraction source, IKeyPropertiesAsStringFullInteraction destination)
    {
        string[] ids;
        source.GetAllKey(out ids);
        Eloi.E_CodeTag.NotTimeNowButUrgent.Info("Not tested.");
        foreach (var item in ids)
        {
            source.GetValue(in item,out bool found, out string value);
            if (Eloi.E_StringUtility.IsFilled(in value))
                destination.SetValue(in item,in value, out bool wasConverted);
        }
    }

    private static void InjectFromToIn<T>(KeyProperties<T> source, KeyProperties<T> destination)
    {
        string[] ids;
        source.GetAllKey(out ids);
        foreach (var item in ids)
        {
            source.GetValue(in item, out T value);
            destination.SetValue(in item, value);
        }
    }
    private static void InjectFromToIfFill(KeyProperties<string> source, KeyProperties<string> destination)
    {
        string[] ids;
        source.GetAllKey(out ids);
        foreach (var item in ids)
        {
            source.GetValue(in item, out string value);
            if(Eloi.E_StringUtility.IsFilled(in value))
                destination.SetValue(in item, value);
        }
    }


}
