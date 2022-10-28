using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MatchingLoadingScript : MonoBehaviour
{
    public Text MatchingPeople;

    private void OnEnable()
    {
        MatchingPeople.text = "Matching\n\n0/4";
    }

    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            MatchingPeople.text = "Matching\n\n" + PhotonNetwork.CurrentRoom.PlayerCount + "/4";
        }
    }
}
