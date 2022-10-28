using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Lux_AttackExplosion : MonoBehaviourPunCallbacks
{
    bool isBlueTeam;
    StatScript statScript;
    public CircleCollider2D circleCollider2D;

    private void Start()
    {
        StartCoroutine(doDestroyCRT());
    }

    public void Init(bool isBlueTeam, StatScript statScript)
    {
        this.statScript = statScript;
        this.isBlueTeam = isBlueTeam;
        circleCollider2D.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if (tempStatScript == statScript)
            {
                col.GetComponent<PhotonView>().RPC("OnLuxBuff", RpcTarget.All);
                statScript.character_Base.UltGauge += 1f;
            }
            else if(tempStatScript.BlueTeam == isBlueTeam)
            {
                col.GetComponent<PhotonView>().RPC("OnLuxBuff", RpcTarget.All);
                statScript.character_Base.UltGauge += 2f;
            }
            else if (!tempStatScript.isInvincible)
            {
                tempStatScript.GetDamage(statScript.ATK * 0.3f + 20f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                statScript.character_Base.UltGauge += 4f;
            }
        }

        if (col.CompareTag("Creature"))
        {
            if (col.GetComponent<GetDamageScript>().BlueTeam != isBlueTeam)
            {
                col.GetComponent<GetDamageScript>().GetDamage(statScript.ATK * 0.3f + 20f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
            }
            else if (col.GetComponent<GetDamageScript>().BlueTeam == isBlueTeam)
            {
                col.GetComponent<PhotonView>().RPC("OnLuxBuff", RpcTarget.All);
            }
        }
    }

    IEnumerator doDestroyCRT()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
