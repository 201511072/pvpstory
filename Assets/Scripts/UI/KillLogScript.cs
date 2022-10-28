using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillLogScript : MonoBehaviour
{
    public Image BackGround;
    public Image MyCharacterTool;//내 캐릭터면 노란색 테두리 띄우기
    public Image Kill;
    public Image Death;

    public RectTransform rectTransform;

    public void OFF()
    {
        BackGround.enabled = false;
        MyCharacterTool.enabled = false;
        Kill.enabled = false;
        Death.enabled = false;
    }

    public void ON()
    {
        BackGround.enabled = true;
        Kill.enabled = true;
        Death.enabled = true;
    }
}
