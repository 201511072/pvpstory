using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerScript : Character_Base
{
    public int jumpCount = 2;

    public BladerAttackRangeScript bladerAttackRangeScript;

    public BoxCollider2D S1Col;
    public BladerS1HitboxScript bladerS1HitboxScript;
    public BoxCollider2D S1Hitbox;
    ContactFilter2D contactFilter2D;
    Vector2 S1tempVector;
    bool isS1ing;
    public BladerS1ObjectPool bladerS1ObjectPool;
    public BladerS1EffectObjectPool bladerS1EffectObjectPool;
    public BladerS1HitEffectObjectPool bladerS1HitEffectObjectPool;
    public bool s1ColDir;//스킬1 쓰는방향 오른쪽이면0,왼쪽이면1


    public BladerS2Script bladerS2Script;
    public SpriteRenderer S2SR;
    public Animator S2AN;
    public BladerS2EffectObjectPool bladerS2EffectObjectPool;
    public bool onS2;//스킬2를 사용중인지 나타냄. 부쉬속에서 상대에게 안보이게 하기 위해서 사용됨

    public BladerUltScript bladerUltScript;
    bool onUlt;
    public StatScript bladerUltEnemy;
    public bool OnBladerUlt;
    public SpriteRenderer UltSR;
    public Animator UltAN;
    public float EnemyColY;//sizeY의 절반
    public float BladerColY = 0.745f;//혹시 사이즈 바뀌면 이것도 바꿔주기

    public Blader_SoundScript blader_SoundScript;



    public override void Start()
    {
        base.Start();
        statScript.JobCode = 2;
        statScript.bladerScript = this;

        contactFilter2D.SetLayerMask(11 << 9);
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
                AN.SetBool(skill2ID, false);
                AN.SetBool(ultID, false);
                AN.SetBool(walkID, false);
                AN.SetBool(jumpID, false);
                AN.SetBool(jump2ID, false);
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
        AN.SetBool(skill2ID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(ultStartID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(jump2ID, false);
        AN.SetBool(fallID, false);
        //모든모션 끄기
        AN.SetBool(deathID, true);
        StartCoroutine(DeathAgainCRT());
        jumpCount = 2;
        if(tempUltCRT!=null) StopCoroutine(tempUltCRT);
        if (tempSoundUltCRT != null) StopCoroutine(tempSoundUltCRT);
        if (tempUltPositionCRT != null) StopCoroutine(tempUltPositionCRT);
        if (tempUltStartCRT != null) StopCoroutine(tempUltStartCRT);
        bladerUltScript.circleCollider2D.enabled = false;
        OnBladerUlt = false;
        onUlt = false;
        if (PV.IsMine)
        {
            NetworkManager.instance.BladerJumpBtnScript.onClickBtnCheck = true;
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
            AN.SetBool(skill2ID, false);
            AN.SetBool(ultID, false);
            AN.SetBool(ultStartID, false);
            AN.SetBool(hurtID, false);
            AN.SetBool(walkID, false);
            AN.SetBool(jumpID, false);
            AN.SetBool(jump2ID, false);
            AN.SetBool(fallID, false);
            if (floatingJoystick != null)
            {
                floatingJoystick.Horizontal = 0f;
                floatingJoystick.Vertical = 0f;
            }
            jumpCount = 2;

            statScript.OnSunFire = false;
        }
    }

    public override void ANWalk()
    {
        if (!AN.GetBool(jumpID) && !AN.GetBool(jump2ID) && !AN.GetBool(fallID) && !AN.GetBool(deathID))
        {
            AN.SetBool(attackID, false);
            AN.SetBool(skill1ID, false);
            AN.SetBool(skill2ID, false);
            AN.SetBool(ultID, false);
            AN.SetBool(ultStartID, false);
            AN.SetBool(hurtID, false);
            AN.SetBool(jumpID, false);
            AN.SetBool(jump2ID, false);
            AN.SetBool(fallID, false);
            AN.SetBool(walkID, true);
        }
    }



    private void Update()
    {
        if (PV.IsMine)
        {
            isGround = Physics2D.OverlapArea((Vector2)transform.position + new Vector2(-0.39f, -0.755f), (Vector2)transform.position + new Vector2(0.39f, -1.25f), 11 << 9);

            if (isGround && Land && !AN.GetBool(attackID) && !AN.GetBool(skill1ID) && !AN.GetBool(skill2ID) && !AN.GetBool(ultID) && !AN.GetBool(ultStartID) && !AN.GetBool(hurtID) && !AN.GetBool(jumpID) && !AN.GetBool(jump2ID) &&
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

            if (!isGround && !AN.GetBool(fallID) && !AN.GetBool(attackID) && !AN.GetBool(skill1ID) && !AN.GetBool(skill2ID) && !AN.GetBool(ultID) && !AN.GetBool(ultStartID) && !AN.GetBool(hurtID) && !AN.GetBool(jumpID) && !AN.GetBool(jump2ID) &&
                !AN.GetBool(deathID))
            {
                PV.RPC("ANFallRPC", RpcTarget.All, true);
            }

            if (isGround && AN.GetBool(fallID))
            {
                PV.RPC("ANFallRPC", RpcTarget.All, false);
            }


            //여기부터는 blader에 필요한것들
            if (jumpCount < 2 && !statScript.isJumping && isGround)
            {
                jumpCount = 2;
            }

            if (UltGauge < 100f)
            {
                UltGauge += Time.deltaTime * UltGaugeSpeed;
            }


            ////여기서부터 스킬 조이스틱 S1추가부분
            //
            //if (skillJoystick != Vector2.zero)
            //{
            //    S1tempVector = skillJoystick;
            //}
            //else
            //{
            //    S1tempVector = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical);
            //}
            //
            ////여기까지 스킬 조이스틱 S1추가부분
            //
            //
            //if (statScript.Aggroed)
            //{
            //    S1tempVector = (statScript.AggroedVector - (Vector2)transform.position);
            //    S1Col.offset = S1tempVector;
            //}
            //else
            //{
            //    if (S1tempVector.x == 0f && S1tempVector.y == 0f)
            //    {
            //        S1tempVector.x = SR.flipX ? -1f : 1f;
            //    }
            //    else
            //    {
            //        S1tempVector = S1tempVector.normalized;
            //    }
            //    S1Col.offset = S1tempVector * 4.5f;
            //}
        }

        if (onUlt && isGround)
        {
            RB.velocity = new Vector2(0f, RB.velocity.y);
        }

        if (S2SR.enabled)
        {
            S2SR.flipX = SR.flipX;
        }

        if (OnBladerUlt)
        {
            transform.position = bladerUltEnemy.transform.position + Vector3.down * (EnemyColY - BladerColY);
        }
    }


    [PunRPC]
    public void ANLandRPC()
    {
        AN.SetBool(attackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(jump2ID, false);
        AN.SetBool(fallID, false);
        AN.SetTrigger(landID);
        blader_SoundScript.AS_Land_Play();
    }

    [PunRPC]
    public void ANFallRPC(bool value)
    {
        if (value)
        {
            AN.SetBool(attackID, false);
            AN.SetBool(skill1ID, false);
            AN.SetBool(skill2ID, false);
            AN.SetBool(hurtID, false);
            AN.SetBool(jumpID, false);
            AN.SetBool(jump2ID, false);
        }
        AN.SetBool(fallID, value);
    }



    public override void Jump()
    {
        if (!statScript.isJumping && !statScript.JumpLock && !statScript.StunJumpLock && jumpCount > 0&&!statScript.OnZonya)
        {
            StartCoroutine(isJumpingCRT());
            if (isGround)
            {
                jumpCount--;
                PV.RPC("JumpRPC", RpcTarget.All);
            }
            else
            {
                jumpCount -= 2;
                PV.RPC("SecondJumpRPC", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public override void JumpRPC()
    {
        blader_SoundScript.AS_Jump_Play();

        RB.velocity = Vector2.zero;
        RB.AddForce(Vector2.up * 700);
        AN.SetBool(attackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(ultStartID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jump2ID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(jumpID, true);
    }

    [PunRPC]
    public void SecondJumpRPC()
    {
        blader_SoundScript.AS_Jump_Play();

        RB.velocity = Vector2.zero;
        RB.AddForce(Vector2.up * JumpPower);
        AN.SetBool(attackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(ultStartID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(jump2ID, true);
    }

    public void Jump2End()
    {
        if (AN.GetBool(jump2ID))
            AN.SetBool(jump2ID, false);
    }


    public override void Attack()
    {
        PV.RPC("ANAttackRPC", RpcTarget.All);

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
                    tempVector.x = temp_flipX ? -1f : 1f;
                }
            }
        }

        bool isRightAttack = tempVector.x > 0;

        bladerAttackRangeScript.onAttack(isRightAttack, statScript.BlueTeam);
    }

    [PunRPC]
    public void ANAttackRPC()
    {
        StartCoroutine(ANAttackCRT());
    }

    public IEnumerator ANAttackCRT()
    {
        //sound
        blader_SoundScript.AS_Attack_Play();

        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(ultStartID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(jump2ID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(attackID, true);
        yield return new WaitForSeconds(0.4f);
        AN.SetBool(attackID, false);
    }


    public override void Skill1()
    {
        statScript.SkillLock = true;
        statScript.MoveLock = true;
        statScript.JumpLock = true;
        //여기서부터 스킬 조이스틱 S1추가부분

        if (skillJoystick != Vector2.zero)
        {
            S1tempVector = skillJoystick;
        }
        else
        {
            S1tempVector = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical);
        }

        //여기까지 스킬 조이스틱 S1추가부분


        if (statScript.Aggroed)
        {
            S1tempVector = (statScript.AggroedVector - (Vector2)transform.position);
            S1Col.offset = S1tempVector;
        }
        else
        {
            if (S1tempVector.x == 0f && S1tempVector.y == 0f)
            {
                S1tempVector.x = SR.flipX ? -1f : 1f;
            }
            else
            {
                S1tempVector = S1tempVector.normalized;
            }
            S1Col.offset = S1tempVector * 4.5f;
        }
        PV.RPC("ANSkill1RPC", RpcTarget.All);
        List<Collider2D> colList = new List<Collider2D>();
        int i = 0;
        while (i < 45)
        {
            i++;
            colList.Clear();
            S1Col.OverlapCollider(contactFilter2D, colList);
            if (colList.Count < 1)
            {
                PV.RPC("S1MoveRPC", RpcTarget.All, S1Col.offset);
                break;
            }
            else
            {
                S1Col.offset -= S1tempVector * 0.1f;
            }
        }


        bladerS1HitboxScript.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * (Mathf.Atan2(S1Col.offset.y, S1Col.offset.x)));
        bladerS1HitboxScript.boxCollider2D.offset = new Vector2(-S1Col.offset.magnitude * 0.5f, 0f);
        bladerS1HitboxScript.boxCollider2D.size = new Vector2(S1Col.offset.magnitude + 1f, 1.4f);
        SR.flipX = S1Col.offset.x < 0;
        PV.RPC("ANSkill1EffectRPC", RpcTarget.All, (Vector2)transform.position + S1Col.offset * -0.5f, bladerS1HitboxScript.transform.rotation,
            new Vector2(bladerS1HitboxScript.boxCollider2D.size.x / 4.5f, 1f), S1Col.offset.x < 0);
        s1ColDir = S1Col.offset.x < 0;//맞은 적의 hitEffect의 SR.flipX방향을 정함
        StartCoroutine(S1HitboxCRT());
    }

    [PunRPC]
    public void ANSkill1RPC()
    {
        StartCoroutine(ANSkill1CRT());
    }

    public IEnumerator ANSkill1CRT()
    {
        //sound
        blader_SoundScript.AS_Skill1_Play();

        AN.SetBool(attackID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(ultStartID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(jump2ID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(skill1ID, true);
        yield return new WaitForSeconds(0.1f);
        AN.SetBool(skill1ID, false);
    }

    [PunRPC]
    public void ANSkill1EffectRPC(Vector2 position, Quaternion rotation, Vector2 size, bool flipY)
    {
        bladerS1EffectObjectPool.GetObject(position, rotation, size, flipY);
    }

    IEnumerator S1HitboxCRT()
    {
        bladerS1HitboxScript.boxCollider2D.enabled = true;
        yield return new WaitForSeconds(0.1f);
        bladerS1HitboxScript.boxCollider2D.enabled = false;
        statScript.SkillLock = false;
        statScript.MoveLock = false;
        statScript.JumpLock = false;
    }

    [PunRPC]
    public void ANSkill1HitEffectRPC(Vector2 position, bool flipX)
    {
        bladerS1HitEffectObjectPool.GetObject(position, flipX);
    }

    [PunRPC]
    void S1MoveRPC(Vector2 value)
    {
        bladerS1ObjectPool.GetObject(transform.position.x, transform.position.y, SR.flipX);
        transform.position += (Vector3)value;
        RB.velocity = Vector2.zero;
    }

    public override void Skill2()
    {
        statScript.SkillLock = true;
        PV.RPC("ANSkill2RPC", RpcTarget.All, true);
        StartCoroutine(Skill2CRT());
    }

    [PunRPC]
    public void ANSkill2RPC(bool value)
    {
        onS2 = value;

        //sound
        if (value) blader_SoundScript.AS_Skill2_On_Play();
        else blader_SoundScript.AS_Skill2_Off_Play();

        S2AN.SetBool(skill2ID, false);
        S2AN.SetBool(skill2EndID, false);
        S2AN.enabled = value;
        if (value)
        {
            StartCoroutine(ANSkill2CRT());
        }

        if (statScript.OnBush)
        {
            if (NetworkManager.instance.MyStatScript.BlueTeam == statScript.BlueTeam)
            {
                S2SR.color = new Color(1f, 1f, 1f, 0.5f);
                S2SR.enabled = value;
            }
            else S2SR.enabled = false;
        }
        else
        {
            S2SR.enabled = value;
        }
    }

    public IEnumerator ANSkill2CRT()
    {
        S2AN.SetBool(skill2StartID, true);
        yield return new WaitForSeconds(0.2f);
        S2AN.SetBool(skill2StartID, false);
        S2AN.SetBool(skill2ID, true);
        yield return new WaitForSeconds(1.73333f);
        S2AN.SetBool(skill2ID, false);
        S2AN.SetBool(skill2EndID, true);
        yield return new WaitForSeconds(0.06667f);
        S2AN.SetBool(skill2EndID, false);
    }

    IEnumerator Skill2CRT()
    {
        PV.RPC("DontHitByArrowRPC", RpcTarget.All,true);
        bladerS2Script.boxCollider2D.enabled = true;
        yield return new WaitForSeconds(2f);
        bladerS2Script.boxCollider2D.enabled = false;
        PV.RPC("DontHitByArrowRPC", RpcTarget.All,false);
        PV.RPC("ANSkill2RPC", RpcTarget.All, false);
        statScript.SkillLock = false;
    }

    [PunRPC]
    public void Skill2RPC(Vector2 position, bool flipX, int random)
    {
        bladerS2EffectObjectPool.GetObject(position, flipX, random);
    }


    public Coroutine tempUltCRT;
    public Coroutine tempSoundUltCRT;
    public Coroutine tempUltPositionCRT;
    public Coroutine tempUltStartCRT;

    public override void Ult()
    {
        onUlt = true;
        PV.RPC("ANUltStartRPC", RpcTarget.All);
        statScript.MoveLock = true;
        statScript.SkillLock = true;
        statScript.JumpLock = true;
        bladerUltScript.EnemyStatScript = null;
        bladerUltEnemy = null;
        bladerUltScript.circleCollider2D.enabled = true;
        tempUltCRT = StartCoroutine(UltCRT());
    }


    [PunRPC]
    void ANUltStartRPC()
    {
        statScript.imuneStiff = true;
        statScript.imuneStun = true;
        StartCoroutine(ANUltStartCRT());
    }

    public IEnumerator ANUltStartCRT()
    {
        //sound
        blader_SoundScript.AS_Ult_Play();

        AN.SetBool(attackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(jump2ID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(ultStartID, true);
        UltAN.enabled = true;
        UltAN.SetTrigger(ultID);
        UltSR.enabled = true;
        yield return new WaitForSeconds(1.5f);
        AN.SetBool(ultStartID, false);
        UltSR.enabled = false;
        UltAN.enabled = false;
        statScript.imuneStiff = false;
        statScript.imuneStun = false;
    }


    IEnumerator UltCRT()
    {
        yield return new WaitForSeconds(1.5f);
        onUlt = false;
        bladerUltScript.circleCollider2D.enabled = false;

        if (bladerUltScript.EnemyStatScript == null) { }
        else if (bladerUltScript.EnemyStatScript == NetworkManager.instance.Blue1statScript)
        {
            PV.RPC("UltRPC", RpcTarget.All, 1, EnemyColY);
        }
        else if (bladerUltScript.EnemyStatScript == NetworkManager.instance.Red1statScript)
        {
            PV.RPC("UltRPC", RpcTarget.All, 2, EnemyColY);
        }
        else if (bladerUltScript.EnemyStatScript == NetworkManager.instance.Red2statScript)
        {
            PV.RPC("UltRPC", RpcTarget.All, 3, EnemyColY);
        }
        else if (bladerUltScript.EnemyStatScript == NetworkManager.instance.Blue2statScript)
        {
            PV.RPC("UltRPC", RpcTarget.All, 4, EnemyColY);
        }

        if (bladerUltScript.EnemyStatScript != null)
        {
            bladerUltEnemy = bladerUltScript.EnemyStatScript;
            UltStart();
        }
        else
        {
            statScript.MoveLock = false;
            statScript.SkillLock = false;
            statScript.JumpLock = false;
        }
    }

    [PunRPC]
    void UltRPC(int value, float enemyColY)
    {
        if (!PV.IsMine)
        {
            bladerUltEnemy = null;
            if (value == 0)
            {
            }
            if (value == 1)
            {
                bladerUltEnemy = NetworkManager.instance.Blue1statScript;
            }
            else if (value == 2)
            {
                bladerUltEnemy = NetworkManager.instance.Red1statScript;
            }
            else if (value == 3)
            {
                bladerUltEnemy = NetworkManager.instance.Red2statScript;
            }
            else if (value == 4)
            {
                bladerUltEnemy = NetworkManager.instance.Blue2statScript;
            }

            EnemyColY = enemyColY;
        }
    }

    public void UltStart()
    {
        PV.RPC("ANUltRPC", RpcTarget.All);
        tempUltStartCRT=StartCoroutine(UltStartCRT());
    }

    [PunRPC]
    public void ANUltRPC()
    {
        StartCoroutine(ANUltCRT());
        tempUltPositionCRT= StartCoroutine(UltPositionCRT());
        tempSoundUltCRT=StartCoroutine(SoundUltCRT());
    }

    public IEnumerator ANUltCRT()
    {
        AN.SetBool(attackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(ultStartID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(jump2ID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(ultID, true);
        yield return new WaitForSeconds(2f);
        AN.SetBool(ultID, false);
    }

    public IEnumerator SoundUltCRT()
    {
        blader_SoundScript.AS_Ult1_Play();
        yield return new WaitForSeconds(0.25f);
        blader_SoundScript.AS_Ult2_Play();
        yield return new WaitForSeconds(0.35f);
        blader_SoundScript.AS_Ult3_Play();
        yield return new WaitForSeconds(0.5f);
        blader_SoundScript.AS_Ult4_Play();
    }

    public IEnumerator UltPositionCRT()
    {
        statScript.MyCol.enabled = false;
        OnBladerUlt = true;
        yield return new WaitForSeconds(2f);
        RB.velocity = Vector2.zero;
        OnBladerUlt = false;
        transform.position = bladerUltEnemy.transform.position + Vector3.down * (EnemyColY - BladerColY);
        statScript.MyCol.enabled = true;
    }

    IEnumerator UltStartCRT()
    {
        if (!bladerUltEnemy.OnZonya)
            bladerUltEnemy.GetDamage(statScript.ATK * 0.3f + 20f, 84f, 0f, 0f, 0f, 0.2f, statScript.myPlayerNumber);
        yield return new WaitForSeconds(0.25f);
        if (!bladerUltEnemy.OnZonya)
            bladerUltEnemy.GetDamage(statScript.ATK * 0.3f + 20f, 84f, 0f, 0f, 0f, 0.2f, statScript.myPlayerNumber);
        yield return new WaitForSeconds(0.375f);
        if (!bladerUltEnemy.OnZonya)
            bladerUltEnemy.GetDamage(statScript.ATK * 0.3f + 20f, 84f, 0f, 0f, 0f, 0.2f, statScript.myPlayerNumber);
        yield return new WaitForSeconds(0.75f);
        if (!bladerUltEnemy.OnZonya)
            bladerUltEnemy.GetDamage(statScript.ATK * 0.3f + 100f, 235f, 0f, 0f, 0f, 0.2f, statScript.myPlayerNumber);
        yield return new WaitForSeconds(0.625f);
        statScript.MoveLock = false;
        statScript.SkillLock = false;
        statScript.JumpLock = false;
    }
}
