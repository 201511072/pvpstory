using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LinkerS1CircleScript : MonoBehaviourPunCallbacks
{
    public StatScript statScript;
    public bool isConnected;
    public LinkerS1LRScript linkerS1LRScript;
    public StatScript teamStatScript;
    public float connectedTime;
    public float lerpValue;
    public bool pullTeam;
    public PhotonView PV;
    public bool pullTeam1Time;
    public bool linkerClient;
    public CircleCollider2D circleCollider2D;

    public LinkerS1EyeEffectScript linkerS1EyeEffectScript;

    public override void OnEnable()
    {
        base.OnEnable();
        connectedTime = 0f;
        lerpValue = 0f;
        linkerS1LRScript.LerpValue = 0f;
        pullTeam = false;

        if (linkerClient)
        {
            StartCoroutine(ConnectCheckCRT());
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if(tempStatScript.BlueTeam == statScript.BlueTeam && !tempStatScript.isInvincible)
            {
                isConnected = true;
                statScript.PV.RPC("LinkS1ConnectedRPC", RpcTarget.All, true);
                teamStatScript.PV.RPC("LinkS1ConnectedRPC", RpcTarget.All, true);
                linkerS1LRScript.LerpValue = 0f;
                PV.RPC("LRScriptRPC", RpcTarget.All, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StatScript tempStatScript = col.GetComponent<StatScript>();
            if(tempStatScript.BlueTeam == statScript.BlueTeam)
            {
                isConnected = false;
                statScript.PV.RPC("LinkS1ConnectedRPC", RpcTarget.All, false);
                teamStatScript.PV.RPC("LinkS1ConnectedRPC", RpcTarget.All, false);
                PV.RPC("LRScriptRPC", RpcTarget.All, false);
                if (!teamStatScript.isInvincible)
                {
                    teamStatScript.PV.RPC("StunRPC", RpcTarget.All, 1.5f);
                }
                if (!statScript.isInvincible)
                {
                    statScript.PV.RPC("StunRPC", RpcTarget.All, 1.5f);
                }
                PV.RPC("ActiveRPC", RpcTarget.All, false);
            }
        }
    }

    public void Update()
    {
        if(connectedTime < 3f)
        {
            connectedTime += Time.deltaTime;
        }
        else if(pullTeam1Time)//linker 클라이언트에서만 실행가능하게 하기
        {
            pullTeam1Time = false;
            PV.RPC("pullTeamRPC", RpcTarget.All);
        }
        if (pullTeam)
        {
            if (lerpValue < 1f)
            {
                lerpValue += Time.deltaTime * 1.4f;
                teamStatScript.transform.position = new Vector2(Mathf.Lerp(teamStatScript.transform.position.x, transform.position.x, lerpValue),
                    Mathf.Lerp(teamStatScript.transform.position.y, transform.position.y, lerpValue));
            }
            if (lerpValue >= 1f)
            {
                pullTeam = false;
                if (linkerClient)
                {
                    NetworkManager.instance.PV.RPC("SoundRPC",RpcTarget.All, (Vector2)transform.position, 38);
                    teamStatScript.PV.RPC("StunRPC", RpcTarget.All, 1.5f);
                    statScript.PV.RPC("StunRPC", RpcTarget.All, 1.5f);
                    PV.RPC("ActiveRPC", RpcTarget.All, false);
                }
            }
        }
    }



    IEnumerator ConnectCheckCRT()
    {
        yield return new WaitForSeconds(0.2f);
        if (!isConnected)
        {
            statScript.PV.RPC("StunRPC", RpcTarget.All, 1.5f);
            PV.RPC("ActiveRPC", RpcTarget.All, false);
        }
    }

    [PunRPC]
    void pullTeamRPC()
    {
        pullTeam = true;
    }

    [PunRPC]
    void ActiveRPC(bool value)
    {
        if (!value)
        {
            circleCollider2D.enabled = false;
        }
        gameObject.SetActive(value);
    }

    [PunRPC]
    void LRScriptRPC(bool value)
    {
        linkerS1LRScript.gameObject.SetActive(value);
    }
}
