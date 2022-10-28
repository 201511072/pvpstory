using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraUltObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<TaraUltScript> poolingObjectQueue = new Queue<TaraUltScript>();


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
            PhotonNetwork.Instantiate("TaraUlt", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public TaraUltScript GetObject(Vector2 taraPosition, bool isBlueTeam, Vector2 tempVector)
    {
        TaraUltScript obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.isBlueTeam = isBlueTeam;
        obj.transform.position = taraPosition;
        obj.SR.flipY = tempVector.x < 0;
        obj.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(tempVector.y, tempVector.x));
        
        obj.gameObject.SetActive(true);
        obj.AN.enabled = true;
        obj.AN.SetBool("appear", true);
        obj.SR.enabled = true;
        StartCoroutine(obj.TriggerCRT());
        StartCoroutine(obj.ReturnCRT());

        //sound
        obj.AS.Play();

        return obj;
    }

    public void ReturnObject(TaraUltScript obj)
    {
        if (statScript.PV.IsMine)
        {
            obj.Trigger.enabled = false;
        }
        obj.SR.enabled = false;
        obj.AN.enabled = false;
        obj.startMove = false;
        obj.transform.SetParent(transform);

        //sound
        obj.AS.Stop();
        poolingObjectQueue.Enqueue(obj);
    }
}
