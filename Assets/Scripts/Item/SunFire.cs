using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFire : MonoBehaviour
{
    public StatScript statScript;
    public Coroutine tempCRT;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if (tempStatScript.BlueTeam != statScript.BlueTeam && tempStatScript.tempSunFireCRT == null)
            {
                tempStatScript.tempSunFireCRT = StartCoroutine(tempStatScript.SunFireCRT(statScript.myPlayerNumber));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if(tempStatScript.BlueTeam != statScript.BlueTeam && tempStatScript.tempSunFireCRT != null)
            {
                StopCoroutine(tempStatScript.tempSunFireCRT);
                tempStatScript.tempSunFireCRT = null;
            }
        }
    }
}
