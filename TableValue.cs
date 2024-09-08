using UnityEngine;
using I2.Loc;
using System;
using System.ComponentModel;
using System.Globalization;

public static class TableValue
{
    public static T GetValue<T>(this LanguageSourceAsset table, string row, string column)
    {

        for (var i = 0; i < table.mSource.mTerms.Count; i++)
        {
            if (table.mSource.mLanguages[i].Name == column)
            {
                for (var j = 0; j < table.mSource.mTerms.Count; j++)
                {
                    if (table.mSource.mTerms[j].Term == row)
                    {
                        return (T)Convert.ChangeType(table.mSource.mTerms[j].Languages[i], typeof(T), CultureInfo.InvariantCulture);
                    }
                }
            }
        }
        throw new Exception("there is no suitable option");
    } 
}