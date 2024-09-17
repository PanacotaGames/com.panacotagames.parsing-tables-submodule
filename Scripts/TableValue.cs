using I2.Loc;
using System.Globalization;
using System;
using UnityEngine;

namespace I2.Loc
{
    public static class TableValue
    {
        public static T GetValue<T>(this LanguageSourceAsset table, string row, string column)
        {
            CultureInfo info = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            info.NumberFormat.NumberDecimalSeparator = ".";

            row = "Sheet1/" + row;
            for (var i = 0; i < table.mSource.mTerms.Count; i++)
            {
                if (table.mSource.mTerms[i].Term == row)
                {
                    for (var j = 0; j < table.mSource.mLanguages.Count; j++)
                    {
                        if (table.mSource.mLanguages[j].Name == column)
                        {
                            try 
                            {
                                return (T)Convert.ChangeType(table.mSource.mTerms[i].Languages[j], typeof(T), info);
                            }
                            catch (Exception e) 
                            {
                                Debug.LogError($"Can't convert {table.mSource.mTerms[i].Languages[j]} to {typeof(T)}");
                            }
                        }
                    }
                }
            }
            throw new Exception("there is no suitable option");
        }
    }
}
