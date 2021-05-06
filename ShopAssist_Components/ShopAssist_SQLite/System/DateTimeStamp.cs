using System;
using System.Collections.Generic;
using System.Text;

namespace ShopAssist_SQLite.System
{
    public class DateTimeStamp
    {
        public static string Stamp()
        {
            DateTime date = DateTime.Now;

            return date.ToString("ss:mm:HH dd-MM-yyyy");
        }
    }
}
