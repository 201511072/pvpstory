using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerS2EffectObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<BladerS2EffectScript> poolingObjectQueue = new Queue<BladerS2EffectScript>();


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
            PhotonNetwork.Instantiate("BladerS2Effect", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public BladerS2EffectScript GetObject(Vector2 position, bool flipX, int random)
    {
        BladerS2EffectScript obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = position;
        obj.SR.flipX = flipX;
        obj.AN.enabled = true;
        RandomMotion(obj, random);
        obj.SR.enabled = true;
        StartCoroutine(obj.ReturnCRT());

        return obj;
    }

    public void ReturnObject(BladerS2EffectScript obj)
    {
        obj.SR.enabled = false;
        obj.AN.enabled = false;
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }

    public void RandomMotion(BladerS2EffectScript obj,int random)
    {
        if (random == 0)
        {
            obj.AN.SetTrigger("motion0");
        }
        else if (random == 1)
        {
            obj.AN.SetTrigger("motion1");
        }
        else if (random == 2)
        {
            obj.AN.SetTrigger("motion2");
        }
        else if (random == 3)
        {
            obj.AN.SetTrigger("motion3");
        }
    }
}