using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooltimeScript : MonoBehaviour
{
    public Image CooltimeImage;
    public bool GetCooltime = false;
    public float Cooltimer;
    public float CooltimerCount;

    public RectTransform BtnRT;
    public RectTransform CooltimeRT;


    private void Start()
    {
        CooltimeImage = GetComponent<Image>();
        CooltimeRT.sizeDelta = BtnRT.sizeDelta;
        NetworkManager.instance.CooltimerCountZeroEvent.AddListener(CooltimerCountZero);
    }

    private void Update()
    {
        if (GetCooltime)
        {
            CooltimerCount += -Time.deltaTime;
            CooltimeImage.fillAmount = CooltimerCount / Cooltimer;

            if (CooltimerCount <= 0f)
            {
                GetCooltime = false;
            }
        }
    }

    public void CooltimerCountZero()
    {
        CooltimerCount = 0f;
    }
}
