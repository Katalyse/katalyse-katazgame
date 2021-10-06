using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonDataOfCVR : MonoBehaviour
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
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
