using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UltBtnScript : MonoBehaviourPun
{
    public GameObject Player;
    public Character_Base character;
    public UltCooltimeScript ultCooltimeScript;
    public StatScript statScript;

    public virtual void Init()
    {
        NetworkManager.instance.OnExitRoom.AddListener(SelfDestroy);
        NetworkManager.instance.UltCool = ultCooltimeScript;
        foreach (GameObject FindMyPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (FindMyPlayer.GetComponent<PhotonView>().IsMine)
            {
                Player = FindMyPlayer;
                character = Player.GetComponent<Character_Base>();
                statScript = Player.GetComponent<StatScript>();
                ultCooltimeScript.character = character;
                ultCooltimeScript.GetCooltime = true;
            }
        }
    }

    public void OnClickButton(Vector2 skillJoystick)
    {
        if (character != null)
        {
            if (!ultCooltimeScript.GetCooltime && !statScript.SkillLock && !statScript.StunSkillLock)
            {
                if (statScript.CanUltOnAir)
                {
                    character.skillJoystick = skillJoystick;
                    ultCooltimeScript.GetCooltime = true;
                    character.UltGauge = 0f;
                    character.Ult();
                }
                else if (character.isGround)
                {
                    character.skillJoystick = skillJoystick;
                    ultCooltimeScript.GetCooltime = true;
                    character.UltGauge = 0f;
                    character.Ult();
                }
            }
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
