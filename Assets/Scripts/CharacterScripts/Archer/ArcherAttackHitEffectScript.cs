using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArcherAttackHitEffectScript : MonoBehaviour
{
    public ArcherScript archerScript;
    public ArcherAttackHitEffectObjectPool archerAttackHitEffectObjectPool;

    public SpriteRenderer SR;
    public Animator AN;
    public PhotonView PV;

    public void Init()
    {
        archerScript = GameObject.Find("Archer(Clone)").GetComponent<ArcherScript>();
        archerAttackHitEffectObjectPool = archerScript.archerAttackHitEffectObjectPool;
        transform.SetParent(archerAttackHitEffectObjectPool.transform);
        archerAttackHitEffectObjectPool.poolingObjectQueue.Enqueue(this);
    }

    [PunRPC]
    void InitRPC()
    {
        Init();
    }

    public IEnumerator ReturnCRT()
    {
        yield return new WaitForSeconds(0.47f);
        archerAttackHitEffectObjectPool.ReturnObject(this);
    }
}