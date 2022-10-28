using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Zonya : MonoBehaviourPunCallbacks
{
    public GameObject Player;
    public ZonyaCool zonyaCool;
    public StatScript statScript;
    public RectTransform RT;


    public void Init()
    {
        NetworkManager.instance.OnExitRoom.AddListener(SelfDestroy);
        foreach (GameObject FindMyPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (FindMyPlayer.GetComponent<PhotonView>().IsMine)
            {
                Player = FindMyPlayer;
                statScript = Player.GetComponent<StatScript>();
            }
        }
    }

    public void OnClickButton()
    {
        if (statScript != null)
        {
            if (!zonyaCool.GetCooltime && !statScript.SkillLock)
            {
                zonyaCool.GetCooltime = true;
                zonyaCool.Cooltimer = statScript.zonyaCool;
                zonyaCool.CooltimerCount = statScript.zonyaCool;
                statScript.PV.RPC("ZonyaRPC", RpcTarget.All);
            }
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}

