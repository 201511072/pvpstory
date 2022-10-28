using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Lux_AttackBall : ProjectileScript
{
    public float speed;
    public StatScript statScript;
    public int count;
    public LuxAttackObjectPool luxAttackObjectPool;
    public CircleCollider2D circleCollider2D;
    public CircleCollider2D trigger;
    public bool canCount = true;
    public bool isExploded;
    public float time;
    public bool isEnabled;

    public Vector2 tempVelocity;
    public Animator AN;

    public void Init()
    {
        luxAttackObjectPool = GameObject.Find("LuxAttackObjectPool").GetComponent<LuxAttackObjectPool>();
        statScript = luxAttackObjectPool.statScript;
        isBlueTeam = luxAttackObjectPool.statScript.BlueTeam;
        transform.SetParent(luxAttackObjectPool.transform);
        luxAttackObjectPool.poolingObjectQueue.Enqueue(this);
    }


    private void Update()
    {
        if (isEnabled)
        {
            time += Time.deltaTime;
            if (time > 20f && !isExploded && statScript.PV.IsMine)
            {
                time = 0f;
                PV.RPC("AN_End_RPC", RpcTarget.All);
            }

            if (tempVelocity != RB.velocity)
            {
                tempVelocity = RB.velocity;
                SR.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(tempVelocity.y, tempVelocity.x));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            if (canCount)
            {
                canCount = false;
                count++;
                StartCoroutine(countCRT());
                if (count >= 3 && !isExploded)
                {
                    isExploded = true;
                    if (statScript.PV.IsMine)
                    {
                        PhotonNetwork.Instantiate("Lux_Ball_Explosion", transform.position, Quaternion.identity).GetComponent<Lux_AttackExplosion>().Init(isBlueTeam, statScript);
                        PV.RPC("AN_End_RPC", RpcTarget.All);
                    }
                }
                if (count < 3)
                {
                    NetworkManager.instance.soundObjectPool.GetObject(transform.position, 23);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (PV.IsMine)
        {
            if (count > 0 && !isExploded && col.CompareTag("Player") && col.GetComponent<StatScript>() == statScript)
            {
                isExploded = true;
                PhotonNetwork.Instantiate("Lux_Ball_Explosion", transform.position, Quaternion.identity).GetComponent<Lux_AttackExplosion>().Init(isBlueTeam, statScript);
                PV.RPC("AN_End_RPC", RpcTarget.All);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (statScript.PV.IsMine)
        {
            if (col.CompareTag("Player") && !isExploded)
            {
                StatScript tempStatScript = col.GetComponent<StatScript>();
                if(tempStatScript != statScript)
                {
                    if(tempStatScript.BlueTeam == isBlueTeam)
                    {
                        isExploded = true;
                        PhotonNetwork.Instantiate("Lux_Ball_Explosion", transform.position, Quaternion.identity).GetComponent<Lux_AttackExplosion>().Init(isBlueTeam, statScript);
                        PV.RPC("AN_End_RPC", RpcTarget.All);
                    }
                    else if(!tempStatScript.DontHitByProjectile&& !tempStatScript.isInvincible)
                    {
                        isExploded = true;
                        PhotonNetwork.Instantiate("Lux_Ball_Explosion", transform.position, Quaternion.identity).GetComponent<Lux_AttackExplosion>().Init(isBlueTeam, statScript);
                        PV.RPC("AN_End_RPC", RpcTarget.All);
                    }
                }
            }

            if (col.CompareTag("Creature") && !isExploded)
            {
                isExploded = true;
                PhotonNetwork.Instantiate("Lux_Ball_Explosion", transform.position, Quaternion.identity).GetComponent<Lux_AttackExplosion>().Init(isBlueTeam, statScript);
                PV.RPC("AN_End_RPC", RpcTarget.All);
            }
        }
    }




    IEnumerator countCRT()
    {
        yield return new WaitForSeconds(0.05f);
        canCount = true;
    }


    [PunRPC]
    public void isParryingedRPC()
    {
        isParryinged = true;
        isBlueTeam = !isBlueTeam;
        RB.velocity *= -1f;
    }

    [PunRPC]
    public void InitRPC()
    {
        Init();
    }

    [PunRPC]
    public void ReturnRPC()
    {
        luxAttackObjectPool.ReturnObject(this);
    }

    [PunRPC]
    public void AN_End_RPC()
    {
        NetworkManager.instance.soundObjectPool.GetObject(transform.position, 24);
        isExploded = true;
        circleCollider2D.enabled = false;
        trigger.enabled = false;
        RB.velocity = Vector2.zero;
        isEnabled = false;
        time = 0f;

        AN.SetBool("start", false);
        AN.SetBool("loop", false);
        AN.SetBool("end", false);
        AN.SetBool("end", true);

        StartCoroutine(ReturnCRT());
    }

    public IEnumerator ReturnCRT()
    {
        yield return new WaitForSeconds(0.2f);
        luxAttackObjectPool.ReturnObject(this);
    }

    public IEnumerator AN_Loop_CRT()
    {
        yield return new WaitForSeconds(0.26667f);
        if (AN.GetBool("start"))
        {
            AN.SetBool("start", false);
            AN.SetBool("loop", false);
            AN.SetBool("end", false);
            AN.SetBool("loop", true);
        }
    }
}
