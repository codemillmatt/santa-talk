public static class SendPicture
{
    static string subscriptionKey = Environment.GetEnvironmentVariable("APIKey");
    static string endpoint = Environment.GetEnvironmentVariable("APIEndPoint");
    static string subscriptionKeyFace = Environment.GetEnvironmentVariable("APIKey");
    static string endpointFace = Environment.GetEnvironmentVariable("APIEndPoint");

    [FunctionName("SendPicture")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");
		
        var filesProvider = await req.Content.ReadAsMultipartAsync();
        var fileContents = filesProvider.Contents.FirstOrDefault();


        byte[] payload = await fileContents.ReadAsByteArrayAsync();
        Stream stream = new MemoryStream(payload);
     
        faceFactory ff = new faceFactory(endpointFace, subscriptionKeyFace);
        var result = ff.analyzeImageAsync(stream);
        Task.WaitAll(result);
      
        string responseBody = JsonConvert.SerializeObject(result.Result);
        return (ActionResult)new OkObjectResult(responseBody);

    }



}

public class faceFactory
{
    
    const string RECOGNITION_MODEL2 = RecognitionModel.Recognition02;
    const string RECOGNITION_MODEL1 = RecognitionModel.Recognition01;
    private string endpoint { get; set; }
    private string subscriptionKey { get; set; }
    public faceFactory(string _endpoint, string _subscriptionKey)
    {
        endpoint = _endpoint;
        subscriptionKey = _subscriptionKey;
    }

    public async Task<List<FaceInfo>> analyzeImageAsync(Stream st)
    {
        IFaceClient client = Authenticate(endpoint, subscriptionKey);

        return await DetectFace(client, st, RECOGNITION_MODEL2);

    }

    protected Stream GetStream(String gazouUrl)
    {
        Stream rtn = null;
        System.Net.HttpWebRequest aRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(gazouUrl);
        System.Net.HttpWebResponse aResponse = (System.Net.HttpWebResponse)aRequest.GetResponse();
        return aResponse.GetResponseStream();

        using (StreamReader sReader = new StreamReader(aResponse.GetResponseStream(), System.Text.Encoding.Default))
        {
            rtn = sReader.BaseStream;
        }
        return rtn;
    }


    private static IFaceClient Authenticate(string endpoint, string key)
    {
        return new FaceClient(new Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
    }

    public static async Task<List<FaceInfo>> DetectFace(IFaceClient client, Stream image, string recognitionModel)
    {
        List<FaceInfo> facesI = new List<FaceInfo>();
        try
        {


            Console.WriteLine("========DETECT FACES========");
            Console.WriteLine();
            IList<DetectedFace> detectedFaces = await client.Face.DetectWithStreamAsync(image,
                     returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Accessories, FaceAttributeType.Age,
                FaceAttributeType.Blur, FaceAttributeType.Emotion, FaceAttributeType.Exposure, FaceAttributeType.FacialHair,
                FaceAttributeType.Gender, FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.HeadPose,
                FaceAttributeType.Makeup, FaceAttributeType.Noise, FaceAttributeType.Occlusion, FaceAttributeType.Smile },
                     recognitionModel: recognitionModel);

            Console.WriteLine($"{detectedFaces.Count} face(s) detected from image .");
            // Parse and print all attributes of each detected face.
            foreach (var face in detectedFaces)
            {
                Console.WriteLine($"Face attributes ");
                // Get emotion on the face
                string emotionType = string.Empty;
                double emotionValue = 0.0;
                Emotion emotion = face.FaceAttributes.Emotion;
                if (emotion.Anger > emotionValue) { emotionValue = emotion.Anger; emotionType = "Anger"; }
                if (emotion.Contempt > emotionValue) { emotionValue = emotion.Contempt; emotionType = "Contempt"; }
                if (emotion.Disgust > emotionValue) { emotionValue = emotion.Disgust; emotionType = "Disgust"; }
                if (emotion.Fear > emotionValue) { emotionValue = emotion.Fear; emotionType = "Fear"; }
                if (emotion.Happiness > emotionValue) { emotionValue = emotion.Happiness; emotionType = "Happiness"; }
                if (emotion.Neutral > emotionValue) { emotionValue = emotion.Neutral; emotionType = "Neutral"; }
                if (emotion.Sadness > emotionValue) { emotionValue = emotion.Sadness; emotionType = "Sadness"; }
                if (emotion.Surprise > emotionValue) { emotionType = "Surprise"; }
                Console.WriteLine($"Emotion : {emotionType}");
                FaceInfo f = new FaceInfo();
                f.Age = face.FaceAttributes.Age;
                f.emotion = emotionType;
                f.Gender = face.FaceAttributes.Gender.Value.ToString();
                f.smile = face.FaceAttributes.Smile;
                facesI.Add(f);
                Console.WriteLine();
            }
        }
        catch (Exception error)
        {

      
        }
        return facesI;
    }
}
