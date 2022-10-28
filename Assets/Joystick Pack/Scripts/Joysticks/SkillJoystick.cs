using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillJoystick : Joystick
{
    public bool isVisible;//처음 시작할 때 보이게 할것인지
    public bool notUseSkillJoystick;//skillJoystick을 안 이용할경우 true
    public Button Btn;

    public BtnAttribute BtnAttribute { get { return BtnAttribute; } set { BtnAttribute = value; } }
    public BtnAttribute btnAttribute;
    public AttackBtnScript attackBtnScript;
    public S1BtnScript s1BtnScript;
    public S2BtnScript s2BtnScript;
    public UltBtnScript ultBtnScript;
    public Action action;
    public bool useValue;
    public float minDistanceSquare;//이 값의 제곱근 이상 움직이면 이 값을 사용하기 시작함

    protected override void Start()
    {
        base.Start();

        if (notUseSkillJoystick)
        {
            gameObject.SetActive(false);
        }

        if (!isVisible)
        {
            background.gameObject.SetActive(false);
        }

        if (btnAttribute == BtnAttribute.AttackBtn)
        {
            if (notUseSkillJoystick)
            {
                Btn.onClick.AddListener(()=>attackBtnScript.OnClickButton(Vector2.zero));
                gameObject.SetActive(false);
            }
            else
            {
                action = () => attackBtnScript.OnClickButton(Direction);
            }
        }
        else if (btnAttribute == BtnAttribute.S1Btn)
        {
            if (notUseSkillJoystick)
            {
                Btn.onClick.AddListener(() => s1BtnScript.OnClickButton(Vector2.zero));
                gameObject.SetActive(false);
            }
            else
            {
                action = () => s1BtnScript.OnClickButton(Direction);
            }
        }
        else if (btnAttribute == BtnAttribute.S2Btn)
        {
            if (notUseSkillJoystick)
            {
                Btn.onClick.AddListener(() => s2BtnScript.OnClickButton(Vector2.zero));
                gameObject.SetActive(false);
            }
            else
            {
                action = () => s2BtnScript.OnClickButton(Direction);
            }
        }
        else if (btnAttribute == BtnAttribute.UltBtn)
        {
            if (notUseSkillJoystick)
            {
                Btn.onClick.AddListener(() => ultBtnScript.OnClickButton(Vector2.zero));
                gameObject.SetActive(false);
            }
            else
            {
                action = () => ultBtnScript.OnClickButton(Direction);
            }
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        action();
        if (!isVisible)
        {
            background.gameObject.SetActive(false);
        }
        background.anchoredPosition = backgroundPosition;
        useValue = false;
        base.OnPointerUp(eventData);//원래 위치로 되돌리는 부분
    }

    public override void OnDrag(PointerEventData eventData)
    {
        /*cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;*///원래있던 부분2021-06-02(1)

        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Vector2 radius = background.sizeDelta / 2;
        if(!useValue&&Vector2.SqrMagnitude((eventData.position - position) / (radius * canvas.scaleFactor))> minDistanceSquare)
        {
            useValue = true;
            background.gameObject.SetActive(true);
        }
        if (useValue)
        {
            input = (eventData.position - position) / (radius * canvas.scaleFactor);
            //FormatInput();
            HandleInput(input.magnitude, input.normalized, radius, cam);
            handle.anchoredPosition = input * radius * handleRange;
        }
    }
}

public enum BtnAttribute { None ,AttackBtn, S1Btn, S2Btn, UltBtn }
