using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class EndLobbyScript : MonoBehaviour
{
    public Image Blue1_Portrait;
    public Text Blue1_Nickname;
    public Text Blue1_Kill;
    public Text Blue1_Death;
    public Text Blue1_TotalDamage;
    public Text Blue1_RankPoint;
    public Text Blue1_RankPointChanged;

    public Image Blue2_Portrait;
    public Text Blue2_Nickname;
    public Text Blue2_Kill;
    public Text Blue2_Death;
    public Text Blue2_TotalDamage;
    public Text Blue2_RankPoint;
    public Text Blue2_RankPointChanged;

    public Image Red1_Portrait;
    public Text Red1_Nickname;
    public Text Red1_Kill;
    public Text Red1_Death;
    public Text Red1_TotalDamage;
    public Text Red1_RankPoint;
    public Text Red1_RankPointChanged;

    public Image Red2_Portrait;
    public Text Red2_Nickname;
    public Text Red2_Kill;
    public Text Red2_Death;
    public Text Red2_TotalDamage;
    public Text Red2_RankPoint;
    public Text Red2_RankPointChanged;

    public Sprite VictorySprite;
    public Sprite DefeatSprite;
    public Image VictoryDeafeatImg;

    public void Init()
    {
        Blue1_Portrait.sprite = NetworkManager.instance.CharacterSelectScript.Blue1.sprite;
        Blue1_Nickname.text = NetworkManager.instance.CharacterSelectScript.Blue1Text.text;

        Red1_Portrait.sprite = NetworkManager.instance.CharacterSelectScript.Red1.sprite;
        Red1_Nickname.text = NetworkManager.instance.CharacterSelectScript.Red1Text.text;

        Red2_Portrait.sprite = NetworkManager.instance.CharacterSelectScript.Red2.sprite;
        Red2_Nickname.text = NetworkManager.instance.CharacterSelectScript.Red2Text.text;

        Blue2_Portrait.sprite = NetworkManager.instance.CharacterSelectScript.Blue2.sprite;
        Blue2_Nickname.text = NetworkManager.instance.CharacterSelectScript.Blue2Text.text;


        if (NetworkManager.instance.isMyTeamWin)
        {
            VictoryDeafeatImg.sprite = VictorySprite;
            NetworkManager.instance.PV.RPC("EndLobbyInitRPC", RpcTarget.All, NetworkManager.instance.myPlayerNumber, NetworkManager.instance.myKillInThisGame, NetworkManager.instance.myDeathInThisGame,(int)NetworkManager.instance.myTotalDamageInThisGame, NetworkManager.instance.WinReward);
        }
        else
        {
            VictoryDeafeatImg.sprite = DefeatSprite;
            NetworkManager.instance.PV.RPC("EndLobbyInitRPC", RpcTarget.All, NetworkManager.instance.myPlayerNumber, NetworkManager.instance.myKillInThisGame, NetworkManager.instance.myDeathInThisGame, (int)NetworkManager.instance.myTotalDamageInThisGame, NetworkManager.instance.LoseReward);
        }
    }
}
