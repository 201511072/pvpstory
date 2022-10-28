using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkerS1LRScript : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public LineRenderer LR;
    public float LerpValue;

    private void Update()
    {
        LR.SetPosition(0, Player1.transform.position+new Vector3(0f,0f,-1f));
        if (LerpValue < 1f)
        {
            LerpValue += Time.deltaTime * 4f;
            LR.SetPosition(1, new Vector3(Mathf.Lerp(Player1.transform.position.x, Player2.transform.position.x, LerpValue),
            Mathf.Lerp(Player1.transform.position.y, Player2.transform.position.y, LerpValue), -1f));
        }
        else
        {
            LR.SetPosition(1, Player2.transform.position + new Vector3(0f, 0f, -1f));
        }
    }
}
