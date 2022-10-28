using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class MoaiS2BtnScript : S2BtnScript, IPointerUpHandler, IPointerDownHandler
{
    public MoaiScript MoaiScript;
    bool skill2Start;
    float MaxDuration;
    float MinDuration;
    bool doOnPointerUp;

    public override void Init()
    {
        NetworkManager.instance.OnExitRoom.AddListener(SelfDestroy);
        NetworkManager.instance.S2Cool = cooltimeScript;
        foreach (GameObject FindMyPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (FindMyPlayer.GetComponent<PhotonView>().IsMine)
            {
                Player = FindMyPlayer;
                MoaiScript = Player.GetComponent<MoaiScript>();
                statScript = Player.GetComponent<StatScript>();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (MoaiScript != null)
        {
            if (!cooltimeScript.GetCooltime && !statScript.SkillLock && !statScript.StunSkillLock && MoaiScript.isGround && !doOnPointerUp)
            {
                skill2Start = true;
                statScript.SkillLock = true;
                statScript.JumpLock = true;
                statScript.MoveLock = true;
                statScript.RB.constraints = RigidbodyConstraints2D.FreezeAll;
                MoaiScript.PV.RPC("ANSkill2RPC", RpcTarget.All);
                MoaiScript.PV.RPC("DamageReductionRPC", RpcTarget.All, 0.9f);
                MoaiScript.PV.RPC("ANS2HitboxStartRPC", RpcTarget.All);
                MoaiScript.S2Hitbox.enabled = true;
                statScript.PV.RPC("ImuneStiffRPC", RpcTarget.All, true);
                statScript.PV.RPC("ImuneStunRPC", RpcTarget.All, true);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (skill2Start)
        {
            if (MinDuration < 0.5f)
            {
                doOnPointerUp = true;
            }
            else
            {
                skill2Start = false;
                doOnPointerUp = false;
                MaxDuration = 0f;
                MinDuration = 0f;
                MoaiScript.PV.RPC("ANSkill2EndRPC", RpcTarget.All);
                MoaiScript.PV.RPC("DamageReductionRPC", RpcTarget.All, -0.9f);
                MoaiScript.PV.RPC("ANS2HitboxEndRPC", RpcTarget.All);
                MoaiScript.S2Hitbox.enabled = false;
                StartCoroutine(MoaiScript.S2EndHitboxScript.S2EndHitboxCRT());
                statScript.PV.RPC("ImuneStiffRPC", RpcTarget.All, false);
                statScript.PV.RPC("ImuneStunRPC", RpcTarget.All, false);
                MoaiScript.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
                statScript.SkillLock = false;
                statScript.JumpLock = false;
                statScript.MoveLock = false;
                cooltimeScript.GetCooltime = true;
                cooltimeScript.Cooltimer = MoaiScript.Skill2Delay;
                cooltimeScript.CooltimerCount = MoaiScript.Skill2Delay;
            }
        }
    }



    private void Update()
    {
        if (skill2Start)
        {
            if (MaxDuration < 4f)
            {
                MaxDuration += Time.deltaTime;
            }
            else
            {
                OnPointerUp(null);
            }

            if (MinDuration < 0.5f)
            {
                MinDuration += Time.deltaTime;
            }
            else if (doOnPointerUp)
            {
                OnPointerUp(null);
            }
        }
    }
}
