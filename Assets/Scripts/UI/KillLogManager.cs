using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLogManager : MonoBehaviour
{
    public RectTransform KillLogPanel_Left;
    public RectTransform KillLogPanel_Right;

    public KillLogScript LeftKillLog_0;
    public KillLogScript LeftKillLog_1;
    public KillLogScript LeftKillLog_2;
    public KillLogScript LeftKillLog_3;
    public KillLogScript RightKillLog_0;
    public KillLogScript RightKillLog_1;
    public KillLogScript RightKillLog_2;
    public KillLogScript RightKillLog_3;

    public Sprite Blue_Left_Background;
    public Sprite Blue_Right_Background;
    public Sprite Red_Left_Background;
    public Sprite Red_Right_Background;

    public Sprite Portrait_Blue1;
    public Sprite Portrait_Blue2;
    public Sprite Portrait_Red1;
    public Sprite Portrait_Red2;
    public Sprite Baron;

    public int myPlayerNumber;
    public bool isBlueTeam;

    public int current_KillLogNumber_Left;//0,1,2,3 총 4개
    public int current_KillLogNumber_Right;//0,1,2,3 총 4개

    public void Init()
    {
        if (NetworkManager.instance.myPlayerNumber == 1 || NetworkManager.instance.myPlayerNumber == 4)
        {
            LeftKillLog_0.BackGround.sprite = Blue_Left_Background;
            LeftKillLog_1.BackGround.sprite = Blue_Left_Background;
            LeftKillLog_2.BackGround.sprite = Blue_Left_Background;
            LeftKillLog_3.BackGround.sprite = Blue_Left_Background;
            RightKillLog_0.BackGround.sprite = Red_Right_Background;
            RightKillLog_1.BackGround.sprite = Red_Right_Background;
            RightKillLog_2.BackGround.sprite = Red_Right_Background;
            RightKillLog_3.BackGround.sprite = Red_Right_Background;
            isBlueTeam = true;
        }
        else
        {
            LeftKillLog_0.BackGround.sprite = Red_Left_Background;
            LeftKillLog_1.BackGround.sprite = Red_Left_Background;
            LeftKillLog_2.BackGround.sprite = Red_Left_Background;
            LeftKillLog_3.BackGround.sprite = Red_Left_Background;
            RightKillLog_0.BackGround.sprite = Blue_Right_Background;
            RightKillLog_1.BackGround.sprite = Blue_Right_Background;
            RightKillLog_2.BackGround.sprite = Blue_Right_Background;
            RightKillLog_3.BackGround.sprite = Blue_Right_Background;
            isBlueTeam = false;
        }

        Portrait_Blue1 = SetSpriteByJobCode(NetworkManager.instance.Blue1statScript.JobCode);
        Portrait_Red1 = SetSpriteByJobCode(NetworkManager.instance.Red1statScript.JobCode);
        if (NetworkManager.instance.Red2statScript != null)
        {
            Portrait_Red2 = SetSpriteByJobCode(NetworkManager.instance.Red2statScript.JobCode);
        }
        if (NetworkManager.instance.Blue2statScript != null)
        {
            Portrait_Blue2 = SetSpriteByJobCode(NetworkManager.instance.Blue2statScript.JobCode);
        }

        KillLogPanel_Left.anchoredPosition = new Vector2(160f, 350f);
        KillLogPanel_Right.anchoredPosition = new Vector2(-160f, 350f);

        LeftKillLog_0.rectTransform.anchoredPosition = new Vector2(0f, 0f);
        LeftKillLog_1.rectTransform.anchoredPosition = new Vector2(0f, -140f);
        LeftKillLog_2.rectTransform.anchoredPosition = new Vector2(0f, -280f);
        LeftKillLog_3.rectTransform.anchoredPosition = new Vector2(0f, -420f);
        RightKillLog_0.rectTransform.anchoredPosition = new Vector2(0f, 0f);
        RightKillLog_1.rectTransform.anchoredPosition = new Vector2(0f, -140f);
        RightKillLog_2.rectTransform.anchoredPosition = new Vector2(0f, -280f);
        RightKillLog_3.rectTransform.anchoredPosition = new Vector2(0f, -420f);

        current_KillLogNumber_Left = 0;
        current_KillLogNumber_Right = 0;

        LeftKillLog_0.OFF();
        LeftKillLog_1.OFF();
        LeftKillLog_2.OFF();
        LeftKillLog_3.OFF();
        RightKillLog_0.OFF();
        RightKillLog_1.OFF();
        RightKillLog_2.OFF();
        RightKillLog_3.OFF();

        myPlayerNumber = NetworkManager.instance.myPlayerNumber;
    }

    public void ShowKillLog(int attackerPlayerNumber, int deathPlayerNumber)
    {
        if (attackerPlayerNumber == 1 || attackerPlayerNumber == 4)
        {
            if (current_KillLogNumber_Left == 0)
            {
                SetKillLog(LeftKillLog_0, attackerPlayerNumber, deathPlayerNumber);
            }
            else if (current_KillLogNumber_Left == 1)
            {
                SetKillLog(LeftKillLog_1, attackerPlayerNumber, deathPlayerNumber);
            }
            else if (current_KillLogNumber_Left == 2)
            {
                SetKillLog(LeftKillLog_2, attackerPlayerNumber, deathPlayerNumber);
            }
            else if (current_KillLogNumber_Left == 3)
            {
                SetKillLog(LeftKillLog_3, attackerPlayerNumber, deathPlayerNumber);
            }

            current_KillLogNumber_Left++;
            if (current_KillLogNumber_Left > 3)
            {
                current_KillLogNumber_Left = 0;
            }
        }
        else
        {
            if (current_KillLogNumber_Right == 0)
            {
                SetKillLog(RightKillLog_0, attackerPlayerNumber, deathPlayerNumber);
            }
            else if (current_KillLogNumber_Right == 1)
            {
                SetKillLog(RightKillLog_1, attackerPlayerNumber, deathPlayerNumber);
            }
            else if (current_KillLogNumber_Right == 2)
            {
                SetKillLog(RightKillLog_2, attackerPlayerNumber, deathPlayerNumber);
            }
            else if (current_KillLogNumber_Right == 3)
            {
                SetKillLog(RightKillLog_3, attackerPlayerNumber, deathPlayerNumber);
            }

            current_KillLogNumber_Right++;
            if (current_KillLogNumber_Right > 3)
            {
                current_KillLogNumber_Right = 0;
            }
        }
    }



    public Sprite SetSpriteByJobCode(int jobCode)
    {
        if (jobCode == 1)
        {
            return NetworkManager.instance.CharacterSelectScript.Character1;
        }
        else if (jobCode == 2)
        {
            return NetworkManager.instance.CharacterSelectScript.Character2;
        }
        else if (jobCode == 3)
        {
            return NetworkManager.instance.CharacterSelectScript.Character3;
        }
        else if (jobCode == 4)
        {
            return NetworkManager.instance.CharacterSelectScript.Character4;
        }
        else if (jobCode == 5)
        {
            return NetworkManager.instance.CharacterSelectScript.Character5;
        }
        else if (jobCode == 6)
        {
            return NetworkManager.instance.CharacterSelectScript.Character6;
        }
        else return NetworkManager.instance.CharacterSelectScript.Character1;//예외처리 위해서 넣음
    }

    public Sprite SetSpriteByPlayerNumber(int playerNumber)
    {
        if (playerNumber == 1)
        {
            return Portrait_Blue1;
        }
        else if (playerNumber == 2)
        {
            return Portrait_Red1;
        }
        else if (playerNumber == 3)
        {
            return Portrait_Red2;
        }
        else if (playerNumber == 4)
        {
            return Portrait_Blue2;
        }
        else return Portrait_Blue1;//예외처리용
    }

    public void SetKillLog(KillLogScript killLogScript, int attackerPlayerNumber, int deathPlayerNumber)
    {
        killLogScript.Kill.sprite = SetSpriteByPlayerNumber(attackerPlayerNumber);
        killLogScript.Death.sprite = SetSpriteByPlayerNumber(deathPlayerNumber);
        killLogScript.ON();
        if (attackerPlayerNumber == myPlayerNumber)
        {
            killLogScript.MyCharacterTool.rectTransform.anchoredPosition = new Vector2(isBlueTeam ? -90f : -100f, 0f);
            killLogScript.MyCharacterTool.enabled = true;

            NetworkManager.instance.myKillInThisGame += 1;
        }
        else if (deathPlayerNumber == myPlayerNumber)
        {
            killLogScript.MyCharacterTool.rectTransform.anchoredPosition = new Vector2(isBlueTeam ? 100f : 90f, 0f);
            killLogScript.MyCharacterTool.enabled = true;

            NetworkManager.instance.myDeathInThisGame += 1;
        }

        StartCoroutine(KillLogCRT(killLogScript, attackerPlayerNumber));
    }

    public IEnumerator KillLogCRT(KillLogScript killLogScript, int attackerPlayerNumber)
    {
        yield return new WaitForSeconds(5.0f);
        killLogScript.OFF();
        killLogScript.rectTransform.anchoredPosition -= new Vector2(0f, 560f);
        if (attackerPlayerNumber == 1 || attackerPlayerNumber == 4)
        {
            KillLogPanel_Left.anchoredPosition += new Vector2(0f, 140f);
        }
        else
        {
            KillLogPanel_Right.anchoredPosition += new Vector2(0f, 140f);
        }
    }
}
