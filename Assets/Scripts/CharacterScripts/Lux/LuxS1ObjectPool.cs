using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LuxS1ObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<LuxS1Script> poolingObjectQueue = new Queue<LuxS1Script>();


    private void Awake()
    {
        if (statScript.PV.IsMine)
        {
            Initialize(3);
        }
    }


    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PhotonNetwork.Instantiate("LuxS1", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public LuxS1Script GetObject(Vector2 position, Quaternion rotation)
    {
        LuxS1Script obj = poolingObjectQueue.Dequeue();
        obj.time = 0f;
        obj.return1Time = true;
        obj.isEnabled = true;
        obj.transform.SetParent(null);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.AN.enabled = true;
        obj.AN.SetTrigger("skill1");
        obj.SR.enabled = true;
        if (statScript.PV.IsMine)
        {
            StartCoroutine(obj.Skill1CRT());
        }

        return obj;
    }

    public void ReturnObject(LuxS1Script obj)
    {
        if (obj.isEnabled)
        {
            obj.isEnabled = false;
            obj.return1Time = false;
            obj.SR.enabled = false;
            obj.AN.enabled = false;
            obj.col.enabled = false;
            obj.transform.SetParent(transform);
            poolingObjectQueue.Enqueue(obj);
        }
    }
}