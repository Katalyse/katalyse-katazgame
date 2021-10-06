using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class JavascriptHook : MonoBehaviour
{
    [SerializeField] private WordingMovement wmWordingMovement;
    [SerializeField] private Text tTextRecognizedSpeech;
    [SerializeField] private GameObject goImageResult;
    [SerializeField] private Sprite[] sDirSprites;

    private Image iImageResult;
    private RectTransform rtInfoDir;

    private string sRecognizedSpeech;
    public enum Direction
    {
        Up, Down, Left, Right, Stop
    };


    private void Start()
    {
        iImageResult = goImageResult.GetComponent<Image>();
        rtInfoDir = goImageResult.GetComponent<RectTransform>();
    }

    //Get Recognized Speech to String
    public void ReturnRecognizeSpeechText(string str)
    {
        sRecognizedSpeech = str;
        StartCoroutine(LuisManager.getInstance().SubmitRequestToLuis(str));
        SetRecognizeSpeechText();
    }

    //Set Output speech text in UI
    public void SetRecognizeSpeechText()
    {
        tTextRecognizedSpeech.text = sRecognizedSpeech;
    }

    public void SetImageDirInfo(Direction newdir)
    {
        print(newdir.ToString());
        if (newdir == Direction.Up)
        {
            iImageResult.sprite = sDirSprites[0];
            rtInfoDir.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (newdir == Direction.Down)
        {
            iImageResult.sprite = sDirSprites[0];
            rtInfoDir.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (newdir == Direction.Left)
        {
            iImageResult.sprite = sDirSprites[0];
            rtInfoDir.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (newdir == Direction.Right)
        {
            iImageResult.sprite = sDirSprites[0];
            rtInfoDir.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (newdir == Direction.Stop)
        {
            iImageResult.sprite = sDirSprites[1];
            rtInfoDir.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void SetStopImageInfo()
    {
        iImageResult.sprite = sDirSprites[2];
        rtInfoDir.rotation = Quaternion.Euler(0, 0, 0);
    }
}