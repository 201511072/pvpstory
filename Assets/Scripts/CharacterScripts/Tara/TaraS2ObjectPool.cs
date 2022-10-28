using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraS2ObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<TaraS2Script> poolingObjectQueue = new Queue<TaraS2Script>();


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
            PhotonNetwork.Instantiate("TaraS2", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public TaraS2Script GetObject(Vector2 position, bool isBlueTeam)
    {
        TaraS2Script obj = poolingObjectQueue.Dequeue();
        obj.time = 0f;
        obj.return1Time = true;
        obj.transform.SetParent(null);
        obj.isEnabled = true;
        obj.transform.position = position + new Vector2(0f, 15f);
        obj.AN.enabled = true;
        obj.AN.SetTrigger("appear");
        obj.SR.enabled = true;
        obj.isBlueTeam = isBlueTeam;
        obj.S2Col.enabled = true;
        StartCoroutine(obj.ANSkill2CRT());
        obj.AS_Appear.Play();

        return obj;
    }

    public void ReturnObject(TaraS2Script obj)
    {
        if (obj.isEnabled)
        {
            obj.isEnabled = false;
            obj.SR.enabled = false;
            obj.AN.enabled = false;
            obj.S2Col.enabled = false;
            obj.transform.SetParent(transform);
            poolingObjectQueue.Enqueue(obj);
        }
    }
}

