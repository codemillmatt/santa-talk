using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Xamarin.Forms;

[assembly: Dependency(typeof(SantaTalk.UWP.UserInformation))]
namespace SantaTalk.UWP
{
    public class UserInformation : IUserInformation
    {
        public async Task<string> GetFirstName()
        {
            IReadOnlyList<User> users = await User.FindAllAsync();

            var current = users.Where(p => p.AuthenticationStatus == UserAuthenticationStatus.LocallyAuthenticated && p.Type == UserType.LocalUser).FirstOrDefault();

            var data = await current.GetPropertyAsync(KnownUserProperties.FirstName);
            string firstname = data.ToString();

            return firstname;
        }
    }
}
