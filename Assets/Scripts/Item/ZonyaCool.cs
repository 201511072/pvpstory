using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZonyaCool : MonoBehaviour
{
    public Image CooltimeImage;
    public bool GetCooltime;
    public float Cooltimer;
    public float CooltimerCount;

    public RectTransform BtnRT;
    public RectTransform CooltimeRT;

    private void Start()
    {
        CooltimeImage = GetComponent<Image>();
        CooltimeRT.sizeDelta = BtnRT.sizeDelta;
    }

    private void Update()
    {
        if (GetCooltime)
        {
            CooltimerCount += -Time.deltaTime;
            CooltimeImage.fillAmount = 1.0f * CooltimerCount / Cooltimer;

            if (CooltimerCount <= 0.0f)
            {
                GetCooltime = false;
            }
        }
    }
}
