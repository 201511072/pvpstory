using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillRound : MonoBehaviour
{
    float distance;
    public Image ImgFill, ImgStartDot;
    public ImgsFillDynamic ImgsFD;
    private void Awake()
    {
        this.distance = Vector3.Distance(this.transform.position, this.ImgStartDot.transform.position);
    }

    public void SetFill(float _amount)
    {
        this.ImgFill.fillAmount = _amount;
        //this.RefreshAngle();
    }


    float GetAngle(float _amount)
    {
        return _amount * 360F;
    }
}
