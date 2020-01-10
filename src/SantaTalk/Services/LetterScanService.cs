using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using SantaTalk.Services.Base;

namespace SantaTalk
{
    public class LetterScanService : BaseService
    {
        public async Task<string> ScanLetterForSanta(Stream image)
        {
            string results = string.Empty;
            try
            {
                var httpResponse = await httpClient.PostAsync("ScanSanta", new StreamContent(image));

                results = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            if (string.IsNullOrWhiteSpace(results))
                results = "The letter is illegible";

            return results;
        }
    }
}