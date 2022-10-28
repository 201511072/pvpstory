using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraAttackExplosionObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<TaraAttackExplosionScript> poolingObjectQueue = new Queue<TaraAttackExplosionScript>();


    private void Awake()
    {
        if (statScript.PV.IsMine)
        {
            Initialize(6);
        }
    }


    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PhotonNetwork.Instantiate("TaraAttackExplosion", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public TaraAttackExplosionScript GetObject(float positionX, float positionY, bool isBlueTeam)
    {
        TaraAttackExplosionScript obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.AN.SetTrigger("attackExplosion");
        obj.SR.enabled = true;
        obj.transform.position = new Vector2(positionX, positionY);
        if (statScript.PV.IsMine)
        {
            StartCoroutine(obj.TriggerCRT());
        }
        StartCoroutine(obj.ColCRT());

        return obj;
    }

    public void ReturnObject(TaraAttackExplosionScript obj)
    {
        obj.SR.enabled = false;
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}
