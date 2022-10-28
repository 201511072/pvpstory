using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnPositionSettingScript : MonoBehaviour, IDragHandler
{
    public RectTransform RT;
    public Canvas canvas;
    public BtnPositionManager btnPositionManager;


    public void OnDrag(PointerEventData eventData)
    {
        if (!btnPositionManager.BtnSizeSettingMode)
        {
            RT.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}
