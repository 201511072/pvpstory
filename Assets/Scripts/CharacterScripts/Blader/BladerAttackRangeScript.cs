using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerAttackRangeScript : MonoBehaviour
{
    public BoxCollider2D RightRange;
    public BoxCollider2D LeftRange;
    public StatScript statScript;
    public bool isBlueTeam;

    public void onAttack(bool isRight, bool isBlueTeam)
    {
        this.isBlueTeam = isBlueTeam;

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
                if (!tempStatScript.BladerAttack1Hit)
                {
                    StartCoroutine(tempStatScript.For1HitByBladerAttack());
                    tempStatScript.GetDamage(statScript.ATK * 0.9f + 225f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                    statScript.character_Base.UltGauge += 6f;
                    NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)transform.position, 10);
                }
            }
        }

        else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != isBlueTeam)
        {
            GetDamageScript tempGetDamageScript = col.GetComponent<GetDamageScript>();
            if (!tempGetDamageScript.BladerAttack1Hit)
            {
                StartCoroutine(tempGetDamageScript.For1HitByBladerAttack());
                tempGetDamageScript.GetDamage(statScript.ATK * 0.9f + 225f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)transform.position, 10);
            }
        }
    }

    public IEnumerator RightAttack()
    {
        statScript.MoveLock = true;
        statScript.JumpLock = true;
        statScript.RB.velocity = new Vector2(0f, statScript.RB.velocity.y);
        yield return new WaitForSeconds(0.05f);
        RightRange.enabled = true;
        yield return new WaitForSeconds(0.35f);
        statScript.MoveLock = false;
        statScript.JumpLock = false;
        RightRange.enabled = false;
    }

    public IEnumerator LeftAttack()
    {
        statScript.MoveLock = true;
        statScript.JumpLock = true;
        statScript.RB.velocity = new Vector2(0f, statScript.RB.velocity.y);
        yield return new WaitForSeconds(0.05f);
        LeftRange.enabled = true;
        yield return new WaitForSeconds(0.35f);
        statScript.MoveLock = false;
        statScript.JumpLock = false;
        LeftRange.enabled = false;
    }
}
