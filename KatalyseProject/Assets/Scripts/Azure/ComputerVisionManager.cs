using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System;

public class ComputerVisionManager : MonoBehaviour
{
	// you must insert your service key here!    
	private string authorizationKey = "2c54a2e263e647f39efdc66b5a838fa2";
	private const string ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
	private string visionAnalysisEndpoint = "https://francecentral.api.cognitive.microsoft.com/vision/v3.1/read/analyze?language=en";
	private string sResponseHeader;

	private byte[] imageBytes;

	public CameraCapture ccCameraCapture;
	private static ComputerVisionManager _instance;
	public static ComputerVisionManager getInstance()
	{
		return _instance;
	}
	private void Awake()
	{
		// allows this class instance to behave like a singleton
		_instance = this;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			StartCoroutine(AnalyseLastImageCaptured());
		}
	}

	public IEnumerator AnalyseLastImageCaptured()
	{
		//POST
		print("POST");
		WWWForm webForm = new WWWForm();
		using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(visionAnalysisEndpoint, webForm))
		{
			// gets a byte array out of the saved image
			imageBytes = ccCameraCapture.Capture();
			unityWebRequest.SetRequestHeader("Content-Type", "application/octet-stream");
			unityWebRequest.SetRequestHeader(ocpApimSubscriptionKeyHeader, authorizationKey);

			// the download handler will help receiving the analysis from Azure
			unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

			// the upload handler will help uploading the byte array with the request
			unityWebRequest.uploadHandler = new UploadHandlerRaw(imageBytes);
			unityWebRequest.uploadHandler.contentType = "application/octet-stream";

			yield return unityWebRequest.SendWebRequest();

			long responseCode = unityWebRequest.responseCode;

			try
			{
				sResponseHeader = unityWebRequest.GetResponseHeaders()["Operation-Location"];
				StartCoroutine(AnalyseResult(sResponseHeader));
				/*
				string jsonResponse = null;
				jsonResponse = unityWebRequest.downloadHandler.text;
				print($"jsonResponse = {jsonResponse}");

				// The response will be in Json format
				// therefore it needs to be deserialized into the classes AnalysedObject and TagData
				JsonDataOfOCR.Root analysedObject;// = new AnalysedObject();
				analysedObject = JsonUtility.FromJson<JsonDataOfOCR.Root>(jsonResponse);

				if (analysedObject.Regions == null)
				{
					Debug.Log("analysedObject.Regions is null");
				}
				else
				{
					string sFullText = "";
					foreach (var region in analysedObject.Regions)
					{
						foreach (var line in region.Lines)
						{
							foreach (var word in line.Words)
							{
								sFullText += word.Text + " ";
							}
						}
					}
					print($"sFullText = {sFullText}");
				}*/
			}
			catch (Exception exception)
			{
				Debug.Log("Json exception.Message: " + exception.Message);
			}

			yield return null;
		}
	}
	public IEnumerator AnalyseResult(string url)
	{
		yield return new WaitForSeconds(1f);
		//GET RESULTS
		using (UnityWebRequest unityWebGetRequest = UnityWebRequest.Get(url))
		{
			unityWebGetRequest.SetRequestHeader(ocpApimSubscriptionKeyHeader, authorizationKey);
			yield return unityWebGetRequest.SendWebRequest();

			if (unityWebGetRequest.isNetworkError || unityWebGetRequest.isHttpError)
			{
				Debug.Log(unityWebGetRequest.error);
			}
			else
			{
				try
				{
					print(unityWebGetRequest.downloadHandler.text);
					JsonDataOfCVR.Root analysedQuery = JsonConvert.DeserializeObject<JsonDataOfCVR.Root>(unityWebGetRequest.downloadHandler.text);

					AnalyseResponseElements(analysedQuery);
				}
				catch (Exception exception)
				{
					Debug.Log("GET Request Exception Message: " + exception.Message);
				}
			}
			yield return null;
		}
	}
	private void AnalyseResponseElements(JsonDataOfCVR.Root aQuery)
	{
		string sFullText = "";
		if (aQuery.Status == "succeeded")
		{
			foreach (var results in aQuery.AnalyzeResult.ReadResults)
			{
				foreach (var line in results.Lines)
				{
					foreach (var word in line.Words)
					{
						sFullText += word.Text + " ";
					}
				}
			}
		}
		else
		{
			StartCoroutine(AnalyseResult(sResponseHeader));
			return;
		}

		print($"sFullText (Text Détécté) = {sFullText}");
		sFullText = sFullText.Remove(sFullText.IndexOf(' '));
		GameManager gmGameManager = GameManager.getInstance();
		if (sFullText.ToLower() == gmGameManager.egEnigmeGenerator.GetTrueStringCode()) // EN CAS DE BON CODE
		{
			gmGameManager.TheEnd();
		}
		else // EN CAS DE MAUVAIS CODE
		{
			gmGameManager.tiTextInformation.SetText("Détecté: " + sFullText + " => Mauvais Code... ");
			gmGameManager.dDrawing.DeleteDraw();
		}
	}

	public void TryCode()
	{
		StartCoroutine(AnalyseLastImageCaptured());
	}
	/*public KeyCode screenshotKey;

	private string ApiKey = "2c54a2e263e647f39efdc66b5a838fa2";
	//private string emotionURL = "https://francecentral.api.cognitive.microsoft.com/vision/v3.0/ocr?language=unk&detectOrientation=true";
	private string emotionURL = "https://francecentral.api.cognitive.microsoft.com/vision/v3.1/read/analyze?language=fr";

	public CameraCapture ccCameraCapture;

	string responseData;

	public static ComputerVisionManager lmInstance;
	private void Awake()
	{
		// allows this class instance to behave like a singleton
		lmInstance = this;
	}

	private void Update()
	{
		if (Input.GetKeyDown(screenshotKey))
		{
			getData();
		}
	}

	public void getData()
	{
		StartCoroutine(GetDataFromImages());
	}

	IEnumerator GetDataFromImages()
	{
		byte[] bytes = ccCameraCapture.Capture();

		var headers = new Dictionary<string, string>() {
			{ "Ocp-Apim-Subscription-Key", ApiKey },
			{ "Content-Type", "application/octet-stream" }
		};

		WWW www = new WWW(emotionURL, bytes, headers);

		yield return www;
		responseData = www.text;

		//JsonDataOfOCR.Root analysedQuery = JsonConvert.DeserializeObject<JsonDataOfOCR.Root>(responseData);

		//AnalyseResponseElements(analysedQuery);

		print($"ResponseData: {responseData}");
	}

	private void AnalyseResponseElements(JsonDataOfOCR.Root aQuery)
	{
		string sFullText = "";
		foreach (var region in aQuery.Regions)
		{
			foreach (var line in region.Lines)
			{
				foreach (var word in line.Words)
				{
					sFullText += word.Text + " ";
				}
			}
		}
		GameManager.getInstance().tiTextInformation.SetText(sFullText);
	}*/
}