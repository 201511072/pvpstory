using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerS2EffectScript : MonoBehaviour
{
    public BladerScript bladerScript;
    public StatScript statScript;
    public BladerS2EffectObjectPool bladerS2EffectObjectPool;
    public SpriteRenderer SR;
    public Animator AN;


    [PunRPC]
    void InitRPC()
    {
        Init();
    }

    public void Init()
    {
        bladerScript = GameObject.Find("Blader(Clone)").GetComponent<BladerScript>();
        statScript = bladerScript.statScript;
        bladerS2EffectObjectPool = bladerScript.bladerS2EffectObjectPool;
        transform.SetParent(bladerS2EffectObjectPool.transform);
        bladerS2EffectObjectPool.poolingObjectQueue.Enqueue(this);
    }


    public IEnumerator ReturnCRT()
    {
        yield return new WaitForSeconds(0.26667f);
        bladerS2EffectObjectPool.ReturnObject(this);
    }
}
