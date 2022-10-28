using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LinkerScript : Character_Base
{
    public LinkerAttackColScript linkerAttackColScript;
    public LinkerAttackHitEffectObjectPool LinkerAttackHitEffectObjectPool;
    public LinkerS1ObjectPool linkerS1ObjectPool;
    private LinkerS1Script tempLinkerS1Script;
    public SpriteRenderer S2SR;
    public Animator S2AN;
    public LinkerUltObjectPool linkerUltObjectPool;

    public SpriteRenderer FrontArm;
    public Animator FrontArmAN;
    public SpriteRenderer BehindArm;
    public Animator BehindArmAN;

    public Linker_SoundScript linker_SoundScript;


    public override void Start()
    {
        base.Start();
        statScript.JobCode = 5;
        statScript.linkerScript = this;
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
                AN.SetBool(walkID, false);
                AN.SetBool(jumpID, false);
                AN.SetBool(fallID, false);
                FrontArm.enabled = false;
                BehindArm.enabled = false;
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
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(hurtID, false);
        FrontArm.enabled = false;
        BehindArm.enabled = false;
        //모든모션 끄기
        AN.SetBool(deathID, true);

        if (floatingJoystick != null)
        {
            floatingJoystick.Horizontal = 0f;
            floatingJoystick.Vertical = 0f;
        }

        StartCoroutine(DeathAgainCRT());
        //if (tempUltCRT != null) StopCoroutine(tempUltCRT);
        PV.RPC("GravityRPC", RpcTarget.All, statScript.OriginalGravityScale);
        PV.RPC("MoaiS2OptionRPC", RpcTarget.All, true);
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
            AN.SetBool(walkID, false);
            AN.SetBool(jumpID, false);
            AN.SetBool(fallID, false);
            AN.SetBool(hurtID, false);
            FrontArm.enabled = false;
            BehindArm.enabled = false;
            if (floatingJoystick != null)
            {
                floatingJoystick.Horizontal = 0f;
                floatingJoystick.Vertical = 0f;
            }

            statScript.OnSunFire = false;
        }
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            isGround = Physics2D.OverlapArea((Vector2)transform.position + new Vector2(-0.54f, -0.705f), (Vector2)transform.position + new Vector2(0.54f, -1.2f), 11 << 9);


            if (!statScript.isJumping && isGround)
            {
                if (AN.GetBool(jumpID))
                {
                    AN.SetBool(jumpID, false);
                }
            }

            if (!isGround && !AN.GetBool(fallID) && !AN.GetBool(attackID)  && !AN.GetBool(skill1ID)  && !AN.GetBool(jumpID) && !AN.GetBool(hurtID) && !AN.GetBool(deathID))
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

    public override void ANWalk()
    {
        if (!AN.GetBool(jumpID) && !AN.GetBool(fallID) && !AN.GetBool(deathID))
        {
            FrontArm.enabled = false;
            BehindArm.enabled = false;
            AN.SetBool(attackID, false);
            AN.SetBool(skill1ID, false);
            AN.SetBool(walkID, false);
            AN.SetBool(jumpID, false);
            AN.SetBool(fallID, false);
            AN.SetBool(hurtID, false);
            AN.SetBool(walkID, true);
        }
    }

    [PunRPC]
    public void ANFallRPC(bool value)
    {
        FrontArm.enabled = false;
        BehindArm.enabled = false;
        AN.SetBool(fallID, value);
    }


    public override void AttackEnd()
    {
        if (AN.GetBool(attackID))
        {
            AN.SetBool(attackID, false);
            FrontArm.enabled = false;
            BehindArm.enabled = false;
            FrontArmAN.SetBool(attackID, false);
            BehindArmAN.SetBool(attackID, false);
        }
    }

    public override void Skill1End()
    {
        if (AN.GetBool(skill1ID))
        {
            AN.SetBool(skill1ID, false);
            FrontArm.enabled = false;
            BehindArm.enabled = false;
            FrontArmAN.SetBool(skill1ID, false);
            BehindArmAN.SetBool(skill1ID, false);
        }
    }

    public override void Attack()
    {
        StartCoroutine(AttackCRT());
    }

    public IEnumerator AttackCRT()
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
        Vector2 tempPosition = (Vector2)transform.position + tempVector.normalized * 0.4f;


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


        if (tempVector.x == 0 && tempVector.y != 0f)
        {
            if (!SR.flipX)
            {
                tempVector.x = 0.000001f;
            }
            else
            {
                tempVector.x = -0.000001f;
            }
        }


        if (tempVector.x == 0f && tempVector.y == 0f)
        {
            if (!temp_flipX)
            {
                PV.RPC("ANAttackRPC", RpcTarget.All, false, 0f, (Vector2)transform.position);
            }
            else
            {
                PV.RPC("ANAttackRPC", RpcTarget.All, true, 0f, (Vector2)transform.position);
            }
        }
        else if (tempVector.x > 0)
        {
            PV.RPC("ANAttackRPC", RpcTarget.All, false, Mathf.Rad2Deg * (Mathf.Atan(tempVector.y / tempVector.x)), (Vector2)transform.position);
        }
        else if (tempVector.x < 0)
        {
            PV.RPC("ANAttackRPC", RpcTarget.All, true, Mathf.Rad2Deg * (Mathf.Atan(tempVector.y / tempVector.x)), (Vector2)transform.position);
        }

        float rotationZ = Mathf.Rad2Deg * (Mathf.Atan2(tempVector.y, tempVector.x));
        PV.RPC("MoveLockRPC", RpcTarget.All, 0.45f);
        yield return new WaitForSeconds(0.15f);
        PV.RPC("AttackRPC", RpcTarget.All, (Vector2)gameObject.transform.position + new Vector2(tempVector.x * 2f, tempVector.y * 2f), rotationZ);
    }

    [PunRPC]
    public void ANAttackRPC(bool flipX, float rotationZ, Vector2 linkerPosition)
    {
        FrontArm.flipX = flipX;
        BehindArm.flipX = flipX;
        FrontArm.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        FrontArm.transform.position = linkerPosition + new Vector2(-0.275f * (flipX ? -1f : 1f), 0.4f);
        BehindArm.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        BehindArm.transform.position = linkerPosition + new Vector2(-0.1375f * (flipX ? -1f : 1f), 0.3875f);
        FrontArmAN.SetBool(skill1ID, false);
        BehindArmAN.SetBool(skill1ID, false);
        FrontArmAN.SetBool(attackID, true);
        BehindArmAN.SetBool(attackID, true);
        FrontArm.color = SR.color;
        BehindArm.color = SR.color;
        FrontArm.enabled = true;
        BehindArm.enabled = true;


        AN.SetBool(attackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(attackID, true);

        //사운드 넣기
        linker_SoundScript.AS_Attack_Play();
    }

    [PunRPC]
    void AttackRPC(Vector2 position, float rotationZ)
    {
        //linkerAttackColScript.transform.position = position;
        linkerAttackColScript.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        linkerAttackColScript.AN.SetTrigger(attackID);
        linkerAttackColScript.SR.enabled = true;
        if (PV.IsMine)
        {
            linkerAttackColScript.col.enabled = true;
        }
        StartCoroutine(AttackColCRT());
    }

    public IEnumerator AttackColCRT()
    {
        yield return new WaitForSeconds(0.3f);
        linkerAttackColScript.SR.enabled = false;
        if (PV.IsMine)
        {
            linkerAttackColScript.col.enabled = false;
        }
    }

    [PunRPC]
    public void ANAttackHitEffectRPC(Vector2 position, bool flipX)
    {
        LinkerAttackHitEffectObjectPool.GetObject(position, flipX);
    }

    public override void Skill1()
    {
        StartCoroutine(Skill1CRT());
    }

    public IEnumerator Skill1CRT()
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
        Vector2 tempPosition = (Vector2)transform.position + tempVector.normalized * 0.4f;


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


        if (tempVector.x == 0 && tempVector.y != 0f)
        {
            if (!SR.flipX)
            {
                tempVector.x = 0.000001f;
            }
            else
            {
                tempVector.x = -0.000001f;
            }
        }

        if (tempVector.x == 0f && tempVector.y == 0f)
        {
            if (!temp_flipX)
            {
                PV.RPC("ANSkill1RPC", RpcTarget.All, false, 0f, (Vector2)transform.position);
            }
            else
            {
                PV.RPC("ANSkill1RPC", RpcTarget.All, true, 0f, (Vector2)transform.position);
            }
        }
        else if (tempVector.x > 0)
        {
            PV.RPC("ANSkill1RPC", RpcTarget.All, false, Mathf.Rad2Deg * (Mathf.Atan(tempVector.y / tempVector.x)), (Vector2)transform.position);
        }
        else if (tempVector.x < 0)
        {
            PV.RPC("ANSkill1RPC", RpcTarget.All, true, Mathf.Rad2Deg * (Mathf.Atan(tempVector.y / tempVector.x)), (Vector2)transform.position);
        }

        float rotationZ = Mathf.Rad2Deg * (Mathf.Atan2(tempVector.y, tempVector.x));
        PV.RPC("MoveLockRPC", RpcTarget.All, 0.2f);
        yield return new WaitForSeconds(0.2f);
        PV.RPC("Skill1RPC", RpcTarget.All, tempPosition.x, tempPosition.y, temp_flipX, statScript.BlueTeam, tempVector.x, tempVector.y);
    }

    [PunRPC]
    public void ANSkill1RPC(bool flipX, float rotationZ, Vector2 linkerPosition)
    {
        FrontArm.flipX = flipX;
        BehindArm.flipX = flipX;
        FrontArm.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        FrontArm.transform.position = linkerPosition + new Vector2(-0.275f * (flipX ? -1f : 1f), 0.4f);
        BehindArm.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        BehindArm.transform.position = linkerPosition + new Vector2(-0.1375f * (flipX ? -1f : 1f), 0.3875f);
        FrontArmAN.SetBool(attackID, false);
        BehindArmAN.SetBool(attackID, false);
        FrontArmAN.SetBool(skill1ID, true);
        BehindArmAN.SetBool(skill1ID, true);
        FrontArm.color = SR.color;
        BehindArm.color = SR.color;
        FrontArm.enabled = true;
        BehindArm.enabled = true;


        AN.SetBool(attackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(skill1ID, true);

        //사운드 넣기
        linker_SoundScript.AS_Skill1_Play();
    }

    [PunRPC]
    void Skill1RPC(float positionX, float positionY, bool flipX, bool isBlueTeam, float vectorX, float vectorY)
    {
        linkerS1ObjectPool.GetObject(positionX, positionY, flipX, isBlueTeam, vectorX, vectorY);
    }


    public override void Skill2()
    {
        PV.RPC("Skill2RPC", RpcTarget.All);
    }

    [PunRPC]
    void Skill2RPC()
    {
        StartCoroutine(Skill2CRT());
    }

    public IEnumerator Skill2CRT()
    {
        linker_SoundScript.AS_Skill2_Play();
        S2AN.SetTrigger(skill2ID);
        S2SR.enabled = true;
        statScript.DEF += 120f;
        statScript.ShieldQue.Enqueue(new ShieldScript(4f, 300f));
        yield return new WaitForSeconds(0.533f);
        S2SR.enabled = false;
        yield return new WaitForSeconds(4f-0.533f);
        statScript.DEF -= 120f;
    }


    public override void Ult()
    {
        PV.RPC("MoveLockRPC", RpcTarget.All, 0.5333f);
        PV.RPC("UltRPC", RpcTarget.All,(Vector2)transform.position);
    }

    [PunRPC]
    void UltRPC(Vector2 position)
    {
        linkerUltObjectPool.GetObject(position);
        linker_SoundScript.AS_Ult_Play();
    }
}
