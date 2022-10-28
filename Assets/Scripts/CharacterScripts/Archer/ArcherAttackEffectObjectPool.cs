using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArcherAttackEffectObjectPool : MonoBehaviour
{
    public StatScript statScript;
    public ArcherScript archerScript;

    public Queue<ArcherAttackEffectScript> poolingObjectQueue = new Queue<ArcherAttackEffectScript>();


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
            PhotonNetwork.Instantiate("ArcherAttackEffect", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public ArcherAttackEffectScript GetObject(Vector2 archerPosition, Vector2 tempVector, float arrowPlusATK)
    {
        ArcherAttackEffectScript obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = archerPosition + tempVector;
        obj.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(tempVector.y, tempVector.x));

        obj.SR.flipY = tempVector.x < 0;
        if (arrowPlusATK == archerScript.arrowPlusATK[0])
        {
            obj.AN.SetTrigger("attackEffect");
        }
        else if (arrowPlusATK == archerScript.arrowPlusATK[1])
        {
            obj.AN.SetTrigger("enhanceAttackEffect");
        }
        obj.AN.enabled = true;
        obj.SR.enabled = true;
        StartCoroutine(obj.ReturnCRT());
        return obj;
    }

    public void ReturnObject(ArcherAttackEffectScript obj)
    {
        obj.SR.enabled = false;
        obj.AN.enabled = false;
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}
