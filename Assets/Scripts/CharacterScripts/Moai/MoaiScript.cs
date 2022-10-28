using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoaiScript : Character_Base
{
    public Animator AN1;//not ult
    public Animator AN2;//ult
    public SpriteRenderer SR1;//not ult
    public SpriteRenderer SR2;//ult

    public MoaiAttackRangeScript moaiAttackRangeScript;
    public bool ANFallDisabled;

    //header(skill1)
    public BoxCollider2D MoaiSkill1Col;
    public bool isSkill1;
    public bool ANLandDisabled;
    public MoaiS1ObjectPool moaiS1ObjectPool;

    //header(skill2)
    public Collider2D S2Hitbox;
    public SpriteRenderer S2HitboxSR;
    public Animator S2HitboxAN;
    public MoaiS2HitboxScript S2HitboxScript;
    public MoaiS2EndHitboxScript S2EndHitboxScript;

    public bool onUlt;
    public bool ANUlting;//ult가 풀렸을때 an1이 자연스럽게 이어지기 위해서 an1도 실행해주기 위함


    public int ANShortNameHash;

    public Moai_SoundScript moai_SoundScript;



    public override void Start()
    {
        base.Start();
        statScript.JobCode = 3;
        statScript.moaiScript = this;

        AN = AN1;
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
                AN.SetBool(skill1ID, false);
                AN.SetBool(skill2StartID, false);
                AN.SetBool(skill2ID, false);
                AN.SetBool(skill2EndID, false);
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
        AN1.SetBool(skill1ID, false);
        AN1.SetBool(skill2StartID, false);
        AN1.SetBool(skill2ID, false);
        AN1.SetBool(skill2EndID, false);
        AN1.SetBool(ultID, false);
        AN1.SetBool(walkID, false);
        AN1.SetBool(hurtID, false);
        AN1.SetBool(jumpID, false);
        AN1.SetBool(fallID, false);
        //모든모션 끄기
        AN1.SetBool(deathID, true);

        AN2.SetBool(skill1ID, false);
        AN2.SetBool(skill2StartID, false);
        AN2.SetBool(skill2ID, false);
        AN2.SetBool(skill2EndID, false);
        AN2.SetBool(ultID, false);
        AN2.SetBool(walkID, false);
        AN2.SetBool(hurtID, false);
        AN2.SetBool(jumpID, false);
        AN2.SetBool(fallID, false);
        //모든모션 끄기
        AN2.SetBool(deathID, true);
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
            AN1.SetBool(skill1ID, false);
            AN1.SetBool(skill2StartID, false);
            AN1.SetBool(skill2ID, false);
            AN1.SetBool(skill2EndID, false);
            AN1.SetBool(ultID, false);
            AN1.SetBool(walkID, false);
            AN1.SetBool(hurtID, false);
            AN1.SetBool(jumpID, false);
            AN1.SetBool(fallID, false);

            AN2.SetBool(skill1ID, false);
            AN2.SetBool(skill2StartID, false);
            AN2.SetBool(skill2ID, false);
            AN2.SetBool(skill2EndID, false);
            AN2.SetBool(ultID, false);
            AN2.SetBool(walkID, false);
            AN2.SetBool(hurtID, false);
            AN2.SetBool(jumpID, false);
            AN2.SetBool(fallID, false);
            if (floatingJoystick != null)
            {
                floatingJoystick.Horizontal = 0f;
                floatingJoystick.Vertical = 0f;
            }

            statScript.OnSunFire = false;
        }
    }

    public override void DeathEnd()
    {
        SR1.enabled = false;
        SR2.enabled = false;
        AN1.SetBool(deathID, false);
        AN2.SetBool(deathID, false);        
    }



    public override void ANWalk()
    {
        if (!AN.GetBool(jumpID) && !AN.GetBool(fallID) && !AN.GetBool(deathID))
        {
            AN.SetBool(skill1ID, false);
            AN.SetBool(skill2StartID, false);
            AN.SetBool(skill2ID, false);
            AN.SetBool(skill2EndID, false);
            AN.SetBool(ultID, false);
            AN.SetBool(hurtID, false);
            AN.SetBool(jumpID, false);
            AN.SetBool(fallID, false);
            AN.SetBool(walkID, true);
            if (ANUlting)
            {
                AN1.SetBool(skill1ID, false);
                AN1.SetBool(skill2StartID, false);
                AN1.SetBool(skill2ID, false);
                AN1.SetBool(skill2EndID, false);
                AN1.SetBool(ultID, false);
                AN1.SetBool(hurtID, false);
                AN1.SetBool(jumpID, false);
                AN1.SetBool(fallID, false);
                AN1.SetBool(walkID, true);
            }
        }
    }

    public override void ANWalkFalse()
    {
        AN.SetBool(walkID, false);
        if (ANUlting)
        {
            AN1.SetBool(walkID, false);
        }
    }

    [PunRPC]
    public void ANLandRPC()
    {
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2StartID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(skill2EndID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(jumpID, false);
        AN.SetTrigger(landID);
        if (ANUlting)
        {
            AN1.SetTrigger(landID);
        }
        moai_SoundScript.AS_Land_Play();
    }

    [PunRPC]
    public void ANFallRPC(bool value)
    {
        if (value)
        {
            AN.SetBool(skill1ID, false);
            AN.SetBool(skill2StartID, false);
            AN.SetBool(skill2ID, false);
            AN.SetBool(skill2EndID, false);
            AN.SetBool(ultID, false);
            AN.SetBool(walkID, false);
            AN.SetBool(hurtID, false);
            AN.SetBool(jumpID, false);
        }
        AN.SetBool(fallID, value);
        if (ANUlting)
        {
            if (value)
            {
                AN1.SetBool(skill1ID, false);
                AN1.SetBool(skill2StartID, false);
                AN1.SetBool(skill2ID, false);
                AN1.SetBool(skill2EndID, false);
                AN1.SetBool(ultID, false);
                AN1.SetBool(walkID, false);
                AN1.SetBool(hurtID, false);
                AN1.SetBool(jumpID, false);
            }
            AN1.SetTrigger(landID);
        }
    }


    private void Update()
    {
        if (PV.IsMine)
        {
            //(ground,wall)Layer만 감지
            isGround = Physics2D.OverlapArea((Vector2)transform.position + new Vector2(-0.777f, -0.885f), (Vector2)transform.position + new Vector2(0.777f, -1.38f), 11 << 9);

            ANShortNameHash = AN.GetCurrentAnimatorStateInfo(0).shortNameHash;

            if (isGround && Land && ANShortNameHash != attack1ID && ANShortNameHash != attack2ID && !AN.GetBool(skill1ID) && ANShortNameHash != skill1EndID && !AN.GetBool(skill2ID) && !AN.GetBool(hurtID) && !AN.GetBool(jumpID) && !AN.GetBool(deathID) &&
                !AN.GetBool(ultID) && !ANLandDisabled)
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
                    if (ANUlting)
                    {
                        AN1.SetBool(jumpID, false);
                    }
                }
            }

            if (!isGround && !AN.GetBool(fallID) && ANShortNameHash != attack1ID && ANShortNameHash != attack2ID && !AN.GetBool(skill1ID) && ANShortNameHash != skill1EndID && !AN.GetBool(skill2ID) && !AN.GetBool(jumpID) && !AN.GetBool(hurtID) && !AN.GetBool(deathID) &&
                !AN.GetBool(ultID) && !ANFallDisabled)
            {
                PV.RPC("ANFallRPC", RpcTarget.All, true);
            }

            if (isGround && AN.GetBool(fallID))
            {
                PV.RPC("ANFallRPC", RpcTarget.All, false);
            }


            if (isSkill1 && !statScript.OnZonya && statScript.stiffTime <= 0 && statScript.stunTime <= 0)
            {
                float axis = 0;
                if (floatingJoystick.Horizontal > 0)
                {
                    axis = 1;
                }
                else if (floatingJoystick.Horizontal < 0)
                {
                    axis = -1;
                }
                RB.velocity += new Vector2(axis * statScript.speed * Time.deltaTime * 2, 0f);
            }

            if (UltGauge < 100f)
            {
                UltGauge += Time.deltaTime * UltGaugeSpeed;
            }

            if (onUlt && isGround)
            {
                RB.velocity = new Vector2(0f, RB.velocity.y);
            }
        }
    }

    public override void Attack()
    {
        StartCoroutine(ANAttackCRT());
        int atttack_Number = Random.Range(0, 2);
        PV.RPC("ANAttackRPC", RpcTarget.All, atttack_Number);


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
            if (tempVector.x == 0f)
            {
                if (tempVector.y == 0f)
                {
                    tempVector.x = temp_flipX ? -1f : 1f;
                }
                else
                {
                    tempVector.x = temp_flipX ? -0.00001f : 0.00001f;
                }
            }
            else
            {
                tempVector = tempVector.normalized;
            }
        }

        bool isRightAttack = tempVector.x > 0;

        moaiAttackRangeScript.onAttack(isRightAttack, statScript.BlueTeam, atttack_Number);
    }

    public IEnumerator ANAttackCRT()
    {
        ANFallDisabled = true;
        yield return new WaitForSeconds(0.25f);
        ANFallDisabled = false;
    }

    [PunRPC]
    protected void ANAttackRPC(int value)
    {
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2StartID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(skill2EndID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        if (value == 0)
        {
            AN.SetTrigger(attack1ID);
            if (ANUlting)
            {
                AN1.SetTrigger(attack1ID);
            }
            moai_SoundScript.AS_Attack1_Play();
        }
        else
        {
            AN.SetTrigger(attack2ID);
            if (ANUlting)
            {
                AN1.SetTrigger(attack2ID);
            }
            moai_SoundScript.AS_Attack2_Play();
            
        }
    }


    public override void Skill1()
    {
        StartCoroutine(doSkill1());
    }

    public IEnumerator doSkill1()
    {
        ANLandDisabled = true;
        PV.RPC("ImuneStiffRPC", RpcTarget.All, true);
        PV.RPC("ImuneStunRPC", RpcTarget.All, true);
        PV.RPC("ANSkill1RPC", RpcTarget.All);

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
            tempVector = (statScript.AggroedVector - (Vector2)transform.position);
        }
        else if (tempVector.x == 0f && tempVector.y == 0f)
        {
            tempVector = new Vector2(temp_flipX ? -0.966f : 0.966f, 0.259f);
        }
        else if (isGround)
        {
            if (tempVector.y < 0.259f)
            {
                tempVector = new Vector2(temp_flipX ? -0.966f : 0.966f, 0.259f);
            }
        }

        tempVector = tempVector.normalized * 17f;

        statScript.MoveLock = true;
        statScript.SkillLock = true;
        statScript.JumpLock = true;
        isSkill1 = true;
        PV.RPC("Skill1RPC", RpcTarget.All, tempVector.x, tempVector.y);
        yield return new WaitForSeconds(0.1f);
        MoaiSkill1Col.enabled = true;
    }

    [PunRPC]
    public void ANSkill1RPC()
    {
        moai_SoundScript.AS_Skill1_Start_Play();
        AN.SetBool(skill2StartID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(skill2EndID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(skill1ID, true);

        if (ANUlting)
        {
            AN1.SetBool(skill2StartID, false);
            AN1.SetBool(skill2ID, false);
            AN1.SetBool(skill2EndID, false);
            AN1.SetBool(ultID, false);
            AN1.SetBool(walkID, false);
            AN1.SetBool(hurtID, false);
            AN1.SetBool(jumpID, false);
            AN1.SetBool(fallID, false);
            AN1.SetBool(skill1ID, true);
        }
    }

    [PunRPC]
    void Skill1RPC(float x, float y)
    {
        RB.velocity = new Vector2(x, y);
    }

    [PunRPC]
    public void ANSkill1EndRPC()
    {
        moai_SoundScript.AS_Skill1_End_Play();
        AN.SetBool(skill1ID, false);
        AN.SetTrigger(skill1EndID);
        if (ANUlting)
        {
            AN1.SetBool(skill1ID, false);
            AN1.SetTrigger(skill1EndID);
        }
        statScript.imuneStiff = false;
        statScript.imuneStun = false;
    }

    [PunRPC]
    public void Skill1HitboxRPC(float positionX, float positionY, bool isBlueTeam)
    {
        moaiS1ObjectPool.GetObject(positionX, positionY, isBlueTeam);
    }





    [PunRPC]
    public void ANS2HitboxStartRPC()
    {
        StartCoroutine(ANS2HitboxStartCRT());
    }

    public IEnumerator ANS2HitboxStartCRT()
    {
        S2HitboxSR.enabled = true;
        S2HitboxAN.SetBool(skill2StartID, true);
        yield return new WaitForSeconds(0.13333f);
        S2HitboxAN.SetBool(skill2StartID, false);
        S2HitboxAN.SetBool(skill2ID, true);
    }

    [PunRPC]
    public void ANS2HitboxEndRPC()
    {
        StartCoroutine(ANS2HitboxEndCRT());
    }

    public IEnumerator ANS2HitboxEndCRT()
    {
        S2HitboxAN.SetBool(skill2ID, false);
        S2HitboxAN.SetBool(skill2EndID, true);
        yield return new WaitForSeconds(0.26666f);
        S2HitboxAN.SetBool(skill2EndID, false);
        S2HitboxSR.enabled = false;
    }


    [PunRPC]
    public void ANSkill2RPC()
    {
        moai_SoundScript.AS_Skill2_Start_Play();
        StartCoroutine(ANSkill2CRT());
    }

    public IEnumerator ANSkill2CRT()
    {
        AN.SetBool(skill1ID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(skill2StartID, true);
        if (ANUlting)
        {
            AN1.SetBool(skill2StartID, true);
        }
        yield return new WaitForSeconds(0.2f);
        AN.SetBool(skill2StartID, false);
        AN.SetBool(skill2ID, true);
        if (ANUlting)
        {
            AN1.SetBool(skill2StartID, false);
            AN1.SetBool(skill2ID, true);
        }
    }

    [PunRPC]
    public void ANSkill2EndRPC()
    {
        moai_SoundScript.AS_Skill2_End_Play();
        StartCoroutine(ANSkill2EndCRT());
    }

    public IEnumerator ANSkill2EndCRT()
    {
        AN.SetBool(skill2ID, false);
        AN.SetBool(skill2EndID, true);
        if (ANUlting)
        {
            AN1.SetBool(skill2ID, false);
            AN1.SetBool(skill2EndID, true);
        }
        yield return new WaitForSeconds(0.3333f);
        AN.SetBool(skill2EndID, false);
        if (ANUlting)
        {
            AN1.SetBool(skill2EndID, false);
        }
    }


    public override void Ult()
    {
        onUlt = true;
        PV.RPC("UltRPC", RpcTarget.All);
    }

    [PunRPC]
    void UltRPC()
    {
        moai_SoundScript.AS_Ult_Play();
        statScript.ShieldQue.Enqueue(new ShieldScript(10f, 400f));
        StartCoroutine(UltCRT());
        statScript.imuneStiff = true;
        statScript.imuneStun = true;
    }

    public IEnumerator UltCRT()
    {
        moai_SoundScript.Pitch_Change_OnUlt(true);
        ClearAN1();
        ClearAN2();
        AN2.enabled = true;
        AN.SetBool(ultID, true);
        Ulting = true;
        statScript.MoveLock = true;
        statScript.JumpLock = true;
        statScript.SkillLock = true;
        yield return new WaitForSeconds(1.5f);
        statScript.imuneStiff = false;
        statScript.imuneStun = false;
        AttackDelay *= 0.5f;
        Skill1Delay *= 0.5f;
        Skill2Delay *= 0.5f;
        if (PV.IsMine)
        {
            NetworkManager.instance.AttackCool.Cooltimer *= 0.5f;
            NetworkManager.instance.AttackCool.CooltimerCount *= 0.5f;
            NetworkManager.instance.S1Cool.Cooltimer *= 0.5f;
            NetworkManager.instance.S1Cool.CooltimerCount *= 0.5f;
            NetworkManager.instance.S2Cool.Cooltimer *= 0.5f;
            NetworkManager.instance.S2Cool.CooltimerCount *= 0.5f;
        }
        AN.SetBool(ultID, false);
        AN = AN2;
        statScript.AN = AN2;
        bool tempBool = SR1.enabled;//bush에서 썼을경우 체크용
        SR1.enabled = false;
        SR2.flipX = SR1.flipX;
        SR = SR2;
        statScript.SR = SR2;
        SR2.enabled = tempBool;
        ANUlting = true;
        onUlt = false;
        statScript.MoveLock = false;
        statScript.JumpLock = false;
        statScript.SkillLock = false;
        yield return new WaitForSeconds(10f);
        ANUlting = false;
        AN2.enabled = false;
        AN = AN1;
        statScript.AN = AN1;
        tempBool = SR2.enabled;//bush에서 궁 끝났을 경우 체크용
        SR2.enabled = false;
        SR1.flipX = SR2.flipX;
        SR = SR1;
        statScript.SR = SR1;
        if (!statScript.isDead)
        {
            SR1.enabled = tempBool;
        }
        AttackDelay *= 2f;
        Skill1Delay *= 2f;
        Skill2Delay *= 2f;
        if (PV.IsMine)
        {
            NetworkManager.instance.AttackCool.Cooltimer *= 2f;
            NetworkManager.instance.AttackCool.CooltimerCount *= 2f;
            NetworkManager.instance.S1Cool.Cooltimer *= 2f;
            NetworkManager.instance.S1Cool.CooltimerCount *= 2;
            NetworkManager.instance.S2Cool.Cooltimer *= 2f;
            NetworkManager.instance.S2Cool.CooltimerCount *= 2f;
        }
        moai_SoundScript.Pitch_Change_OnUlt(false);
        Ulting = false;
    }

    public void ClearAN1()
    {
        AN1.SetBool(skill1ID, false);
        AN1.SetBool(skill2StartID, false);
        AN1.SetBool(skill2ID, false);
        AN1.SetBool(skill2EndID, false);
        AN1.SetBool(ultID, false);
        AN1.SetBool(walkID, false);
        AN1.SetBool(hurtID, false);
        AN1.SetBool(jumpID, false);
        AN1.SetBool(fallID, false);
    }

    public void ClearAN2()
    {
        AN2.SetBool(skill1ID, false);
        AN2.SetBool(skill2StartID, false);
        AN2.SetBool(skill2ID, false);
        AN2.SetBool(skill2EndID, false);
        AN2.SetBool(ultID, false);
        AN2.SetBool(walkID, false);
        AN2.SetBool(hurtID, false);
        AN2.SetBool(jumpID, false);
        AN2.SetBool(fallID, false);
    }
}
