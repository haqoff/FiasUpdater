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


        public static int BuildUpdateSqlCommand(FIASClassesDataContext mainDB, FIASClassesDataContext tempDB, string tableName, string key, IEnumerable<string> fields)
        {
            int updatedRows = 0;

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

            sb.Clear();
            foreach (var field in fields)
            {
                if (sb.Length != 0) sb.Append(',');
                sb.Append(field);
            }

            sb.Append("," + key);
            var fieldsString = sb.ToString();


            sb.Clear();
            foreach (var field in fields)
            {
                if (sb.Length != 0) sb.Append(',');
                sb.Append("t2.");
                sb.Append(field);
            }
            sb.Append(",t2." + key);

            var fieldsWithNamedTable = sb.ToString();
            string insertConstruct = string.Format("INSERT INTO {0} ({1}) SELECT {2} FROM {0} as t1 RIGHT JOIN {3}.dbo.{0} as t2 " +
                "ON t1.{4} = t2.{4} WHERE t1.{4} IS NULL", tableName, fieldsString, fieldsWithNamedTable, tempDB.Connection.Database, key);

            try
            {
                mainDB.Connection.Open();
                using (var command = (SqlCommand)mainDB.Connection.CreateCommand())
                {
                    command.CommandTimeout = 0;

                    command.CommandText = updateConstruct;
                    updatedRows = command.ExecuteNonQuery();

                    command.CommandText = insertConstruct;
                    updatedRows += command.ExecuteNonQuery();

                    mainDB.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return updatedRows;
        }


    }
}
