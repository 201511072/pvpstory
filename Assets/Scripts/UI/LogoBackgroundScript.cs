using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoBackgroundScript : MonoBehaviour
{
    public Image LogoBackground;
    float timeValue=1f;
    float controller= -0.3333f;

    void Update()
    {
        if(timeValue <= 0f)
        {
            controller = 0.6666f;
        }
        else if (timeValue >= 1f)
        {
            controller = -0.6666f;
        }

        timeValue += Time.deltaTime * controller;

        LogoBackground.color = new Color(0f, 0f, 0f, Mathf.Lerp(181f, 99f, timeValue)/255f);
    }
}
