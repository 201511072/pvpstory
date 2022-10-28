using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoaiS1HitboxScript : MonoBehaviour
{
    public MoaiScript moaiScript;
    public StatScript statScript;
    public bool isBlueTeam;
    public MoaiS1ObjectPool moaiS1ObjectPool;
    public SpriteRenderer SR;
    public Animator AN;
    public BoxCollider2D hitbox;
    public SpriteRenderer MoaiSkill1EffectSR;

    public void Init()
    {
        moaiScript = GameObject.Find("Moai(Clone)").GetComponent<MoaiScript>();
        statScript = moaiScript.statScript;
        isBlueTeam = statScript.BlueTeam;
        moaiS1ObjectPool = moaiScript.moaiS1ObjectPool;
        transform.SetParent(moaiS1ObjectPool.transform);
        moaiS1ObjectPool.poolingObjectQueue.Enqueue(this);
    }

    [PunRPC]
    void InitRPC()
    {
        Init();
    }

    public IEnumerator HitboxCRT()
    {
        hitbox.enabled = true;
        yield return new WaitForSeconds(0.2f);
        hitbox.enabled = false;
    }


    public void Skill1EffectEnd()
    {
        moaiS1ObjectPool.ReturnObject(this);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if (tempStatScript.BlueTeam != isBlueTeam && !tempStatScript.isInvincible)
            {
                if (!tempStatScript.MoaiS1Hit)
                {
                    StartCoroutine(tempStatScript.For1HitByMoaiS1());
                    tempStatScript.PV.RPC("MoaiSkill1RPC", RpcTarget.All);
                    tempStatScript.GetDamage(statScript.ATK * 0.5f + 162f, 0f, 0f, 0f, 0f, 0f, statScript.myPlayerNumber);
                    if (!statScript.character_Base.Ulting)
                    {
                        statScript.character_Base.UltGauge += 14f;
                    }
                }
            }
        }

        else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != statScript.BlueTeam)
        {
            GetDamageScript tempGetDamageScript = col.GetComponent<GetDamageScript>();
            if (!tempGetDamageScript.MoaiS1Hit)
            {
                StartCoroutine(tempGetDamageScript.For1HitByMoaiS1());
                tempGetDamageScript.PV.RPC("MoaiSkill1RPC", RpcTarget.All);
                tempGetDamageScript.GetDamage(statScript.ATK * 0.5f + 162f, 0f, 0f, 0f, 0f, 0f, statScript.myPlayerNumber);
            }
        }
    }
}
