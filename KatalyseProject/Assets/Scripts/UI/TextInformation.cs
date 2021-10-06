using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInformation : MonoBehaviour
{
    Text tText;
    private void Start()
    {
        tText = GetComponent<Text>();
        GameManager.getInstance().tiTextInformation = this;
    }

    public void SetText(string newText)
    {
        tText.text = newText;
    }

    public void SetColor(Color color)
    {
        tText.color = color;
    }
}
