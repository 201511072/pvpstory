using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MatchingPeopleScript : MonoBehaviour
{
    public Text MatchingPeople;

    private void OnEnable()
    {
        MatchingPeople.text = "매칭중입니다\n\n0/4";
    }

    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            MatchingPeople.text = "매칭중입니다\n\n" + PhotonNetwork.CurrentRoom.PlayerCount + "/4";
        }
    }
}
