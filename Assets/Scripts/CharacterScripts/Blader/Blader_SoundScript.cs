using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blader_SoundScript : Character_Sound_Base
{
    public AudioSource AS_Skill2_On;
    public AudioSource AS_Skill2_Off;
    public AudioSource AS_Ult1;
    public AudioSource AS_Ult2;
    public AudioSource AS_Ult3;
    public AudioSource AS_Ult4;

    public void AS_Skill2_On_Play() => AS_Skill2_On.Play();

    public void AS_Skill2_Off_Play() => AS_Skill2_Off.Play();

    public void AS_Ult1_Play() => AS_Ult1.Play();
    public void AS_Ult2_Play() => AS_Ult2.Play();
    public void AS_Ult3_Play() => AS_Ult3.Play();
    public void AS_Ult4_Play() => AS_Ult4.Play();
}