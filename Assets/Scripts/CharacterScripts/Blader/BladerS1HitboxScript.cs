using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerS1HitboxScript : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;
    public BoxCollider2D myCharacterBoxCollider2D;
    public BladerScript bladerScript;
    public StatScript statScript;

    private void Start()
    {
        Physics2D.IgnoreCollision(boxCollider2D, myCharacterBoxCollider2D);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript EnemyStatScript = col.GetComponent<StatScript>();
            if (EnemyStatScript.BlueTeam != statScript.BlueTeam && !EnemyStatScript.isInvincible)
            {
                if (!EnemyStatScript.BladerS1Hit)
                {
                    StartCoroutine(EnemyStatScript.For1HitByBladerS1());
                    bladerScript.PV.RPC("ANSkill1HitEffectRPC", RpcTarget.All, (Vector2)EnemyStatScript.transform.position, bladerScript.s1ColDir);
                    if (EnemyStatScript.BladerS1Aura)
                    {
                        EnemyStatScript.BladerS1Aura = false;
                        EnemyStatScript.BladerS1AuraImage.enabled = false;
                        bladerScript.s1BtnScript.cooltimeScript.CooltimerCount = 0f;
                    }
                    EnemyStatScript.GetDamage(statScript.ATK * 0.9f + 307.5f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                    bladerScript.UltGauge += 14f;
                    NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)transform.position, 11);
                }
            }
        }

        if (col.CompareTag("Creature") && col.GetComponent<GetDamageScript>().BlueTeam != statScript.BlueTeam)
        {
            GetDamageScript EnemyGetDamageScript = col.GetComponent<GetDamageScript>();
            if (!EnemyGetDamageScript.BladerS1Hit)
            {
                StartCoroutine(EnemyGetDamageScript.For1HitByBladerS1());
                if (EnemyGetDamageScript.BladerS1Aura)
                {
                    EnemyGetDamageScript.BladerS1Aura = false;
                    EnemyGetDamageScript.BladerS1AuraImage.enabled = false;
                    bladerScript.s1BtnScript.cooltimeScript.CooltimerCount = 0f;
                }
                EnemyGetDamageScript.GetDamage(statScript.ATK * 0.9f + 307.5f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)transform.position, 11);
            }
        }
    }
}
