using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraUltShowRangeObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<TaraUltShowRangeScript> poolingObjectQueue = new Queue<TaraUltShowRangeScript>();


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
            PhotonNetwork.Instantiate("TaraUltShowRange", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public TaraUltShowRangeScript GetObject(Vector2 taraPosition, Vector2 tempVector)
    {
        TaraUltShowRangeScript obj = poolingObjectQueue.Dequeue();
        obj.transform.position = taraPosition;
        obj.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(tempVector.y, tempVector.x));
        if (tempVector.x < 0)
        {
            obj.SR2.flipX = true;
            obj.SR2.flipY = true;
        }
        else
        {
            obj.SR2.flipX = false;
            obj.SR2.flipY = false;
        }
        obj.SR2.enabled = true;
        StartCoroutine(obj.ReturnCRT());

        return obj;
    }

    public void ReturnObject(TaraUltShowRangeScript obj)
    {
        obj.SR2.enabled = false;
        poolingObjectQueue.Enqueue(obj);
    }
}
