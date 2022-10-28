using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class HpKitScript : MonoBehaviourPunCallbacks
{
     public Animator AN;
    public BoxCollider2D trigger;

    bool isEnable;
    float timer;
    public float cooltime = 18f;

    public PhotonView PV;

    public Image gauge;

    private void Start()
    {
        isEnable = true;
        trigger.enabled = true;
    }



    //public void SelfDestroy()
    //{
    //    Destroy(transform.parent.gameObject);
    //}


    private void Update()
    {
        if (!isEnable)
        {
            timer += Time.deltaTime;

            gauge.fillAmount = 1f - timer / cooltime;

            if (timer > cooltime)
            {
                timer = 0f;
                PV.RPC("HpKitOn", RpcTarget.All);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if (tempStatScript.PV.IsMine && tempStatScript.PlayerHP < tempStatScript.PlayerMaxHP)
            {
                trigger.enabled = false;
                PV.RPC("HpKitOff", RpcTarget.All);
                if (tempStatScript.PlayerMaxHP - tempStatScript.PlayerHP > 1000f)
                {
                    tempStatScript.PV.RPC("PlayerHPChangeRPC", RpcTarget.All, tempStatScript.PlayerHP + 1000f, 0);
                }
                else
                {
                    tempStatScript.PV.RPC("PlayerHPChangeRPC", RpcTarget.All, tempStatScript.PlayerMaxHP, 0);
                }
            }
        }
    }

    [PunRPC]
    void HpKitOn()
    {
        if (!isEnable)
        {
            isEnable = true;
            AN.SetBool("on", true);
            timer = 0f;
            trigger.enabled = true;
            gauge.fillAmount = 0f;
            NetworkManager.instance.soundObjectPool.GetObject(transform.position, 31);
        }
    }

    [PunRPC]
    void HpKitOff()
    {
        trigger.enabled = false;
        isEnable = false;
        AN.SetBool("on", false);
        NetworkManager.instance.soundObjectPool.GetObject(transform.position, 32);
    }
}
