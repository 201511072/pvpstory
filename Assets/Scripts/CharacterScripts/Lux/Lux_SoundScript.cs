using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lux_SoundScript : Character_Sound_Base
{
    public AudioSource AS_Skill2_On;
    public AudioSource AS_Skill2_Off;

    public virtual void AS_Skill2_On_Play()
    {
        AS_Skill2_On.Play();
    }

    public virtual void AS_Skill2_Off_Play()
    {
        AS_Skill2_Off.Play();
    }

    public override void Update()
    {
    }
}
