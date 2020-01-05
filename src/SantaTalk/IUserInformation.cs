using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SantaTalk
{
    // Get current user info, taken from https://doumer.me/get-the-device-current-user-name-with-xamarin-forms/

    public interface IUserInformation
    {
        Task<string> GetFirstName();
    }
}
