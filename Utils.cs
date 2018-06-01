using System;
using System.Data.Common;
using System.Text;
using System.Windows.Forms;

namespace FIASUpdater
{
    public static class Utils
    {
        /// <summary>
        /// Возвращает случайно-сгенерированную уникальную строку.
        /// </summary>
        /// <returns></returns>
        public static string GenerateUnique()
        {
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");
            guidString = guidString.Replace("/", "");
            guidString = guidString.Replace(@"\", "");

            return guidString;
        }

        /// <summary>
        /// Строит SQL-коммнаду.
        /// </summary>
        /// <param name="mainDb"></param>
        /// <param name="tempDb"></param>
        /// <param name="tableName"></param>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <returns>Возвращает количество затронутых строк.</returns>
        public static int BuildUpdateSqlCommand(DbConnection mainDb, DbConnection tempDb, string tableName, string key, string[] fields)
        {
            var updateSetFields = new StringBuilder();
            var insertFields = new StringBuilder();
            var insertValues = new StringBuilder();

            insertFields.Append(key);
            insertFields.Append(','); 

            insertValues.Append("SOURCE.");
            insertValues.Append(key);
            insertValues.Append(',');

            for (var i = 0; i < fields.Length; i++)
            {
                updateSetFields.Append(string.Format("TARGET.{0} = SOURCE.{0}", fields[i]));

                insertFields.Append(',');
                insertValues.Append(',');

                insertFields.Append(fields[i]);
                insertValues.Append(fields[i]);

                if (i != fields.Length - 1)
                {
                    updateSetFields.Append(',');
                }
            }

            var res = 0;
            try
            {
                using (var cmd = mainDb.CreateCommand())
                {
                    cmd.CommandText = string.Format(
                        "MERGE [{0}].[dbo].[{1}] AS TARGET\r\nUSING [{2}].[dbo].[{1}] AS SOURCE \r\nON (TARGET.{3} = SOURCE.{3}) \r\nWHEN MATCHED THEN \r\nUPDATE SET {4} \r\nWHEN NOT MATCHED BY TARGET THEN \r\nINSERT ({5}) \r\nVALUES ({6});\r\nSELECT @@ROWCOUNT;\r\nGO",
                        mainDb.Database, tableName, tempDb.Database, key, updateSetFields, insertFields, insertValues);

                    cmd.CommandTimeout = 0;

                    mainDb.Open();
                    cmd.Prepare();
                    res = (int) cmd.ExecuteScalar();
                    mainDb.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка во время выполнения запроса.");
            }

            return res;
        }
    }
}
