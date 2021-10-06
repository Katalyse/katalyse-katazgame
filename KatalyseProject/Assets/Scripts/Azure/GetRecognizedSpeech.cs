using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GetRecognizedSpeech : MonoBehaviour
{
    [SerializeField] private Text ButtonText;

    private bool bStartRecognized;

    [DllImport("__Internal")]
    private static extern void RecognizedSpeech();

    public void StartOrStopRecognize()
    {
        bStartRecognized = !bStartRecognized;
        if (bStartRecognized)
        {
            ButtonText.text = "Stop Recognize";
        }
        else
        {
            ButtonText.text = "Start Recognize";
            GameManager.getInstance().goJavascriptHook.GetComponent<JavascriptHook>().SetStopImageInfo();
        }
        RecognizedSpeech();
    }
}
