using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            guidString = guidString.Replace(@"\","");

            return guidString;
        }

    }
}
