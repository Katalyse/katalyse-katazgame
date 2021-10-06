using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Company.Function
{
    public static class HttpTrigger_API_Cognitive
    {
        [FunctionName("HttpTrigger_API_Cognitive")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string typeCS = req.Query["typeCS"];
            string responseMessage = "ok";
            string responseBetween = "";
            if (typeCS == "STT"){
                MemoryStream ms = new MemoryStream();
                req.Body.CopyTo(ms);
                byte[] listByte = ms.ToArray();
                responseBetween = await RequestToSttPOST(listByte);
                responseMessage = await RequestToLuis(responseBetween);
            }
            else if (typeCS == "OCR"){
                string responseURL = "";

                MemoryStream ms = new MemoryStream();
                req.Body.CopyTo(ms);
                byte[] listByte = ms.ToArray();

                responseURL = await RequestToOcrPOST(listByte);
                responseMessage = await RequestToOcrGET(responseURL);
            } 
            return new OkObjectResult(responseMessage);      
        }

        static async Task<String> RequestToSttPOST(byte[] audio_Byte)
        {
            string authorizationKey = "2c54a2e263e647f39efdc66b5a838fa2";
            string ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
            string speechToTextEndpoint = "https://francecentral.stt.speech.microsoft.com/speech/recognition/conversation/cognitiveservices/v1?language=fr-FR";
            string result = "";
            STTroot data = new STTroot();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add(ocpApimSubscriptionKeyHeader, authorizationKey);
            var uri = speechToTextEndpoint;

            HttpResponseMessage response;
            using (var content = new ByteArrayContent(audio_Byte))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                var res = await response.Content.ReadAsStringAsync();
                result = res.ToString();
                data = JsonConvert.DeserializeObject<STTroot>(result);
                return data.DisplayText;
            }
        }

        static async Task<String> RequestToOcrPOST(byte[] image_Byte)
        {
            string authorizationKey = "2c54a2e263e647f39efdc66b5a838fa2";
            string ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
            string visionAnalysisEndpoint = "https://francecentral.api.cognitive.microsoft.com/vision/v3.1/read/analyze?language=en";
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add(ocpApimSubscriptionKeyHeader, authorizationKey);
            var uri = visionAnalysisEndpoint;

            HttpResponseMessage response;
            using (var content = new ByteArrayContent(image_Byte))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                IEnumerable<string> operationLocation = response.Headers.GetValues("Operation-Location");
                string url_get = "";
                foreach(var elem in operationLocation){
                    url_get += elem;   
                }
                return url_get;
            }
        }

        static async Task<String> RequestToOcrGET(string url)
        {
            System.Threading.Thread.Sleep(1000);
            string authorizationKey = "2c54a2e263e647f39efdc66b5a838fa2";
            string ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
            string result = "";
            string sFullText = "";
            JsonDataOfOCR.Root data = new JsonDataOfOCR.Root();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(ocpApimSubscriptionKeyHeader, authorizationKey);
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        result = await content.ReadAsStringAsync();
                        data = JsonConvert.DeserializeObject<JsonDataOfOCR.Root>(result);
                        foreach (var results in data.AnalyzeResult.ReadResults)
                        {
                            foreach (var line in results.Lines)
                            {
                                foreach (var word in line.Words)
                                {
                                    sFullText += word.Text + " ";
                                }
                            }
                        }
                        return sFullText;
                    }
                }
            }
        }

        static async Task<String> RequestToLuis(string texte)
        {
            //var data = JsonConvert.DeserializeObject<Texte>(requestBody); 
            string luisEndpoint = "https://test-ia-cognitive-service.cognitiveservices.azure.com/luis/prediction/v3.0/apps/08dc6c5a-edc4-4e63-94bd-02020bad0437/slots/production/predict?subscription-key=2c54a2e263e647f39efdc66b5a838fa2&verbose=true&show-all-intents=true&log=true&query=";
            string query = luisEndpoint + texte;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(query))
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = await content.ReadAsStringAsync();
                        return result;
                    }
                }
            }
        }
    }

    public class Texte {
        public string text;
    }

    public class JsonDataOfOCR
    {
        public class Word
        {
            [JsonProperty("boundingBox")]
            public List<int> BoundingBox { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("confidence")]
            public double Confidence { get; set; }
        }

        public class Line
        {
            [JsonProperty("boundingBox")]
            public List<int> BoundingBox { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("words")]
            public List<Word> Words { get; set; }
        }

        public class ReadResult
        {
            [JsonProperty("page")]
            public int Page { get; set; }

            [JsonProperty("angle")]
            public float Angle { get; set; }

            [JsonProperty("width")]
            public int Width { get; set; }

            [JsonProperty("height")]
            public int Height { get; set; }

            [JsonProperty("unit")]
            public string Unit { get; set; }

            [JsonProperty("language")]
            public string Language { get; set; }

            [JsonProperty("lines")]
            public List<Line> Lines { get; set; }
        }

        public class AnalyzeResult
        {
            [JsonProperty("version")]
            public string Version { get; set; }

            [JsonProperty("readResults")]
            public List<ReadResult> ReadResults { get; set; }
        }

        public class Root
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("analyzeResult")]
            public AnalyzeResult AnalyzeResult { get; set; }
        }
    }
    public class STTroot
    {
        [JsonProperty("RecognitionStatus")]
        public string RecognitionStatus { get; set; }

        [JsonProperty("DisplayText")]
        public string DisplayText { get; set; }

        [JsonProperty("Offset")]
        public int Offset { get; set; }

        [JsonProperty("Duration")]
        public int Duration { get; set; }
    }
}

