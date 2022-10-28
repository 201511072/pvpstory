using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerS1Btn : S1BtnScript
{
    public override void OnClickButton(Vector2 skillJoystick)
    {
        if (cooltimeScript != null)
        {
            if (!cooltimeScript.GetCooltime && !statScript.SkillLock && !statScript.StunSkillLock && !statScript.ArcherHurricaneHit)
            {
                character.skillJoystick = skillJoystick;
                cooltimeScript.GetCooltime = true;
                cooltimeScript.Cooltimer = character.Skill1Delay;
                cooltimeScript.CooltimerCount = character.Skill1Delay;

                character.Skill1();
            }
        }
    }
}
