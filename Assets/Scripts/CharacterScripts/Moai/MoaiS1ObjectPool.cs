using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoaiS1ObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<MoaiS1HitboxScript> poolingObjectQueue = new Queue<MoaiS1HitboxScript>();


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
            PhotonNetwork.Instantiate("MoaiS1Hitbox", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public MoaiS1HitboxScript GetObject(float positionX, float positionY, bool isBlueTeam)
    {
        MoaiS1HitboxScript obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = SetPosition(positionX, positionY);
        obj.isBlueTeam = isBlueTeam;
        if (statScript.PV.IsMine)
        {
            StartCoroutine(obj.HitboxCRT());
        }
        obj.AN.enabled = true;
        obj.AN.SetTrigger("skill1");
        obj.SR.enabled = true;

        return obj;
    }

    public Vector2 SetPosition(float positionX, float positionY)
    {
        Vector2 Position;
        Vector2 tempPosition1;
        Vector2 tempPosition2;
        Vector2 tempPosition3;

        tempPosition1 = Physics2D.Raycast(new Vector2(positionX, positionY), Vector2.down, 1.5f, 1 << LayerMask.NameToLayer("Ground")).point;
        tempPosition2 = Physics2D.Raycast(new Vector2(positionX + 0.787f, positionY), Vector2.down, 1.5f, 1 << LayerMask.NameToLayer("Ground")).point;
        tempPosition3 = Physics2D.Raycast(new Vector2(positionX - 0.787f, positionY), Vector2.down, 1.5f, 1 << LayerMask.NameToLayer("Ground")).point;

        if (tempPosition1 == Vector2.zero)
        {
            tempPosition1 = Vector2.down * 100f;
        }
        if (tempPosition2 == Vector2.zero)
        {
            tempPosition2 = Vector2.down * 100f;
        }
        if (tempPosition3 == Vector2.zero)
        {
            tempPosition3 = Vector2.down * 100f;
        }

        if (tempPosition1.y>= tempPosition2.y)
        {
            if (tempPosition1.y >= tempPosition3.y)
            {
                Position = tempPosition1;
            }
            else 
            {
                Position = tempPosition3;
            }
        }
        else if(tempPosition2.y >= tempPosition3.y)
        {
            Position = tempPosition2;
        }
        else
        {
            Position = tempPosition3;
        }

        Position.x = positionX;

        return Position;
    }

    public void ReturnObject(MoaiS1HitboxScript obj)
    {
        obj.SR.enabled = false;
        obj.AN.enabled = false;
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}