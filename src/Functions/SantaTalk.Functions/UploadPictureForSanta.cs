using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Collections.Generic;
using SantaTalk.Models;
using System.Linq;

namespace SantaTalk.Functions
{
    public static class UploadPictureForSanta
    {
        static FaceClient faceClient;

        static UploadPictureForSanta()
        {
            var keys = new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("FaceAPIKey"));

            faceClient = new FaceClient(keys) { Endpoint = Environment.GetEnvironmentVariable("FaceAPIEndpoint") };
        }

        [FunctionName("UploadPictureForSanta")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var base64encodedstring = await new StreamReader(req.Body).ReadToEndAsync();

                var bytes = Convert.FromBase64String(base64encodedstring);
                MemoryStream pictureStream = new MemoryStream(bytes);

                // The list of Face attributes to return.
                IList<FaceAttributeType> faceAttributes = new FaceAttributeType[]
                {
                    FaceAttributeType.Gender,
                    FaceAttributeType.Age,
                    FaceAttributeType.Smile
                };

                IList<DetectedFace> faceList = await faceClient.Face.DetectWithStreamAsync(pictureStream, true, false, faceAttributes);
                pictureStream.Dispose();

                PictureForSantaResults result = new PictureForSantaResults { Smile = -1 };

                if (faceList.Count > 0)
                {
                    var face = faceList.First();
                    result.Age = face.FaceAttributes.Age ?? 0;
                    result.Gender = face.FaceAttributes.Gender?.ToString() ?? "Male";
                    result.Smile = face.FaceAttributes.Smile ?? -1;
                }
                       
                return new OkObjectResult(result);
            }
            catch (APIErrorException f)
            {
                log.LogError(f.Message.ToString());

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                log.LogError(e.Message.ToString());

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
