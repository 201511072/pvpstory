using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LuxUltScript : MonoBehaviour
{
    public LuxScript luxScript;
    public StatScript statScript;
    public LuxUltObjectPool luxUltObjectPool;
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
        luxScript = GameObject.Find("Lux(Clone)").GetComponent<LuxScript>();
        statScript = luxScript.statScript;
        luxUltObjectPool = luxScript.luxUltObjectPool;
        transform.SetParent(luxUltObjectPool.transform);
        luxUltObjectPool.poolingObjectQueue.Enqueue(this);
    }


    private void Update()
    {
        if (statScript != null)
        {
            if (return1Time && (NetworkManager.instance.PausedDeltaTime() + time) >= 1.5f)
            {
                return1Time = false;
                luxUltObjectPool.ReturnObject(this);
            }
            else if (return1Time)
            {
                time += Time.deltaTime;
            }
        }
    }


    public IEnumerator UltCRT()
    {
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
        yield return new WaitForSeconds(0.9f);
        col.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if (tempStatScript.BlueTeam != statScript.BlueTeam && !tempStatScript.isInvincible && !tempStatScript.LuxUltHit)
            {
                StartCoroutine(tempStatScript.For1HitByLuxUlt());
                tempStatScript.GetDamage(statScript.ATK * 1.5f + 840f, 0f, 0f, 0f, 0f, 0.2f, statScript.myPlayerNumber);
                NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)col.transform.position, 25);
            }
        }

        else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != statScript.BlueTeam)
        {
            GetDamageScript tempGetDamageScript = col.GetComponent<GetDamageScript>();
            if (!tempGetDamageScript.LuxUltHit)
            {
                StartCoroutine(tempGetDamageScript.For1HitByLuxUlt());
                tempGetDamageScript.GetDamage(statScript.ATK * 1.5f + 840f, 0f, 0f, 0f, 0f, 0.2f, statScript.myPlayerNumber);
                NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)col.transform.position, 25);
            }
        }
    }
}
