using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PortalScript : MonoBehaviour
{
    public float time;
    public bool OnTrigger;
    public Vector2 TransmissionPosition;
    public PhotonView PV;

    public GameObject TransmissionLightPrefab;
    public Queue<TransmissionLightScript> poolingObjectQueue = new Queue<TransmissionLightScript>();

    public Slider slider;

    public SpriteRenderer PortalEffectSR;
    public Animator PortalEffectAN;

    public Collider2D[] collider2DArr = new Collider2D[4];

    public void Init()
    {
        for (int i = 0; i < 8; i++)
        {
            Instantiate(TransmissionLightPrefab).GetComponent<TransmissionLightScript>().Init(this);
        }
    }

    public TransmissionLightScript GetObject(Vector2 position)
    {
        TransmissionLightScript obj = poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = position;
        obj.AN.SetTrigger("idle");
        obj.SR.enabled = true;
        StartCoroutine(obj.ReturnCRT());

        return obj;
    }

    public void ReturnObject(TransmissionLightScript obj)
    {
        obj.SR.enabled = false;
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            for (int i = 0; i < 4; i++)
            {
                if (collider2DArr[i] == null)
                {
                    collider2DArr[i] = col;
                    break;
                }
            }

            PortalEffectSR.enabled = true;
            PortalEffectAN.enabled = true;

            if (col.GetComponent<PhotonView>().IsMine)
            {
                OnTrigger = true;
                time = 0f;
                slider.value = 0f;
                slider.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            for (int i = 0; i < 4; i++)
            {
                if (collider2DArr[i] == col)
                {
                    collider2DArr[i] = null;
                    break;
                }
            }

            bool collider2DArrEmpty = true;

            for (int i = 0; i < 4; i++)
            {
                if (collider2DArr[i] != null)
                {
                    collider2DArrEmpty = false;
                    break;
                }
            }

            if (collider2DArrEmpty)
            {
                PortalEffectSR.enabled = false;
                PortalEffectAN.enabled = false;
            }


            if (col.GetComponent<PhotonView>().IsMine)
            {
                OnTrigger = false;
                time = 0f;
                slider.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (OnTrigger)
        {
            time += Time.deltaTime;
            slider.value = time / 3.0f;

            if (time > 3f)
            {
                time = 0f;
                OnTrigger = false;
                PV.RPC("TransmissionLight", RpcTarget.All, (Vector2)NetworkManager.instance.MyStatScript.transform.position, TransmissionPosition);
                NetworkManager.instance.MyStatScript.PV.RPC("Portal", RpcTarget.All, TransmissionPosition);
            }
        }
    }

    [PunRPC]
    public void TransmissionLight(Vector2 CharacterPosition, Vector2 TransmissionPosition)
    {
        NetworkManager.instance.soundObjectPool.GetObject(CharacterPosition, 30);
        NetworkManager.instance.soundObjectPool.GetObject(TransmissionPosition, 30);
        GetObject(CharacterPosition);
        GetObject(TransmissionPosition + new Vector2(0f, 0.75f));
    }
}
