using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherS1CooltimeScript : CooltimeScript
{
    public int Skill1Count;
    public Image S1CountImage1;
    public Image S1CountImage2;
    public Image S1CountImage3;


    private void Start()
    {
        CooltimeImage = GetComponent<Image>();
        CooltimeRT.sizeDelta = BtnRT.sizeDelta;
    }

    private void Update()
    {
        if (Skill1Count < 3)
        {
            CooltimerCount += -Time.deltaTime;
            CooltimeImage.fillAmount = CooltimerCount / Cooltimer;

            if (CooltimerCount <= 0.0f)
            {
                ++Skill1Count;
                CooltimerCount = Cooltimer;
                if (Skill1Count == 1)
                {
                    S1CountImage1.enabled = true;
                }
                else if (Skill1Count == 2)
                {
                    S1CountImage2.enabled = true;
                }
                else if (Skill1Count == 3)
                {
                    S1CountImage3.enabled = true;
                }
            }
        }
    }
}
