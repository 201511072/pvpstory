using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraS1AttackObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<TaraS1AttackScript> poolingObjectQueue = new Queue<TaraS1AttackScript>();


    private void Awake()
    {
        if (statScript.PV.IsMine)
        {
            Initialize(15);
        }
    }


    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PhotonNetwork.Instantiate("TaraS1Attack", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public TaraS1AttackScript GetObject(float positionX, float positionY, bool isBlueTeam, float rotationZ)
    {
        TaraS1AttackScript obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = new Vector2(positionX, positionY);
        obj.isBlueTeam = isBlueTeam;
        obj.isParryinged = false;
        obj.isEnabled = true;
        obj.Hit1time = true;
        obj.attackTrigger.enabled = true;
        obj.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        obj.AN.enabled = true;
        obj.AN.SetBool("disappear", false);
        obj.SR.enabled = true;

        return obj;
    }

    public void ReturnObject(TaraS1AttackScript obj)
    {
        obj.SR.enabled = false;
        obj.AN.SetBool("disappear", false);
        obj.AN.enabled = false;
        obj.Hit1time = false;
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}