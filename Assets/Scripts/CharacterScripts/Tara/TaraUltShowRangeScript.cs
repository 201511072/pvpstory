using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraUltShowRangeScript : MonoBehaviour
{
    TaraScript taraScript;
    StatScript statScript;
    TaraUltShowRangeObjectPool taraUltShowRangeObjectPool;

    public SpriteRenderer SR1;
    public SpriteRenderer SR2;
    public PhotonView PV;

    public void Init()
    {
        taraScript = GameObject.Find("Tara(Clone)").GetComponent<TaraScript>();
        statScript = taraScript.statScript;
        taraUltShowRangeObjectPool = taraScript.taraUltShowRangeObjectPool;
        taraUltShowRangeObjectPool.poolingObjectQueue.Enqueue(this);
    }

    [PunRPC]
    void InitRPC()
    {
        Init();
    }




    public IEnumerator ReturnCRT()
    {
        yield return new WaitForSeconds(10f);
        taraScript.Ulting = false;
        taraUltShowRangeObjectPool.ReturnObject(this);
    }
}