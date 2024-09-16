using I2.Loc;
using System.Globalization;
using System;

namespace Core.LockedObjects
{
    public static class TableValue
    {
        public static T GetValue<T>(this LanguageSourceAsset table, string row, string column)
        {
            row = "Sheet1/" + row;
            for (var i = 0; i < table.mSource.mTerms.Count; i++)
            {
                if (table.mSource.mTerms[i].Term == row)
                {
                    for (var j = 0; j < table.mSource.mLanguages.Count; j++)
                    {
                        if (table.mSource.mLanguages[j].Name == column)
                        {
                            return (T)Convert.ChangeType(table.mSource.mTerms[i].Languages[j], typeof(T), CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
            throw new Exception("there is no suitable option");
        }
    }
}
