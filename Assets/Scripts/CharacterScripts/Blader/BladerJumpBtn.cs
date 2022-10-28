using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BladerJumpBtn : JumpBtnScript
{
    public bool onClickBtnCheck = true;

    private void Start()
    {
        NetworkManager.instance.BladerJumpBtnScript = this;
    }

    public override void OnClickButton()
    {
        if (onClickBtnCheck)
        {
            StartCoroutine(Wait());
            if (character != null)
            {
                character.Jump();
            }
        }
    }

    public IEnumerator Wait()
    {
        onClickBtnCheck = false;
        yield return new WaitForSeconds(0.05f);
        onClickBtnCheck = true;
    }
}