using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraUltScript : MonoBehaviour
{
    TaraScript taraScript;
    StatScript statScript;
    TaraUltObjectPool taraUltObjectPool;

    public SpriteRenderer SR;
    public Animator AN;
    public BoxCollider2D Trigger;
    public PhotonView PV;
    public bool isBlueTeam;
    public bool startMove;

    public int ultID = Animator.StringToHash("ult");

    public AudioSource AS;


    public void Init()
    {
        taraScript = GameObject.Find("Tara(Clone)").GetComponent<TaraScript>();
        statScript = taraScript.statScript;
        taraUltObjectPool = taraScript.taraUltObjectPool;
        transform.SetParent(taraUltObjectPool.transform);
        taraUltObjectPool.poolingObjectQueue.Enqueue(this);
    }

    [PunRPC]
    void InitRPC()
    {
        Init();
    }


    private void Update()
    {
        if (startMove)
        {
            transform.Translate(Time.deltaTime * 3.5f, 0f, 0f);
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if(tempStatScript.BlueTeam != isBlueTeam && !tempStatScript.isInvincible)
            {
                if (!tempStatScript.TaraUlt1Hit)
                {
                    StartCoroutine(tempStatScript.For1HitByTaraUlt());
                    tempStatScript.GetDamage(statScript.ATK * 0.7f + 2100f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                    NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)col.transform.position, 21);
                }
            }
        }

        else if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != isBlueTeam)
        {
            GetDamageScript tempGetDamageScript = col.GetComponent<GetDamageScript>();
            if (!tempGetDamageScript.TaraUlt1Hit)
            {
                StartCoroutine(tempGetDamageScript.For1HitByTaraUlt());
                tempGetDamageScript.GetDamage(statScript.ATK * 0.7f + 2100f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)col.transform.position, 21);
            }
        }
    }

    public IEnumerator TriggerCRT()
    {
        yield return new WaitForSeconds(1.2f);
        if (PV.IsMine)
        {
            Trigger.enabled = true;
        }
        AN.SetBool("appear", false);
        AN.SetBool(ultID ,true);
        startMove = true;
    }

    public IEnumerator ReturnCRT()
    {
        yield return new WaitForSeconds(10f);
        AN.SetBool(ultID , false);
        taraUltObjectPool.ReturnObject(this);
    }
}