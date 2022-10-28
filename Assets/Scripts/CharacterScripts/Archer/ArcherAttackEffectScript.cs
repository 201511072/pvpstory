using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArcherAttackEffectScript : MonoBehaviour
{
    public ArcherScript archerScript;
    public ArcherAttackEffectObjectPool archerAttackEffectObjectPool;

    public SpriteRenderer SR;
    public Animator AN;
    public PhotonView PV;

    public void Init()
    {
        archerScript = GameObject.Find("Archer(Clone)").GetComponent<ArcherScript>();
        archerAttackEffectObjectPool = archerScript.archerAttackEffectObjectPool;
        transform.SetParent(archerAttackEffectObjectPool.transform);
        archerAttackEffectObjectPool.poolingObjectQueue.Enqueue(this);
    }

    [PunRPC]
    void InitRPC()
    {
        Init();
    }

    public IEnumerator ReturnCRT()
    {
        yield return new WaitForSeconds(0.47f);
        archerAttackEffectObjectPool.ReturnObject(this);
    }
}