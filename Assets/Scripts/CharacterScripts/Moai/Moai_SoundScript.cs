using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moai_SoundScript : Character_Sound_Base
{
    public AudioSource AS_Attack1;
    public AudioSource AS_Attack2;
    public AudioSource AS_Skill1_Start;
    public AudioSource AS_Skill1_End;
    public AudioSource AS_Skill2_Start;
    public AudioSource AS_Skill2_End;

    public void AS_Attack1_Play()
    {
        AS_Attack1.Play();
    }

    public void AS_Attack2_Play()
    {
        AS_Attack2.Play();
    }

    public void AS_Skill1_Start_Play()
    {
        AS_Skill1_Start.Play();
    }

    public void AS_Skill1_End_Play()
    {
        AS_Skill1_End.Play();
    }

    public void AS_Skill2_Start_Play()
    {
        AS_Skill2_Start.Play();
    }

    public void AS_Skill2_End_Play()
    {
        AS_Skill2_Start.Stop();
        AS_Skill2_End.Play();
    }

    public void Pitch_Change_OnUlt(bool value)
    {
        if (value)
        {
            AS_Attack1.pitch = 2f;
            AS_Attack2.pitch = 2f;
        }
        else
        {
            AS_Attack1.pitch = 1f;
            AS_Attack2.pitch = 1f;
        }
    }
}
