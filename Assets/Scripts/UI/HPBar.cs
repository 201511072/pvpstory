using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider hpBar;
    public RectTransform fill_shield;

    public GameObject HpLineFolder;

    //격자 세팅하는 함수.
    public void setHpBar(float maxHp)
    {
        float scaleX = 32000f / maxHp;//16000f / maxHp;
        foreach (RectTransform child in HpLineFolder.GetComponentsInChildren<RectTransform>())
        {
            child.sizeDelta = new Vector2(scaleX, child.sizeDelta.y);
        }
        HpLineFolder.GetComponent<RectTransform>().offsetMin = new Vector2(0, HpLineFolder.GetComponent<RectTransform>().offsetMin.y);
        HpLineFolder.GetComponent<RectTransform>().offsetMax = new Vector2(0, HpLineFolder.GetComponent<RectTransform>().offsetMax.y);
    }

    //체력변화 적용 함수
    public void setHp(float curHp, float maxHp, float shield)
    {
        if (curHp + shield != 0f)
        {
            if (curHp + shield > maxHp)
            {
                maxHp = curHp + shield;
                setHpBar(curHp + shield);
            }
            else setHpBar(maxHp);

            float tempShieldAmount = shield / (curHp + shield);
            fill_shield.localScale = new Vector3(tempShieldAmount, 1, 1);
            hpBar.value = (curHp + shield) / maxHp;
        }
    }
}