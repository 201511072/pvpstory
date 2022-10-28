using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraS2CheckTriggerScript : MonoBehaviour
{
    public TaraScript taraScript;

    public BoxCollider2D CheckTrigger;
    public bool S2Ready;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            S2Ready = false;
        }
    }

    public void FireWall()
    {
        taraScript.PV.RPC("Skill2RPC", RpcTarget.All, Physics2D.Raycast(transform.position, Vector2.down, 30f, 1 << LayerMask.NameToLayer("Ground")).point);
    }
}
