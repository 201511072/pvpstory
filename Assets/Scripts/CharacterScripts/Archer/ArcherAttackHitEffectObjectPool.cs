using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArcherAttackHitEffectObjectPool : MonoBehaviour
{
    public StatScript statScript;
    public ArcherScript archerScript;

    public Queue<ArcherAttackHitEffectScript> poolingObjectQueue = new Queue<ArcherAttackHitEffectScript>();


    private void Awake()
    {
        if (statScript.PV.IsMine)
        {
            Initialize(20);
        }
    }


    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PhotonNetwork.Instantiate("ArcherAttackHitEffect", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public ArcherAttackHitEffectScript GetObject(Vector2 archerPosition, bool flipX, float arrowPlusATK)
    {
        ArcherAttackHitEffectScript obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = archerPosition;

        obj.SR.flipX = flipX;
        if (arrowPlusATK == archerScript.arrowPlusATK[0])
        {
            obj.AN.SetTrigger("attackHitEffect");
        }
        else if (arrowPlusATK == archerScript.arrowPlusATK[1])
        {
            obj.AN.SetTrigger("enhanceAttackHitEffect");
        }
        obj.AN.enabled = true;
        obj.SR.enabled = true;
        StartCoroutine(obj.ReturnCRT());
        return obj;
    }

    public void ReturnObject(ArcherAttackHitEffectScript obj)
    {
        obj.SR.enabled = false;
        obj.AN.enabled = false;
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}
