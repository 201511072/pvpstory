using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountPanelScript : MonoBehaviour
{
    public Text CountText;
    public float count;
    public bool startText1Time;

    public SpriteRenderer ProhibitedAreaSR;
    public BoxCollider2D ProhibitedAreaCol;

    private void Update()
    {
        count -= Time.deltaTime;

        if (count > 0f)
        {
            CountText.text = "" + Mathf.CeilToInt(count);
        }
        else if (startText1Time && count <= 0f)
        {
            startText1Time = false;
            CountText.text = "Start";
            StartCoroutine(StartTextCRT());
        }
    }

    public IEnumerator StartTextCRT()
    {
        yield return new WaitForSeconds(1f);
        MapSetting();
        gameObject.SetActive(false);
        NetworkManager.instance.GameStarted = true;
    }

    public void MapSetting()
    {
        if (NetworkManager.instance.Map == 0)
        {

        }
        else if (NetworkManager.instance.Map == 1)
        {
            ProhibitedAreaSR.enabled = false;
            ProhibitedAreaCol.enabled = false;
            if (NetworkManager.instance.Blue1statScript != null) NetworkManager.instance.Blue1statScript.SkillLock = false;
            if (NetworkManager.instance.Blue2statScript != null) NetworkManager.instance.Blue2statScript.SkillLock = false;
            if (NetworkManager.instance.Red1statScript != null) NetworkManager.instance.Red1statScript.SkillLock = false;
            if (NetworkManager.instance.Red2statScript != null) NetworkManager.instance.Red2statScript.SkillLock = false;
            NetworkManager.instance.CaptureAreaScript.captureAreaColOnOff(true);
        }
    }

}
