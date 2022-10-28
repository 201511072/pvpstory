using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltCooltimeScript : MonoBehaviour
{
    public Image CooltimeImage;
    public bool GetCooltime;
    public Character_Base character;
    public float GaugeMax=100f;

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
            CooltimeImage.fillAmount = (GaugeMax - character.UltGauge) / GaugeMax;

            if (GaugeMax - character.UltGauge <= 0f)
            {
                GetCooltime = false;
            }
        }
    }
}
