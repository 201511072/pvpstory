using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArcherS1Btn : S1BtnScript
{
    public ArcherS1CooltimeScript archerS1CooltimeScript;

    public override void Init()
    {
        NetworkManager.instance.OnExitRoom.AddListener(SelfDestroy);
        archerS1CooltimeScript.Skill1Count = 3;
        foreach (GameObject FindMyPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (FindMyPlayer.GetComponent<PhotonView>().IsMine)
            {
                Player = FindMyPlayer;
                character = Player.GetComponent<Character_Base>();
                statScript = Player.GetComponent<StatScript>();
            }
        }
        archerS1CooltimeScript.Cooltimer = character.Skill1Delay;
        archerS1CooltimeScript.CooltimerCount = character.Skill1Delay;
    }

    public override void OnClickButton(Vector2 skillJoystick)
    {
        if (character != null)
        {
            if (!archerS1CooltimeScript.GetCooltime && archerS1CooltimeScript.Skill1Count > 0 && !statScript.SkillLock && !statScript.StunSkillLock)
            {
                character.skillJoystick = skillJoystick;
                archerS1CooltimeScript.GetCooltime = true;
                --archerS1CooltimeScript.Skill1Count;

                if (archerS1CooltimeScript.Skill1Count == 2)
                {
                    archerS1CooltimeScript.S1CountImage3.enabled = false;
                }
                else if (archerS1CooltimeScript.Skill1Count == 1)
                {
                    archerS1CooltimeScript.S1CountImage2.enabled = false;
                }
                else if (archerS1CooltimeScript.Skill1Count == 0)
                {
                    archerS1CooltimeScript.S1CountImage1.enabled = false;
                }
                character.Skill1();
            }
        }
    }
}
