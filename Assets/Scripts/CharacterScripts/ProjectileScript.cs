using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileScript : MonoBehaviourPunCallbacks
{
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public bool isBlueTeam;
    public PhotonView PV;
    public bool isParryinged;

    
}
