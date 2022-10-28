using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerS1EffectObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<BladerS1EffectScript> poolingObjectQueue = new Queue<BladerS1EffectScript>();


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
            PhotonNetwork.Instantiate("BladerS1Effect", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public BladerS1EffectScript GetObject(Vector2 position, Quaternion rotation, Vector2 size, bool flipY)
    {
        BladerS1EffectScript obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.localScale = new Vector3(size.x, size.y, 1f);
        obj.SR.flipY = flipY;
        obj.AN.enabled = true;
        obj.AN.SetTrigger("skill1");
        obj.SR.enabled = true;
        StartCoroutine(obj.ReturnCRT());

        return obj;
    }

    public void ReturnObject(BladerS1EffectScript obj)
    {
        obj.SR.enabled = false;
        obj.AN.enabled = false;
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}