using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using SantaTalk.iOS.Services;
using SQLite.Net;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ISQLiteDbInterface_iOS))]
namespace SantaTalk.iOS.Services
{

    public class ISQLiteDbInterface_iOS : ISQLiteInterface
    {
        
        public SQLiteConnection GetSQLiteConnection()
        {
            var fileName = "userDB.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, fileName);
            var platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
            var connection = new SQLite.Net.SQLiteConnection(platform, path);
            return connection;
        }
    }
}