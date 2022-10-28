using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraS1ObjectPool : MonoBehaviour
{
    public StatScript statScript;

    public Queue<TaraS1Script> poolingObjectQueue = new Queue<TaraS1Script>();


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
            PhotonNetwork.Instantiate("TaraS1", new Vector3(100f, 100f, 0f), Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }

    public TaraS1Script GetObject(float positionX, float positionY, bool isBlueTeam, bool flipX)
    {
        TaraS1Script obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = new Vector2(positionX, positionY);
        obj.PlayerHP = obj.PlayerMaxHP;
        obj.canvas.SetActive(true);
        obj.SR.flipX = flipX;
        obj.SR.enabled = true;
        obj.BlueTeam = isBlueTeam;
        obj.S1Col.enabled = true;
        obj.isEnabled = true;
        obj.AN.enabled = true;
        obj.gameObject.SetActive(true);
        obj.AN.SetBool("disappear", false);
        obj.AN.SetBool("shoot", false);
        obj.AN.SetBool("reload", false);
        obj.AN.SetBool("reload2", false);
        obj.AN.SetBool("appear", true);
        if (statScript.PV.IsMine)
        {
            obj.taraS1AttackRangeScript.ANReload1Time = true;
            obj.taraS1AttackRangeScript.AttackTime = 0f;
            obj.taraS1AttackRangeScript.ReturnTime = 0f;
            obj.taraS1AttackRangeScript.gameObject.SetActive(true);
        }

        if (!obj.isHpBarColorChagned) obj.HPBarColorChange();

        return obj;
    }

    public void ReturnObject(TaraS1Script obj)
    {
        obj.gameObject.SetActive(false);
        obj.canvas.SetActive(false);
        obj.SR.enabled = false;
        obj.AN.SetBool("disappear", false);
        obj.AN.enabled = false;
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}
