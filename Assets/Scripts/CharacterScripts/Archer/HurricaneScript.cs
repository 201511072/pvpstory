using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HurricaneScript : MonoBehaviourPun
{
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public Animator AN;
    public AudioSource AS;
    public bool isBlueTeam;
    public PhotonView PV;
    public BoxCollider2D boxCollider2D;
    public BoxCollider2D trigger;
    public int dir;
    StatScript statScript;
    public bool isEnable;
    public HurricaneObjectPool hurricaneObjectPool;
    public float time;


    public void Init()
    {
        hurricaneObjectPool = GameObject.Find("HurricaneObjectPool").GetComponent<HurricaneObjectPool>();
        statScript = hurricaneObjectPool.statScript;
        RB.gravityScale = 0f;
        transform.SetParent(hurricaneObjectPool.transform);
        hurricaneObjectPool.poolingObjectQueue.Enqueue(this);
    }


    private void Update()
    {
        if (isEnable)
        {
            RB.velocity = new Vector2(dir * 7f, RB.velocity.y);

            if (PV.IsMine)
            {
                time += Time.deltaTime;
                if (time > 5f)
                {
                    PV.RPC("ReturnRPC", RpcTarget.All);
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if(tempStatScript.BlueTeam != isBlueTeam && !tempStatScript.isInvincible)
            {
                tempStatScript.PV.RPC("OnHurricaneRPC", RpcTarget.All);
                statScript.character_Base.UltGauge += 20f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") && col.GetComponent<StatScript>().BlueTeam != isBlueTeam)
        {
            col.GetComponent<StatScript>().PV.RPC("OffHurricaneRPC", RpcTarget.All);
        }
    }


    [PunRPC]
    void InitRPC()
    {
        Init();
    }

    [PunRPC]
    public void ReturnRPC()
    {
        hurricaneObjectPool.ReturnObject(this);
    }
}
