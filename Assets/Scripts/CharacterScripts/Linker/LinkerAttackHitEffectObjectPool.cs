using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LinkerAttackHitEffectObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<LinkerAttackHitEffectScript> poolingObjectQueue = new Queue<LinkerAttackHitEffectScript>();


    private void Awake()
    {
        if (statScript.PV.IsMine)
        {
            Initialize(7);
        }
    }


    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PhotonNetwork.Instantiate("LinkerAttackHitEffect", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public LinkerAttackHitEffectScript GetObject(Vector2 position, bool flipX)
    {
        LinkerAttackHitEffectScript obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = position;
        obj.SR.flipX = flipX;
        obj.AN.enabled = true;
        obj.AN.SetTrigger("attack");
        obj.SR.enabled = true;
        StartCoroutine(obj.ReturnCRT());
        NetworkManager.instance.soundObjectPool.GetObject((Vector2)obj.transform.position, 35);

        return obj;
    }

    public void ReturnObject(LinkerAttackHitEffectScript obj)
    {
        obj.SR.enabled = false;
        obj.AN.enabled = false;
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}