using System;
using System.Net.Http;

using Xamarin.Essentials;

namespace SantaTalk.Services.Base
{
    public class BaseService
    {
        //string santaUrl = "{REPLACE WITH YOUR FUNCTION URL}/api/";

        protected string santaUrl = "http://localhost:7071/api/";
        protected static HttpClient httpClient = new HttpClient();

        public BaseService()
        {
            // if we're on the Android emulator, running functions locally, need to swap out the function url
            if (santaUrl.Contains("localhost") && DeviceInfo.DeviceType == DeviceType.Virtual && DeviceInfo.Platform == DevicePlatform.Android)
            {
                santaUrl = santaUrl.Replace("localhost", "10.5.132.243");
            }

            httpClient.BaseAddress = new Uri(santaUrl);
        }
    }
}