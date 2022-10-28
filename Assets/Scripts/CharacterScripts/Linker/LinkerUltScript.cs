using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LinkerUltScript : MonoBehaviour
{
    public LinkerScript linkerScript;
    public StatScript statScript;
    public LinkerUltObjectPool linkerUltObjectPool;
    public SpriteRenderer SR;
    public Animator AN;
    public BoxCollider2D col;

    public float time;
    public bool return1Time;
    public bool isEnabled;


    [PunRPC]
    void InitRPC()
    {
        Init();
    }

    public void Init()
    {
        linkerScript = GameObject.Find("Linker(Clone)").GetComponent<LinkerScript>();
        statScript = linkerScript.statScript;
        linkerUltObjectPool = linkerScript.linkerUltObjectPool;
        transform.SetParent(linkerUltObjectPool.transform);
        linkerUltObjectPool.poolingObjectQueue.Enqueue(this);
    }


    private void Update()
    {
        if (statScript != null)
        {
            if (return1Time && (NetworkManager.instance.PausedDeltaTime() + time) >= 1.3333f)
            {
                return1Time = false;
                linkerUltObjectPool.ReturnObject(this);
            }
            else if (return1Time)
            {
                time += Time.deltaTime;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if (tempStatScript.BlueTeam != statScript.BlueTeam && !tempStatScript.isInvincible&& !tempStatScript.LinkerUltHit)
            {
                StartCoroutine(tempStatScript.For1HitByLinkerUlt());
                col.GetComponent<StatScript>().PV.RPC("StunRPC", RpcTarget.All, 4f);
            }
        }

        else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != statScript.BlueTeam)
        {
            GetDamageScript tempGetDamageScript = col.GetComponent<GetDamageScript>();
            if (!tempGetDamageScript.LinkerUltHit)
            {
                StartCoroutine(tempGetDamageScript.For1HitByLinkerUlt());
                tempGetDamageScript.PV.RPC("StunRPC", RpcTarget.All, 4f);
            }
        }
    }
}


//public class LinkerUltScript : MonoBehaviour
//{
//    public StatScript statScript;
//    public BoxCollider2D Range;
//    public SpriteRenderer SR;
//    private float time;
//
//    private void OnEnable()
//    {
//        transform.localScale = Vector3.one;
//        time = 0f;
//    }
//
//    private void Update()
//    {
//        if (time <= 0.2f)
//        {
//            time += Time.deltaTime;
//            transform.localScale = new Vector3(Mathf.Lerp(0f, 10f, time * 5f), Mathf.Lerp(0f, 10f, time * 5f), 1f);
//        }
//    }
//
//    private void OnTriggerEnter2D(Collider2D col)
//    {
//        if (col.CompareTag("Player"))
//        {
//            StatScript tempStatScript = col.GetComponent<StatScript>();
//            if (tempStatScript.BlueTeam != statScript.BlueTeam && !tempStatScript.isInvincible&& !tempStatScript.LinkerUltHit)
//            {
//                StartCoroutine(tempStatScript.For1HitByLinkerUlt());
//                col.GetComponent<StatScript>().PV.RPC("StunRPC", RpcTarget.All, 4f);
//            }
//        }
//
//        else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != statScript.BlueTeam)
//        {
//            GetDamageScript tempGetDamageScript = col.GetComponent<GetDamageScript>();
//            if (!tempGetDamageScript.LinkerUltHit)
//            {
//                StartCoroutine(tempGetDamageScript.For1HitByLinkerUlt());
//                tempGetDamageScript.PV.RPC("StunRPC", RpcTarget.All, 4f);
//            }
//        }
//    }
//}
//