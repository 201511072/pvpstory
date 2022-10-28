using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObjectPool : MonoBehaviour
{
    public Queue<SoundScript> poolingObjectQueue = new Queue<SoundScript>();
    public GameObject SoundScript;



    private void Awake()
    {
        Initialize(50);
    }


    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(SoundScript, new Vector3(100f, 100f, 0f), Quaternion.identity, gameObject.transform).GetComponent<SoundScript>().Init(this);
        }
    }


    public int AC_Code;//0 null
    public AudioClip ArcherAttackHit;//1
    public AudioClip ArcherAttackEnhancedHit;//2
    public AudioClip ArcherAttackHitRock;//3
    public AudioClip ArcherAttackEnhancedHitRock;//4
    public AudioClip BladerSkill2Parrying1;//5
    public AudioClip BladerSkill2Parrying2;//6
    public AudioClip BladerSkill2Parrying3;//7
    public AudioClip BladerSkill2Parrying4;//8
    public AudioClip BladerSkill2Parrying5;//9
    public AudioClip BladerAttackHit;//10
    public AudioClip BladerSkill1Hit;//11
    public AudioClip MoaiAttack1Hit;//12
    public AudioClip MoaiAttack2Hit;//13
    public AudioClip TaraAttack1;//14
    public AudioClip TaraAttack2;//15
    public AudioClip TaraAttackHit;//16
    public AudioClip TaraSkill1On;//17
    public AudioClip TaraSkill1Off;//18
    public AudioClip TaraSkill1Shot;//19
    public AudioClip TaraSkill1Reload;//20
    public AudioClip TaraUltHit;//21
    public AudioClip TaraSkill2Hit;//22
    public AudioClip LuxAttackBound;//23
    public AudioClip LuxAttackExplosion;//24
    public AudioClip LuxUltHit;//25
    public AudioClip Sunfire;//26
    public AudioClip Zonya;//27
    public AudioClip BaronOn;//28
    public AudioClip BaronOff;//29
    public AudioClip Portal;//30
    public AudioClip HpKitOn;//31
    public AudioClip HpKitOff;//32
    public AudioClip GameEnd;//33
    public AudioClip LuxSkill1Hit;//34
    public AudioClip LinkerAttackHit;//35
    public AudioClip LinkerSkill1Connect;//36
    public AudioClip LinkerSkill1Joint;//37
    public AudioClip LinkerSkill1Stun;//38



    public void GetObject(Vector2 position, int AC_Code)
    {
        if (AC_Code != 0) this.AC_Code = AC_Code;
        SoundScript obj = poolingObjectQueue.Dequeue();
        obj.transform.position = position;        
        obj.AS.clip = GetAC(this.AC_Code);
        StartCoroutine(ReturnObject(obj.AS.clip.length, obj));
        obj.AS.Play();
    }

    public AudioClip GetAC(int AC_Code)
    {
        if (AC_Code == 1) return ArcherAttackHit;
        else if (AC_Code == 2) return ArcherAttackEnhancedHit;
        else if (AC_Code == 3) return ArcherAttackHitRock;
        else if (AC_Code == 4) return ArcherAttackEnhancedHitRock;
        else if (AC_Code == 5) return BladerSkill2Parrying1;
        else if (AC_Code == 6) return BladerSkill2Parrying2;
        else if (AC_Code == 7) return BladerSkill2Parrying3;
        else if (AC_Code == 8) return BladerSkill2Parrying4;
        else if (AC_Code == 9) return BladerSkill2Parrying5;
        else if (AC_Code == 10) return BladerAttackHit;
        else if (AC_Code == 11) return BladerSkill1Hit;
        else if (AC_Code == 12) return MoaiAttack1Hit;
        else if (AC_Code == 13) return MoaiAttack2Hit;
        else if (AC_Code == 14) return TaraAttack1;
        else if (AC_Code == 15) return TaraAttack2;
        else if (AC_Code == 16) return TaraAttackHit;
        else if (AC_Code == 17) return TaraSkill1On;
        else if (AC_Code == 18) return TaraSkill1Off;
        else if (AC_Code == 19) return TaraSkill1Shot;
        else if (AC_Code == 20) return TaraSkill1Reload;
        else if (AC_Code == 21) return TaraUltHit;
        else if (AC_Code == 22) return TaraSkill2Hit;
        else if (AC_Code == 23) return LuxAttackBound;
        else if (AC_Code == 24) return LuxAttackExplosion;
        else if (AC_Code == 25) return LuxUltHit;
        else if (AC_Code == 26) return Sunfire;
        else if (AC_Code == 27) return Zonya;
        else if (AC_Code == 28) return BaronOn;
        else if (AC_Code == 29) return BaronOff;
        else if (AC_Code == 30) return Portal;
        else if (AC_Code == 31) return HpKitOn;
        else if (AC_Code == 32) return HpKitOff;
        else if (AC_Code == 33) return GameEnd;
        else if (AC_Code == 34) return LuxSkill1Hit;
        else if (AC_Code == 35) return LinkerAttackHit;
        else if (AC_Code == 36) return LinkerSkill1Connect;
        else if (AC_Code == 37) return LinkerSkill1Joint;
        else if (AC_Code == 38) return LinkerSkill1Stun;

        
        return null;
    }

    public IEnumerator ReturnObject(float time,SoundScript obj)
    {
        yield return new WaitForSeconds(time+0.1f);
        obj.transform.position = new Vector3(100f, 100f, 0f);
        poolingObjectQueue.Enqueue(obj);
    }
}


