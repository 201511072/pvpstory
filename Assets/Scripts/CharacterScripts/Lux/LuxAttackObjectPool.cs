using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LuxAttackObjectPool : MonoBehaviour
{
    public Queue<Lux_AttackBall> poolingObjectQueue = new Queue<Lux_AttackBall>();
    public StatScript statScript;


    private void Awake()
    {
        if (statScript.PV.IsMine)
        {
            Initialize(20);
        }
    }


    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PhotonNetwork.Instantiate("Lux_Ball", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<Lux_AttackBall>().PV.RPC("InitRPC", RpcTarget.All);
        }
    }

    public Lux_AttackBall GetObject(float positionX, float positionY, bool flipX, bool isBlueTeam, float vectorX, float vectorY)
    {
        Lux_AttackBall obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.isBlueTeam = isBlueTeam;
        obj.isParryinged = false;
        obj.AN.enabled = true;
        obj.AN.SetBool("start", false);
        obj.AN.SetBool("loop", false);
        obj.AN.SetBool("end", false);
        obj.AN.SetBool("start", true);
        StartCoroutine(obj.AN_Loop_CRT());
        obj.SR.enabled = true;
        obj.RB.velocity = new Vector2(vectorX, vectorY) * obj.speed;
        obj.transform.position = new Vector2(positionX, positionY);
        obj.isExploded = false;
        obj.canCount = true;
        obj.count = 0;
        obj.time = 0f;
        obj.circleCollider2D.enabled = true;
        obj.trigger.enabled = true;
        obj.isEnabled = true;

        return obj;
    }

    public void ReturnObject(Lux_AttackBall obj)
    {
        obj.SR.enabled = false;
        obj.AN.enabled = false;
        obj.transform.SetParent(transform);
        obj.time = 0f;
        poolingObjectQueue.Enqueue(obj);
    }
}
