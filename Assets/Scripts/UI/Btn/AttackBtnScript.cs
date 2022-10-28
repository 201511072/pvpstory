using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackBtnScript : MonoBehaviourPun
{
    public GameObject Player;
    public Character_Base character;
    public CooltimeScript cooltimeScript;
    public StatScript statScript;

    public virtual void Init()
    {
        NetworkManager.instance.OnExitRoom.AddListener(SelfDestroy);
        NetworkManager.instance.AttackCool = cooltimeScript;
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
    
    public void OnClickButton(Vector2 skillJoystick)
    {
        if (character != null)
        {
            if (!cooltimeScript.GetCooltime && !statScript.SkillLock && !statScript.StunSkillLock)
            {
                character.skillJoystick = skillJoystick;
                cooltimeScript.GetCooltime = true;
                cooltimeScript.Cooltimer = character.AttackDelay;
                cooltimeScript.CooltimerCount = character.AttackDelay;
                character.Attack();
            }
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
