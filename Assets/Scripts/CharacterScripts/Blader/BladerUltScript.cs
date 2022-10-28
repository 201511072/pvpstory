using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladerUltScript : MonoBehaviour
{
    public CircleCollider2D circleCollider2D;
    public StatScript MyStatScript;
    public StatScript EnemyStatScript;
    public BladerScript bladerScript;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if(tempStatScript.BlueTeam != MyStatScript.BlueTeam && !tempStatScript.isInvincible)
            {
                if (EnemyStatScript != null)
                {
                    if (EnemyStatScript.SumHPShield() < tempStatScript.SumHPShield())
                    {
                        EnemyStatScript = tempStatScript;
                        bladerScript.EnemyColY = tempStatScript.MyBoxCol.size.y / 2f;
                    }
                }
                else
                {
                    EnemyStatScript = tempStatScript;
                    bladerScript.EnemyColY = tempStatScript.MyBoxCol.size.y / 2f;
                }
            }
        }
    }
}
