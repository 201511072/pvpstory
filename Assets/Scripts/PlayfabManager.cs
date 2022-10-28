using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;

public class PlayfabManager : MonoBehaviour
{
    public Text GoogleLoginText;
    public GameObject StartScreenManager;
    public GameObject SelectModeScreen;
    public GameObject SetNickNameScreen;
    public NetworkManager networkManager;


    public Text log;

    public string myPlayFabID;
    public string myDisplayname;
    public Text newDisplayname;

    public Text LogText;
    public Text tempText;
    public Text temp2Text;

    


    void Awake()
    {
        //PlayGamesPlatform.DebugLogEnabled = true;
        //PlayGamesPlatform.Activate();

        //GoogleLogin();

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
.AddOauthScope("profile")
.RequestServerAuthCode(false)
.Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void GoogleLogin()
    {
        NetworkManager.instance.LoadingCirclePanel.SetActive(true);
        Social.localUser.Authenticate((success) =>
        {
            if (success) { LogText.text = "구글 로그인 성공"; PlayFabLogin(); }
            else NetworkManager.instance.LoadingCirclePanel.SetActive(false);
        });
    }

    public void GoogleLogout()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        LogText.text = "구글 로그아웃";
    }

    public void PlayFabLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = Social.localUser.id + "@rand.com", Password = Social.localUser.id };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => PlayFabLoginSuccess(), (error) => PlayFabRegister());
    }

    public void PlayFabLoginSuccess()
    {
        LogText.text = "플레이팹 로그인 성공\n" + Social.localUser.userName;
        var request2 = new GetAccountInfoRequest { PlayFabId = myPlayFabID };
        PlayFabClientAPI.GetAccountInfo(request2, GetAccountSuccess, GetAccountFailure);
    }

    public void PlayFabRegister()
    {
        var request = new RegisterPlayFabUserRequest { Email = Social.localUser.id + "@rand.com", Password = Social.localUser.id, Username = Social.localUser.userName };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { LogText.text = "플레이팹 회원가입 성공"; PlayFabLogin(); }, (error) => LogText.text = "플레이팹 회원가입 실패");        
    }

    private void GetAccountSuccess(GetAccountInfoResult result)
    {
        log.text = "Accout를 정상적으로 받아옴";

        myDisplayname = result.AccountInfo.TitleInfo.DisplayName;
        if (myDisplayname == null)
        {
            log.text = "닉네임null임";
            SetNickNameScreen.SetActive(true);
        }
        else
        {
            log.text = "닉네임있음";
            networkManager.SetNickName(myDisplayname);
            networkManager.Connect();
            SelectModeScreen.SetActive(true);
            StartScreenManager.SetActive(false);
        }
        NetworkManager.instance.LoadingCirclePanel.SetActive(false);
    }

    private void GetAccountFailure(PlayFabError error)
    {
        log.text = "닉네임을 받아오지 못함";
        NetworkManager.instance.LoadingCirclePanel.SetActive(false);
    }

    public void SelectNickname()
    {
        UpdateDisplayName(newDisplayname.text);
    }

    public void UpdateDisplayName(string newPlayerDisplayName)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = newPlayerDisplayName },
            result => { networkManager.Connect(); StartScreenManager.SetActive(false); SelectModeScreen.SetActive(true); }, error => { });
        //랭킹점수 초기화도 함
        SetStat();

        networkManager.SetNickName(newPlayerDisplayName);
        SelectModeScreen.SetActive(true);
        StartScreenManager.SetActive(false);
        SetNickNameScreen.SetActive(false);
    }



    public void OnSignInButtonClicked()
    {
        GoogleLoginText.text = "Start";
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                GoogleLoginText.text = "Google Signed In";
                var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                GoogleLoginText.text = "Server Auth Code: " + serverAuthCode;

                PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
                {
                    TitleId = PlayFabSettings.TitleId,
                    ServerAuthCode = serverAuthCode,
                    CreateAccount = true
                }, (result) =>
                {
                    GoogleLoginText.text = "Signed In as " + result.PlayFabId;
                    myPlayFabID = result.PlayFabId;
                    log.text = myPlayFabID;
                    var request = new GetAccountInfoRequest { PlayFabId = myPlayFabID };
                    PlayFabClientAPI.GetAccountInfo(request, GetAccountSuccess, GetAccountFailure);
                }, OnPlayFabError);

            }
            else
            {
                GoogleLoginText.text = "Google Failed to Authorize your login";
                Debug.Log("Google Failed to Authorize your login");
            }

        });

    }


    private void OnPlayFabError(PlayFabError obj)
    {
        GoogleLoginText.text = "Playfab error";
    }



    public void SetStat()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate {StatisticName = "RankPoint", Value = NetworkManager.instance.RankPoint}
            }
        },
        (result) => { temp2Text.text = "값 저장됨"; },
        (error) => { temp2Text.text = "값 저장실패"; });
    }

    
    public void GetStat()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            (result) =>
            {
                tempText.text = "";
                foreach (var eachStat in result.Statistics)
                {
                    switch (eachStat.StatisticName)
                    {
                        case "RankPoint": NetworkManager.instance.RankPoint=eachStat.Value; tempText.text = "값 받아옴"; break;
                    }
                }
                NetworkManager.instance.RankPointText2.text = Convert.ToString(NetworkManager.instance.RankPoint);
            },
            (error) => { tempText.text = "값 불러오기 실패"; });
    }

    public string[] RankList_Name = new string[10];
    public int[] RankList_Point = new int[10];
    public Text NickNameText_1;
    public Text NickNameText_2;
    public Text NickNameText_3;
    public Text NickNameText_4;
    public Text NickNameText_5;
    public Text NickNameText_6;
    public Text NickNameText_7;
    public Text NickNameText_8;
    public Text NickNameText_9;
    public Text NickNameText_10;
    public Text RankPointText_1;
    public Text RankPointText_2;
    public Text RankPointText_3;
    public Text RankPointText_4;
    public Text RankPointText_5;
    public Text RankPointText_6;
    public Text RankPointText_7;
    public Text RankPointText_8;
    public Text RankPointText_9;
    public Text RankPointText_10;

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "RankPoint", MaxResultsCount = 10, ProfileConstraints = new PlayerProfileViewConstraints() { ShowDisplayName = true } };
        PlayFabClientAPI.GetLeaderboard(request, (result) =>
        {
            for (int i = 0; i < result.Leaderboard.Count; i++)
            {
                var curBoard = result.Leaderboard[i];
                RankList_Name[i] = curBoard.DisplayName;
                RankList_Point[i] = curBoard.StatValue;
            }

            if (result.Leaderboard.Count > 0)
            {
                NickNameText_1.text = RankList_Name[0]; RankPointText_1.text = RankList_Point[0]+"";
            }
            if (result.Leaderboard.Count > 1)
            {
                NickNameText_2.text = RankList_Name[1]; RankPointText_2.text = RankList_Point[1] + "";
            }
            if (result.Leaderboard.Count > 2)
            {
                NickNameText_3.text = RankList_Name[2]; RankPointText_3.text = RankList_Point[2] + "";
            }
            if (result.Leaderboard.Count > 3)
            {
                NickNameText_4.text = RankList_Name[3]; RankPointText_4.text = RankList_Point[3] + "";
            }
            if (result.Leaderboard.Count > 4)
            {
                NickNameText_5.text = RankList_Name[4]; RankPointText_5.text = RankList_Point[4] + "";
            }
            if (result.Leaderboard.Count > 5)
            {
                NickNameText_6.text = RankList_Name[5]; RankPointText_6.text = RankList_Point[5] + "";
            }
            if (result.Leaderboard.Count > 6)
            {
                NickNameText_7.text = RankList_Name[6]; RankPointText_7.text = RankList_Point[6] + "";
            }
            if (result.Leaderboard.Count > 7)
            {
                NickNameText_8.text = RankList_Name[7]; RankPointText_8.text = RankList_Point[7] + "";
            }
            if (result.Leaderboard.Count > 8)
            {
                NickNameText_9.text = RankList_Name[8]; RankPointText_9.text = RankList_Point[8] + "";
            }
            if (result.Leaderboard.Count > 9)
            {
                NickNameText_10.text = RankList_Name[9]; RankPointText_10.text = RankList_Point[9] + "";
            }
        },
        (error) => print("리더보드 불러오기 실패"));
    }
}