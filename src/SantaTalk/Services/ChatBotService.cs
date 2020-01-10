using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SantaTalk.Services.RestModels;

namespace SantaTalk.Services
{
    public class ChatBotService
    {
        const string endpoint = "ENDPOINT_URL";
        const string endpointKey = "ENDPOINT_KEY";
        const string kbId = "KNOWLENGE_BASE_ID";



        static HttpClient httpClient = new HttpClient();

        public async Task<string> SendMessage(string question)
        {
            var uri = endpoint + "/qnamaker/knowledgebases/" + kbId + "/generateAnswer";
            var questionContent = new { question = question }; 
            using (var request = new HttpRequestMessage())
            {
                var test = JsonConvert.SerializeObject(questionContent);
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(test, Encoding.UTF8, "application/json");
                request.Headers.Add("Authorization", "EndpointKey " + endpointKey);

                var response = await httpClient.SendAsync(request);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<ChatBotResponse>(jsonResponse);
                var answer = responseModel.Answers.FirstOrDefault()?.Message;

                return answer;
            }
        }
    }


}
