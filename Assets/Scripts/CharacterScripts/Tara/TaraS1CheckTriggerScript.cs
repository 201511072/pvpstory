using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaraS1CheckTriggerScript : MonoBehaviour
{
    public BoxCollider2D CheckTrigger;
    public bool S1Ready;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            S1Ready = false;
        }
    }
}
