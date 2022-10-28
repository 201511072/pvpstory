using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CharacterSelectCountScript : MonoBehaviour
{
    public Text State;
    public Text CharacterSelectCount;
    public float count;
    public CharacterSelectScript characterSelectScript;
    public PhotonView PV;


    public bool ItemSelecting;

    public bool Blue1Ready;
    public bool Blue2Ready;
    public bool Red1Ready;
    public bool Red2Ready;

    public Image Blue1ReadyImage;
    public Image Blue2ReadyImage;
    public Image Red1ReadyImage;
    public Image Red2ReadyImage;

    public GameObject ItemSelectCompleteBtn;

    public bool ItemSelectComplete1Time;


    private void OnEnable()
    {
        ItemSelectCompleteBtn.SetActive(false);
        ItemSelectComplete1Time = false;
        Blue1Ready = false;
        Blue2Ready = false;
        Red1Ready = false;
        Red2Ready = false;
        Blue1ReadyImage.enabled = false;
        Blue2ReadyImage.enabled = false;
        Red1ReadyImage.enabled = false;
        Red2ReadyImage.enabled = false;
        ItemSelecting = false;
        count = 15.0f;
        characterSelectScript.Red1Glow.color = Color.white;
        characterSelectScript.Blue2Glow.color = Color.white;
        characterSelectScript.Blue1Glow.color = new Color(0.46484375f, 1f, 0f);
        State.text = "Character Select";
        State.color = new Color(61 / 255f, 152 / 255f, 1f, 1f);
    }

    public void SelectComplete()
    {
        if (characterSelectScript.SelectingPlayer == NetworkManager.instance.myPlayerNumber)
        {
            PV.RPC("SelectCompleteRPC", RpcTarget.All);
        }
    }

    public void ItemSelectComplete()
    {
        if (!ItemSelectComplete1Time)
        {
            PV.RPC("ItemSelectCompleteRPC", RpcTarget.All, NetworkManager.instance.myPlayerNumber);
        }
    }

    void Update()
    {
        count += -Time.deltaTime;
        CharacterSelectCount.text = Mathf.FloorToInt(count) + "";
        
        if(ItemSelecting)
        {
            if (NetworkManager.instance.PlayerCount == 4)
            {
                if (Blue1Ready && Red1Ready && Red2Ready && Blue2Ready)
                {
                    count = 0f;
                }
            }
            else if (NetworkManager.instance.PlayerCount == 2)
            {
                if (Blue1Ready && Red1Ready)
                {
                    count = 0f;
                }
            }
        }

        if (count <= 0.0f)
        {
            count = 15.0f;

            if (characterSelectScript.SelectingPlayer == NetworkManager.instance.myPlayerNumber)
            {
                AutoSelect();
                characterSelectScript.PV.RPC("ShowCharacterSelectedRPC", RpcTarget.All, characterSelectScript.SelectingPlayer, characterSelectScript.SelectedCharacter);
            }

            characterSelectScript.SelectingPlayer += 1;

            if (characterSelectScript.SelectingPlayer > PhotonNetwork.CurrentRoom.PlayerCount)
            {
                if (!ItemSelecting)
                {
                    ItemSelectCompleteBtn.SetActive(true);
                    ItemSelecting = true;
                    State.text = "Item Select";
                    State.color = new Color(174 / 255f, 61 / 255f, 1f, 1f);
                }
                else
                {
                    ItemSelectCompleteBtn.SetActive(false);
                    Blue1ReadyImage.enabled = false;
                    Blue2ReadyImage.enabled = false;
                    Red1ReadyImage.enabled = false;
                    Red2ReadyImage.enabled = false;
                    NetworkManager.instance.GameStart();
                }
            }
            else if (characterSelectScript.SelectingPlayer == 2)
            {
                characterSelectScript.Blue1Glow.color=Color.white;
                characterSelectScript.Red1Glow.color= new Color(0.46484375f,1f,0f);
            }
            else if (characterSelectScript.SelectingPlayer == 3)
            {
                characterSelectScript.Red1Glow.color = Color.white;
                characterSelectScript.Red2Glow.color = new Color(0.46484375f, 1f, 0f);
            }
            else if (characterSelectScript.SelectingPlayer == 4)
            {
                characterSelectScript.Red2Glow.color = Color.white;
                characterSelectScript.Blue2Glow.color = new Color(0.46484375f, 1f, 0f);
            }
        }
    }

    public void AutoSelect()
    {
        if (characterSelectScript.SelectedCharacter == 0)
        {
            if (!characterSelectScript.Character1Selected)
            {
                characterSelectScript.SelectedCharacter = 1;
            }
            else if (!characterSelectScript.Character2Selected)
            {
                characterSelectScript.SelectedCharacter = 2;
            }
            else if (!characterSelectScript.Character3Selected)
            {
                characterSelectScript.SelectedCharacter = 3;
            }
            else if (!characterSelectScript.Character4Selected)
            {
                characterSelectScript.SelectedCharacter = 4;
            }
            else if (!characterSelectScript.Character5Selected)
            {
                characterSelectScript.SelectedCharacter = 5;
            }
            else if (!characterSelectScript.Character6Selected)
            {
                characterSelectScript.SelectedCharacter = 6;
            }
        }
    }


        [PunRPC]
    void SelectCompleteRPC()
    {
        count = 0.0f;
    }

    [PunRPC]
    void ItemSelectCompleteRPC(int value)
    {
        if (value == 1)
        {
            Blue1Ready = true;
            Blue1ReadyImage.enabled = true;
        }
        else if (value == 2)
        {
            Red1Ready = true;
            Red1ReadyImage.enabled = true;
        }
        else if (value == 3)
        {
            Red2Ready = true;
            Red2ReadyImage.enabled = true;
        }
        else if (value == 4)
        {
            Blue2Ready = true;
            Blue2ReadyImage.enabled = true;
        }
    }
}
