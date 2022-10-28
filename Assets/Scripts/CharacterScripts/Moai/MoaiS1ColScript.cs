using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoaiS1ColScript : MonoBehaviour
{
    public BoxCollider2D Skill1Col;
    public MoaiScript moaiScript;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            Skill1Col.enabled = false;
            moaiScript.isSkill1 = false;
            StartCoroutine(HitBoxOn());
            moaiScript.RB.velocity = Vector2.zero;
        }
    }

    public IEnumerator HitBoxOn()
    {
        moaiScript.PV.RPC("Skill1HitboxRPC", RpcTarget.All, transform.position.x, transform.position.y, moaiScript.statScript.BlueTeam);
        moaiScript.PV.RPC("ANSkill1EndRPC", RpcTarget.All);
        yield return new WaitForSeconds(0.3f);
        moaiScript.statScript.MoveLock = false;
        moaiScript.statScript.SkillLock = false;
        moaiScript.statScript.JumpLock = false;
        moaiScript.Land = false;
        moaiScript.ANLandDisabled = false;
        moaiScript.PV.RPC("ImuneStiffRPC", RpcTarget.All, false);
    }
}
