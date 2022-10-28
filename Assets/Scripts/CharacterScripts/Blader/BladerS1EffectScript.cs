using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerS1EffectScript : MonoBehaviour
{
    public BladerScript bladerScript;
    public StatScript statScript;
    public BladerS1EffectObjectPool bladerS1EffectObjectPool;
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
        bladerS1EffectObjectPool = bladerScript.bladerS1EffectObjectPool;
        transform.SetParent(bladerS1EffectObjectPool.transform);
        bladerS1EffectObjectPool.poolingObjectQueue.Enqueue(this);
    } 


    public IEnumerator ReturnCRT()
    {
        yield return new WaitForSeconds(0.3f);
        bladerS1EffectObjectPool.ReturnObject(this);
    }
}