using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BaronScript : GetDamageScript
{
    //게임 시작 후 처음 생기는 데 걸리는 시간
    public float generateTime;
    //파괴된 후 다시 생기는 데 걸리는 시간
    public float generateDealy;
    public float generateTimeCounter;
    public bool isAlive;

    [PunRPC]
    public void InitRPC()
    {
        Init();
    }

    public void Init()
    {
        NetworkManager.instance.BaronTimer.enabled = true;
        NetworkManager.instance.baronScript = this;

        //내가 Blader면 aura 켬
        if (NetworkManager.instance.MyStatScript.JobCode == 2) BladerS1AuraImage.enabled = true;
    }

    //쿨타임 돼서 생성될 때 실행될 코드
    public void Generate()
    {
        BlueTeam = !NetworkManager.instance.MyStatScript.BlueTeam;
        PlayerHP = PlayerMaxHP;
        AN.SetBool("blue", false);
        AN.SetBool("red", false);
        AN.SetTrigger("generate");
        SR.enabled = true;
        StartCoroutine(GenerateCRT());

        NetworkManager.instance.BaronTimer.enabled = false;
    }

    public IEnumerator GenerateCRT()
    {
        //sound
        NetworkManager.instance.soundObjectPool.GetObject((Vector2)transform.position, 28);
        yield return new WaitForSeconds(0.666667f);
        canvas.SetActive(true);
        MyCol.enabled = true;
    }

    public override void Update()
    {
        base.Update();
        if (!isAlive && NetworkManager.instance.GameStarted)
        {
            generateTimeCounter += Time.deltaTime;
        }
        if (generateTime < generateTimeCounter && !isAlive)
        {
            isAlive = true;
            generateTime = generateDealy;
            generateTimeCounter = 0f;
            Generate();
        }

        if (NetworkManager.instance != null && !isAlive)
        {
            NetworkManager.instance.BaronTimer.text = "바론 : " + ((int)(generateTime - generateTimeCounter)).ToString();
        }
    }

    public override void DeathCheck(int attackerPlayerNumber)
    {
        if (PlayerHP <= 0f)
        {
            //sound
            NetworkManager.instance.soundObjectPool.GetObject((Vector2)transform.position, 29);

            canvas.SetActive(false);
            MyCol.enabled = false;
            isAlive = false;

            //바론 킬로그 표시
            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("BuffRPC", RpcTarget.All, attackerPlayerNumber);
            }

            NetworkManager.instance.BaronTimer.enabled = true;
        }
    }

    public override void GetDamage(float ATK, float FixATK, float PerDamage, float PEN, float PerPEN, float StiffTime, int attackerPlayerNumber)
    {
        if (LinkerS1Connected)
        {
            teamStatScript.LinkerS1ConnectedGetDamage(ATK, FixATK, PerDamage, PEN, PerPEN, StiffTime, attackerPlayerNumber);
        }

        PV.RPC("StiffRPC", RpcTarget.All, StiffTime);

        float tempDEF = DEF * (1f - PerPEN * 0.01f) - PEN;
        if (tempDEF < 0f)
        {
            tempDEF = 0f;
        }


        float overDamage = 0f;

        while (true)
        {
            if (ShieldQue.Count < 1)
            {
                ShieldRemain = false;
                break;
            }

            TempShieldScript = ShieldQue.Dequeue();

            if (overDamage > 0f)
            {
                TempShieldScript.shield -= overDamage;
            }
            else
            {
                TempShieldScript.shield -= (100f / (tempDEF + 100f) * ATK + FixATK + PlayerHP * PerDamage * 0.01f) * (1f - DamageReduction);
            }

            if (TempShieldScript.shield < 0f)
            {
                overDamage -= TempShieldScript.shield;
            }
            else if (TempShieldScript.shield > 0f)
            {
                ShieldQue.Enqueue(TempShieldScript);
                overDamage = 0f;
                ShieldRemain = true;
                break;
            }
            else if (TempShieldScript.shield == 0f)
            {
                overDamage = 0f;
                ShieldRemain = true;
                break;
            }
        }


        if (overDamage > 0f)
        {
            PlayerHP -= overDamage;
        }
        else if (!ShieldRemain)
        {
            PlayerHP -= (100f / (tempDEF + 100f) * ATK + FixATK + PlayerHP * PerDamage * 0.01f) * (1f - DamageReduction);
        }
        PV.RPC("ShieldChangeRPC", RpcTarget.All, ShieldQueForRPC());
        PV.RPC("PlayerHPChangeRPC", RpcTarget.All, PlayerHP, attackerPlayerNumber);
    }

    [PunRPC]
    public override void StiffRPC(float time)
    {
        if (!AN.GetBool("red") && !AN.GetBool("blue"))
        {
            AN.SetTrigger("hurt");
        }
    }

    [PunRPC]
    public void BuffRPC(int attackerPlayerNumber)
    {
        //BlueTeam이 죽였으면 Blue팀 버프줌
        if (attackerPlayerNumber == 1 || attackerPlayerNumber == 4)
        {
            AN.SetBool("blue", true);
            StartCoroutine(ANBuffCRT());
            StartCoroutine(BuffATKCRT(NetworkManager.instance.Blue1statScript));
            NetworkManager.instance.Blue1statScript.ShieldQue.Enqueue(new ShieldScript(15f, 700f));
            StartCoroutine(NetworkManager.instance.Blue1statScript.BaronBuffCRT());
            if (NetworkManager.instance.Blue2statScript != null)
            {
                StartCoroutine(BuffATKCRT(NetworkManager.instance.Blue2statScript));
                NetworkManager.instance.Blue2statScript.ShieldQue.Enqueue(new ShieldScript(15f, 700f));
                StartCoroutine(NetworkManager.instance.Blue2statScript.BaronBuffCRT());
            }
        }
        else
        {
            AN.SetBool("red", true);
            StartCoroutine(ANBuffCRT());
            StartCoroutine(BuffATKCRT(NetworkManager.instance.Red1statScript));
            NetworkManager.instance.Red1statScript.ShieldQue.Enqueue(new ShieldScript(15f, 700f));
            StartCoroutine(NetworkManager.instance.Red1statScript.BaronBuffCRT());
            if (NetworkManager.instance.Red2statScript != null)
            {
                StartCoroutine(BuffATKCRT(NetworkManager.instance.Red2statScript));
                NetworkManager.instance.Red2statScript.ShieldQue.Enqueue(new ShieldScript(15f, 700f));
                StartCoroutine(NetworkManager.instance.Red2statScript.BaronBuffCRT());
            }
        }
    }

    public IEnumerator ANBuffCRT()
    {
        yield return new WaitForSeconds(0.733333f);
        SR.enabled = false;
        AN.SetBool("blue", false);
        AN.SetBool("red", false);
    }


    public IEnumerator BuffATKCRT(StatScript statScript)
    {
        statScript.ATK += 100f;
        yield return new WaitForSeconds(15f);
        statScript.ATK -= 100f;
    }



}
