using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_SoundScript : Character_Sound_Base
{
    public AudioSource AS_Attack2;

    public virtual void AS_Attack2_Play()
    {
        AS_Attack2.Play();
    }
}
