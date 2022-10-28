using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArrowScript : ProjectileScript
{
    public BoxCollider2D BoxCollider2D;
    public ArcherS1CooltimeScript archerS1CooltimeScript;
    public StatScript statScript;
    public bool isEnabled;
    public ArcherAttackObjectPool archerAttackObjectPool;
    public ArcherScript archerScript;
    public float time;
    public bool UltArrow;
    public float PlusATK;
    public bool Hit1time;

    public Animator AN;

    public Vector2 HitEffectPosition;



    public void Init()
    {
        archerScript = GameObject.Find("Archer(Clone)").GetComponent<ArcherScript>();
        archerAttackObjectPool = archerScript.archerAttackObjectPool;
        statScript = archerAttackObjectPool.statScript;
        isBlueTeam = archerAttackObjectPool.statScript.BlueTeam;
        transform.SetParent(archerAttackObjectPool.transform);
        if (PV.IsMine)
        {
            archerS1CooltimeScript = archerScript.archerS1Btn.archerS1CooltimeScript;
        }
        archerAttackObjectPool.poolingObjectQueue.Enqueue(this);
    }

    [PunRPC]
    void InitRPC()
    {
        Init();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (PV.IsMine)
        {
            if (Hit1time)
            {
                if (col.CompareTag("Ground"))
                {
                    Hit1time = false;
                    PV.RPC("HitEffectRPC", RpcTarget.All, (Vector2)transform.position + HitEffectPosition, RB.velocity.x < 0f, PlusATK);
                    PV.RPC("ReturnRPC", RpcTarget.All);

                    //sound
                    if (PlusATK == 342f) NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)transform.position, 3);
                    else NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)transform.position, 4);
                }

                if (col.CompareTag("Player"))
                {
                    StatScript tempStatScript = col.GetComponent<StatScript>();
                    if (tempStatScript.BlueTeam != isBlueTeam && !tempStatScript.DontHitByProjectile && !tempStatScript.isInvincible)
                    {
                        Hit1time = false;
                        PV.RPC("HitEffectRPC", RpcTarget.All, ((Vector2)tempStatScript.transform.position + (Vector2)transform.position) * 0.5f, RB.velocity.x < 0f, PlusATK);
                        tempStatScript.GetDamage(statScript.ATK * 0.8f + PlusATK, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                        if (!isParryinged)
                        {
                            if (!UltArrow)
                            {
                                archerScript.UltGauge += 7f;
                            }
                            ArcherS1Count();
                        }
                        else
                        {
                            PV.RPC("ReturnRPC", RpcTarget.All);
                        }

                        //sound
                        if (PlusATK == 342f) NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)transform.position, 1);
                        else NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)transform.position, 2);
                    }
                }

                if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != isBlueTeam)
                {
                    Hit1time = false;
                    col.GetComponent<GetDamageScript>().GetDamage(statScript.ATK * 0.8f + PlusATK, 0f, 0f, 0f, 0f, 0f, statScript.myPlayerNumber);
                    ArcherS1Count();

                    //sound
                    if (PlusATK == 342f) NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)transform.position, 1);
                    else NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)transform.position, 2);
                }
            }
        }
    }

    void ArcherS1Count()
    {
        if (archerS1CooltimeScript.Skill1Count == 2)
        {
            archerS1CooltimeScript.Skill1Count++;
            archerS1CooltimeScript.S1CountImage3.enabled = true;
            archerS1CooltimeScript.CooltimerCount = archerS1CooltimeScript.Cooltimer;
            archerS1CooltimeScript.CooltimeImage.fillAmount = 0f;
        }
        else if (archerS1CooltimeScript.Skill1Count == 1)
        {
            archerS1CooltimeScript.Skill1Count++;
            archerS1CooltimeScript.S1CountImage2.enabled = true;
        }
        else if (archerS1CooltimeScript.Skill1Count == 0)
        {
            archerS1CooltimeScript.Skill1Count++;
            archerS1CooltimeScript.S1CountImage1.enabled = true;
        }

        PV.RPC("ReturnRPC", RpcTarget.All);
    }

    private void Update()
    {
        if (isEnabled)
        {
            time += Time.deltaTime;
            if (time > 15f)
            {
                archerAttackObjectPool.ReturnObject(this);
            }
        }
    }






    [PunRPC]
    public void ReturnRPC()
    {
        archerAttackObjectPool.ReturnObject(this);
    }

    [PunRPC]
    void isParryingedRPC()
    {
        isBlueTeam = !isBlueTeam;
        isParryinged = true;
        RB.velocity *= -1f;
        transform.Rotate(0f, 0f, 180f);
    }

    [PunRPC]
    public void HitEffectRPC(Vector2 effectPosition, bool flipX, float arrowPlusATK)
    {
        archerScript.archerAttackHitEffectObjectPool.GetObject(effectPosition, flipX, arrowPlusATK);
    }
}
