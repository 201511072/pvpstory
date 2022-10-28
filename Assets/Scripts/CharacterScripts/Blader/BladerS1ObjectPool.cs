using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerS1ObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<BladerS1Script> poolingObjectQueue = new Queue<BladerS1Script>();


    private void Awake()
    {
        if (statScript.PV.IsMine)
        {
            Initialize(5);
        }
    }


    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PhotonNetwork.Instantiate("BladerS1", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public BladerS1Script GetObject(float positionX, float positionY,bool flipX)
    {
        BladerS1Script obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = new Vector2(positionX, positionY);
        obj.SR.flipX = flipX;
        obj.AN.enabled = true;
        obj.AN.SetTrigger("skill1");
        obj.SR.enabled = true;
        StartCoroutine(obj.ReturnCRT());

        return obj;
    }

    public void ReturnObject(BladerS1Script obj)
    {
        obj.SR.enabled = false;
        obj.AN.enabled = false;
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}