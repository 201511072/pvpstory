using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class TaraAttackUseRangeScript : MonoBehaviour, IPointerDownHandler
{
    public TaraScript taraScript;
    public TaraAttackExplosionObjectPool taraAttackExplosionObjectPool;

    public float time;
    public int count;

    private void Update()
    {
        if (time < 0.35f)
        {
            time += Time.deltaTime;
        }
        else if (count >= 3)
        {
            count = 0;
            gameObject.SetActive(false);
            NetworkManager.instance.ControlUI.SetActive(true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (time >= 0.35f && count < 3)
        {
            time = 0f;
            count++;
            Vector2 tempVector2 = Camera.main.ScreenToWorldPoint(eventData.position);
            taraScript.PV.RPC("AttackRPC", RpcTarget.All, tempVector2.x, tempVector2.y, taraScript.statScript.BlueTeam, count);
        }
    }
}
