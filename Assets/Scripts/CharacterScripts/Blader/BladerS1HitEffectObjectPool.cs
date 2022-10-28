using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerS1HitEffectObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<BladerS1HitEffectScript> poolingObjectQueue = new Queue<BladerS1HitEffectScript>();


    private void Awake()
    {
        if (statScript.PV.IsMine)
        {
            Initialize(10);
        }
    }


    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PhotonNetwork.Instantiate("BladerS1HitEffect", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public BladerS1HitEffectScript GetObject(Vector2 position,bool flipX)
    {
        BladerS1HitEffectScript obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = position;
        obj.SR.flipX = flipX;
        obj.AN.enabled = true;
        obj.AN.SetTrigger("skill1");
        obj.SR.enabled = true;
        StartCoroutine(obj.ReturnCRT());

        return obj;
    }

    public void ReturnObject(BladerS1HitEffectScript obj)
    {
        obj.SR.enabled = false;
        obj.AN.enabled = false;
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}