using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArcherAttackObjectPool : MonoBehaviour
{
    public Queue<ArrowScript> poolingObjectQueue = new Queue<ArrowScript>();
    public StatScript statScript;
    public ArcherScript archerScript;


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
            PhotonNetwork.Instantiate("Arrow", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public ArrowScript GetObject(float positionX, float positionY, bool isBlueTeam, float vectorX, float vectorY, float arrowPlusATK, float arrowSpeed)
    {
        ArrowScript obj = poolingObjectQueue.Dequeue();
        obj.isEnabled = true;
        obj.transform.SetParent(null);
        obj.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * (Mathf.Atan2(vectorY, vectorX)));
        obj.HitEffectPosition = new Vector2(vectorX, vectorY) * 0.42f;
        obj.isBlueTeam = isBlueTeam;
        obj.isParryinged = false;
        if (arrowPlusATK == archerScript.arrowPlusATK[0])
        {
            obj.AN.SetBool("enhanceArrow", false);
        }
        else if (arrowPlusATK == archerScript.arrowPlusATK[1])
        {
            obj.AN.SetBool("enhanceArrow", true);
        }
        obj.AN.enabled = true;
        obj.SR.enabled = true;
        obj.PlusATK = arrowPlusATK;
        obj.RB.velocity = new Vector2(vectorX, vectorY) * arrowSpeed;
        obj.transform.position = new Vector2(positionX, positionY) + new Vector2(vectorX, vectorY);
        obj.time = 0f;
        obj.BoxCollider2D.enabled = true;
        obj.UltArrow = archerScript.Ulting;
        obj.Hit1time = true;

        return obj;
    }

    public void ReturnObject(ArrowScript obj)
    {
        if (obj.isEnabled)
        {
            obj.SR.enabled = false;
            obj.AN.enabled = false;
            obj.BoxCollider2D.enabled = false;
            obj.RB.velocity = Vector2.zero;
            obj.transform.SetParent(transform);
            obj.isEnabled = false;
            obj.time = 0f;
            obj.transform.rotation = Quaternion.identity;
            poolingObjectQueue.Enqueue(obj);
        }
    }
}
