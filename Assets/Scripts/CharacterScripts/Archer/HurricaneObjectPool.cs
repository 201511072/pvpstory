using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HurricaneObjectPool : MonoBehaviour
{

    public Queue<HurricaneScript> poolingObjectQueue = new Queue<HurricaneScript>();
    public StatScript statScript;

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
            PhotonNetwork.Instantiate("Hurricane", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<HurricaneScript>().PV.RPC("InitRPC", RpcTarget.All);
        }
    }

    public HurricaneScript GetObject(float positionX, float positionY, int dir, bool isBlueTeam)
    {
        HurricaneScript obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.dir = dir;
        obj.SR.flipX = dir != 1;
        obj.isBlueTeam = isBlueTeam;
        obj.AN.enabled = true;
        obj.SR.enabled = true;
        obj.boxCollider2D.enabled = true;
        if (statScript.PV.IsMine)
        {
            obj.trigger.enabled = true;
            obj.time = 0f;
        }
        obj.RB.gravityScale = 1.5f;
        obj.transform.position = new Vector3(positionX, positionY, 0f);
        obj.isEnable = true;
        obj.AS.Play();

        return obj;
    }

    public void ReturnObject(HurricaneScript obj)
    {
        if (obj.isEnable)
        {
            obj.trigger.enabled = false;
            obj.SR.enabled = false;
            obj.AN.enabled = false;
            obj.boxCollider2D.enabled = false;
            obj.RB.gravityScale = 0f;
            obj.RB.velocity = Vector2.zero;
            obj.isEnable = false;
            if (statScript.PV.IsMine)
            {
                obj.trigger.enabled = false;
                obj.time = 0f;
            }
            obj.AS.Stop();
            obj.transform.SetParent(transform);
            poolingObjectQueue.Enqueue(obj);
        }
    }
}
