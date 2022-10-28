using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public AudioSource AS;
    public AudioClip AC_Login;
    public AudioClip AC_Main;
    public AudioClip AC_Story;
    public AudioClip AC_CharacterSelect;
    public AudioClip AC_CaptureMap;



    public Sound_Background Sound_Background { get { return Sound_Background; } set { Sound_Background = value; } }
    public Sound_Background sound_Background;

    public void AS_Setting(Sound_Background value)
    {
        if(value== Sound_Background.Login)
        {
            AS.clip = AC_Login;
        }
        else if (value == Sound_Background.Main)
        {
            AS.clip = AC_Main;
        }
        else if (value == Sound_Background.Story)
        {
            AS.clip = AC_Story;
        }
        else if (value == Sound_Background.CharacterSelect)
        {
            AS.clip = AC_CharacterSelect;
        }
        else if (value == Sound_Background.CaptureMap)
        {
            AS.clip = AC_CaptureMap;
        }
        AS.Play();
    }
}

public enum Sound_Background { Login, Main, Story, CharacterSelect, CaptureMap }
