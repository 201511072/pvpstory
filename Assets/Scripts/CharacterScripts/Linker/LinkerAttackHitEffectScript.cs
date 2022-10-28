using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LinkerAttackHitEffectScript : MonoBehaviour
{
    public LinkerScript linkerScript;
    public StatScript statScript;
    public LinkerAttackHitEffectObjectPool linkerAttackHitEffectObjectPool;
    public SpriteRenderer SR;
    public Animator AN;


    [PunRPC]
    void InitRPC()
    {
        Init();
    }

    public void Init()
    {
        linkerScript = GameObject.Find("Linker(Clone)").GetComponent<LinkerScript>();
        statScript = linkerScript.statScript;
        linkerAttackHitEffectObjectPool = linkerScript.LinkerAttackHitEffectObjectPool;
        transform.SetParent(linkerAttackHitEffectObjectPool.transform);
        linkerAttackHitEffectObjectPool.poolingObjectQueue.Enqueue(this);
    }


    public IEnumerator ReturnCRT()
    {
        yield return new WaitForSeconds(0.333f);
        linkerAttackHitEffectObjectPool.ReturnObject(this);
    }
}