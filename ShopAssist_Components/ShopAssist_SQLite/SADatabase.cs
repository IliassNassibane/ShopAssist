using System;
using System.IO;

namespace ShopAssist_SQLite
{
    public class SADatabase
    {
        protected static string LoadConnectionString(string dbValue = "ShopAssist_DefaultDB.db")
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbValue);
        }
    }
}
