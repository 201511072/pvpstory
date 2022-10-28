using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CharacterSelectScript : MonoBehaviourPun
{
    public CharacterInfoScript characterInfoScript;
    public int SelectingPlayer;
    public Image MySlotImage;
    public Sprite EmptySprite;
    public Sprite Character1;
    public Sprite Character2;
    public Sprite Character3;
    public Sprite Character4;
    public Sprite Character5;
    public Sprite Character6;

    public Image Blue1;
    public Image Blue2;
    public Image Red1;
    public Image Red2;

    public Image Blue1Glow;
    public Image Blue2Glow;
    public Image Red1Glow;
    public Image Red2Glow;

    public int SelectedCharacter;
    public PhotonView PV;

    public Text Blue1Text;
    public Text Blue2Text;
    public Text Red1Text;
    public Text Red2Text;

    public Text Blue1RP;
    public Text Blue2RP;
    public Text Red1RP;
    public Text Red2RP;


    public bool Character1Selected;
    public bool Character2Selected;
    public bool Character3Selected;
    public bool Character4Selected;
    public bool Character5Selected;
    public bool Character6Selected;

    public Image Btn1Image;
    public Image Btn2Image;
    public Image Btn3Image;
    public Image Btn4Image;
    public Image Btn5Image;
    public Image Btn6Image;

    public Image Btn1Tool_Image;
    public Image Btn2Tool_Image;
    public Image Btn3Tool_Image;
    public Image Btn4Tool_Image;
    public Image Btn5Tool_Image;
    public Image Btn6Tool_Image;


    private void OnEnable()
    {
        characterInfoScript.Reset_All();
        characterInfoScript.gameObject.SetActive(false);

        SelectedCharacter = 0;
        NetworkManager.instance.soundManagerScript.AS_Setting(Sound_Background.CharacterSelect);

        Blue1.sprite = EmptySprite;
        Blue2.sprite = EmptySprite;
        Red1.sprite = EmptySprite;
        Red2.sprite = EmptySprite;
        Blue1.color = new Color(1f, 1f, 1f, 0f);
        Blue2.color = new Color(1f, 1f, 1f, 0f);
        Red1.color = new Color(1f, 1f, 1f, 0f);
        Red2.color = new Color(1f, 1f, 1f, 0f);

        Btn1Image.color = Color.white;
        Btn2Image.color = Color.white;
        Btn3Image.color = Color.white;
        Btn4Image.color = Color.white;
        Btn5Image.color = Color.white;
        Btn6Image.color = Color.white;
        Btn1Tool_Image.color = Color.white;
        Btn2Tool_Image.color = Color.white;
        Btn3Tool_Image.color = Color.white;
        Btn4Tool_Image.color = Color.white;
        Btn5Tool_Image.color = Color.white;
        Btn6Tool_Image.color = Color.white;

        SelectingPlayer = 1;
        Blue1Text.color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 255 / 255f);
        Blue2Text.color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 255 / 255f);
        Red1Text.color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 255 / 255f);
        Red2Text.color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 255 / 255f);

        if (NetworkManager.instance.myPlayerNumber == 1)
        {
            NetworkManager.instance.PV.RPC("NicknameTextRPC", RpcTarget.All, NetworkManager.instance.myNickName, 1, NetworkManager.instance.RankPoint);
            Blue1Text.color = new Color(37 / 255f, 255 / 255f, 0 / 255f, 255 / 255f);
        }
        else if (NetworkManager.instance.myPlayerNumber == 2)
        {
            NetworkManager.instance.PV.RPC("NicknameTextRPC", RpcTarget.All, NetworkManager.instance.myNickName, 2, NetworkManager.instance.RankPoint);
            Red1Text.color = new Color(37 / 255f, 255 / 255f, 0 / 255f, 255 / 255f);
        }
        else if (NetworkManager.instance.myPlayerNumber == 3)
        {
            NetworkManager.instance.PV.RPC("NicknameTextRPC", RpcTarget.All, NetworkManager.instance.myNickName, 3, NetworkManager.instance.RankPoint);
            Red2Text.color = new Color(37 / 255f, 255 / 255f, 0 / 255f, 255 / 255f);
        }
        else if (NetworkManager.instance.myPlayerNumber == 4)
        {
            NetworkManager.instance.PV.RPC("NicknameTextRPC", RpcTarget.All, NetworkManager.instance.myNickName, 4, NetworkManager.instance.RankPoint);
            Blue2Text.color = new Color(37 / 255f, 255 / 255f, 0 / 255f, 255 / 255f);
        }

        if (NetworkManager.instance.myPlayerNumber == 1)
        {
            MySlotImage = Blue1;
        }
        else if (NetworkManager.instance.myPlayerNumber == 2)
        {
            MySlotImage = Red1;
        }
        else if (NetworkManager.instance.myPlayerNumber == 3)
        {
            MySlotImage = Red2;
        }
        else if (NetworkManager.instance.myPlayerNumber == 4)
        {
            MySlotImage = Blue2;
        }

        Character1Selected = false;
        Character2Selected = false;
        Character3Selected = false;
        Character4Selected = false;
        Character5Selected = false;
        Character6Selected = false;
    }



    public void Character1SelectButton()
    {
        if (SelectingPlayer == NetworkManager.instance.myPlayerNumber && !Character1Selected)
        {
            CharacterSelect(Character1);
            SelectedCharacter = 1;
        }
        characterInfoScript.Set(1);
        characterInfoScript.gameObject.SetActive(true);
    }

    public void Character2SelectButton()
    {
        if (SelectingPlayer == NetworkManager.instance.myPlayerNumber && !Character2Selected)
        {
            CharacterSelect(Character2);
            SelectedCharacter = 2;
        }
        characterInfoScript.Set(2);
        characterInfoScript.gameObject.SetActive(true);
    }

    public void Character3SelectButton()
    {
        if (SelectingPlayer == NetworkManager.instance.myPlayerNumber && !Character3Selected)
        {
            CharacterSelect(Character3);
            SelectedCharacter = 3;
        }
        characterInfoScript.Set(3);
        characterInfoScript.gameObject.SetActive(true);
    }

    public void Character4SelectButton()
    {
        if (SelectingPlayer == NetworkManager.instance.myPlayerNumber && !Character4Selected)
        {
            CharacterSelect(Character4);
            SelectedCharacter = 4;
        }
        characterInfoScript.Set(4);
        characterInfoScript.gameObject.SetActive(true);
    }

    public void Character5SelectButton()
    {
        if (SelectingPlayer == NetworkManager.instance.myPlayerNumber && !Character5Selected)
        {
            CharacterSelect(Character5);
            SelectedCharacter = 5;
        }
        characterInfoScript.Set(5);
        characterInfoScript.gameObject.SetActive(true);
    }

    public void Character6SelectButton()
    {
        if (SelectingPlayer == NetworkManager.instance.myPlayerNumber && !Character6Selected)
        {
            CharacterSelect(Character6);
            SelectedCharacter = 6;
        }
        characterInfoScript.Set(6);
        characterInfoScript.gameObject.SetActive(true);
    }

    public void CharacterSelect(Sprite CharacterSprite)
    {
        MySlotImage.sprite = CharacterSprite;
        MySlotImage.color = new Color(1f, 1f, 1f, 1f);
    }



    [PunRPC]
    void ShowCharacterSelectedRPC(int SelectingPlayer, int SelectedCharacter)
    {
        if (SelectingPlayer == 1)
        {
            if (SelectedCharacter == 1)
            {
                Blue1.sprite = Character1;
                Character1Selected = true;
                Btn1Image.color = new Color(0.32f, 0.32f, 0.32f);
                Btn1Tool_Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 2)
            {
                Blue1.sprite = Character2;
                Character2Selected = true;
                Btn2Image.color = new Color(0.32f, 0.32f, 0.32f);
                Btn2Tool_Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 3)
            {
                Blue1.sprite = Character3;
                Character3Selected = true;
                Btn3Image.color = new Color(0.32f, 0.32f, 0.32f);
                Btn3Tool_Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 4)
            {
                Blue1.sprite = Character4;
                Character4Selected = true;
                Btn4Image.color = new Color(0.32f, 0.32f, 0.32f);
                Btn4Tool_Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 5)
            {
                Blue1.sprite = Character5;
                Character5Selected = true;
                Btn5Image.color = new Color(0.32f, 0.32f, 0.32f);
                Btn5Tool_Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 6)
            {
                Blue1.sprite = Character6;
                Character6Selected = true;
                Btn6Image.color = new Color(0.32f, 0.32f, 0.32f);
                Btn6Tool_Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            Blue1.color = new Color(1f, 1f, 1f, 1f);
        }
        else if (SelectingPlayer == 2)
        {
            if (SelectedCharacter == 1)
            {
                Red1.sprite = Character1;
                Character1Selected = true;
                Btn1Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 2)
            {
                Red1.sprite = Character2;
                Character2Selected = true;
                Btn2Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 3)
            {
                Red1.sprite = Character3;
                Character3Selected = true;
                Btn3Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 4)
            {
                Red1.sprite = Character4;
                Character4Selected = true;
                Btn4Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 5)
            {
                Red1.sprite = Character5;
                Character5Selected = true;
                Btn5Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 6)
            {
                Red1.sprite = Character6;
                Character6Selected = true;
                Btn6Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            Red1.color = new Color(1f, 1f, 1f, 1f);
        }
        else if (SelectingPlayer == 3)
        {
            if (SelectedCharacter == 1)
            {
                Red2.sprite = Character1;
                Character1Selected = true;
                Btn1Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 2)
            {
                Red2.sprite = Character2;
                Character2Selected = true;
                Btn2Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 3)
            {
                Red2.sprite = Character3;
                Character3Selected = true;
                Btn3Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 4)
            {
                Red2.sprite = Character4;
                Character4Selected = true;
                Btn4Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 5)
            {
                Red2.sprite = Character5;
                Character5Selected = true;
                Btn5Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 6)
            {
                Red2.sprite = Character6;
                Character6Selected = true;
                Btn6Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            Red2.color = new Color(1f, 1f, 1f, 1f);
        }
        else if (SelectingPlayer == 4)
        {
            if (SelectedCharacter == 1)
            {
                Blue2.sprite = Character1;
                Character1Selected = true;
                Btn1Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 2)
            {
                Blue2.sprite = Character2;
                Character2Selected = true;
                Btn2Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 3)
            {
                Blue2.sprite = Character3;
                Character3Selected = true;
                Btn3Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 4)
            {
                Blue2.sprite = Character4;
                Character4Selected = true;
                Btn4Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 5)
            {
                Blue2.sprite = Character5;
                Character5Selected = true;
                Btn5Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            else if (SelectedCharacter == 6)
            {
                Blue2.sprite = Character6;
                Character6Selected = true;
                Btn6Image.color = new Color(0.32f, 0.32f, 0.32f);
            }
            Blue2.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
