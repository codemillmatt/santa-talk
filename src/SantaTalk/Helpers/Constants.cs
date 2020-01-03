using System;
using System.IO;

namespace SantaTalk.Helpers
{
    public static class Constants
    {

        public static readonly string basicUrl = "http://localhost:7071/";     

        public const string DatabaseFilename = "DataBase.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}
