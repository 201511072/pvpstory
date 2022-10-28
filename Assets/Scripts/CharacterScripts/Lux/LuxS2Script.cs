using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LuxS2Script : MonoBehaviour
{
    public LuxScript luxScript;
    public StatScript statScript;
    public LuxS2ObjectPool luxS2ObjectPool;
    public SpriteRenderer SR;
    public Animator AN;

    public float time;
    public bool return1Time;
    public bool isEnabled;


    [PunRPC]
    void InitRPC()
    {
        Init();
    }

    public void Init()
    {
        luxScript = GameObject.Find("Lux(Clone)").GetComponent<LuxScript>();
        statScript = luxScript.statScript;
        luxS2ObjectPool = luxScript.luxS2ObjectPool;
        transform.SetParent(luxS2ObjectPool.transform);
        luxS2ObjectPool.poolingObjectQueue.Enqueue(this);
    }


    private void Update()
    {
        if (statScript != null)
        {
            if (return1Time && (NetworkManager.instance.PausedDeltaTime() + time) >= 0.2f)
            {
                return1Time = false;
                luxS2ObjectPool.ReturnObject(this);
            }
            else if (return1Time)
            {
                time += Time.deltaTime;
            }
        }
    }
}
