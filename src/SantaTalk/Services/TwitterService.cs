using Newtonsoft.Json;
using SantaTalk.Helpers;
using SantaTalk.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SantaTalk.Services
{
    public class TwitterService
    {
        HttpClient _client;
        public TwitterService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://prod-20.westus2.logic.azure.com:443");
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
        }
        public async Task<string> SendTweet(MessageModel objModel)
        {
            string responseMessage = string.Empty;
            try
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(objModel), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(Common.SendTweetUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    responseMessage= JsonConvert.DeserializeObject<string>(jsonContent);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _client = null;
            }
            return responseMessage;
        }

    }
}
