using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraS1AttackScript : ProjectileScript
{
    public TaraScript taraScript;
    public StatScript statScript;
    public TaraS1AttackObjectPool taraS1AttackObjectPool;

    public CircleCollider2D attackTrigger;
    public Animator AN;
    public bool Hit1time;
    public bool isEnabled;


    public void Init()
    {
        taraScript = GameObject.Find("Tara(Clone)").GetComponent<TaraScript>();
        statScript = taraScript.statScript;
        isBlueTeam = statScript.BlueTeam;
        taraS1AttackObjectPool = taraScript.taraS1AttackObjectPool;
        transform.SetParent(taraS1AttackObjectPool.transform);
        taraS1AttackObjectPool.poolingObjectQueue.Enqueue(this);
    }

    [PunRPC]
    void InitRPC()
    {
        Init();
    }


    private void Update()
    {
        if (isEnabled)
        {
            transform.Translate(3f*Time.deltaTime, 0f,0f);
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (PV.IsMine)
        {
            if (Hit1time)
            {
                if (col.CompareTag("Ground"))
                {
                    Hit1time = false;
                    PV.RPC("ReturnRPC", RpcTarget.All);
                }

                if (col.CompareTag("Player"))
                {
                    StatScript tempStatScript = col.GetComponent<StatScript>();
                    if (tempStatScript.BlueTeam != statScript.BlueTeam && !tempStatScript.DontHitByProjectile && !tempStatScript.isInvincible)
                    {
                        Hit1time = false;
                        col.GetComponent<StatScript>().GetDamage(statScript.ATK * 0.7f + 39f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                        if (!isParryinged)
                        {
                            if (!taraScript.Ulting)
                            {
                                taraScript.UltGauge += 1f;
                            }
                            PV.RPC("ReturnRPC", RpcTarget.All);
                        }
                        else
                        {
                            PV.RPC("ReturnRPC", RpcTarget.All);
                        }
                    }
                }

                if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != isBlueTeam)
                {
                    Hit1time = false;
                    col.GetComponent<GetDamageScript>().GetDamage(statScript.ATK * 0.7f + 39f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                    PV.RPC("ReturnRPC", RpcTarget.All);
                }
            }
        }
    }


    [PunRPC]
    void isParryingedRPC()
    {
        isBlueTeam = !isBlueTeam;
        isParryinged = true;
        attackTrigger.enabled = false;
        attackTrigger.enabled = true;
        RB.velocity *= -1f;
        transform.Rotate(0f, 0f, 180f);
    }

    [PunRPC]
    void ReturnRPC()
    {
        if (isEnabled)
        {
            isEnabled = false;
            attackTrigger.enabled = false;
            AN.SetBool("disappear", true);
            RB.velocity = Vector2.zero;
            StartCoroutine(DisappearCRT());
        }
    }

    public IEnumerator DisappearCRT()
    {        
        yield return new WaitForSeconds(0.2f);
        taraS1AttackObjectPool.ReturnObject(this);
    }
}
