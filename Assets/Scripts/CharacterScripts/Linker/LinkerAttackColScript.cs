using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LinkerAttackColScript : MonoBehaviour
{
    public LinkerScript LinkerScript;
    public StatScript statScript;
    public SpriteRenderer SR;
    public Animator AN;
    public BoxCollider2D col;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if (tempStatScript.BlueTeam != statScript.BlueTeam && !tempStatScript.isInvincible && !tempStatScript.LinkerAttackHit)
            {
                StartCoroutine(tempStatScript.For1HitByLinkerAttack());
                LinkerScript.PV.RPC("ANAttackHitEffectRPC", RpcTarget.All, (Vector2)tempStatScript.transform.position, LinkerScript.SR.flipX);
                tempStatScript.GetDamage(statScript.ATK * 0.7f + 102f, 0f, 0f, 0f, 0f, 0.3f, statScript.myPlayerNumber);
                LinkerScript.UltGauge += 4f;
            }
        }

        else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != statScript.BlueTeam)
        {
            GetDamageScript tempGetDamageScript = col.GetComponent<GetDamageScript>();
            if (!tempGetDamageScript.LinkerAttackHit)
            {
                StartCoroutine(tempGetDamageScript.For1HitByLinkerAttack());
                LinkerScript.PV.RPC("ANAttackHitEffectRPC", RpcTarget.All, (Vector2)tempGetDamageScript.transform.position, LinkerScript.SR.flipX);
                tempGetDamageScript.GetDamage(statScript.ATK * 0.7f + 102f, 0f, 0f, 0f, 0f, 0.3f, statScript.myPlayerNumber);
            }
        }
    }
}
