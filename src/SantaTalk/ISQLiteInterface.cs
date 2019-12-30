using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace SantaTalk
{
    public interface ISQLiteInterface
    {
        SQLiteConnection GetSQLiteConnection();
    }
}
