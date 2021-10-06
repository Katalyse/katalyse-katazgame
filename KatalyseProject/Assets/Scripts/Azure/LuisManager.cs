using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.Events;

public class LuisManager : MonoBehaviour
{
    private static LuisManager _instance;
    private PlayerMovement pmPlayerMovement;
    private PlayerActions paPlayerActions;

    private bool bCanTryCode = false;

    //Substitute the value of luis Endpoint with your own End Point
    string luisEndpoint = "https://test-ia-cognitive-service.cognitiveservices.azure.com/luis/prediction/v3.0/apps/08dc6c5a-edc4-4e63-94bd-02020bad0437/slots/production/predict?subscription-key=2c54a2e263e647f39efdc66b5a838fa2&verbose=true&show-all-intents=true&log=true&query=";

    public static LuisManager getInstance()
    {
        return _instance;
    }
    private void Awake()
    {
        // allows this class instance to behave like a singleton
        _instance = this;
    }
    private void Start()
    {
        pmPlayerMovement = GameManager.getInstance().goPlayer.GetComponentInChildren<PlayerMovement>();
        paPlayerActions = GameManager.getInstance().goPlayer.GetComponentInChildren<PlayerActions>();
    }

    /// <summary>
    /// Call LUIS to submit a dictation result.
    /// The done Action is called at the completion of the method.
    /// </summary>
    public IEnumerator SubmitRequestToLuis(string dictationResult)
    {
        string queryString = string.Concat(Uri.EscapeDataString(dictationResult));
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(luisEndpoint + queryString))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.Log(unityWebRequest.error);
            }
            else
            {
                JsonDataOfLUIS.Root analysedQuery = JsonConvert.DeserializeObject<JsonDataOfLUIS.Root>(unityWebRequest.downloadHandler.text);

                    //analyse the elements of the response 
                    AnalyseResponseElements(analysedQuery);
               
            }
            yield return null;
        }
    }
    private void AnalyseResponseElements(JsonDataOfLUIS.Root aQuery)
    {
        string topIntent = aQuery.Prediction.TopIntent;

        print($"aQuery.Prediction.TopIntent: {aQuery.Prediction.TopIntent}");
        switch (aQuery.Prediction.TopIntent)
        {
            case "Deplacement":
            {
                if (aQuery.Prediction.Entities.Direction != null)
                {
                    foreach (var item in aQuery.Prediction.Entities.Direction)
                    {
                        switch (item[0].ToString())
                        {
                            case "droite":
                            {
                                pmPlayerMovement.jhJavascriptHook.SetImageDirInfo(JavascriptHook.Direction.Right);
                                pmPlayerMovement.RotateTo(90);
                                pmPlayerMovement.setCanGoForward(true);
                                break;
                            }
                            case "gauche":
                            {
                                pmPlayerMovement.jhJavascriptHook.SetImageDirInfo(JavascriptHook.Direction.Left);
                                pmPlayerMovement.RotateTo(270);
                                pmPlayerMovement.setCanGoForward(true);
                                break;
                            }
                            case "haut":
                            {
                                pmPlayerMovement.jhJavascriptHook.SetImageDirInfo(JavascriptHook.Direction.Up);
                                pmPlayerMovement.setCanGoForward(true);
                                break;
                            }
                            case "bas":
                            {
                                pmPlayerMovement.jhJavascriptHook.SetImageDirInfo(JavascriptHook.Direction.Down);
                                pmPlayerMovement.RotateTo(180);
                                pmPlayerMovement.setCanGoForward(true);
                                break;
                            }
                            case "stop":
                            {
                                pmPlayerMovement.jhJavascriptHook.SetImageDirInfo(JavascriptHook.Direction.Stop);
                                pmPlayerMovement.setCanGoForward(false);
                                break;
                            }
                        }
                    }
                }
                if (aQuery.Prediction.Entities.Number != null)
                {
                    foreach (var item in aQuery.Prediction.Entities.Number)
                    {
                        pmPlayerMovement.SetMaxDistance(item);
                    }
                }
                else
                {
                    pmPlayerMovement.SetMaxDistance(-1);
                }
                break;
            }
            case "Décalage":
            {
                if (aQuery.Prediction.Entities.Objet != null)
                {
                    GameManager.Objects tmpObj = GetObjectsOfString(aQuery.Prediction.Entities.Objet[0][0]);
                    if (tmpObj == GameManager.Objects.tableau && paPlayerActions.ueAction.ContainsKey(GetObjectsOfString(aQuery.Prediction.Entities.Objet[0][0])))
                    {
                        paPlayerActions.StartEventWithKey(tmpObj);
                    }
                }
                break;
            }
            case "Ouvrir":
            {
                if (aQuery.Prediction.Entities.Objet != null)
                {
                    GameManager.Objects tmpObj = GetObjectsOfString(aQuery.Prediction.Entities.Objet[0][0]);
                    if ((tmpObj == GameManager.Objects.coffre || tmpObj == GameManager.Objects.porte) && paPlayerActions.ueAction.ContainsKey(GetObjectsOfString(aQuery.Prediction.Entities.Objet[0][0])))
                    {
                        paPlayerActions.StartEventWithKey(tmpObj);
                    }
                }
                break;
            }
            case "Code":
            {
                if (bCanTryCode)
                {
                    ComputerVisionManager.getInstance().TryCode();
                }
                break;
            }
            case "Vue":
            {
                GameManager.getInstance().cmCameraMovement.ChangeView();
                break;
            }
            case "None":
            {

                break;
            }
        }
    }

    GameManager.Objects GetObjectsOfString(string str)
    {
        GameManager.Objects myobj = GameManager.Objects.none;
        if (GameManager.Objects.porte.ToString() == str)
        {
            myobj = GameManager.Objects.porte;
        }
        else if (GameManager.Objects.coffre.ToString() == str)
        {
            myobj = GameManager.Objects.coffre;
        }
        else if (GameManager.Objects.tableau.ToString() == str)
        {
            myobj = GameManager.Objects.tableau;
        }
        return myobj;
    }

    public void setCanTryCode(bool b)
    {
        bCanTryCode = b;
    }
}
