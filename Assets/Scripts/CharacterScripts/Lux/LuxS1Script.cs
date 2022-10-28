using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LuxS1Script : MonoBehaviour
{
    public LuxScript luxScript;
    public StatScript statScript;
    public LuxS1ObjectPool luxS1ObjectPool;
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
        luxS1ObjectPool = luxScript.luxS1ObjectPool;
        transform.SetParent(luxS1ObjectPool.transform);
        luxS1ObjectPool.poolingObjectQueue.Enqueue(this);
    }


    private void Update()
    {
        if (statScript != null)
        {
            if (return1Time && (NetworkManager.instance.PausedDeltaTime() + time) >= 0.6f)
            {
                return1Time = false;
                luxS1ObjectPool.ReturnObject(this);
            }
            else if (return1Time)
            {
                time += Time.deltaTime;
            }
        }
    }


    public IEnumerator Skill1CRT()
    {
        yield return new WaitForSeconds(0.35f);
        col.enabled = true;
        yield return new WaitForSeconds(0.15f);
        col.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        //중간에 나가서 ReturnObject됐다면 데미지 안주기 위해서 넣음
        if (isEnabled)
        {
            if (col.CompareTag("Player"))
            {
                StatScript tempStatScript = col.GetComponent<StatScript>();
                if (tempStatScript.BlueTeam != statScript.BlueTeam && !tempStatScript.isInvincible && !tempStatScript.LuxS1Hit)
                {
                    StartCoroutine(tempStatScript.For1HitByLuxS1());
                    tempStatScript.GetDamage(statScript.ATK * 1f + 340f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                    luxScript.UltGauge += 20f;
                    NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)tempStatScript.transform.position, 34);
                }
            }

            else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != statScript.BlueTeam)
            {
                GetDamageScript tempGetDamageScript = col.GetComponent<GetDamageScript>();
                if (!tempGetDamageScript.LuxS1Hit)
                {
                    StartCoroutine(tempGetDamageScript.For1HitByLuxS1());
                    tempGetDamageScript.GetDamage(statScript.ATK * 1f + 340f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                    NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)tempGetDamageScript.transform.position, 34);
                }
            }
        }
    }
}
