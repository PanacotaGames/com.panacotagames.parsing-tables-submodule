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
                                if (typeof(T) == typeof(int) || typeof(T) == typeof(long) || typeof(T) == typeof(short)) 
                                {
                                    float result = (float)Convert.ChangeType(table.mSource.mTerms[i].Languages[j], typeof(float), info);
                                    if (typeof(T) == typeof(int)) 
                                    {
                                        return (T)(object)(int)result;
                                    }
                                    if (typeof(T) == typeof(long))
                                    {
                                        return (T)(object)(long)result;
                                    }
                                    if (typeof(T) == typeof(short))
                                    {
                                        return (T)(object)(short)result;
                                    }
                                }
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
