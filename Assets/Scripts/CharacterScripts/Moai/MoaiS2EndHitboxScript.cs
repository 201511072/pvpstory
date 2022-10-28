using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoaiS2EndHitboxScript : MonoBehaviour
{
    public StatScript statScript;
    public Collider2D S2EndHitbox;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if(tempStatScript.BlueTeam != statScript.BlueTeam && !tempStatScript.isInvincible)
            {
                if (!tempStatScript.MoaiS2End)
                {
                    StartCoroutine(tempStatScript.For1HitByMoaiS2End());
                    tempStatScript.GetDamage(statScript.ATK * 0.5f + 47f, 0f, 0f, 0f, 0f, 0.3f, statScript.myPlayerNumber);
                    if (!statScript.character_Base.Ulting)
                    {
                        statScript.character_Base.UltGauge += 10f;
                    }
                    tempStatScript.PV.RPC("StunRPC", RpcTarget.All, 1f);
                }
            }
        }

        if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != statScript.BlueTeam)
        {
            GetDamageScript tempGetDamageScript = col.GetComponent<GetDamageScript>();
            if (!tempGetDamageScript.MoaiS2End)
            {
                StartCoroutine(tempGetDamageScript.For1HitByMoaiS2End());
                tempGetDamageScript.GetDamage(statScript.ATK * 0.5f + 47f, 0f, 0f, 0f, 0f, 0.3f, statScript.myPlayerNumber);
                tempGetDamageScript.PV.RPC("StunRPC", RpcTarget.All, 1f);
            }
        }
    }

    public IEnumerator S2EndHitboxCRT()
    {
        S2EndHitbox.enabled = true;
        yield return new WaitForSeconds(0.2f);
        S2EndHitbox.enabled = false;
    }
}
