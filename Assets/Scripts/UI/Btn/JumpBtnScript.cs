using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JumpBtnScript : MonoBehaviourPun
{
    protected GameObject MyCharacter;
    protected Character_Base character;

    public void Init()
    {
        NetworkManager.instance.OnExitRoom.AddListener(SelfDestroy);
        foreach (GameObject myCharacter in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (myCharacter.GetComponent<PhotonView>().IsMine)
            {
                MyCharacter = myCharacter;
                character = MyCharacter.GetComponent<Character_Base>();
            }
        }

    }

    public virtual void OnClickButton()
    {
        if (character != null)
        {
            character.Jump();
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
