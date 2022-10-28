using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TaraScript : Character_Base
{
    public TaraAttackUseRangeScript taraAttackUseRangeScript;
    public TaraAttackExplosionObjectPool taraAttackExplosionObjectPool;

    public TaraS1UseRangeScript taraS1UseRangeScript;
    public TaraS1ObjectPool taraS1ObjectPool;
    public TaraS1AttackObjectPool taraS1AttackObjectPool;

    public TaraS2UseRangeScript taraS2UseRangeScript;
    public TaraS2ObjectPool taraS2ObjectPool;

    public TaraUltObjectPool taraUltObjectPool;
    public TaraUltShowRangeObjectPool taraUltShowRangeObjectPool;

    public Animator ArmAN;
    public SpriteRenderer ArmSR;

    public Tara_SoundScript tara_SoundScript;


    public override void Start()
    {
        base.Start();
        statScript.JobCode = 6;
        statScript.taraScript = this;
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
                AN.SetBool("attack1", false);
                AN.SetBool("attack2", false);
                AN.SetBool("attack3", false);
                AN.SetBool(skill1ID, false);
                AN.SetBool(skill2ID, false);
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
        AN.SetBool("attack1", false);
        AN.SetBool("attack2", false);
        AN.SetBool("attack3", false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        //모든모션 끄기
        AN.SetBool(deathID, true);
        StartCoroutine(DeathAgainCRT());
        taraAttackUseRangeScript.gameObject.SetActive(false);
        taraS1UseRangeScript.RangeImage.enabled = false;
        taraS2UseRangeScript.RangeImage.enabled = false;
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
            AN.SetBool("attack1", false);
            AN.SetBool("attack2", false);
            AN.SetBool("attack3", false);
            AN.SetBool(skill1ID, false);
            AN.SetBool(skill2ID, false);
            AN.SetBool(ultID, false);
            AN.SetBool(walkID, false);
            AN.SetBool(hurtID, false);
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
            AN.SetBool("attack1", false);
            AN.SetBool("attack2", false);
            AN.SetBool("attack3", false);
            AN.SetBool(skill1ID, false);
            AN.SetBool(skill2ID, false);
            AN.SetBool(ultID, false);
            AN.SetBool(hurtID, false);
            AN.SetBool(walkID, true);
        }
    }


    private void Update()
    {
        if (PV.IsMine)
        {
            isGround = Physics2D.OverlapArea((Vector2)transform.position + new Vector2(-0.37f, -0.68f), (Vector2)transform.position + new Vector2(0.37f, -1.175f), 11 << 9);

            if (isGround && Land && !AN.GetBool("attack1") && !AN.GetBool("attack2") && !AN.GetBool("attack3") && !AN.GetBool(skill1ID) && !AN.GetBool(skill2ID) && !AN.GetBool(hurtID) && !AN.GetBool(jumpID) && !AN.GetBool(deathID))
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

            if (!isGround && !AN.GetBool(fallID) && !AN.GetBool("attack1") && !AN.GetBool("attack2") && !AN.GetBool("attack3") && !AN.GetBool(skill1ID) && !AN.GetBool(skill2ID) && !AN.GetBool(jumpID) && !AN.GetBool(hurtID)&& !AN.GetBool(deathID))
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

    


    public override void Attack()
    {
        NetworkManager.instance.ControlUI.SetActive(false);

        floatingJoystick.Horizontal = 0f;
        floatingJoystick.Vertical = 0f;

        StartCoroutine(AttackUseRangeCRT());
    }

    public IEnumerator AttackUseRangeCRT()
    {
        yield return new WaitForSeconds(0.25f);
        taraAttackUseRangeScript.time = 0.5f;
        taraAttackUseRangeScript.count = 0;
        taraAttackUseRangeScript.gameObject.SetActive(true);
    }


    [PunRPC]
    void AttackRPC(float x, float y, bool blueTeam, int count)
    {
        if (count == 1)
        {
            NetworkManager.instance.soundObjectPool.GetObject(new Vector2(x,y), 14);
            StartCoroutine(ANAttack1CRT());
        }
        else if (count == 2)
        {
            NetworkManager.instance.soundObjectPool.GetObject(new Vector2(x, y), 15);
            StartCoroutine(ANAttack2CRT());
        }
        else if (count == 3)
        {
            NetworkManager.instance.soundObjectPool.GetObject(new Vector2(x, y), 14);
            StartCoroutine(ANAttack3CRT());
        }
        taraAttackExplosionObjectPool.GetObject(x, y, blueTeam);
    }

    public IEnumerator ANAttack1CRT()
    {
        AN.SetBool("attack2", false);
        AN.SetBool("attack3", false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool("attack1", true);
        yield return new WaitForSeconds(0.533f);
        AN.SetBool("attack1", false);
    }

    public IEnumerator ANAttack2CRT()
    {
        AN.SetBool("attack1", false);
        AN.SetBool("attack2", true);
        yield return new WaitForSeconds(0.533f);
        AN.SetBool("attack2", false);
    }

    public IEnumerator ANAttack3CRT()
    {
        AN.SetBool("attack2", false);
        AN.SetBool("attack3", true);
        yield return new WaitForSeconds(0.533f);
        AN.SetBool("attack3", false);
    }

    public void Attack1End()
    {
        if (AN.GetBool("attack1"))
            AN.SetBool("attack1", false);
    }
    public void Attack2End()
    {
        if (AN.GetBool("attack2"))
            AN.SetBool("attack2", false);
    }

    public void Attack3End()
    {
        if (AN.GetBool("attack3"))
            AN.SetBool("attack3", false);
    }




    public override void Skill1()
    {
        statScript.JumpLock = true;
        statScript.SkillLock = true;

        NetworkManager.instance.ControlUI.SetActive(false);

        floatingJoystick.Horizontal = 0f;
        floatingJoystick.Vertical = 0f;

        taraS1UseRangeScript.isChecking = false;
        taraS1UseRangeScript.RangeImage.enabled = true;
    }

    [PunRPC]
    void Skill1RPC(float x, float y, bool blueTeam, bool flipX)
    {
        NetworkManager.instance.soundObjectPool.GetObject(new Vector2(x, y), 17);
        AN.SetBool("attack1", false);
        AN.SetBool("attack2", false);
        AN.SetBool("attack3", false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(skill1ID, true);
        taraS1ObjectPool.GetObject(x, y, blueTeam, flipX);
    }

    public IEnumerator Skill1MoveLockCRT()
    {
        yield return new WaitForSeconds(0.2f);
        statScript.JumpLock = false;
        statScript.SkillLock = false;
    }




    public override void Skill2()
    {
        statScript.JumpLock = true;
        statScript.SkillLock = true;

        NetworkManager.instance.ControlUI.SetActive(false);

        floatingJoystick.Horizontal = 0f;
        floatingJoystick.Vertical = 0f;

        taraS2UseRangeScript.isChecking = false;
        taraS2UseRangeScript.RangeImage.enabled = true;
    }

    [PunRPC]
    void Skill2RPC(Vector2 position)
    {
        AN.SetBool("attack1", false);
        AN.SetBool("attack2", false);
        AN.SetBool("attack3", false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(ultID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(skill2ID, true);
        taraS2ObjectPool.GetObject(position, statScript.BlueTeam);
    }

    public IEnumerator Skill2MoveLockCRT()
    {
        yield return new WaitForSeconds(0.25f);
        statScript.JumpLock = false;
        statScript.SkillLock = false;
    }



    public override void Ult()
    {
        statScript.JumpLock = true;
        statScript.MoveLock = true;
        statScript.SkillLock = true;

        Ulting = true;
        RB.velocity = Vector2.zero;

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
                tempVector = tempVector.normalized;
            }
        }

        PV.RPC("ImuneStunRPC", RpcTarget.All, true);
        PV.RPC("ImuneStiffRPC", RpcTarget.All, true);
        PV.RPC("UltShowRangeRPC", RpcTarget.All, (Vector2)transform.position, tempVector);
        PV.RPC("UltRPC", RpcTarget.All, (Vector2)transform.position, statScript.BlueTeam, tempVector);
        StartCoroutine(UltCRT(tempVector));
    }

    [PunRPC]
    public void UltShowRangeRPC(Vector2 taraPosition, Vector2 tempVector)
    {
        //sound
        tara_SoundScript.AS_Ult_Play();

        statScript.curPos = taraPosition;
        transform.position = taraPosition;

        AN.SetBool("attack1", false);
        AN.SetBool("attack2", false);
        AN.SetBool("attack3", false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(ultID, true);

        if (tempVector.x < 0)
        {
            ArmSR.flipY = true;
            ArmSR.transform.position = taraPosition + new Vector2(-0.37778f, 0.366681f);
        }
        else
        {
            ArmSR.flipY = false;
            ArmSR.transform.position = taraPosition + new Vector2(0.37778f, 0.366681f);
        }

        ArmSR.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * (Mathf.Atan2(tempVector.y, tempVector.x)));
        if (!statScript.OnBush)
        {
            ArmSR.enabled = true;
        }
        ArmAN.SetTrigger(ultID);
        StartCoroutine(ANArmCRT());
        taraUltShowRangeObjectPool.GetObject(taraPosition, tempVector);
    }

    public IEnumerator ANArmCRT()
    {
        yield return new WaitForSeconds(1.0667f);
        ArmSR.enabled = false;
    }

    public IEnumerator UltCRT(Vector2 tempVector)
    {
        yield return new WaitForSeconds(1.2f);
        PV.RPC("ImuneStunRPC", RpcTarget.All, false);
        PV.RPC("ImuneStiffRPC", RpcTarget.All, false);
        statScript.JumpLock = false;
        statScript.MoveLock = false;
        statScript.SkillLock = false;
    }

    [PunRPC]
    public void UltRPC(Vector2 taraPosition, bool blueTeam, Vector2 tempVector)
    {
        taraUltObjectPool.GetObject(taraPosition, blueTeam, tempVector);
    }

}
