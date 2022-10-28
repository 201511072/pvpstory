using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

public class TaraS2UseRangeScript : MonoBehaviour, IPointerDownHandler
{
    public TaraScript taraScript;
    public TaraS2CheckTriggerScript TaraS2CheckTrigger;
    public bool isChecking;

    public Image RangeImage;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isChecking)
        {
            isChecking = true;
            Vector2 tempVector2 = Camera.main.ScreenToWorldPoint(eventData.position);
            TaraS2CheckTrigger.S2Ready = true;
            TaraS2CheckTrigger.transform.position = tempVector2;
            TaraS2CheckTrigger.CheckTrigger.enabled = true;
            StartCoroutine(CheckCRT());
        }
    }

    IEnumerator CheckCRT()
    {
        yield return new WaitForSeconds(0.1f);
        TaraS2CheckTrigger.CheckTrigger.enabled = false;

        if (TaraS2CheckTrigger.S2Ready)
        {
            TaraS2CheckTrigger.FireWall();
            RangeImage.enabled = false;
            NetworkManager.instance.ControlUI.SetActive(true);
            StartCoroutine(taraScript.Skill2MoveLockCRT());
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
