using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Sound_Base : MonoBehaviour
{
    public AudioSource AS_Walk_Grass;
    public AudioSource AS_Walk_Concrete;
    public AudioSource AS_Jump;
    public AudioSource AS_Land_Grass;
    public AudioSource AS_Land_Concrete;
    public AudioSource AS_Attack;
    public AudioSource AS_Skill1;
    public AudioSource AS_Skill2;
    public AudioSource AS_Ult;

    float GrassOrConcreteY=-0.5f;



    public Character_Base character_Base;

    public Animator AN;//character AN

    //AN Parameter Hash
    private int attackID;
    private int skill1ID;
    private int skill2ID;
    private int ultID;
    private int hurtID;
    private int walkID;
    private int jumpID;
    private int fallID;
    private int landID;
    private int deathID;
    
    private int jumpAttackID;
    private int jumpUltID;
    private int jump2ID;
    
    private int attack1ID;
    private int attack2ID;
    
    private int skill1EndID;
    
    private int skill2StartID;
    private int skill2EndID;
    
    private int ultStartID;

    //Walk
    public virtual void AS_Walk_Play()
    {
        if (character_Base.transform.position.y < GrassOrConcreteY)
        {
            AS_Walk_Grass.Play();
        }
        else
        {
            AS_Walk_Concrete.Play();
        }
    }
    public virtual void AS_Walk_Stop()
    {
        AS_Walk_Grass.Stop();
        AS_Walk_Concrete.Stop();
    }

    //Jump
    public virtual void AS_Jump_Play() => AS_Jump.Play();

    //Land
    public virtual void AS_Land_Play()
    {
        if (character_Base.transform.position.y < GrassOrConcreteY)
        {
            AS_Land_Grass.Play();
        }
        else
        {
            AS_Land_Concrete.Play();
        }
    }

    //Attack
    public virtual void AS_Attack_Play()
    {
        AS_Attack.Play();
    }

    public virtual void AS_Skill1_Play()
    {
        AS_Skill1.Play();
    }

    public virtual void AS_Skill2_Play() => AS_Skill2.Play();

    public virtual void AS_Ult_Play() => AS_Ult.Play();

    private void Start()
    {
        attackID = character_Base.attackID;
        skill1ID = character_Base.skill1ID;
        skill2ID = character_Base.skill2ID;
        ultID = character_Base.ultID;
        hurtID = character_Base.hurtID;
        walkID = character_Base.walkID;
        jumpID = character_Base.jumpID;
        fallID = character_Base.fallID;
        landID = character_Base.landID;
        deathID = character_Base.deathID;

        jumpAttackID = character_Base.jumpAttackID;
        jumpUltID = character_Base.jumpUltID;
        jump2ID = character_Base.jump2ID;

        attack1ID = character_Base.attack1ID;
        attack2ID = character_Base.attack2ID;

        skill1EndID = character_Base.skill1EndID;

        skill2StartID = character_Base.skill2StartID;
        skill2EndID = character_Base.skill2EndID;

        ultStartID = character_Base.ultStartID;
    }

    public virtual void Update()
    {
        if (AN.GetBool(walkID))
        {
            if (!AS_Walk_Grass.isPlaying && !AS_Walk_Concrete.isPlaying) AS_Walk_Play();
        }
        else
        {
            if (AS_Walk_Grass.isPlaying || AS_Walk_Concrete.isPlaying) AS_Walk_Stop();
        }
    }
}
