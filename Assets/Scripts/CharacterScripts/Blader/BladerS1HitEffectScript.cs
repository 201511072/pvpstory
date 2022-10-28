using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerS1HitEffectScript : MonoBehaviour
{
    public BladerScript bladerScript;
    public StatScript statScript;
    public BladerS1HitEffectObjectPool bladerS1HitEffectObjectPool;
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
        bladerS1HitEffectObjectPool = bladerScript.bladerS1HitEffectObjectPool;
        transform.SetParent(bladerS1HitEffectObjectPool.transform);
        bladerS1HitEffectObjectPool.poolingObjectQueue.Enqueue(this);
    }


    public IEnumerator ReturnCRT()
    {
        yield return new WaitForSeconds(0.3f);
        bladerS1HitEffectObjectPool.ReturnObject(this);
    }
}