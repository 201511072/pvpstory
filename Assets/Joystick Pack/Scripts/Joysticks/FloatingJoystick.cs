using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public bool isVisible;//처음 시작할 때 보이게 할것인지

    protected override void Start()
    {
        base.Start();
        if (!isVisible)
        {
            background.gameObject.SetActive(false);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!isVisible)
        {
            background.gameObject.SetActive(false);
        }
        background.anchoredPosition = backgroundPosition;
        base.OnPointerUp(eventData);//원래위치로 되돌리는 부분
    }
}