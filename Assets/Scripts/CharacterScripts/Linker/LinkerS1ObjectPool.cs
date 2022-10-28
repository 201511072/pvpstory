using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LinkerS1ObjectPool : MonoBehaviour
{
    public Queue<LinkerS1Script> poolingObjectQueue = new Queue<LinkerS1Script>();
    public StatScript statScript;
    public GameObject LinkerS1Col;


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
            PhotonNetwork.Instantiate("LinkerS1", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<LinkerS1Script>().PV.RPC("InitRPC",RpcTarget.All);
        }
    }

    public LinkerS1Script GetObject(float positionX, float positionY, bool flipX, bool isBlueTeam, float vectorX, float vectorY)
    {
        LinkerS1Script obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.SR.flipX = flipX ? true : false;
        obj.isBlueTeam = isBlueTeam;
        obj.isParryinged = false;
        obj.AN.SetBool("end", false);
        obj.AN.SetBool("skill1", true);
        obj.SR.enabled = true;
        obj.RB.velocity = new Vector2(vectorX, vectorY) * 8f;
        obj.transform.position = new Vector2(positionX, positionY);
        obj.isEnable = true;
        obj.time = 0f;
        obj.col.enabled = true;
        obj.returnRPC1Time = true;

        return obj;
    }

    public void ReturnObject(LinkerS1Script obj)
    {
        if (obj.isEnable)
        {
            obj.isEnable = false;
            obj.SR.enabled = false;
            obj.col.enabled = false;
            obj.RB.velocity = Vector2.zero;
            obj.transform.SetParent(transform);
            obj.time = 0f;
            poolingObjectQueue.Enqueue(obj);
        }
    }
}
