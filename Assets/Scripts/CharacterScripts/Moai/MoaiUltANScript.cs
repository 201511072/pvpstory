using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MoaiUltANScript : MonoBehaviourPunCallbacks
{
    public MoaiScript moaiScript;

    public void ANWalk()
    {
        moaiScript.ANWalk();
    }

    public void JumpEnd()
    {
        moaiScript.JumpEnd();
    }
}
