using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LuxUltObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<LuxUltScript> poolingObjectQueue = new Queue<LuxUltScript>();


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
            PhotonNetwork.Instantiate("LuxUlt", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public LuxUltScript GetObject(Vector2 position, Quaternion rotation)
    {
        LuxUltScript obj = poolingObjectQueue.Dequeue();
        obj.time = 0f;
        obj.return1Time = true;
        obj.isEnabled = true;
        obj.transform.SetParent(null);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.AN.enabled = true;
        obj.AN.SetTrigger("ult");
        obj.SR.enabled = true;
        if (statScript.PV.IsMine)
        {
            StartCoroutine(obj.UltCRT());
        }

        return obj;
    }

    public void ReturnObject(LuxUltScript obj)
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