using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerS1Script : MonoBehaviour
{
    public BladerScript bladerScript;
    public StatScript statScript;
    public BladerS1ObjectPool bladerS1ObjectPool;
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
        bladerS1ObjectPool = bladerScript.bladerS1ObjectPool;
        transform.SetParent(bladerS1ObjectPool.transform);
        bladerS1ObjectPool.poolingObjectQueue.Enqueue(this);
    }


    public IEnumerator ReturnCRT()
    {
        yield return new WaitForSeconds(0.3f);
        bladerS1ObjectPool.ReturnObject(this);
    }
}
