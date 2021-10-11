using Eloi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ExportUserPermaRegisterAsCSV : MonoBehaviour
{
    public UserPermaPrefRegisterMono m_register;

    public string m_csvNameWithExtension="default.csv";
    public string[] m_columnNameWanted=new string[] { "mail", "firstname", "lastname","bestscore"  };
    [TextArea(0,10)]
    public string m_csvDebug;

    public void GetProposeCSVName(out string csvName) {
        csvName = m_csvNameWithExtension;
    }

    [ContextMenu("Generate csv ")]
    public void ExportCSVWithParamsColumns()
    {
        UserPermaPrefExportToCSV.ExportCSVAsFullData(in m_register, out m_csvDebug, m_columnNameWanted);
    }
    public void ExportCSVWithParamsColumns(out string csv)
    {
        UserPermaPrefExportToCSV.ExportCSVAsFullData(in m_register, out csv, m_columnNameWanted);
    }
    [ContextMenu("Generate csv with all")]
    public void ExportCSVWithAllDataInIt()
    {
        UserPermaPrefExportToCSV.ExportCSVWithAllDataInIt(in m_register, out m_csvDebug);
    }
 


}


public class UserPermaPrefExportToCSV{

    public static void ExportCSVWithAllDataInIt(in UserPermaPrefRegisterMono registerMono, out string csv)
    {
        Dictionary<string, string> columnDico = new Dictionary<string, string>();
        registerMono.AppendKeysAsDistinctIn(ref columnDico);
        UserPermaPrefExportToCSV.ExportCSVAsFullData(in registerMono, out csv, columnDico.Keys.ToArray());
    }



    public static void ExportCSVAsFullData(in UserPermaPrefRegisterMono registerMono, out string csv, params string [] columnNameRequested)
    {
        AbstractUserPermaPrefRegister reg = registerMono.GetRegisterRef();
        ExportCSVAsFullData(in reg, out csv, columnNameRequested);
    }
    public static void ExportCSVAsFullData(in AbstractUserPermaPrefRegister register,   out string csv, params string[] columnNameRequested)
    {
        csv = "";
        StringBuilder sb = new StringBuilder();
        register.GetAllUserPermaPref(out UserPermaPref[] users);


        List<string> columnInProgress = new List<string>();

        ///DRAW COLUMN
        for (int j = 0; j < columnNameRequested.Length; j++)
        {
            columnNameRequested[j] = columnNameRequested[j].Trim();
        }
        sb.Append("id;" + string.Join(";", columnNameRequested));
        sb.Append("\n");



        ///For each users try to generate fields;
        for (int i = 0; i < users.Length; i++)
        {
            columnInProgress.Clear();
            columnInProgress.Add(users[i].UserId);
            for (int j = 0; j < columnNameRequested.Length; j++)
            {
                users[i].TryToGuestKeyValueAsString(in columnNameRequested[j], out string value);
                columnInProgress.Add(value);
            }


            sb.Append(string.Join(";", columnInProgress));
            sb.Append("\n");
        }

        csv = sb.ToString();

    }
    public void ExportCSVAsFullDataWithPreferenceCells(params string[] columnOrderPreference)
    {
        E_CodeTag.NotTimeNow.Info("This code would be greate. The idea is to display all but starting with the column given.");
    }


}