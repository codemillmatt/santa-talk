using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using SantaTalk.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SantaTalk.Services
{
    public class HttpClientService
    {
        private HttpClient _client;

        public HttpClient HttpClient
        {
            get { return _client; }

        }

        public HttpClientService(string url)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(url);
            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Common.SubscriptionKey);
            _client.Timeout = new System.TimeSpan(0, 0, 10);
        }


        public HttpClient GetHttpClient()
        {

            return HttpClient;
        }

        //public Task<object> PostClient(Object)
        //{

        //}
    }
}
