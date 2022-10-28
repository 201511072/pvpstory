using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class S2BtnScript : MonoBehaviourPun
{
    protected GameObject Player;
    protected Character_Base character;
    public CooltimeScript cooltimeScript;
    public StatScript statScript;

    public virtual void Init()
    {
        NetworkManager.instance.OnExitRoom.AddListener(SelfDestroy);
        foreach (GameObject FindMyPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (FindMyPlayer.GetComponent<PhotonView>().IsMine)
            {
                Player = FindMyPlayer;
                character = Player.GetComponent<Character_Base>();
                statScript = Player.GetComponent<StatScript>();
            }
        }
    }

    public virtual void OnClickButton(Vector2 skillJoystick)
    {
        if (character != null)
        {
            if (!cooltimeScript.GetCooltime && !statScript.SkillLock && !statScript.StunSkillLock)
            {
                character.skillJoystick = skillJoystick;
                cooltimeScript.GetCooltime = true;
                cooltimeScript.Cooltimer = character.Skill2Delay;
                cooltimeScript.CooltimerCount = character.Skill2Delay;
                character.Skill2();
            }
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
