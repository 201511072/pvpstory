using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class CaptureAreaScript : MonoBehaviourPunCallbacks
{
    public bool BlueTeamOn;
    public bool RedTeamOn;
    float BluePoint;
    float RedPoint;
    public Text BluePointText;
    public Text RedPointText;
    float BlueGauge;
    float RedGauge;
    public ImgsFillDynamic BlueGaugeIFD;
    public ImgsFillDynamic RedGaugeIFD;
    public enum CaptureState { None, BlueTeam, RedTeam };
    public CaptureState State;
    float BlueGaugeResetTime;
    float RedGaugeResetTime;
    public Sprite BlueOnSp;
    public Sprite BlueOffSp;
    public Sprite RedOnSp;
    public Sprite RedOffSp;

    public Sprite Blue_Left_On;
    public Sprite Blue_Left_Off;
    public Sprite Blue_Right_On;
    public Sprite Blue_Right_Off;
    public Sprite Red_Left_On;
    public Sprite Red_Left_Off;
    public Sprite Red_Right_On;
    public Sprite Red_Right_Off;


    public Image BluePointImg;
    public Image RedPointImg;
    public PhotonView PV;
    bool BlueTeamCaptured1Time;
    bool RedTeamCaptured1Time;
    public GameObject ResultPanel;
    public GameObject VictoryImage;
    public GameObject DefeatImage;
    public GameObject RespawnPanel;

    public BoxCollider2D captureAreaCol;
    public Animator AN;

    public bool GameEnd;

    public bool Result1Time;


    public override void OnEnable()
    {
        base.OnEnable();
        BlueGaugeIFD.SetValue(0f);
        RedGaugeIFD.SetValue(0f);
        BlueGauge = 0f;
        RedGauge = 0f;
        BluePoint = 0f;
        RedPoint = 0f;
        BlueGaugeResetTime = 0f;
        RedGaugeResetTime = 0f;
        BlueTeamCaptured1Time = false;
        RedTeamCaptured1Time = false;
        GameEnd = false;
        State = CaptureState.None;
        BluePointText.text = 0 + " %";
        RedPointText.text = 0 + " %";
        BluePointImg.sprite = BlueOffSp;
        RedPointImg.sprite = RedOffSp;
        BlueTeamOn = false;
        RedTeamOn = false;
        AN.SetTrigger("idle");
        Result1Time = true;
    }

    public void SetPointColor(bool isBlueTeam)
    {
        if (isBlueTeam)
        {
            BlueOnSp = Blue_Left_On;
            BlueOffSp = Blue_Left_Off;
            RedOnSp = Red_Right_On;
            RedOffSp = Red_Right_Off;

            BlueGaugeIFD.FillRound.ImgFill.color = Color.blue;
            BlueGaugeIFD.FillRound.ImgStartDot.color = Color.blue;
            BlueGaugeIFD.TxtValue.color= Color.blue;
            RedGaugeIFD.FillRound.ImgFill.color = Color.red;
            RedGaugeIFD.FillRound.ImgStartDot.color = Color.red;
            RedGaugeIFD.TxtValue.color = Color.red;
        }
        else
        {
            BlueOnSp = Red_Left_On;
            BlueOffSp = Red_Left_Off;
            RedOnSp = Blue_Right_On;
            RedOffSp = Blue_Right_Off;

            BlueGaugeIFD.FillRound.ImgFill.color = Color.red;
            BlueGaugeIFD.FillRound.ImgStartDot.color = Color.red;
            BlueGaugeIFD.TxtValue.color = Color.red;
            RedGaugeIFD.FillRound.ImgFill.color = Color.blue;
            RedGaugeIFD.FillRound.ImgStartDot.color = Color.blue;
            RedGaugeIFD.TxtValue.color = Color.blue;
        }

        BluePointImg.sprite = BlueOffSp;
        RedPointImg.sprite = RedOffSp;
    }


    public void captureAreaColOnOff(bool value)
    {
        captureAreaCol.enabled = value;
    }


    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (col.GetComponent<StatScript>().BlueTeam)
            {
                BlueTeamOn = true;
            }
            if (!col.GetComponent<StatScript>().BlueTeam)
            {
                RedTeamOn = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (col.GetComponent<StatScript>().BlueTeam)
            {
                BlueTeamOn = false;
            }
            else
            {
                RedTeamOn = false;
            }
        }
    }

    private void Update()
    {
        if (!GameEnd)
        {
            if (BlueTeamOn && !RedTeamOn)
            {
                BlueGaugeResetTime = 0f;
                if (State == CaptureState.None)
                {
                    if (RedGauge > 0f)
                    {
                        RedGauge -= Time.deltaTime * 30f;
                    }
                    else if (BlueGauge < 100f)
                    {
                        BlueGauge += Time.deltaTime * 30f;
                    }
                    else
                    {
                        PV.RPC("BlueTeamCaptured", RpcTarget.All);
                    }
                }
                if (State == CaptureState.RedTeam)
                {
                    if (BlueGauge < 100f)
                    {
                        BlueGauge += Time.deltaTime * 30f;
                    }
                    else
                    {
                        PV.RPC("BlueTeamCaptured", RpcTarget.All);
                    }
                }
            }

            else if (!BlueTeamOn && RedTeamOn)
            {
                RedGaugeResetTime = 0f;
                if (State == CaptureState.None)
                {
                    if (BlueGauge > 0f)
                    {
                        BlueGauge -= Time.deltaTime * 30f;
                    }
                    else if (RedGauge < 100f)
                    {
                        RedGauge += Time.deltaTime * 30f;
                    }
                    else
                    {
                        PV.RPC("RedTeamCaptured", RpcTarget.All);
                    }
                }
                if (State == CaptureState.BlueTeam)
                {
                    if (RedGauge < 100f)
                    {
                        RedGauge += Time.deltaTime * 30f;
                    }
                    else
                    {
                        PV.RPC("RedTeamCaptured", RpcTarget.All);
                    }
                }
            }

            else if (BlueTeamOn && RedTeamOn)
            {
                BlueGaugeResetTime = 0f;
                RedGaugeResetTime = 0f;
                //접전중 이미지 켜주기
            }

            if (!BlueTeamOn && BlueGauge > 0f)
            {
                BlueGaugeResetTime += Time.deltaTime;
                if (BlueGaugeResetTime > 5f)
                {
                    BlueGauge -= Time.deltaTime * 30f;
                }
            }

            if (!RedTeamOn && RedGauge > 0f)
            {
                RedGaugeResetTime += Time.deltaTime;
                if (RedGaugeResetTime > 5f)
                {
                    RedGauge -= Time.deltaTime * 30f;
                }
            }

            if (State == CaptureState.BlueTeam)
            {
                BluePoint += Time.deltaTime;
                //BluePoint += Time.deltaTime * 40f;
                BluePointText.text = Mathf.FloorToInt(BluePoint) + " %";
                if (BluePoint >= 100f)
                {
                    GameEnd = true;
                    PV.RPC("BlueTeamWin", RpcTarget.All);
                }
            }
            else if (State == CaptureState.RedTeam)
            {
                RedPoint += Time.deltaTime;
                RedPointText.text = Mathf.FloorToInt(RedPoint) + " %";
                if (RedPoint >= 100f)
                {
                    GameEnd = true;
                    PV.RPC("RedTeamWin", RpcTarget.All);
                }
            }

            if (BlueGauge > 0f)
            {
                if (!BlueGaugeIFD.gameObject.activeSelf)
                {
                    BlueGaugeIFD.gameObject.SetActive(true);
                }
                BlueGaugeIFD.SetValue(BlueGauge * 0.01f, true);
            }
            else if (BlueGaugeIFD.gameObject.activeSelf)
            {
                BlueGaugeIFD.gameObject.SetActive(false);
            }

            if (RedGauge > 0f)
            {
                if (!RedGaugeIFD.gameObject.activeSelf)
                {
                    RedGaugeIFD.gameObject.SetActive(true);
                }
                RedGaugeIFD.SetValue(RedGauge * 0.01f, true);
            }
            else if (RedGaugeIFD.gameObject.activeSelf)
            {
                RedGaugeIFD.gameObject.SetActive(false);
            }
        }
    }

    [PunRPC]
    void BlueTeamCaptured()
    {
        if (!BlueTeamCaptured1Time)
        {
            StartCoroutine(BlueTeamCaptured1TimeCRT());
            BlueTeamCaptured1Time = true;
            State = CaptureState.BlueTeam;
            BluePointImg.sprite = BlueOnSp;
            RedPointImg.sprite = RedOffSp;
            BlueGauge = 0f;
            RedGauge = 0f;
            AN.SetTrigger("blue");
        }
    }

    [PunRPC]
    void RedTeamCaptured()
    {
        if (!RedTeamCaptured1Time)
        {
            StartCoroutine(RedTeamCaptured1TimeCRT());
            State = CaptureState.RedTeam;
            RedPointImg.sprite = RedOnSp;
            BluePointImg.sprite = BlueOffSp;
            RedGauge = 0f;
            BlueGauge = 0f;
            AN.SetTrigger("red");
        }
    }

    IEnumerator BlueTeamCaptured1TimeCRT()
    {
        BlueTeamCaptured1Time = true;
        yield return new WaitForSeconds(0.2f);
        BlueTeamCaptured1Time = false;
    }
    IEnumerator RedTeamCaptured1TimeCRT()
    {
        RedTeamCaptured1Time = true;
        yield return new WaitForSeconds(0.2f);
        RedTeamCaptured1Time = false;
    }

    [PunRPC]
    void BlueTeamWin()
    {
        if (Result1Time)
        {
            Result1Time = false;
            NetworkManager.instance.soundObjectPool.GetObject((Vector2)NetworkManager.instance.MyStatScript.transform.position, 33);
            RespawnPanel.SetActive(false);
            ResultPanel.SetActive(true);
            if (NetworkManager.instance.myPlayerNumber == 1 || NetworkManager.instance.myPlayerNumber == 4)
            {
                VictoryImage.SetActive(true);
                NetworkManager.instance.RankPoint += NetworkManager.instance.WinReward;
                NetworkManager.instance.isMyTeamWin = true;
            }
            else if (NetworkManager.instance.myPlayerNumber == 2 || NetworkManager.instance.myPlayerNumber == 3)
            {
                DefeatImage.SetActive(true);
                NetworkManager.instance.RankPoint += NetworkManager.instance.LoseReward;
                NetworkManager.instance.isMyTeamWin = false;
            }
            if (!NetworkManager.instance.isTempAccount)
            {
                NetworkManager.instance.playfabManager.SetStat();
            }
            NetworkManager.instance.RankPointText2.text = Convert.ToString(NetworkManager.instance.RankPoint);
            NetworkManager.instance.EndLobbyScript.Init();
        }
    }

    [PunRPC]
    void RedTeamWin()
    {
        if (Result1Time)
        {
            Result1Time = false;
            NetworkManager.instance.soundObjectPool.GetObject((Vector2)NetworkManager.instance.MyStatScript.transform.position, 33);
            RespawnPanel.SetActive(false);
            ResultPanel.SetActive(true);
            if (NetworkManager.instance.myPlayerNumber == 1 || NetworkManager.instance.myPlayerNumber == 4)
            {
                DefeatImage.SetActive(true);
                NetworkManager.instance.RankPoint += NetworkManager.instance.LoseReward;
                NetworkManager.instance.isMyTeamWin = false;
            }
            else if (NetworkManager.instance.myPlayerNumber == 2 || NetworkManager.instance.myPlayerNumber == 3)
            {
                VictoryImage.SetActive(true);
                NetworkManager.instance.RankPoint += NetworkManager.instance.WinReward;
                NetworkManager.instance.isMyTeamWin = true;
            }
            if (!NetworkManager.instance.isTempAccount)
            {
                NetworkManager.instance.playfabManager.SetStat();
            }
            NetworkManager.instance.RankPointText2.text = Convert.ToString(NetworkManager.instance.RankPoint);
            NetworkManager.instance.EndLobbyScript.Init();
        }
    }
}