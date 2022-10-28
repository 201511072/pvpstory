using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushScript : MonoBehaviour
{
    public int thisBushState;//부쉬밖0, 왼쪽부쉬1,오른쪽부쉬2
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            //tempStatScript.OnBush = true;
            //if (tempStatScript.BlueTeam == NetworkManager.instance.MyStatScript.BlueTeam)
            //{
            //    if (tempStatScript.JobCode == 2) tempStatScript.bladerScript.S2SR.color=new Color(1f,1f,1f,0.5f);
            //
            //    tempStatScript.BaronBuffSR.color = new Color(1f, 1f, 1f, 0.5f);
            //}
            //else
            //{
            //    if (tempStatScript.SunFire)
            //    {
            //        tempStatScript.SunFireImage_SR.enabled = false;
            //    }
            //    if (tempStatScript.JobCode == 2) tempStatScript.bladerScript.S2SR.enabled = false;
            //
            //    tempStatScript.BaronBuffSR.enabled = false;
            //    tempStatScript.BladerS1AuraImage.gameObject.SetActive(false);
            //    tempStatScript.NickNameText.enabled = false;
            //    tempStatScript.hpBar.gameObject.SetActive(false);
            //}

            tempStatScript.OnBush = true;
            tempStatScript.bushState = thisBushState;
            ApplyBushState(NetworkManager.instance.Blue1statScript, NetworkManager.instance.Blue1statScript.BlueTeam == NetworkManager.instance.MyStatScript.BlueTeam);
            ApplyBushState(NetworkManager.instance.Blue2statScript, NetworkManager.instance.Blue2statScript.BlueTeam == NetworkManager.instance.MyStatScript.BlueTeam);
            ApplyBushState(NetworkManager.instance.Red1statScript, NetworkManager.instance.Red1statScript.BlueTeam == NetworkManager.instance.MyStatScript.BlueTeam);
            ApplyBushState(NetworkManager.instance.Red2statScript, NetworkManager.instance.Red2statScript.BlueTeam == NetworkManager.instance.MyStatScript.BlueTeam);
        }
    }//부쉬상태 바꾸고 함수실행. bool자리에는 내 캐릭터팀과 같은지 체크한 값 보내기, onbush =true로 하기. 나갈땐 false로.

    public void ApplyBushState(StatScript statScript,bool isMyTeam)
    {
        if (statScript.bushState==0)//부쉬 밖이면
        {
            if (!(statScript.JobCode==4 && statScript.isLuxS2ing))//럭스이고 스킬2 사용중이 아니라면 안투명하게 한다.
            {
                statScript.SR.color = new Color(statScript.SR.color.r, statScript.SR.color.g, statScript.SR.color.b, 1f);
                statScript.BaronBuffSR.color = Color.white;
                statScript.SunFireImage_SR.color= Color.white;
                if(statScript.JobCode == 2)
                {
                    statScript.bladerScript.S2SR.color = Color.white;
                }
                statScript.BaronBuffSR.color = Color.white;
                if (NetworkManager.instance.MyStatScript.JobCode == 2)
                {
                    statScript.BladerS1AuraImage.gameObject.SetActive(true);
                }
                statScript.NickNameText.enabled = true;
                statScript.hpBar.gameObject.SetActive(true);
            }
        }
        else if (isMyTeam)//아군이면
        {
            if (!(statScript.JobCode == 4 && statScript.isLuxS2ing))//럭스이고 스킬2 사용중이 아니라면 반투명하게 한다.
            {
                statScript.SR.color = new Color(statScript.SR.color.r, statScript.SR.color.g, statScript.SR.color.b, 0.5f);
                statScript.BaronBuffSR.color = new Color(1f, 1f, 1f, 0.5f);
                statScript.SunFireImage_SR.color = new Color(1f, 1f, 1f, 0.5f);
                if (statScript.JobCode == 2)
                {
                    statScript.bladerScript.S2SR.color = new Color(1f, 1f, 1f, 0.5f);
                }
                if (NetworkManager.instance.MyStatScript.JobCode == 2)
                {
                    statScript.BladerS1AuraImage.gameObject.SetActive(true);
                }
                statScript.NickNameText.enabled = true;
                statScript.hpBar.gameObject.SetActive(true);
            }
        }
        else if (statScript.myPlayerNumber==1|| statScript.myPlayerNumber == 4)//적군이 블루팀이고
        {
            if(statScript.bushState==NetworkManager.instance.Red1statScript.bushState|| statScript.bushState == NetworkManager.instance.Red2statScript.bushState)//아군과 같은 부쉬에 있다면
            {
                if (!(statScript.JobCode == 4 && statScript.isLuxS2ing))//럭스이고 스킬2 사용중이 아니라면 반투명하게 한다.
                {
                    statScript.SR.color = new Color(statScript.SR.color.r, statScript.SR.color.g, statScript.SR.color.b, 0.5f);
                    statScript.BaronBuffSR.color = new Color(1f, 1f, 1f, 0.5f);
                    statScript.SunFireImage_SR.color = new Color(1f, 1f, 1f, 0.5f);
                    if (statScript.JobCode == 2)
                    {
                        statScript.bladerScript.S2SR.color = new Color(1f, 1f, 1f, 0.5f);
                    }
                    if (NetworkManager.instance.MyStatScript.JobCode == 2)
                    {
                        statScript.BladerS1AuraImage.gameObject.SetActive(true);
                    }
                    statScript.NickNameText.enabled = true;
                    statScript.hpBar.gameObject.SetActive(true);
                }
            }
            else//다른 부쉬에 있다면
            {
                if (!(statScript.JobCode == 4 && statScript.isLuxS2ing))//럭스이고 스킬2 사용중이 아니라면 반투명하게 한다.
                {
                    statScript.SR.color = new Color(statScript.SR.color.r, statScript.SR.color.g, statScript.SR.color.b, 0f);
                    statScript.BaronBuffSR.color = new Color(1f, 1f, 1f, 0f);
                    statScript.SunFireImage_SR.color = new Color(1f, 1f, 1f, 0f);
                    if (statScript.JobCode == 2)
                    {
                        statScript.bladerScript.S2SR.color = new Color(1f, 1f, 1f, 0f);
                    }
                    if (NetworkManager.instance.MyStatScript.JobCode == 2)
                    {
                        statScript.BladerS1AuraImage.gameObject.SetActive(false);
                    }
                    statScript.NickNameText.enabled = false;
                    statScript.hpBar.gameObject.SetActive(false);
                }
            }
        }
        else//적군이 레드팀이고
        {
            if (statScript.bushState == NetworkManager.instance.Blue1statScript.bushState || statScript.bushState == NetworkManager.instance.Blue2statScript.bushState)//아군과 같은 부쉬에 있다면
            {
                if (!(statScript.JobCode == 4 && statScript.isLuxS2ing))//럭스이고 스킬2 사용중이 아니라면 반투명하게 한다.
                {
                    statScript.SR.color = new Color(statScript.SR.color.r, statScript.SR.color.g, statScript.SR.color.b, 0.5f);
                    statScript.BaronBuffSR.color = new Color(1f, 1f, 1f, 0.5f);
                    statScript.SunFireImage_SR.color = new Color(1f, 1f, 1f, 0.5f);
                    if (statScript.JobCode == 2)
                    {
                        statScript.bladerScript.S2SR.color = new Color(1f, 1f, 1f, 0.5f);
                    }
                    if (NetworkManager.instance.MyStatScript.JobCode == 2)
                    {
                        statScript.BladerS1AuraImage.gameObject.SetActive(true);
                    }
                    statScript.NickNameText.enabled = true;
                    statScript.hpBar.gameObject.SetActive(true);
                }
            }
            else//다른 부쉬에 있다면
            {
                if (!(statScript.JobCode == 4 && statScript.isLuxS2ing))//럭스이고 스킬2 사용중이 아니라면 반투명하게 한다.
                {
                    statScript.SR.color = new Color(statScript.SR.color.r, statScript.SR.color.g, statScript.SR.color.b, 0f);
                    statScript.BaronBuffSR.color = new Color(1f, 1f, 1f, 0f);
                    statScript.SunFireImage_SR.color = new Color(1f, 1f, 1f, 0f);
                    if (statScript.JobCode == 2)
                    {
                        statScript.bladerScript.S2SR.color = new Color(1f, 1f, 1f, 0f);
                    }
                    if (NetworkManager.instance.MyStatScript.JobCode == 2)
                    {
                        statScript.BladerS1AuraImage.gameObject.SetActive(false);
                    }
                    statScript.NickNameText.enabled = false;
                    statScript.hpBar.gameObject.SetActive(false);
                }
            }
        }
    }


    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            //tempStatScript.OnBush = false;
            //if (!tempStatScript.DontChangeByOutBush)
            //{
            //    if (tempStatScript.BlueTeam == NetworkManager.instance.MyStatScript.BlueTeam)
            //    {
            //        if (tempStatScript.JobCode == 2) tempStatScript.bladerScript.S2SR.color = new Color(1f, 1f, 1f, 1f);
            //
            //        tempStatScript.BaronBuffSR.color = new Color(1f, 1f, 1f, 1f);
            //    }
            //    else
            //    {
            //        if (tempStatScript.SunFire)
            //        {
            //            tempStatScript.SunFireImage_SR.enabled = true;
            //        }
            //        if (tempStatScript.BaronBuffOn)
            //        {
            //            tempStatScript.BaronBuffSR.enabled = true;
            //        }
            //        if (tempStatScript.JobCode == 2)
            //        {
            //            if(tempStatScript.bladerScript.onS2) tempStatScript.bladerScript.S2SR.enabled = true;
            //        }
            //        tempStatScript.BladerS1AuraImage.gameObject.SetActive(true);
            //        tempStatScript.NickNameText.enabled = true;
            //        tempStatScript.hpBar.gameObject.SetActive(true);
            //    }
            //}

            tempStatScript.OnBush = false;
            tempStatScript.bushState = 0;
            ApplyBushState(NetworkManager.instance.Blue1statScript, NetworkManager.instance.Blue1statScript.BlueTeam == NetworkManager.instance.MyStatScript.BlueTeam);
            ApplyBushState(NetworkManager.instance.Blue2statScript, NetworkManager.instance.Blue2statScript.BlueTeam == NetworkManager.instance.MyStatScript.BlueTeam);
            ApplyBushState(NetworkManager.instance.Red1statScript, NetworkManager.instance.Red1statScript.BlueTeam == NetworkManager.instance.MyStatScript.BlueTeam);
            ApplyBushState(NetworkManager.instance.Red2statScript, NetworkManager.instance.Red2statScript.BlueTeam == NetworkManager.instance.MyStatScript.BlueTeam);
        }
    }
}
