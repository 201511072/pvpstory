using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Character_Base : MonoBehaviourPunCallbacks
{
    public Rigidbody2D RB;
    protected float OriginalGravityScale;
    public Animator AN;
    public SpriteRenderer SR;
    public PhotonView PV;
    public FloatingJoystick floatingJoystick;
    public StatScript statScript;

    public Vector2 skillJoystick;

    [Header("Jump")]
    public JumpBtnScript jumpBtnScript;
    public GameObject JumpBtn;
    public bool isGround;
    public float JumpPower;

    [Header("Attack")]
    public AttackBtnScript attackBtnScript;
    public GameObject AttackBtn;
    public float AttackDelay;

    [Header("Skill1")]
    public S1BtnScript s1BtnScript;
    public GameObject Skill1Btn;
    public float Skill1Delay;

    [Header("Skill2")]
    public S2BtnScript s2BtnScript;
    public GameObject Skill2Btn;
    public float Skill2Delay;

    [Header("Ult")]
    public UltBtnScript ultBtnScript;
    public GameObject UltBtn;
    public float UltGauge;
    public float UltGaugeSpeed = 1f;
    public bool Ulting;

    [Header("Land")]
    public bool Land;

    [Header("AN Parameter ID")]
    public int attackID = Animator.StringToHash("attack");
    public int skill1ID = Animator.StringToHash("skill1");
    public int skill2ID = Animator.StringToHash("skill2");
    public int ultID = Animator.StringToHash("ult");
    public int hurtID = Animator.StringToHash("hurt");
    public int walkID = Animator.StringToHash("walk");
    public int jumpID = Animator.StringToHash("jump");
    public int fallID = Animator.StringToHash("fall");
    public int landID = Animator.StringToHash("land");
    public int deathID = Animator.StringToHash("death");

    public int jumpAttackID = Animator.StringToHash("jumpAttack");
    public int jumpUltID = Animator.StringToHash("jumpUlt");
    public int jump2ID = Animator.StringToHash("jump2");

    public int attack1ID = Animator.StringToHash("attack1");
    public int attack2ID = Animator.StringToHash("attack2");

    public int skill1EndID = Animator.StringToHash("skill1End");

    public int skill2StartID = Animator.StringToHash("skill2Start");
    public int skill2EndID = Animator.StringToHash("skill2End");

    public int ultStartID = Animator.StringToHash("ultStart");

    [Header("Sound")]
    public Character_Sound_Base character_Sound_Base;

    public virtual void Start()
    {
        OriginalGravityScale = RB.gravityScale;
    }

    protected virtual void Awake()
    {
        if (PV.IsMine)
        {
            floatingJoystick = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();

            JumpBtn = GameObject.Find("JumpBtn");
            jumpBtnScript = JumpBtn.GetComponent<JumpBtnScript>();
            jumpBtnScript.Init();

            AttackBtn = GameObject.Find("AttackBtn");
            attackBtnScript = AttackBtn.GetComponent<AttackBtnScript>();
            attackBtnScript.Init();

            Skill1Btn = GameObject.Find("S1Btn");
            s1BtnScript = Skill1Btn.GetComponent<S1BtnScript>();
            s1BtnScript.Init();

            Skill2Btn = GameObject.Find("S2Btn");
            s2BtnScript = Skill2Btn.GetComponent<S2BtnScript>();
            s2BtnScript.Init();

            UltBtn = GameObject.Find("UltBtn");
            ultBtnScript = UltBtn.GetComponent<UltBtnScript>();
            ultBtnScript.Init();
        }
    }

    public virtual void Attack() { }

    public virtual void Skill1() { }

    public virtual void Skill2() { }

    public virtual void Ult() { }


    public virtual void HurtMotion()
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

    public virtual void Jump()
    {
        if (!statScript.isJumping && isGround && !statScript.JumpLock && !statScript.StunJumpLock && !statScript.OnZonya)
        {
            StartCoroutine(isJumpingCRT());
            PV.RPC("JumpRPC", RpcTarget.All);
            AN.SetBool(jumpID, true);
        }
    }

    protected IEnumerator isJumpingCRT()
    {
        statScript.isJumping = true;
        yield return new WaitForSeconds(0.3f);
        statScript.isJumping = false;
    }

    [PunRPC]
    public virtual void JumpRPC()
    {
        RB.velocity = Vector2.zero;
        RB.AddForce(Vector2.up * JumpPower);
        if (statScript.JobCode != 4&& statScript.JobCode != 5) character_Sound_Base.AS_Jump_Play();//럭스 아니면 점프소리 실행
    }




    public void JumpEnd()
    {
        if (AN.GetBool(jumpID))
            AN.SetBool(jumpID, false);
    }

    public virtual void AttackEnd()
    {
        if (AN.GetBool(attackID))
            AN.SetBool(attackID, false);
    }

    public virtual void Skill1End()
    {
        if (AN.GetBool(skill1ID))
            AN.SetBool(skill1ID, false);
    }

    public void Skill2End()
    {
        if (AN.GetBool(skill2ID))
            AN.SetBool(skill2ID, false);
    }

    public virtual void UltEnd()
    {
        if (AN.GetBool(ultID))
            AN.SetBool(ultID, false);
    }

    public virtual void jumpUltEnd()
    {
        if (AN.GetBool(jumpUltID))
            AN.SetBool(jumpUltID, false);
    }

    public virtual void ANWalk()
    {
        if (!AN.GetBool(jumpID) && !AN.GetBool(fallID))
        {
            AN.SetBool(attackID, false);
            AN.SetBool(skill1ID, false);
            AN.SetBool(skill2ID, false);
            AN.SetBool(walkID, true);
        }
    }

    public virtual void ANWalkFalse()
    {
        AN.SetBool(walkID, false);
    }


    public virtual void Death()
    {
        AN.SetBool(attackID, false);
        AN.SetBool(skill1ID, false);
        AN.SetBool(skill2ID, false);
        AN.SetBool(hurtID, false);
        AN.SetBool(walkID, false);
        AN.SetBool(jumpID, false);
        AN.SetBool(fallID, false);
        //모든모션 끄기
        AN.SetBool(deathID, true);
    }

    public virtual void DeathEnd()
    {
        SR.enabled = false;
        AN.SetBool(deathID, false);
    }
}
