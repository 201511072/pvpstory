using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Cinemachine;

public class LuxScript : Character_Base
{

    //header(skill1)
    public GameObject Skill1Object;
    public GameObject canvas;
    public LuxAttackObjectPool luxAttackObjectPool;
    public LuxS1ObjectPool luxS1ObjectPool;
    public LuxS2ObjectPool luxS2ObjectPool;
    public LuxUltObjectPool luxUltObjectPool;

    public GameObject Ball;
    public SpriteRenderer BallSR;
    public Animator BallAN;
    public Lux_SoundScript lux_SoundScript;

    public override void Start()
    {
        base.Start();
        statScript.JobCode = 4;
        statScript.luxScript = this;
    }

    public override void HurtMotion()
    {
        if (statScript.noHurtMotion)
        {
            statScript.stiffTime = 0f;
            statScript.stunTime = 0f;
        }

        if (statScript.stiffTime > 0)
        {
            statScript.stiffTime += -Time.deltaTime;
        }

        if (statScript.stunTime > 0)
        {
            statScript.stunTime += -Time.deltaTime;
        }

        if (statScript.stiffTime > 0 || statScript.stunTime > 0)
        {
            if (!AN.GetBool(hurtID))
            {
                AN.SetBool(attackID, false);
                AN.SetBool(skill1ID, false);
                AN.SetBool(ultID, false);
                AN.SetBool(walkID, false);
                AN.SetBool(jumpID, false);
                AN.SetBool(fallID, false);
                //모든모션 끄기 (모든 캐릭터들마다 override해서 전부 끄게 만들기)
                AN.SetBool(hurtID, true);
            }
        }
        else if (AN.GetBool(hurtID))
        {
            AN.SetBool(hurtID, false);
        }

        if (statScript.stunTime > 0)
        {
            if (!statScript.stunSR.enabled)
            {
                statScript.stunSR.enabled = true;
            }
        }
        else
        {
            if (statScript.stunSR.enabled)
            {
                statScript.stunSR.enabled = false;
            }
        }
    }


    public override void Death()
    {
        AN.SetBool(attackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        //모든모션 끄기
        AN.SetBool(deathID, true);
        StartCoroutine(DeathAgainCRT());
        if (PV.IsMine)
        {
            NetworkManager.instance.CooltimerCountZeroEvent.Invoke();
        }

        statScript.OnSunFire = false;
    }

    public IEnumerator DeathAgainCRT()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.05f);
            AN.SetBool(attackID, false);
            AN.SetBool(skill1ID, false);
            AN.SetBool(ultID, false);
            AN.SetBool(hurtID, false);
            AN.SetBool(walkID, false);
            AN.SetBool(jumpID, false);
            AN.SetBool(fallID, false);
            if (floatingJoystick != null)
            {
                floatingJoystick.Horizontal = 0f;
                floatingJoystick.Vertical = 0f;
            }

            statScript.OnSunFire = false;
        }
    }

    public override void ANWalk()
    {
        if (!AN.GetBool(jumpID) && !AN.GetBool(fallID) && !AN.GetBool(deathID))
        {
            AN.SetBool(attackID, false);
            AN.SetBool(skill1ID, false);
            AN.SetBool(ultID, false);
            AN.SetBool(hurtID, false);
            AN.SetBool(jumpID, false);
            AN.SetBool(fallID, false);
            AN.SetBool(walkID, true);
        }
    }




    void Update()
    {
        if (PV.IsMine)
        {
            isGround = Physics2D.OverlapArea((Vector2)transform.position + new Vector2(-0.37f, -0.485f), (Vector2)transform.position + new Vector2(0.37f, -0.985f), 11 << 9);

            if (isGround && Land && !AN.GetBool(attackID) && !AN.GetBool(skill1ID) && !AN.GetBool(ultID) && !AN.GetBool(hurtID) && !AN.GetBool(jumpID) &&
                !AN.GetBool(deathID))
            {
                Land = false;
                PV.RPC("ANLandRPC", RpcTarget.All);
            }

            if (!isGround && !Land)
            {
                Land = true;
            }


            if (!statScript.isJumping && isGround)
            {
                if (AN.GetBool(jumpID))
                {
                    AN.SetBool(jumpID, false);
                }
            }

            if (!isGround && !AN.GetBool(fallID) && !AN.GetBool(attackID) && !AN.GetBool(skill1ID) && !AN.GetBool(ultID) && !AN.GetBool(hurtID) && !AN.GetBool(jumpID) &&
                !AN.GetBool(deathID))
            {
                PV.RPC("ANFallRPC", RpcTarget.All, true);
            }

            if (isGround && AN.GetBool(fallID))
            {
                PV.RPC("ANFallRPC", RpcTarget.All, false);
            }


            if (UltGauge < 100f)
            {
                UltGauge += Time.deltaTime * UltGaugeSpeed;
            }
        }
    }


    [PunRPC]
    public void ANLandRPC()
    {
        AN.SetTrigger(landID);
    }

    [PunRPC]
    public void ANFallRPC(bool value)
    {
        AN.SetBool(fallID, value);
    }



    public IEnumerator MoveLockCR(float value)
    {
        statScript.JumpLock = true;
        statScript.MoveLock = true;
        statScript.SkillLock = true;
        RB.constraints = RigidbodyConstraints2D.FreezeAll;
        RB.velocity = new Vector2(0.0f, 0.0f);
        yield return new WaitForSeconds(value);
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        RB.velocity = new Vector2(0.0f, 0.0000001f);
        statScript.JumpLock = false;
        statScript.MoveLock = false;
        statScript.SkillLock = false;
    }


    public override void Attack()
    {
        StartCoroutine(doAttack());
    }

    public IEnumerator doAttack()
    {
        Vector2 tempVector;
        bool temp_flipX;
        if (skillJoystick != Vector2.zero)
        {
            tempVector = skillJoystick;
            if (!statScript.Aggroed)
            {
                SR.flipX = tempVector.x < 0;
            }
            temp_flipX = tempVector.x < 0;
        }
        else
        {
            tempVector = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical);
            temp_flipX = SR.flipX;
        }

        if (statScript.Aggroed)
        {
            tempVector = (statScript.AggroedVector - (Vector2)transform.position).normalized;
        }
        else
        {
            if (tempVector.x == 0f && tempVector.y == 0f)
            {
                tempVector.x = temp_flipX ? -1f : 1f;
            }
            else
            {
                if ((tempVector.x > -0.000001f && tempVector.x < 0.000001f) && (tempVector.y > -0.000001f && tempVector.y < 0.000001f))
                {
                    if (tempVector.x * tempVector.x < tempVector.y * tempVector.y)
                    {
                        tempVector.x = 0f;
                        tempVector.y = 1f;
                    }
                    else
                    {
                        tempVector.x = 1f;
                        tempVector.y = 0f;
                    }
                }
                tempVector = tempVector.normalized;
            }
        }
        Vector2 tempPosition = (Vector2)transform.position + tempVector * 0.35f;//0.4였음
        PV.RPC("ANAttackRPC", RpcTarget.All, tempPosition.x, tempPosition.y, tempVector.x, tempVector.y);
        StartCoroutine(MoveLockCR(0.2f));
        yield return new WaitForSeconds(0.2f);
        PV.RPC("AttackRPC", RpcTarget.All, tempPosition.x, tempPosition.y, temp_flipX, statScript.BlueTeam, tempVector.x, tempVector.y);
    }

    [PunRPC]
    public void ANAttackRPC(float positionX, float positionY, float vectorX, float vectorY)
    {
        Ball.transform.position = new Vector2(positionX, positionY);
        Ball.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(vectorY, vectorX));
        BallSR.flipY = vectorX < 0;
        BallAN.SetTrigger(attackID);
        if (statScript.OnBush)
        {
            if (NetworkManager.instance.MyStatScript.BlueTeam != statScript.BlueTeam)
            {
                BallSR.enabled = false;
            }
            else
            {
                BallSR.enabled = true;BallSR.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
        else { BallSR.enabled = true; BallSR.color = new Color(1f, 1f, 1f, 1f); }
        StartCoroutine(ANAttackCRT());

        //sound
        lux_SoundScript.AS_Attack_Play();
    }

    [PunRPC]
    public void AttackRPC(float positionX, float positionY, bool flipX, bool isBlueTeam, float vectorX, float vectorY)
    {
        luxAttackObjectPool.GetObject(positionX, positionY, flipX, isBlueTeam, vectorX, vectorY);
    }


    public IEnumerator ANAttackCRT()
    {
        AN.SetBool(skill1ID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(attackID, true);
        yield return new WaitForSeconds(0.2f);
        AN.SetBool(attackID, false);
        BallSR.enabled = false;
    }


    public override void Skill1()
    {
        Vector2 tempVector;
        bool temp_flipX;
        if (skillJoystick != Vector2.zero)
        {
            tempVector = skillJoystick;
            if (!statScript.Aggroed)
            {
                SR.flipX = tempVector.x < 0;
            }
            temp_flipX = tempVector.x < 0;
        }
        else
        {
            tempVector = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical);
            temp_flipX = SR.flipX;
        }

        if (statScript.Aggroed)
        {
            tempVector = (statScript.AggroedVector - (Vector2)transform.position).normalized;
        }
        else
        {
            if (tempVector.x == 0f && tempVector.y == 0f)
            {
                tempVector.x = temp_flipX ? -1f : 1f;
            }
        }

        Vector2 tempPosition = (Vector2)transform.position + tempVector * 0.8f;
        Vector2 tempPosition_Ball = (Vector2)transform.position + tempVector * 0.35f;

        Quaternion rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(tempVector.y, tempVector.x));

        bool flipY = tempVector.x < 0;

        PV.RPC("ANSkill1RPC", RpcTarget.All, tempPosition_Ball, rotation, flipY);
        PV.RPC("Skill1RPC", RpcTarget.All, tempPosition, rotation);
    }

    [PunRPC]
    public void ANSkill1RPC(Vector2 position, Quaternion rotation, bool flipY)
    {
        StartCoroutine(MoveLockCR(0.6f));
        StartCoroutine(ANSkill1CRT(position, rotation, flipY));
    }

    public IEnumerator ANSkill1CRT(Vector2 position, Quaternion rotation, bool flipY)
    {
        //sound
        lux_SoundScript.AS_Skill1_Play();
        statScript.noHurtMotion = true;
        Ball.transform.position = position;
        Ball.transform.rotation = rotation;
        BallSR.flipY = flipY;
        BallAN.SetTrigger(skill1ID);
        if (statScript.OnBush)
        {
            if (NetworkManager.instance.MyStatScript.BlueTeam != statScript.BlueTeam)
            {
                BallSR.enabled = false;
            }
            else
            {
                BallSR.enabled = true; BallSR.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
        else { BallSR.enabled = true; BallSR.color = new Color(1f, 1f, 1f, 1f); }
        AN.SetBool(attackID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(skill1ID, true);
        yield return new WaitForSeconds(0.6f);
        AN.SetBool(skill1ID, false);
        BallSR.enabled = false;
        statScript.noHurtMotion = false;
    }


    [PunRPC]
    void Skill1RPC(Vector2 position, Quaternion rotation)
    {
        luxS1ObjectPool.GetObject(position, rotation);
    }


    public override void Skill2()
    {
        PV.RPC("Skill2RPC", RpcTarget.All, (Vector2)transform.position, SR.flipX, statScript.OnBush);
    }

    [PunRPC]
    void Skill2RPC(Vector2 position, bool flipX, bool OnBush)
    {
        if (NetworkManager.instance.MyStatScript.BlueTeam == statScript.BlueTeam) StartCoroutine(S2TeamCRT(1.0f));
        else StartCoroutine(S2EnemyCRT(1.0f));

        luxS2ObjectPool.GetObject(position, flipX, OnBush);
    }

    IEnumerator S2TeamCRT(float time)
    {
        statScript.isLuxS2ing = true;//부쉬 계산용
        lux_SoundScript.AS_Skill2_On_Play();
        statScript.DontChangeByOutBush = true;
        //if (!statScript.OnBush) SR.color = new Color(1f, 1f, 1f, 0.5f);
        SR.color = new Color(statScript.SR.color.r, statScript.SR.color.g, statScript.SR.color.b, 0.5f);

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


        PV.RPC("DontHitByArrowRPC", RpcTarget.All, true);
        statScript.Speed_ADD(0, time, 1f);
        statScript.SkillLock = true;

        yield return new WaitForSeconds(time);
        statScript.SkillLock = false;
        statScript.DontChangeByOutBush = false;
        statScript.isLuxS2ing = false;
        //if (!statScript.OnBush) SR.color = new Color(1f, 1f, 1f, 1f);

        //if (!statScript.OnBush)
        //{
        //    statScript.SR.color = new Color(statScript.SR.color.r, statScript.SR.color.g, statScript.SR.color.b, 1f);
        //    statScript.BaronBuffSR.color = Color.white;
        //    statScript.SunFireImage_SR.color = Color.white;
        //    if (statScript.JobCode == 2)
        //    {
        //        statScript.bladerScript.S2SR.color = Color.white;
        //    }
        //    statScript.BaronBuffSR.color = Color.white;
        //    if (NetworkManager.instance.MyStatScript.JobCode == 2)
        //    {
        //        statScript.BladerS1AuraImage.gameObject.SetActive(true);
        //    }
        //    statScript.NickNameText.enabled = true;
        //    statScript.hpBar.gameObject.SetActive(true);
        //}
        ApplyBushState(statScript, statScript.BlueTeam == NetworkManager.instance.MyStatScript.BlueTeam);
        PV.RPC("DontHitByArrowRPC", RpcTarget.All, false);
        lux_SoundScript.AS_Skill2_Off_Play();
        statScript.isLuxS2ing = false;//부쉬 계산용
    }

    public void ApplyBushState(StatScript statScript, bool isMyTeam)
    {
        if (statScript.bushState == 0)//부쉬 밖이면
        {
            if (!(statScript.JobCode == 4 && statScript.isLuxS2ing))//럭스이고 스킬2 사용중이 아니라면 안투명하게 한다.
            {
                statScript.SR.color = new Color(statScript.SR.color.r, statScript.SR.color.g, statScript.SR.color.b, 1f);
                statScript.BaronBuffSR.color = Color.white;
                statScript.SunFireImage_SR.color = Color.white;
                if (statScript.JobCode == 2)
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
        else if (statScript.myPlayerNumber == 1 || statScript.myPlayerNumber == 4)//적군이 블루팀이고
        {
            if (statScript.bushState == NetworkManager.instance.Red1statScript.bushState || statScript.bushState == NetworkManager.instance.Red2statScript.bushState)//아군과 같은 부쉬에 있다면
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

    IEnumerator S2EnemyCRT(float time)
    {
        statScript.isLuxS2ing = true;//부쉬 계산용
        lux_SoundScript.AS_Skill2_On_Play();
        statScript.DontChangeByOutBush = true;
        //if (!statScript.OnBush) SR.color = new Color(1f, 1f, 1f, 0f);
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

        canvas.SetActive(false);
        PV.RPC("DontHitByArrowRPC", RpcTarget.All, true);
        statScript.Speed_ADD(0, time, 1f);
        statScript.SkillLock = true;

        yield return new WaitForSeconds(time);
        canvas.SetActive(true);
        statScript.SkillLock = false;
        statScript.DontChangeByOutBush = false;
        //if (!statScript.OnBush)
        //{
        //    SR.color = new Color(1f, 1f, 1f, 1f);
        //    statScript.BladerS1AuraImage.gameObject.SetActive(true);
        //    statScript.NickNameText.enabled = true;
        //    statScript.hpBar.gameObject.SetActive(true);
        //}
        //if (!statScript.OnBush)
        //{
        //    statScript.SR.color = new Color(statScript.SR.color.r, statScript.SR.color.g, statScript.SR.color.b, 1f);
        //    statScript.BaronBuffSR.color = Color.white;
        //    statScript.SunFireImage_SR.color = Color.white;
        //    if (statScript.JobCode == 2)
        //    {
        //        statScript.bladerScript.S2SR.color = Color.white;
        //    }
        //    statScript.BaronBuffSR.color = Color.white;
        //    if (NetworkManager.instance.MyStatScript.JobCode == 2)
        //    {
        //        statScript.BladerS1AuraImage.gameObject.SetActive(true);
        //    }
        //    statScript.NickNameText.enabled = true;
        //    statScript.hpBar.gameObject.SetActive(true);
        //}
        statScript.isLuxS2ing = false;
        ApplyBushState(statScript, statScript.BlueTeam == NetworkManager.instance.MyStatScript.BlueTeam);

        PV.RPC("DontHitByArrowRPC", RpcTarget.All, false);
        lux_SoundScript.AS_Skill2_Off_Play();
        statScript.isLuxS2ing = false;//부쉬 계산용
    }


    public override void Ult()
    {
        Vector2 tempVector;
        bool temp_flipX;
        if (skillJoystick != Vector2.zero)
        {
            tempVector = skillJoystick;
            if (!statScript.Aggroed)
            {
                SR.flipX = tempVector.x < 0;
            }
            temp_flipX = tempVector.x < 0;
        }
        else
        {
            tempVector = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical);
            temp_flipX = SR.flipX;
        }

        if (statScript.Aggroed)
        {
            tempVector = (statScript.AggroedVector - (Vector2)transform.position).normalized;
        }
        else
        {
            if (tempVector.x == 0f && tempVector.y == 0f)
            {
                tempVector.x = temp_flipX ? -1f : 1f;
            }
        }

        Vector2 tempPosition = (Vector2)transform.position + tempVector * 0.8f;//0.4였고 Ball이 따로 없이 같은 값을 사용했었음
        Vector2 tempPosition_Ball= (Vector2)transform.position + tempVector * 0.35f;

        Quaternion rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(tempVector.y, tempVector.x));

        bool flipY = tempVector.x < 0;

        PV.RPC("ANUltRPC", RpcTarget.All, tempPosition_Ball, rotation, flipY);
        PV.RPC("UltRPC", RpcTarget.All, tempPosition, rotation);
    }


    [PunRPC]
    public void ANUltRPC(Vector2 position, Quaternion rotation, bool flipY)
    {
        StartCoroutine(MoveLockCR(1.5f));
        StartCoroutine(ANUltCRT(position, rotation, flipY));
    }

    public IEnumerator ANUltCRT(Vector2 position, Quaternion rotation, bool flipY)
    {
        statScript.noHurtMotion = true;
        Ball.transform.position = position;
        Ball.transform.rotation = rotation;
        BallSR.flipY = flipY;
        BallAN.SetTrigger(ultID);
        if (statScript.OnBush)
        {
            if (NetworkManager.instance.MyStatScript.BlueTeam != statScript.BlueTeam)
            {
                BallSR.enabled = false;
            }
            else
            {
                BallSR.enabled = true; BallSR.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
        else { BallSR.enabled = true; BallSR.color = new Color(1f, 1f, 1f, 1f); }
        AN.SetBool(attackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(ultID, true);
        yield return new WaitForSeconds(1.5f);
        AN.SetBool(ultID, false);
        BallSR.enabled = false;
        statScript.noHurtMotion = false;
    }


    [PunRPC]
    void UltRPC(Vector2 position, Quaternion rotation)
    {
        lux_SoundScript.AS_Ult_Play();
        luxUltObjectPool.GetObject(position, rotation);
    }
}
