using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MoaiAttackRangeScript : MonoBehaviourPunCallbacks
{
    public BoxCollider2D RightRange;
    public BoxCollider2D LeftRange;
    public bool isRight;
    public StatScript statScript;
    public bool isBlueTeam;
    public bool Hit1Time;
    public int attack_Number;

    public void onAttack(bool isRight, bool isBlueTeam, int attack_Number)
    {
        this.isBlueTeam = isBlueTeam;
        this.attack_Number = attack_Number;
        if (isRight)
        {
            StartCoroutine(RightAttack());
        }
        else if (!isRight)
        {
            StartCoroutine(LeftAttack());
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if(tempStatScript.BlueTeam != isBlueTeam && !tempStatScript.isInvincible)
            {
                if (!tempStatScript.MoaiAttackHit)
                {
                    StartCoroutine(tempStatScript.For1HitByMoaiAttack());
                    tempStatScript.GetDamage(statScript.ATK * 0.5f + 136f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                    if (!statScript.character_Base.Ulting)
                    {
                        statScript.character_Base.UltGauge += 6f;
                    }

                    if (attack_Number == 0) NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)col.transform.position, 12);
                    else if (attack_Number == 1) NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)col.transform.position, 13);
                }
            }
        }

        else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != isBlueTeam)
        {
            GetDamageScript tempGetDamageScript = col.GetComponent<GetDamageScript>();
            if (!tempGetDamageScript.MoaiAttackHit)
            {
                StartCoroutine(tempGetDamageScript.For1HitByMoaiAttack());
                tempGetDamageScript.GetDamage(statScript.ATK * 0.5f + 136f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
            }
        }
    }

    public IEnumerator RightAttack()
    {
        RightRange.enabled = true;
        statScript.MoveLock = true;
        statScript.RB.velocity = new Vector2(0f, statScript.RB.velocity.y);
        yield return new WaitForSeconds(0.2f);
        statScript.MoveLock = false; RightRange.enabled = false;
    }

    public IEnumerator LeftAttack()
    {
        LeftRange.enabled = true;
        statScript.MoveLock = true;
        statScript.RB.velocity = new Vector2(0f, statScript.RB.velocity.y);
        yield return new WaitForSeconds(0.2f);
        statScript.MoveLock = false;
        LeftRange.enabled = false;
    }
}
