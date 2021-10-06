using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonDataOfLUIS : MonoBehaviour
{
    public class Deplacement
    {
        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public class Décalage
    {
        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public class Code
    {
        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public class Vue
    {
        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public class Ouvrir
    {
        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public class None
    {
        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public class Intents
    {
        [JsonProperty("Deplacement")]
        public Deplacement Deplacement { get; set; }

        [JsonProperty("Décalage")]
        public Décalage Décalage { get; set; }

        [JsonProperty("Code")]
        public Code Code { get; set; }

        [JsonProperty("Vue")]
        public Vue Vue { get; set; }

        [JsonProperty("Ouvrir")]
        public Ouvrir Ouvrir { get; set; }

        [JsonProperty("None")]
        public None None { get; set; }
    }

    public class Objet
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("modelTypeId")]
        public int ModelTypeId { get; set; }

        [JsonProperty("modelType")]
        public string ModelType { get; set; }

        [JsonProperty("recognitionSources")]
        public List<string> RecognitionSources { get; set; }
    }

    public class Direction
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("modelTypeId")]
        public int ModelTypeId { get; set; }

        [JsonProperty("modelType")]
        public string ModelType { get; set; }

        [JsonProperty("recognitionSources")]
        public List<string> RecognitionSources { get; set; }
    }

    public class Number
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("modelTypeId")]
        public int ModelTypeId { get; set; }

        [JsonProperty("modelType")]
        public string ModelType { get; set; }

        [JsonProperty("recognitionSources")]
        public List<string> RecognitionSources { get; set; }
    }

    public class Instance
    {
        [JsonProperty("Objet")]
        public List<Objet> Objet { get; set; }

        [JsonProperty("Direction")]
        public List<Direction> Direction { get; set; }

        [JsonProperty("number")]
        public List<Number> Number { get; set; }
    }

    public class Entities
    {
        [JsonProperty("Objet")]
        public List<List<string>> Objet { get; set; }

        [JsonProperty("Direction")]
        public List<List<string>> Direction { get; set; }

        [JsonProperty("number")]
        public List<float> Number { get; set; }

        [JsonProperty("$instance")]
        public Instance Instance { get; set; }
    }

    public class Prediction
    {
        [JsonProperty("topIntent")]
        public string TopIntent { get; set; }

        [JsonProperty("intents")]
        public Intents Intents { get; set; }

        [JsonProperty("entities")]
        public Entities Entities { get; set; }
    }

    public class Root
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("prediction")]
        public Prediction Prediction { get; set; }
    }

}
