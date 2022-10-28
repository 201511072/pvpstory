using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Cinemachine;

public class ArcherScript : Character_Base
{
    public bool Attack2ANBoolChange;
    public bool ArcherArmFalse;

    public ArcherAttackObjectPool archerAttackObjectPool;
    public SpriteRenderer ArcherArm;
    public Animator ArmAN;
    public float ArrowSpeed;
    public ArcherAttackEffectObjectPool archerAttackEffectObjectPool;
    public ArcherAttackHitEffectObjectPool archerAttackHitEffectObjectPool;


    public ArcherS1Btn archerS1Btn;
    public HurricaneObjectPool hurricaneObjectPool;
    public float[] arrowPlusATK = { 342f, 612f, 342f };//0 일반 350데미지(342), 1 스킬1 강화 500데미지 (612), 2 궁극기 350데미지(342)
    public bool S1EnhanceArrow;
    public float S1EnhanceTime;
    bool S1VelocityLerp;
    Vector2 S1Velocity;
    bool S1DirChanged;
    bool S1VelocityXpositive;
    bool S1VelocityYpositive;

    public Animator S1AN;
    public SpriteRenderer S1SR;

    public Archer_SoundScript archer_SoundScript;

    public override void Start()
    {
        base.Start();
        statScript.JobCode = 1;
        statScript.archerScript = this;
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
                AN.SetBool(jumpAttackID, false);
                AN.SetBool(skill1ID, false);
                AN.SetBool(skill2ID, false);
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
        AN.SetBool(jumpAttackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        ArcherArm.enabled = false;
        //모든모션 끄기
        AN.SetBool(deathID, true);

        ArcherArm.enabled = false;
        if (floatingJoystick != null)
        {
            floatingJoystick.Horizontal = 0f;
            floatingJoystick.Vertical = 0f;
        }

        StartCoroutine(DeathAgainCRT());
        if(tempUltCRT!=null) StopCoroutine(tempUltCRT);
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
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.05f);
            AN.SetBool(attackID, false);
            AN.SetBool(jumpAttackID, false);
            AN.SetBool(skill1ID, false);
            AN.SetBool(skill2ID, false);
            AN.SetBool(hurtID, false);
            AN.SetBool(walkID, false);
            AN.SetBool(jumpID, false);
            AN.SetBool(fallID, false);
            ArcherArm.enabled = false;
            if (floatingJoystick != null)
            {
                floatingJoystick.Horizontal = 0f;
                floatingJoystick.Vertical = 0f;
            }

            statScript.OnSunFire = false;
        }
    }


    protected override void Awake()
    {
        base.Awake();
        ArrowSpeed = 10f;
        if (PV.IsMine)
        {
            archerS1Btn = Skill1Btn.GetComponent<ArcherS1Btn>();
        }
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            isGround = Physics2D.OverlapArea((Vector2)transform.position + new Vector2(-0.37f, -0.485f), (Vector2)transform.position + new Vector2(0.37f, -0.98f), 11 << 9);//x는 절반의 0.01만큼 좌우 각각 적게.  y는 절반의 0.005만큼 더 낮은곳에서 0.495더 낮은곳까지

            if (isGround && Land && !AN.GetBool(attackID) && !AN.GetBool(jumpAttackID) && !AN.GetBool(skill1ID) && !AN.GetBool(skill2ID) && !AN.GetBool(hurtID) && !AN.GetBool(jumpID) && !AN.GetBool(deathID) &&
                AN.GetCurrentAnimatorStateInfo(0).shortNameHash != ultID && AN.GetCurrentAnimatorStateInfo(0).shortNameHash != jumpUltID)
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

            if (!isGround && !AN.GetBool(fallID) && !AN.GetBool(attackID) && !AN.GetBool(jumpAttackID) && !AN.GetBool(skill1ID) && !AN.GetBool(skill2ID) && !AN.GetBool(jumpID) && !AN.GetBool(hurtID) && !AN.GetBool(deathID) &&
                AN.GetCurrentAnimatorStateInfo(0).shortNameHash != ultID && AN.GetCurrentAnimatorStateInfo(0).shortNameHash != jumpUltID)
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

        if (S1EnhanceArrow)
        {
            S1EnhanceTime -= Time.deltaTime;
            if (S1EnhanceTime < 0)
            {
                S1EnhanceArrow = false;
                S1AN.enabled = false;
                S1SR.enabled = false;
            }
        }

        if (S1VelocityLerp && !S1DirChanged)
        {
            RB.velocity -= RB.velocity * Time.deltaTime * 3.7f;
            if (S1VelocityXpositive != (RB.velocity.x > 0))
            {
                S1DirChanged = true;
            }
            if (S1VelocityYpositive != (RB.velocity.y > 0.0002))
            {
                S1DirChanged = true;
            }
        }
    }

    public override void ANWalk()
    {
        if (!AN.GetBool(jumpID) && !AN.GetBool(fallID) && !AN.GetBool(deathID))
        {
            ArcherArm.enabled = false;
            ArmAN.SetBool(attackID, false);
            AN.SetBool(attackID, false);
            AN.SetBool(jumpAttackID, false);
            AN.SetBool(skill1ID, false);
            AN.SetBool(skill2ID, false);
            AN.SetBool(hurtID, false);
            AN.SetBool(walkID, true);
        }
    }

    public override void AttackEnd()
    {
        if (AN.GetBool(attackID))
        {
            AN.SetBool(attackID, false);
            ArcherArm.enabled = false;
            ArmAN.SetBool(attackID, false);
        }
    }

    public void JumpAttackEnd()
    {
        if (AN.GetBool(jumpAttackID))
        {
            AN.SetBool(jumpAttackID, false);
            ArcherArm.enabled = false;
            ArmAN.SetBool(attackID, false);
        }
    }

    public override void UltEnd()
    {
        ArcherArm.enabled = false;
    }

    public override void jumpUltEnd()
    {
        ArcherArm.enabled = false;
    }



    [PunRPC]
    public void ANLandRPC()
    {
        ArcherArm.enabled = false;
        ArmAN.SetBool(attackID, false);
        AN.SetTrigger(landID);
        archer_SoundScript.AS_Land_Play();
    }

    [PunRPC]
    public void ANFallRPC(bool value)
    {
        ArcherArm.enabled = false;
        ArmAN.SetBool(attackID, false);
        AN.SetBool(fallID, value);
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
                if (isGround)
                {
                    PV.RPC("ANAttackRPC", RpcTarget.All, false, 0f, (Vector2)transform.position);
                }
                else
                {
                    PV.RPC("ANJumpAttackRPC", RpcTarget.All, false, 0f, (Vector2)transform.position);
                }
            }
            else
            {
                if (isGround)
                {
                    PV.RPC("ANAttackRPC", RpcTarget.All, true, 0f, (Vector2)transform.position);
                }
                else
                {
                    PV.RPC("ANJumpAttackRPC", RpcTarget.All, true, 0f, (Vector2)transform.position);
                }
            }
        }
        else if (tempVector.x > 0)
        {
            if (isGround)
            {
                PV.RPC("ANAttackRPC", RpcTarget.All, false, Mathf.Rad2Deg * (Mathf.Atan(tempVector.y / tempVector.x)), (Vector2)transform.position);
            }
            else
            {
                PV.RPC("ANJumpAttackRPC", RpcTarget.All, false, Mathf.Rad2Deg * (Mathf.Atan(tempVector.y / tempVector.x)), (Vector2)transform.position);
            }
        }
        else if (tempVector.x < 0)
        {
            if (isGround)
            {
                PV.RPC("ANAttackRPC", RpcTarget.All, true, Mathf.Rad2Deg * (Mathf.Atan(tempVector.y / tempVector.x)), (Vector2)transform.position);
            }
            else
            {
                PV.RPC("ANJumpAttackRPC", RpcTarget.All, true, Mathf.Rad2Deg * (Mathf.Atan(tempVector.y / tempVector.x)), (Vector2)transform.position);
            }
        }


        float tempArrowPlusAtk;
        if (S1EnhanceArrow)
        {
            tempArrowPlusAtk = arrowPlusATK[1];
        }
        else
        {
            tempArrowPlusAtk = arrowPlusATK[0];
        }


        PV.RPC("MoveLockRPC", RpcTarget.All, 0.2f);
        yield return new WaitForSeconds(0.2f);

        PV.RPC("AttackRPC", RpcTarget.All, tempPosition.x, tempPosition.y, statScript.BlueTeam, tempVector.x, tempVector.y, tempArrowPlusAtk);
    }

    [PunRPC]
    public void ANAttackRPC(bool flipX, float rotationZ, Vector2 archerPosition)
    {
        ArcherArm.flipX = flipX;
        ArcherArm.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        ArcherArm.transform.position = archerPosition + new Vector2(-0.1062487f * (flipX ? -1f : 1f), 0.18125f);
        ArmAN.SetBool(attackID, true);
        ArcherArm.color = SR.color;
        ArcherArm.enabled = true;
        

        AN.SetBool(jumpAttackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(attackID, true);

        //사운드는 평타강화 여부에 따라서 소리가 달라지므로 ANAttack에서 구현함
    }

    [PunRPC]
    public void ANJumpAttackRPC(bool flipX, float rotationZ, Vector2 archerPosition)
    {
        ArcherArm.flipX = flipX;
        ArcherArm.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        ArcherArm.transform.position = archerPosition + new Vector2(-0.1062487f * (flipX ? -1f : 1f), 0.18125f);
        ArmAN.SetBool(attackID, true);
        ArcherArm.color = SR.color;
        ArcherArm.enabled = true;
        

        AN.SetBool(attackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(jumpAttackID, true);

        //사운드는 평타강화 여부에 따라서 소리가 달라지므로 ANAttack에서 구현함
    }

    [PunRPC]
    public void AttackRPC(float positionX, float positionY, bool isBlueTeam, float vectorX, float vectorY, float arrowPlusATK)
    {
        archerAttackEffectObjectPool.GetObject(new Vector2(positionX, positionY), new Vector2(vectorX, vectorY), arrowPlusATK);
        S1EnhanceArrow = false;
        S1AN.enabled = false;
        S1SR.enabled = false;
        float tempArrowSpeed = 0f;
        if (arrowPlusATK == this.arrowPlusATK[0])
        {
            tempArrowSpeed = 10f;
            archer_SoundScript.AS_Attack_Play();
        }
        else if (arrowPlusATK == this.arrowPlusATK[1])
        {
            tempArrowSpeed = 12f;
            archer_SoundScript.AS_Attack2_Play();
        }
        archerAttackObjectPool.GetObject(positionX, positionY, isBlueTeam, vectorX, vectorY, arrowPlusATK, tempArrowSpeed);
    }


    public override void Skill1()
    {
        statScript.JumpLock = true;
        statScript.MoveLock = true;
        statScript.SkillLock = true;

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
                tempVector = tempVector.normalized;//곱하기 원하는 속도 
            }
        }
        archerS1Btn.archerS1CooltimeScript.GetCooltime = true;
        PV.RPC("Skill1RPC", RpcTarget.All, tempVector.x, tempVector.y);
    }

    [PunRPC]
    public void ANSkill1RPC()
    {
        ArcherArm.enabled = false;
        ArmAN.SetBool(attackID, false);

        AN.SetBool(attackID, false);
        AN.SetBool(jumpAttackID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(skill1ID, true);

        S1AN.enabled = true;
        S1SR.enabled = true;

        archer_SoundScript.AS_Skill1_Play();
    }

    [PunRPC]
    public void Skill1RPC(float vectorX, float vectorY)
    {
        S1EnhanceTime = 5f;
        S1EnhanceArrow = true;
        RB.gravityScale = 0f;
        RB.velocity = new Vector2(vectorX, vectorY) * 16.5f;
        S1Velocity = RB.velocity;
        S1VelocityXpositive = S1Velocity.x > 0;
        S1VelocityYpositive = S1Velocity.y > 0;
        S1DirChanged = false;
        StartCoroutine(S1MoveLockCRT());
    }

    public IEnumerator S1MoveLockCRT()
    {
        yield return new WaitForSeconds(0.03f);
        S1VelocityLerp = true;
        yield return new WaitForSeconds(0.27f);
        S1VelocityLerp = false;
        RB.gravityScale = OriginalGravityScale;
        RB.velocity = Vector2.zero;
        statScript.JumpLock = false;
        statScript.MoveLock = false;
        statScript.SkillLock = false;
        if (PV.IsMine)
        {
            archerS1Btn.archerS1CooltimeScript.GetCooltime = false;
        }
    }


    public override void Skill2()
    {
        StartCoroutine(Skill2CRT());
    }


    IEnumerator Skill2CRT()
    {
        PV.RPC("MoveLockRPC", RpcTarget.All, 0.25f);
        PV.RPC("ANSkill2TrueRPC", RpcTarget.All);

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

        Vector2 tempPosition = (Vector2)transform.position + new Vector2(0f, 0.3f);

        yield return new WaitForSeconds(0.25f);

        PV.RPC("ANSkill2FalseRPC", RpcTarget.All);

        PV.RPC("Skill2RPC", RpcTarget.All, tempPosition.x, tempPosition.y, temp_flipX ? -1 : 1, statScript.BlueTeam);
    }

    [PunRPC]
    void Skill2RPC(float positionX, float positionY, int dir, bool isBlueTeam)
    {
        NetworkManager.instance.tempHurricaneScript = hurricaneObjectPool.GetObject(positionX, positionY, dir, isBlueTeam);
    }

    [PunRPC]
    public void ANSkill2TrueRPC()
    {
        ArcherArm.enabled = false;
        ArmAN.SetBool(attackID, false);

        AN.SetBool(attackID, false);
        AN.SetBool(jumpAttackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(skill2ID, true);

        archer_SoundScript.AS_Skill2_Play();
    }

    [PunRPC]
    public void ANSkill2FalseRPC()
    {
        AN.SetBool(skill2ID, false);
    }

    public Coroutine tempUltCRT;

    public override void Ult()
    {
        PV.RPC("MoaiS2OptionRPC", RpcTarget.All, false);
        PV.RPC("GravityRPC", RpcTarget.All, 0f);
        Ulting = true;
        RB.velocity = Vector2.zero;
        statScript.MoveLock = true;
        statScript.JumpLock = true;
        statScript.SkillLock = true;
        tempUltCRT=StartCoroutine(UltCRT());
    }

    IEnumerator UltCRT()
    {
        for (int i = 0; i < 14; i++)
        {
            UltMethod();
            yield return new WaitForSeconds(0.11f);
        }
        yield return new WaitForSeconds(0.09f);
        PV.RPC("GravityRPC", RpcTarget.All, statScript.OriginalGravityScale);
        PV.RPC("MoaiS2OptionRPC", RpcTarget.All, true);
        statScript.MoveLock = false;
        statScript.JumpLock = false;
        statScript.SkillLock = false;
        Ulting = false;
    }

    public void UltMethod()
    {
        Vector2 tempVector = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical);
        Vector2 tempPosition = (Vector2)transform.position + tempVector.normalized * 0.4f;
        bool temp_flipX = tempVector.x < 0;

        if (tempVector == Vector2.zero)
        {
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
                if((tempVector.x>-0.000001f && tempVector.x < 0.000001f)&&(tempVector.y > -0.000001f && tempVector.y < 0.000001f))
                {
                    if (tempVector.x* tempVector.x < tempVector.y* tempVector.y)
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
                else if (tempVector.y > -0.000001f && tempVector.y < 0.000001f && tempVector.x == 0f)
                {
                    tempVector.x = 1f;
                }
                tempVector = tempVector.normalized;
            }
        }



        if (tempVector.x == 0f && tempVector.y == 0f)
        {
            if (!temp_flipX)
            {
                if (isGround)
                {
                    PV.RPC("ANUltRPC", RpcTarget.All, false, 0f, (Vector2)transform.position);
                }
                else
                {
                    PV.RPC("ANJumpUltRPC", RpcTarget.All, false, 0f, (Vector2)transform.position);
                }
            }
            else
            {
                if (isGround)
                {
                    PV.RPC("ANUltRPC", RpcTarget.All, true, 180f, (Vector2)transform.position);
                }
                else
                {
                    PV.RPC("ANJumpUltRPC", RpcTarget.All, true, 180f, (Vector2)transform.position);
                }
            }
        }
        else if (tempVector.x > 0)
        {
            if (isGround)
            {
                PV.RPC("ANUltRPC", RpcTarget.All, false, Mathf.Rad2Deg * Mathf.Atan2(tempVector.y , tempVector.x), (Vector2)transform.position);
            }
            else
            {
                PV.RPC("ANJumpUltRPC", RpcTarget.All, false, Mathf.Rad2Deg * Mathf.Atan2(tempVector.y, tempVector.x), (Vector2)transform.position);
            }
        }
        else if (tempVector.x < 0)
        {
            if (isGround)
            {
                PV.RPC("ANUltRPC", RpcTarget.All, true, Mathf.Rad2Deg * Mathf.Atan2(tempVector.y, tempVector.x), (Vector2)transform.position);
            }
            else
            {
                PV.RPC("ANJumpUltRPC", RpcTarget.All, true, Mathf.Rad2Deg * Mathf.Atan2(tempVector.y, tempVector.x), (Vector2)transform.position);
            }
        }

        StartCoroutine(UltRPCCRT(tempPosition.x, tempPosition.y, statScript.BlueTeam, tempVector.x, tempVector.y, arrowPlusATK[2]));
    }

    public IEnumerator UltRPCCRT(float tempPositionX, float tempPositionY, bool isBlueTeam, float tempVectorX, float tempVectorY, float arrowPlusATK)
    {
        yield return new WaitForSeconds(0.11f);
        PV.RPC("UltRPC", RpcTarget.All, tempPositionX, tempPositionY, isBlueTeam, tempVectorX, tempVectorY, arrowPlusATK);
    }

    [PunRPC]
    public void UltRPC(float positionX, float positionY, bool isBlueTeam, float vectorX, float vectorY, float arrowPlusATK)
    {
        archerAttackObjectPool.GetObject(positionX, positionY, isBlueTeam, vectorX, vectorY, arrowPlusATK, 15f);
    }

    [PunRPC]
    public void ANUltRPC(bool flipX, float rotationZ, Vector2 archerPosition)
    {
        ArmAN.SetBool(attackID, false);
        SR.flipX = flipX;
        ArcherArm.flipX = false;
        ArcherArm.flipY = flipX;
        ArcherArm.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        ArcherArm.transform.position = archerPosition + new Vector2(-0.1062487f * (flipX ? -1f : 1f), 0.18125f);
        ArmAN.SetTrigger(ultID);
        ArcherArm.color = SR.color;
        ArcherArm.enabled = true;

        AN.SetBool(attackID, false);
        AN.SetBool(jumpAttackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(hurtID, false);
        AN.SetTrigger(ultID);

        archer_SoundScript.AS_Attack_Play();
    }

    [PunRPC]
    public void ANJumpUltRPC(bool flipX, float rotationZ, Vector2 archerPosition)
    {
        ArmAN.SetBool(attackID, false);
        SR.flipX = flipX;
        ArcherArm.flipX = false;
        ArcherArm.flipY = flipX;
        ArcherArm.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        ArcherArm.transform.position = archerPosition + new Vector2(-0.1062487f * (flipX ? -1f : 1f), 0.18125f);
        ArmAN.SetTrigger(ultID);
        ArcherArm.color = SR.color;
        ArcherArm.enabled = true;

        AN.SetBool(attackID, false);
        AN.SetBool(jumpAttackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        AN.SetBool(hurtID, false);
        AN.SetTrigger(jumpUltID);

        archer_SoundScript.AS_Attack_Play();
    }
}
