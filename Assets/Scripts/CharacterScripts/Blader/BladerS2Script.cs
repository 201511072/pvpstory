using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BladerS2Script : MonoBehaviourPunCallbacks
{
    public StatScript statScript;
    public BoxCollider2D boxCollider2D;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Projectile") && col.GetComponent<ProjectileScript>().isBlueTeam != statScript.BlueTeam && !col.GetComponent<ProjectileScript>().isParryinged)
        {
            col.GetComponent<PhotonView>().RPC("isParryingedRPC", RpcTarget.All);
            statScript.character_Base.UltGauge += 20f;
            statScript.PV.RPC("Skill2RPC", RpcTarget.All, (Vector2)col.transform.position, col.transform.position.x < statScript.transform.position.x, Random.Range(0, 4));
            //sound
            NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)transform.position, Random.Range(5, 10));
        }
    }
}
