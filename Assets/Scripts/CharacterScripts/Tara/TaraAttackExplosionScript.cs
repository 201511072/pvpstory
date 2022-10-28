using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraAttackExplosionScript : MonoBehaviour
{
    TaraScript taraScript;
    StatScript statScript;
    TaraAttackExplosionObjectPool taraAttackExplosionObjectPool;

    public SpriteRenderer SR;
    public CircleCollider2D explosionTrigger;
    public PhotonView PV;
    public Animator AN;

    public void Init()
    {
        taraScript = GameObject.Find("Tara(Clone)").GetComponent<TaraScript>();
        statScript = taraScript.statScript;
        taraAttackExplosionObjectPool = taraScript.taraAttackExplosionObjectPool;
        transform.SetParent(taraAttackExplosionObjectPool.transform);
        taraAttackExplosionObjectPool.poolingObjectQueue.Enqueue(this);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)col.transform.position, 16);
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if (tempStatScript.BlueTeam != statScript.BlueTeam && !tempStatScript.isInvincible)
            {
                if (!tempStatScript.TaraAttackHit)
                {
                    StartCoroutine(tempStatScript.For1HitByTaraAttack());
                    tempStatScript.GetDamage(statScript.ATK * 0.7f + 129f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                    if (!taraScript.Ulting)
                    {
                        taraScript.UltGauge += 5f;
                    }
                }
            }
        }
        
        else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != statScript.BlueTeam)
        {
            NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)col.transform.position, 16);
            GetDamageScript tempGetDamageScript = col.GetComponent<GetDamageScript>();
            if (!tempGetDamageScript.TaraAttackHit)
            {
                StartCoroutine(tempGetDamageScript.For1HitByTaraAttack());
                tempGetDamageScript.GetDamage(statScript.ATK * 0.7f + 129f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
            }
        }
    }

    public IEnumerator TriggerCRT()
    {
        yield return new WaitForSeconds(0.15f);
        explosionTrigger.enabled = true;
    }

    public IEnumerator ColCRT()
    {
        yield return new WaitForSeconds(0.3f);
        explosionTrigger.enabled = false;
        yield return new WaitForSeconds(0.1f);
        taraAttackExplosionObjectPool.ReturnObject(this);
    }

    [PunRPC]
    void InitRPC()
    {
        Init();
    }
}
