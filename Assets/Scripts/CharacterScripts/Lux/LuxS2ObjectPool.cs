using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LuxS2ObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<LuxS2Script> poolingObjectQueue = new Queue<LuxS2Script>();


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
            PhotonNetwork.Instantiate("LuxS2", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public LuxS2Script GetObject(Vector2 position, bool flipX,bool OnBush)
    {
        LuxS2Script obj = poolingObjectQueue.Dequeue();
        obj.time = 0f;
        obj.return1Time = true;
        obj.isEnabled = true;
        obj.transform.SetParent(null);
        obj.transform.position = position;
        obj.SR.flipX = flipX;
        obj.AN.enabled = true;
        obj.AN.SetTrigger("skill2");
        if(!OnBush) obj.SR.enabled = true;       

        return obj;
    }

    public void ReturnObject(LuxS2Script obj)
    {
        if (obj.isEnabled)
        {
            obj.isEnabled = false;
            obj.return1Time = false;
            obj.SR.enabled = false;
            obj.AN.enabled = false;
            obj.transform.SetParent(transform);
            poolingObjectQueue.Enqueue(obj);
        }
    }
}