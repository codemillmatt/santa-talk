using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.Accounts;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(SantaTalk.Droid.UserInformation))]
namespace SantaTalk.Droid
{
    public class UserInformation : IUserInformation
    {
        public Task<string> GetFirstName()
        {
            FormsAppCompatActivity activity = CrossCurrentActivity.Current.Activity as FormsAppCompatActivity;

            activity.RequestPermissions(new string[] { Manifest.Permission.GetAccounts }, 1);
            if (activity.CheckSelfPermission(Manifest.Permission.GetAccounts) == Android.Content.PM.Permission.Granted)
            {
                AccountManager manager = AccountManager.Get(activity);
                Account[] accounts = manager.GetAccountsByType("com.google");
                List<String> emails = new List<string>();

                foreach (Account account in accounts)
                {
                    var name = account.Name;
                    emails.Add(account.Name);
                }

                if (emails.Any() && emails[0] != null)
                {
                    String email = emails[0];
                    String[] parts = email.Split('@');

                    if (parts.Length > 1)
                        return Task.FromResult(parts[0]);
                }
            }

            return Task.FromResult("");
        }
    }
}