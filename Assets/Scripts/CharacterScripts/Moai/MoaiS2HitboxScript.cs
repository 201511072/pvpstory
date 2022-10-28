using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoaiS2HitboxScript : MonoBehaviour
{
    public MoaiScript moaiScript;
    public StatScript statScript;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript EnemyStatScript = col.GetComponent<StatScript>();
            if (EnemyStatScript.BlueTeam != moaiScript.statScript.BlueTeam && !EnemyStatScript.isInvincible)
            {
                if (!EnemyStatScript.MoaiS2Hit)
                {
                    StartCoroutine(EnemyStatScript.For1HitByMoaiS2());
                    EnemyStatScript.PV.RPC("DontChangeGravityRPC", RpcTarget.All, 0.2f);
                    EnemyStatScript.GetDamage(statScript.ATK * 0.5f + 47f, 0f, 0f, 0f, 0f, 0.3f, statScript.myPlayerNumber);
                    EnemyStatScript.HitByMoaiS2(moaiScript.transform.position);
                    if (!statScript.character_Base.Ulting)
                    {
                        moaiScript.UltGauge += 10f;
                    }
                }
            }
        }

        else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != moaiScript.statScript.BlueTeam)
        {
            GetDamageScript tempGetDamageScript = col.GetComponent<GetDamageScript>();
            if (!tempGetDamageScript.MoaiS2Hit)
            {
                StartCoroutine(tempGetDamageScript.For1HitByMoaiS2());
                tempGetDamageScript.GetDamage(statScript.ATK * 0.5f + 47f, 0f, 0f, 0f, 0f, 0.3f, statScript.myPlayerNumber);
                tempGetDamageScript.HitByMoaiS2(moaiScript.transform.position);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") && col.GetComponent<StatScript>().BlueTeam != moaiScript.statScript.BlueTeam)
        {
            col.GetComponent<StatScript>().PV.RPC("MoaiS2PullOffRPC", RpcTarget.All);
        }

        else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != moaiScript.statScript.BlueTeam)
        {
            col.GetComponent<GetDamageScript>().PV.RPC("MoaiS2PullOffRPC", RpcTarget.All);
        }
    }
}
