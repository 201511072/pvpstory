using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LinkerUltObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<LinkerUltScript> poolingObjectQueue = new Queue<LinkerUltScript>();


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
            PhotonNetwork.Instantiate("LinkerUlt", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public LinkerUltScript GetObject(Vector2 position)
    {
        LinkerUltScript obj = poolingObjectQueue.Dequeue();
        obj.time = 0f;
        obj.return1Time = true;
        obj.isEnabled = true;
        obj.transform.SetParent(null);
        obj.transform.position = position;
        obj.AN.enabled = true;
        obj.AN.SetTrigger("ult");
        obj.SR.enabled = true;
        if (statScript.PV.IsMine)
        {
            obj.col.enabled = true;
        }
        return obj;
    }

    public void ReturnObject(LinkerUltScript obj)
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