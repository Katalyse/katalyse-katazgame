using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float fTimer;
    Text tTimer;

    public bool isPause;
    void Start()
    {
        fTimer = 0;
        tTimer = GetComponent<Text>();
    }

    void Update()
    {
        if (!isPause)
        {
            fTimer += Time.deltaTime;
            tTimer.text = ((int)fTimer / 60).ToString() + "m " + ((int)fTimer % 60).ToString() + "s";
        }
    }
}
