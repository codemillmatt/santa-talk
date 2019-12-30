using SantaTalk.Models;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace SantaTalk.Helper
{
    public class UserDB
    {
        private SQLiteConnection _SQLiteConnection;

        public UserDB()
        {
            _SQLiteConnection = DependencyService.Get<ISQLiteInterface>().GetSQLiteConnection();
            _SQLiteConnection.CreateTable<SantaLetter>();
        }

        public IEnumerable<SantaLetter> GetMessages()
        {
            return (from u in _SQLiteConnection.Table<SantaLetter>()
                    select u).ToList();
        }
        
        public string AddMessage(SantaLetter user)
        {
            var data = _SQLiteConnection.Table<SantaLetter>();
            var d1 = data.Where(x => x.KidName == user.KidName && x.LetterText == user.LetterText).FirstOrDefault();
            if (d1 == null)
            {
                _SQLiteConnection.Insert(user);
                return "Sucessfully Added";
            }
            else
                return "Message is Empty";


        }
    }
}