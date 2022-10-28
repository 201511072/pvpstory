using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraS1AttackRangeScript : MonoBehaviour
{
    public TaraS1Script taraS1Script;
    public CircleCollider2D RangeTrigger;

    public StatScript[] EnemyStatScriptArr = new StatScript[2];
    public float AttackTime;
    public float ReturnTime;
    public bool ANReload1Time;




    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && col.GetComponent<StatScript>().BlueTeam != taraS1Script.BlueTeam)
        {
            if (EnemyStatScriptArr[0] == null)
            {
                EnemyStatScriptArr[0] = col.GetComponent<StatScript>();
            }
            else
            {
                EnemyStatScriptArr[1] = col.GetComponent<StatScript>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") && col.GetComponent<StatScript>().BlueTeam != taraS1Script.BlueTeam)
        {
            StatScript tempEnemyStatScript = col.GetComponent<StatScript>();
            if (EnemyStatScriptArr[0] == tempEnemyStatScript)
            {
                EnemyStatScriptArr[0] = EnemyStatScriptArr[1];
                EnemyStatScriptArr[1] = null;
            }
            else if (EnemyStatScriptArr[1] == tempEnemyStatScript)
            {
                EnemyStatScriptArr[1] = null;
            }
        }
    }

    private void Update()
    {
        if (taraS1Script.isEnabled)
        {
            if (EnemyStatScriptArr[0] != null)
            {
                AttackTime += Time.deltaTime;

                if (AttackTime > 1.667f&&ANReload1Time)
                {
                    ANReload1Time = false;
                    taraS1Script.PV.RPC("ANReloadRPC", RpcTarget.All);
                }

                if (AttackTime > 2f)
                {
                    AttackTime = 0f;
                    ANReload1Time = true;
                    taraS1Script.Attack(((Vector2)EnemyStatScriptArr[0].transform.position - (Vector2)taraS1Script.transform.position).normalized);
                }
            }
            else
            {
                if (AttackTime < 2f)
                {
                    AttackTime += Time.deltaTime;
                }

                if (AttackTime > 1.667f && ANReload1Time)
                {
                    ANReload1Time = false;
                    taraS1Script.PV.RPC("ANReloadRPC", RpcTarget.All);
                }
            }

            ReturnTime += Time.deltaTime;
            if (ReturnTime > 8f)
            {
                taraS1Script.PV.RPC("ReturnRPC", RpcTarget.All);
            }
        }
    }


    public void EnemyStatScriptArrClear()
    {
        EnemyStatScriptArr[0] = null;
        EnemyStatScriptArr[1] = null;
    }
}
