using System;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using SantaTalk.Models;
using SantaTalk.Services.Base;

namespace SantaTalk
{
    public class LetterDeliveryService : BaseService
    {
        public async Task<SantaResults> WriteLetterToSanta(SantaLetter letter)
        {
            SantaResults results = null;
            try
            {
                var letterJson = JsonConvert.SerializeObject(letter);

                var httpResponse = await httpClient.PostAsync("WriteSanta", new StringContent(letterJson));

                results = JsonConvert.DeserializeObject<SantaResults>(await httpResponse.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            if (results == null)
                results = new SantaResults { SentimentScore = -1 };

            return results;
        }
    }
}