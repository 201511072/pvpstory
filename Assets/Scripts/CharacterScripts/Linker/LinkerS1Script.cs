using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LinkerS1Script : ProjectileScript
{
    public Collider2D col;
    public LinkerS1ObjectPool linkerS1ObjectPool;//인스펙터창에서 값 비워둬야함
    public bool isEnable;
    public float time;
    public Animator AN;

    public bool returnRPC1Time;


    public void Init()
    {
        linkerS1ObjectPool = GameObject.Find("LinkerS1ObjectPool").GetComponent<LinkerS1ObjectPool>();
        transform.SetParent(linkerS1ObjectPool.transform);
        linkerS1ObjectPool.poolingObjectQueue.Enqueue(this);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (PV.IsMine)
        {
            if (col.CompareTag("Player"))
            {
                StatScript tempStatScript = col.GetComponent<StatScript>();
                if (tempStatScript.BlueTeam != isBlueTeam && !tempStatScript.isInvincible && !tempStatScript.DontHitByProjectile)
                {
                    tempStatScript.linkerS1CircleScript.pullTeam1Time = true;
                    tempStatScript.linkerS1CircleScript.linkerClient = true;
                    tempStatScript.linkerS1CircleScript.circleCollider2D.enabled = true;
                    tempStatScript.linkerS1CircleScript.PV.RPC("ActiveRPC", RpcTarget.All, true);
                    linkerS1ObjectPool.statScript.character_Base.UltGauge += 10f;
                    PV.RPC("ReturnRPC", RpcTarget.All);
                }
            }
            else if (col.CompareTag("Ground"))
            {
                PV.RPC("ReturnRPC", RpcTarget.All);
            }
            else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != isBlueTeam)
            {
                PV.RPC("ReturnRPC", RpcTarget.All);
            }
        }
    }


    public IEnumerator EndCRT()
    {
        col.enabled = false;
        RB.velocity = Vector2.zero;
        AN.SetBool("skill1", false);
        AN.SetBool("end", true);
        yield return new WaitForSeconds(0.27f);
        AN.SetBool("end", false);
        linkerS1ObjectPool.ReturnObject(this);
    }

    private void Update()
    {
        if (PV.IsMine&&isEnable&&returnRPC1Time)
        {
            time += Time.deltaTime;
            if (time > 0.7f)
            {
                returnRPC1Time = false;
                PV.RPC("ReturnRPC", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void isParryingedRPC()
    {
        time = 0f;
        isParryinged = true;
        isBlueTeam = !isBlueTeam;
        RB.velocity *= -1f;
    }

    [PunRPC]
    void InitRPC()
    {
        Init();
    }

    [PunRPC]
    public void ReturnRPC()
    {
        StartCoroutine(EndCRT());
    }
}
