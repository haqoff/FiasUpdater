using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

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


        public static int BuildUpdateSqlCommand(FIASClassesDataContext mainDB,FIASClassesDataContext tempDB, string tableName, string key,IEnumerable<string> fields)
        {
            int updatedRows = 0;

            mainDB.Connection.Open();

            var sb = new StringBuilder();
            foreach (var field in fields)
            {
                if (sb.Length != 0) sb.Append(',');
                sb.Append(string.Format("{0} = t2.{0}", field));
            }

            string updateConstruct = String.Format("UPDATE {0}" +
                    " SET {1} " +
                    "FROM {2}.[dbo].{0} As t2 " +
                    "WHERE {0}.{3} = t2.{3}", tableName, sb.ToString(), tempDB.Connection.Database, key);

            try
            {
                using (var command = (SqlCommand)mainDB.Connection.CreateCommand())
                {
                    command.CommandTimeout = 0;
                    command.CommandText = updateConstruct;
                    updatedRows = command.ExecuteNonQuery();
                    mainDB.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string insertConstruct = string.Format("");

            return updatedRows;
        }


    }
}
