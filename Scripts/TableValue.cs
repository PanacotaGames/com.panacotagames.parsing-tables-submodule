using I2.Loc;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace I2.Loc
{
    public static class TableValue
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private const string SHEET_NAME = "Development";
#else
        private const string SHEET_NAME = "Production";
#endif

        public static T GetValue<T>(this LanguageSourceAsset table, string row, string column, string sheetName = SHEET_NAME)
        {
            CultureInfo info = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            info.NumberFormat.NumberDecimalSeparator = ".";
            
            string oldRowname = row;
            row = sheetName + "/" + row;
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
                                if (typeof(T).IsEnum)
                                {
                                    object result = Enum.Parse(typeof(T), table.mSource.mTerms[i].Languages[j]);
                                    return (T)result;
                                }
                                
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
                                throw new Exception($"Can't convert {table.mSource.mTerms[i].Languages[j]} to {typeof(T)}");
                            }
                        }
                    }
                }
            }
            throw new Exception($"Value not found: sheet: [{sheetName}] row: [{oldRowname}] column: [{column}]");
        }

        public static string[] GetRows(this LanguageSourceAsset table, string sheetName = SHEET_NAME)
        {
            sheetName += "/";
            return table.mSource.mTerms.Where(x => x.Term.Contains(sheetName)).Select(x => x.Term.Replace(sheetName, "")).ToArray();
        }

        public static string[] GetColumns(this LanguageSourceAsset table, string sheetName = SHEET_NAME)
        {
            if (table.mSource.mDictionary.Count == 0)
            {
                table.mSource.UpdateDictionary();
            }

            TermData data = table.mSource.mDictionary.First(x => x.Key.Contains(sheetName)).Value;
            
            List<int> languagueIndexes = new();
            foreach (string s in data.Languages.Where(x => string.IsNullOrEmpty(x) == false))
            {
                int index = 0;
                while (index < data.Languages.Count())
                {
                    if (languagueIndexes.Contains(index) || data.Languages.ElementAt(index) != s)
                    {
                        index++;
                        continue;
                    }

                    languagueIndexes.Add(index);
                    index++;
                }
            }
            List<string> columns = new();
            foreach (int index in languagueIndexes)
            {
                columns.Add(table.mSource.mLanguages[index].Name);
            }
            return columns.ToArray();
        }
    }
}
