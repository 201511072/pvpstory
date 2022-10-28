using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

public class TaraS1UseRangeScript : MonoBehaviour, IPointerDownHandler
{
    public TaraScript taraScript;
    public TaraS1CheckTriggerScript TaraS1CheckTrigger;
    public bool isChecking;

    public Image RangeImage;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isChecking)
        {
            isChecking = true;
            Vector2 tempVector2 = Camera.main.ScreenToWorldPoint(eventData.position);
            TaraS1CheckTrigger.S1Ready = true;
            TaraS1CheckTrigger.transform.position = tempVector2;
            TaraS1CheckTrigger.CheckTrigger.enabled = true;
            StartCoroutine(CheckCRT());
        }
    }

    IEnumerator CheckCRT()
    {
        yield return new WaitForSeconds(0.1f);
        TaraS1CheckTrigger.CheckTrigger.enabled = false;

        if (TaraS1CheckTrigger.S1Ready)
        {
            taraScript.PV.RPC("Skill1RPC", RpcTarget.All, TaraS1CheckTrigger.transform.position.x, TaraS1CheckTrigger.transform.position.y, taraScript.statScript.BlueTeam, TaraS1CheckTrigger.transform.position.x < transform.position.x);
            RangeImage.enabled = false;
            NetworkManager.instance.ControlUI.SetActive(true);
            StartCoroutine(taraScript.Skill1MoveLockCRT());
        }
        else
        {
            StartCoroutine(RangeImageChange());
        }

        isChecking = false;
    }

    IEnumerator RangeImageChange()
    {
        RangeImage.color = new Color(1f, 0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.15f);
        RangeImage.color = new Color(0.5f, 1f, 0.5f, 0.5f);
    }
}
