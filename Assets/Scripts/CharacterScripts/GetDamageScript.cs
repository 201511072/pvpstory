using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GetDamageScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;
    public Collider2D MyCol;
    public Character_Base character_Base;
    public GameObject canvas;

    public HPBar hpBar;
    public Image hpBar_fillImgae;
    public float PlayerHP;
    public float PlayerMaxHP;
    public float PlayerTotalShield;    //현재 Player가 가지고있는 Shield의 통합량

    public float speed;
    public float originalSpeed;

    public int myPlayerNumber;

    [Header("Shield")]
    public Queue<ShieldScript> ShieldQue = new Queue<ShieldScript>();
    public ShieldScript tempShieldScript;
    public ShieldScript TempShieldScript;
    public bool ShieldRemain;

    [Header("GetDamage")]
    public bool isInvincible;
    public bool isDead;

    public Text NickNameText;
    public FloatingJoystick floatingJoystick;
    public Rigidbody2D RB;
    public Animator AN;
    public Vector3 curPos;
    public SpriteRenderer SR;
    public bool BlueTeam;
    public StatScript teamStatScript;
    public StatScript enemyStatScript1;
    public StatScript enemyStatScript2;

    [Header("Hit")]
    public bool ArcherHurricaneHit;

    public bool BladerAttack1Hit;
    public bool BladerS1Hit;
    public bool BladerS1Aura = true;
    public float BladerS1AuraCount = 10f;
    public Image BladerS1AuraImage;
    public bool DontHitByProjectile;
    public bool OnBladerUlt;
    public StatScript bladerUltEnemy;
    public float BladerUltStartTime;

    public bool MoaiAttackHit;
    public bool MoaiS1Hit;
    public bool MoaiS2Hit;
    public bool MoaiS2Pull;
    public bool MoaiS2End;

    public bool LuxS1Hit;
    public bool LuxUltHit;

    public bool LinkerAttackHit;
    public bool LinkerS1Connected;
    public bool LinkerUltHit;

    public bool TaraAttackHit;
    public bool TaraUlt1Hit;

    bool MagneticCircleHit;

    public LinkerS1CircleScript linkerS1CircleScript;

    bool dontChangeGravity;

    [Header("Stat")]
    public float DEF;
    public float ATK;
    public float FixATK;
    public float PEN;
    public float PerATK;
    public float traitATK;
    public float itemATK;
    public float DamageReduction;


    public bool isJumping;
    public bool MoveLock;
    public bool JumpLock;
    public bool SkillLock;
    public bool StunMoveLock;
    public bool StunJumpLock;
    public bool StunSkillLock;

    public float OriginalGravityScale;
    public bool Aggroed;
    public Vector2 AggroedVector;
    public bool noHurtMotion;
    public SpriteRenderer stunSR;

    [Header("Map")]
    public int Map;
    public bool isMagneticMap;
    public float RespawnTimer;
    public bool RespawnTimerBool;
    public GameObject RespawnPanel;
    public Text RespawnTimerText;


    [Header("Option")]
    public bool imuneStun;
    public bool imuneStiff;

    //옵션이 켜져있으면 효과받음
    public bool ArcherHurricaneOption;
    public bool BladerS1AuraOption;
    public bool MoaiS1Option;
    public bool MoaiS2Option;
    public bool MoaiS2AggroedOption;
    public bool LinkerS1Option;

    [Header("Motion")]
    public float stiffTime;
    public float stunTime;




    public void MapCheck()
    {
        if (Map == 0)
        {
            isMagneticMap = true;
        }
    }



    public void LinkerS1CircleInit()
    {
        if (myPlayerNumber == 1 && NetworkManager.instance.Blue2statScript != null)
        {
            linkerS1CircleScript.teamStatScript = teamStatScript;
            linkerS1CircleScript.linkerS1LRScript.Player2 = teamStatScript.gameObject;
        }
        else if (myPlayerNumber == 2 && NetworkManager.instance.Red2statScript != null)
        {
            linkerS1CircleScript.teamStatScript = teamStatScript;
            linkerS1CircleScript.linkerS1LRScript.Player2 = teamStatScript.gameObject;
        }
        else if (myPlayerNumber == 3 && NetworkManager.instance.Red1statScript != null)
        {
            linkerS1CircleScript.teamStatScript = teamStatScript;
            linkerS1CircleScript.linkerS1LRScript.Player2 = teamStatScript.gameObject;
        }
        else if (myPlayerNumber == 4 && NetworkManager.instance.Blue1statScript != null)
        {
            linkerS1CircleScript.teamStatScript = teamStatScript;
            linkerS1CircleScript.linkerS1LRScript.Player2 = teamStatScript.gameObject;
        }
    }




    public virtual void Update()
    {
        hpBar.setHp(PlayerHP, PlayerMaxHP, PlayerTotalShield);

        if (PV.IsMine)
        {
            /*
            if (isMagneticMap && !MagneticCircleHit && MagneticCircle.IsOutsideCircle_Static(transform.position))
            {
                MagneticCircleHit = true;
                StartCoroutine(MagneticCircleCRT());
                GetDamage(0f, 100f, 0f, 0f, 0f, 0f);
            }
            */
        }


        if (!BladerS1Aura && BladerS1AuraOption)
        {
            if (BladerS1AuraCount > 0f)
            {
                BladerS1AuraCount += -Time.deltaTime;
            }
            else
            {
                BladerS1Aura = true;
                BladerS1AuraImage.enabled = true;
                BladerS1AuraCount = 10f;
            }
        }

        float tempShield = 0;

        if (ShieldQue.Count > 0)
        {
            ShieldScript[] tempArray = new ShieldScript[10];
            ShieldQue.CopyTo(tempArray, 0);
            ShieldQue.Clear();

            for (int i = 0; i < tempArray.Length; i++)
            {
                if (tempArray[i] == null)
                {
                    break;
                }

                for (int k = i + 1; k < tempArray.Length; k++)
                {
                    if (tempArray[k] == null)
                    {
                        break;
                    }
                    if (tempArray[i].duration > tempArray[k].duration)
                    {
                        tempShieldScript = tempArray[i];
                        tempArray[i] = tempArray[k];
                        tempArray[k] = tempShieldScript;
                    }
                }
            }
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (tempArray[i] == null)
                {
                    break;
                }
                tempArray[i].duration -= Time.deltaTime;
            }
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (tempArray[i] == null)
                {
                    break;
                }
                tempShield += tempArray[i].shield;

                if (tempArray[i].duration > 0f)
                {
                    ShieldQue.Enqueue(tempArray[i]);
                }
            }
        }
        PlayerTotalShield = tempShield;

        if (ArcherHurricaneHit && ArcherHurricaneOption)
        {
            transform.position = NetworkManager.instance.tempHurricaneScript.transform.position;
        }

        if (OnBladerUlt)
        {
            BladerUltStartTime += Time.deltaTime;
            if (BladerUltStartTime < 0.5f)
            {
                transform.position = bladerUltEnemy.transform.position + new Vector3(1.5f, 1f, 0f);
            }
            else if (BladerUltStartTime < 1f)
            {
                transform.position = bladerUltEnemy.transform.position + new Vector3(-1.5f, 1f, 0f);
            }
            else if (BladerUltStartTime < 1.5f)
            {
                transform.position = bladerUltEnemy.transform.position + new Vector3(0f, -1.5f, 0f);
            }
            else if (BladerUltStartTime < 2f)
            {
                transform.position = bladerUltEnemy.transform.position + new Vector3(0f, 1.5f, 0f);
            }
            else
            {
                OnBladerUlt = false;
                RB.velocity = Vector2.zero;
                transform.position = bladerUltEnemy.transform.position;
                MyCol.enabled = true;
            }
        }

        if (MoaiS2Pull && MoaiS2Option)
        {
            RB.gravityScale = 6f;
        }


    }



    public virtual void GetDamage(float ATK, float FixATK, float PerDamage, float PEN, float PerPEN, float StiffTime,int attackerPlayerNumber)
    {
        if (LinkerS1Connected)
        {
            teamStatScript.LinkerS1ConnectedGetDamage(ATK, FixATK, PerDamage, PEN, PerPEN, StiffTime, attackerPlayerNumber);
        }

        if (StiffTime > 0f && !imuneStiff)
        {
            PV.RPC("StiffRPC", RpcTarget.All, StiffTime);
        }
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



    public void LinkerS1ConnectedGetDamage(float ATK, float FixATK, float PerDamage, float PEN, float PerPEN, float StiffTime,int attackerPlayerNumber)
    {
        if (StiffTime > 0f)
        {
            PV.RPC("StiffRPC", RpcTarget.All, StiffTime);
        }
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
    public void ShieldChangeRPC(float[] value)
    {
        ShieldQue.Clear();
        for (int i = 0; i < 10; i++)
        {
            if (value[i * 2] == 0 && value[i * 2 + 1] == 0)
            {
                break;
            }
            ShieldQue.Enqueue(new ShieldScript(value[i * 2], value[i * 2 + 1]));
        }
    }


    [PunRPC]
    public void PlayerHPChangeRPC(float PlayerHP,int attackerPlayerNumber)
    {
        this.PlayerHP = PlayerHP;
        DeathCheck(attackerPlayerNumber);
    }


    public virtual void DeathCheck(int attackerPlayerNumber)
    {
        if (PlayerHP <= 0f)
        {
            isDead = true;
            canvas.SetActive(false);
            SR.enabled = false;
            stunSR.enabled = false;
            RB.gravityScale = 0f;
            RB.velocity = Vector2.zero;
            RB.simulated = false;
            MyCol.enabled = false;
        }
    }

    public float[] ShieldQueForRPC()
    {
        float[] shieldArray = new float[20];
        ShieldScript[] ShieldQueArray = new ShieldScript[10];
        ShieldQue.CopyTo(ShieldQueArray, 0);
        for (int i = 0; i < 10; i++)
        {
            if (ShieldQueArray[i] == null)
            {
                break;
            }
            shieldArray[i * 2] = ShieldQueArray[i].duration;
            shieldArray[i * 2 + 1] = ShieldQueArray[i].shield;
        }
        return shieldArray;
    }






    [PunRPC]
    public void MoveLockRPC(float value)
    {
        StartCoroutine(MoveLockCRT(value));
    }

    public IEnumerator MoveLockCRT(float value)
    {
        JumpLock = true;
        MoveLock = true;
        SkillLock = true;
        if (!dontChangeGravity)
        {
            RB.gravityScale = 0f;
        }
        RB.velocity = new Vector2(0.0f, 0.0f);
        yield return new WaitForSeconds(value);
        if (!dontChangeGravity)
        {
            RB.gravityScale = OriginalGravityScale;
        }
        JumpLock = false;
        MoveLock = false;
        SkillLock = false;
    }



    [PunRPC]
    public void StunRPC(float time)
    {
        StartCoroutine(Stun(time));
    }

    public IEnumerator Stun(float time)
    {
        ANStun(time);
        StunSkillLock = true;
        StunJumpLock = true;
        StunMoveLock = true;
        if (!dontChangeGravity)
        {
            RB.gravityScale = 0f;
        }
        RB.velocity = new Vector2(0.0f, 0.0f);
        yield return new WaitForSeconds(time);
        if (!dontChangeGravity)
        {
            RB.gravityScale = OriginalGravityScale;
        }
        StunSkillLock = false;
        StunJumpLock = false;
        StunMoveLock = false;
    }

    public void ANStun(float value)
    {
        if (!isDead && !imuneStun)
        {
            if (stunTime < value)
            {
                stunTime = value;
            }
        }
    }

    [PunRPC]
    public void ImuneStunRPC(bool value)
    {
        imuneStun = value;
    }


    [PunRPC]
    public virtual void StiffRPC(float time)
    {
        StartCoroutine(Stiff(time));
    }

    public IEnumerator Stiff(float time)
    {
        if (!imuneStiff)
        {
            ANHurt(time);
            SkillLock = true;
            JumpLock = true;
            MoveLock = true;
            if (!dontChangeGravity)
            {
                RB.gravityScale = 0f;
            }
            RB.velocity = new Vector2(0.0f, 0.0f);
            yield return new WaitForSeconds(time);
            if (!dontChangeGravity)
            {
                RB.gravityScale = OriginalGravityScale;
            }
            SkillLock = false;
            JumpLock = false;
            MoveLock = false;
        }
    }

    [PunRPC]
    public void ImuneStiffRPC(bool value)
    {
        imuneStiff = value;
    }


    [PunRPC]
    public void ANHurtRPC(float value)
    {
        ANHurt(value);
    }

    public void ANHurt(float value)
    {
        if (!noHurtMotion)
        {
            if (stiffTime < value)
            {
                stiffTime = value;
            }
        }
    }

    [PunRPC]
    public void NoHurtMotionRPC(bool value)
    {
        noHurtMotion = value;
    }


    [PunRPC]
    public void DontHitByArrowRPC(bool value)
    {
        DontHitByProjectile = value;
    }

    [PunRPC]
    public void AggroedRPC(float x, float y)
    {
        AggroedVector = new Vector2(x, y);
        Aggroed = true;
    }

    [PunRPC]
    public void NotAggroedRPC()
    {
        Aggroed = false;
    }


    public IEnumerator For1HitByBladerAttack()
    {
        BladerAttack1Hit = true;
        yield return new WaitForSeconds(0.08f);
        BladerAttack1Hit = false;
    }

    public IEnumerator For1HitByBladerS1()
    {
        BladerS1Hit = true;
        yield return new WaitForSeconds(0.08f);
        BladerS1Hit = false;
    }


    public IEnumerator For1HitByMoaiAttack()
    {
        MoaiAttackHit = true;
        yield return new WaitForSeconds(0.08f);
        MoaiAttackHit = false;
    }


    public IEnumerator For1HitByMoaiS1()
    {
        MoaiS1Hit = true;
        yield return new WaitForSeconds(0.3f);
        MoaiS1Hit = false;
    }

    [PunRPC]
    public void MoaiSkill1RPC()
    {
        if (MoaiS1Option)
        {
            StartCoroutine(MoaiSkill1());
            ANHurt(1.1f);
        }
    }

    public IEnumerator MoaiSkill1()
    {
        SkillLock = true;
        JumpLock = true;
        MoveLock = true;
        RB.velocity = new Vector2(0f, 13f);
        yield return new WaitForSeconds(1.1f);
        SkillLock = false;
        JumpLock = false;
        MoveLock = false;
    }


    public IEnumerator For1HitByMoaiS2()
    {
        MoaiS2Hit = true;
        yield return new WaitForSeconds(0.15f);
        MoaiS2Hit = false;
    }

    public void HitByMoaiS2(Vector3 MoaiPosition)
    {
        Vector2 PullVector = (MoaiPosition - transform.position) * 150f;
        if (MoaiS2Option)
        {
            PV.RPC("MoaiS2PullOnRPC", RpcTarget.All, PullVector);
        }
        if (MoaiS2AggroedOption)
        {
            PV.RPC("AggroedRPC", RpcTarget.All, MoaiPosition.x, MoaiPosition.y);
        }
    }

    [PunRPC]
    public void MoaiS2PullOnRPC(Vector2 PullVector)
    {
        RB.AddForce(PullVector);
        speed -= 0.5f;
        MoaiS2Pull = true;
    }

    [PunRPC]
    public void MoaiS2PullOffRPC()
    {
        MoaiS2Pull = false;
        Aggroed = false;
        if (RB.gravityScale == 6f)
        {
            RB.gravityScale = OriginalGravityScale;
        }
        speed += 0.5f;
    }

    public IEnumerator For1HitByMoaiS2End()
    {
        MoaiS2End = true;
        yield return new WaitForSeconds(0.2f);
        MoaiS2End = false;
    }


    public IEnumerator For1HitByLuxS1()
    {
        LuxS1Hit = true;
        yield return new WaitForSeconds(0.08f);
        LuxS1Hit = false;
    }

    public IEnumerator For1HitByLuxUlt()
    {
        LuxUltHit = true;
        yield return new WaitForSeconds(0.08f);
        LuxUltHit = false;
    }


    public IEnumerator For1HitByLinkerAttack()
    {
        LinkerAttackHit = true;
        yield return new WaitForSeconds(0.12f);
        LinkerAttackHit = false;
    }

    public IEnumerator For1HitByLinkerUlt()
    {
        LinkerUltHit = true;
        yield return new WaitForSeconds(0.12f);
        LinkerUltHit = false;
    }


    public IEnumerator For1HitByTaraAttack()
    {
        TaraAttackHit = true;
        yield return new WaitForSeconds(0.08f);
        TaraAttackHit = false;
    }

    public IEnumerator For1HitByTaraUlt()
    {
        TaraUlt1Hit = true;
        yield return new WaitForSeconds(3f);
        TaraUlt1Hit = false;
    }


    public float SumHPShield()
    {
        return PlayerHP + PlayerTotalShield;
    }




    public IEnumerator MagneticCircleCRT()
    {
        yield return new WaitForSeconds(1f);
        MagneticCircleHit = false;
    }





    [PunRPC]
    public void FlipXRPC(float axis) => SR.flipX = axis < 0;





    [PunRPC]
    public void DamageReductionRPC(float value)
    {
        DamageReduction += value;
        if (DamageReduction >= 1f)
        {
            DamageReduction = 1f;
        }
        else if (DamageReduction <= 0f)
        {
            DamageReduction = 0f;
        }
    }






    [PunRPC]
    public void LinkS1ConnectedRPC(bool value)
    {
        LinkerS1Connected = value;
    }



    [PunRPC]
    public void OnHurricaneRPC()
    {
        if (ArcherHurricaneOption)
        {
            RB.gravityScale = 0f;
            ArcherHurricaneHit = true;
            MoveLock = true;
        }
    }

    [PunRPC]
    public void OffHurricaneRPC()
    {
        if (ArcherHurricaneOption)
        {
            RB.velocity = Vector2.zero;
            RB.gravityScale = OriginalGravityScale;
            ArcherHurricaneHit = false;
            MoveLock = false;
        }
    }


    [PunRPC]
    public void OnLuxBuff()
    {
        StartCoroutine(DEFCRT(2f, 15f));
        ShieldQue.Enqueue(new ShieldScript(2f, 50f));
        StartCoroutine(ATKCRT(2f, 15f));
    }

    public IEnumerator DEFCRT(float time, float DEF)
    {
        this.DEF += DEF;
        yield return new WaitForSeconds(time);
        this.DEF -= DEF;
    }

    public IEnumerator ATKCRT(float time, float ATK)
    {
        this.ATK += ATK;
        yield return new WaitForSeconds(time);
        this.ATK -= ATK;
    }

    [PunRPC]
    public void GravityRPC(float value)
    {
        RB.gravityScale = value;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(SR.flipX);
        }
        else if (stream.IsReading)
        {
            curPos = (Vector3)stream.ReceiveNext();
            SR.flipX = (bool)stream.ReceiveNext();
        }
    }
}
