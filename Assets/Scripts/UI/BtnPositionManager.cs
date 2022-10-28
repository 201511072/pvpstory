using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnPositionManager : MonoBehaviour
{
    public RectTransform AttackBtnPosition;
    public RectTransform JumpBtnPosition;
    public RectTransform S1BtnPosition;
    public RectTransform S2BtnPosition;
    public RectTransform UltBtnPosition;
    public RectTransform Item1BtnPosition;
    public RectTransform Item2BtnPosition;
    public RectTransform Item3BtnPosition;

    public bool BtnSizeSettingMode;

    public GameObject CompleteSizeSettingBtn;
    public GameObject RevertSizeSettingBtn;
    public Slider SizeSettingSlider;
    public RectTransform SizeSettingRT;//인스펙터 비어있음

    public Vector2 tempSize;//OnRevertSizeSettingBtn() 할 때 필요한 것


    //저장버튼
    public void OnSaveSettingBtn()
    {
        NetworkManager.instance.AttackBtnVector2 = AttackBtnPosition.anchoredPosition;
        NetworkManager.instance.JumpBtnVector2 = JumpBtnPosition.anchoredPosition;
        NetworkManager.instance.S1BtnVector2 = S1BtnPosition.anchoredPosition;
        NetworkManager.instance.S2BtnVector2 = S2BtnPosition.anchoredPosition;
        NetworkManager.instance.UltBtnVector2 = UltBtnPosition.anchoredPosition;
        NetworkManager.instance.Item1BtnVector2 = Item1BtnPosition.anchoredPosition;
        NetworkManager.instance.Item2BtnVector2 = Item2BtnPosition.anchoredPosition;
        NetworkManager.instance.Item3BtnVector2 = Item3BtnPosition.anchoredPosition;

        NetworkManager.instance.AttackBtnSizeVector2 = AttackBtnPosition.sizeDelta;
        NetworkManager.instance.JumpBtnSizeVector2 = JumpBtnPosition.sizeDelta;
        NetworkManager.instance.S1BtnSizeVector2 = S1BtnPosition.sizeDelta;
        NetworkManager.instance.S2BtnSizeVector2 = S2BtnPosition.sizeDelta;
        NetworkManager.instance.UltBtnSizeVector2 = UltBtnPosition.sizeDelta;
        NetworkManager.instance.Item1BtnSizeVector2 = Item1BtnPosition.sizeDelta;
        NetworkManager.instance.Item2BtnSizeVector2 = Item2BtnPosition.sizeDelta;
        NetworkManager.instance.Item3BtnSizeVector2 = Item3BtnPosition.sizeDelta;


        NetworkManager.instance.BtnSettingPanel.SetActive(false);
        NetworkManager.instance.ControlUI.SetActive(false);
        NetworkManager.instance.DisconnectPanel.SetActive(true);
    }


    //초기화버튼
    public void OnRevertBtn()
    {
        AttackBtnPosition.anchoredPosition = new Vector2(-632f, -395f);
        JumpBtnPosition.anchoredPosition = new Vector2(-337f, -322f);
        S1BtnPosition.anchoredPosition = new Vector2(-585f, -231f);
        S2BtnPosition.anchoredPosition = new Vector2(-483f, -95f);
        UltBtnPosition.anchoredPosition = new Vector2(-326f, -21f);
        Item1BtnPosition.anchoredPosition = new Vector2(-746f, -260f);
        Item2BtnPosition.anchoredPosition = new Vector2(-691f, -121f);
        Item3BtnPosition.anchoredPosition = new Vector2(-595f, 2f);

        AttackBtnPosition.sizeDelta = new Vector2(140f, 140f);
        JumpBtnPosition.sizeDelta = new Vector2(224f, 224f);
        S1BtnPosition.sizeDelta = new Vector2(140f, 140f);
        S2BtnPosition.sizeDelta = new Vector2(140f, 140f);
        UltBtnPosition.sizeDelta = new Vector2(140f, 140f);
        Item1BtnPosition.sizeDelta = new Vector2(100f, 100f);
        Item2BtnPosition.sizeDelta = new Vector2(100f, 100f);
        Item3BtnPosition.sizeDelta = new Vector2(100f, 100f);
    }


    //크기조절버튼
    public void OnBtnSizeSettingBtn()
    {
        BtnSizeSettingMode = true;
        CompleteSizeSettingBtn.SetActive(true);
        RevertSizeSettingBtn.SetActive(true);
        SizeSettingSlider.gameObject.SetActive(true);
    }

    public void OnCompleteSizeSettingBtn()
    {
        BtnSizeSettingMode = false;
        CompleteSizeSettingBtn.SetActive(false);
        RevertSizeSettingBtn.SetActive(false);
        SizeSettingSlider.gameObject.SetActive(false);
        SizeSettingRT = null;
    }

    public void OnRevertSizeSettingBtn()
    {
        BtnSizeSettingMode = false;
        CompleteSizeSettingBtn.SetActive(false);
        RevertSizeSettingBtn.SetActive(false);
        SizeSettingSlider.gameObject.SetActive(false);
        if (SizeSettingRT != null)
        {
            SizeSettingRT.sizeDelta = tempSize;
        }
        SizeSettingRT = null;
    }


    public void SetRTForSizeSetting(RectTransform value)
    {
        if (BtnSizeSettingMode)
        {
            tempSize = value.sizeDelta;
            SizeSettingRT = value;
            SizeSettingSlider.value = SizeSettingRT.sizeDelta.x / 250f;
        }
    }

    private void Update()
    {
        if (BtnSizeSettingMode && SizeSettingRT != null)
        {
            SizeSettingRT.sizeDelta = Vector2.one * SizeSettingSlider.value * 250f;
        }
    }
}
