using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.Events;
using Cinemachine;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //Networkmanager 싱글톤. Awake에서 instance에 넣어줌
    public static NetworkManager instance;

    public SoundManagerScript soundManagerScript;
    public SoundObjectPool soundObjectPool;
    [PunRPC]
    public void SoundRPC(Vector2 position, int AC_Code)
    {
        soundObjectPool.GetObject(position, AC_Code);
    }

    public Camera MainCamera;
    public AudioListener AL;
    public string myNickName = "test";
    public Text NickNameText;
    public InputField RoomNumberInput;
    public GameObject FriendModePanel;
    public GameObject FriendModeErrorMessage;
    public GameObject DisconnectPanel;
    public Text RankPointText2;
    public GameObject floatingJoystick;
    public MatchingLoadingScript matchingLoadingScript;
    public GameObject CharacterSelectCanvas;
    public GameObject RespawnPanel;
    public GameObject LoaingPanel;
    public CountPanelScript countPanelScript;
    public GameObject ResultPanel;
    public GameObject VictoryImage;
    public GameObject DefeatImage;
    public PlayfabManager playfabManager;
    public GameObject LoadingCirclePanel;
    public EndLobbyScript EndLobbyScript;

    public KillLogManager killLogManager;

    public bool WaitingPeople = false;
    public RoomOptions RO = new RoomOptions();

    public GameObject ControlUI;
    public GameObject Inventory;

    public CharacterSelectScript CharacterSelectScript;
    public ItemManager ItemManager;

    public Collider2D Blue1Col;
    public Collider2D Blue2Col;
    public Collider2D Red1Col;
    public Collider2D Red2Col;

    [Header("Btn")]
    public Transform ControlUItransform;

    public GameObject AttackBtn;
    public GameObject JumpBtn;
    public GameObject S1Btn;
    public GameObject S2Btn;
    public GameObject UltBtn;

    [Header("SpecialBtn")]
    public GameObject ArcherJumpBtn;
    public GameObject ArcherAttackBtn;
    public GameObject ArcherS1Btn;
    public GameObject ArcherS2Btn;
    public GameObject ArcherUltBtn;
    public GameObject BladerJumpBtn;
    public BladerJumpBtn BladerJumpBtnScript;//bool값 버그 생기는거 고쳐주려고 넣음. 인스펙터창에서 비워둘것
    public GameObject BladerAttackBtn;
    public GameObject BladerS1Btn;
    public GameObject BladerS2Btn;
    public GameObject BladerUltBtn;
    public GameObject MoaiJumpBtn;
    public GameObject MoaiAttackBtn;
    public GameObject MoaiS1Btn;
    public GameObject MoaiS2Btn;
    public GameObject MoaiUltBtn;
    public GameObject LuxJumpBtn;
    public GameObject LuxAttackBtn;
    public GameObject LuxS1Btn;
    public GameObject LuxS2Btn;
    public GameObject LuxUltBtn;
    public GameObject LinkerJumpBtn;
    public GameObject LinkerAttackBtn;
    public GameObject LinkerS1Btn;
    public GameObject LinkerS2Btn;
    public GameObject LinkerUltBtn;
    public GameObject TaraJumpBtn;
    public GameObject TaraAttackBtn;
    public GameObject TaraS1Btn;
    public GameObject TaraS2Btn;
    public GameObject TaraUltBtn;


    [Header("ItemBtn")]
    public GameObject ZonyaBtn;

    [Header("BtnPositionSetting")]
    public RectTransform AttackBtnRT;
    public RectTransform JumpBtnRT;
    public RectTransform S1BtnRT;
    public RectTransform S2BtnRT;
    public RectTransform UltBtnRT;

    public RectTransform ArcherAttackBtnRT;
    public RectTransform ArcherJumpBtnRT;
    public RectTransform ArcherS1BtnRT;
    public RectTransform ArcherS2BtnRT;
    public RectTransform ArcherUltBtnRT;
    public RectTransform BladerAttackBtnRT;
    public RectTransform BladerJumpBtnRT;
    public RectTransform BladerS1BtnRT;
    public RectTransform BladerS2BtnRT;
    public RectTransform BladerUltBtnRT;
    public RectTransform MoaiAttackBtnRT;
    public RectTransform MoaiJumpBtnRT;
    public RectTransform MoaiS1BtnRT;
    public RectTransform MoaiS2BtnRT;
    public RectTransform MoaiUltBtnRT;
    public RectTransform LuxAttackBtnRT;
    public RectTransform LuxJumpBtnRT;
    public RectTransform LuxS1BtnRT;
    public RectTransform LuxS2BtnRT;
    public RectTransform LuxUltBtnRT;
    public RectTransform LinkerAttackBtnRT;
    public RectTransform LinkerJumpBtnRT;
    public RectTransform LinkerS1BtnRT;
    public RectTransform LinkerS2BtnRT;
    public RectTransform LinkerUltBtnRT;
    public RectTransform TaraAttackBtnRT;
    public RectTransform TaraJumpBtnRT;
    public RectTransform TaraS1BtnRT;
    public RectTransform TaraS2BtnRT;
    public RectTransform TaraUltBtnRT;

    public Vector2 AttackBtnVector2;
    public Vector2 JumpBtnVector2;
    public Vector2 S1BtnVector2;
    public Vector2 S2BtnVector2;
    public Vector2 UltBtnVector2;
    public Vector2 Item1BtnVector2;
    public Vector2 Item2BtnVector2;
    public Vector2 Item3BtnVector2;

    public Vector2 AttackBtnSizeVector2;
    public Vector2 JumpBtnSizeVector2;
    public Vector2 S1BtnSizeVector2;
    public Vector2 S2BtnSizeVector2;
    public Vector2 UltBtnSizeVector2;
    public Vector2 Item1BtnSizeVector2;
    public Vector2 Item2BtnSizeVector2;
    public Vector2 Item3BtnSizeVector2;

    public GameObject BtnSettingPanel;


    [Header("Player")]
    public GameObject Blue1;
    public GameObject Blue2;
    public GameObject Red1;
    public GameObject Red2;

    [Header("Stat")]
    public StatScript Blue1statScript;
    public StatScript Blue2statScript;
    public StatScript Red1statScript;
    public StatScript Red2statScript;

    public StatScript MyStatScript;


    public Slider slider;

    public int ReadyPlayers = 0;
    public bool ReadyPlayers1Time = true;
    public bool ItemSetDone1Time;

    public PhotonView PV;

    public List<int> ItemBtnList = new List<int>(3);


    [Header("Cooltime")]
    public CooltimeScript AttackCool;
    public CooltimeScript S1Cool;
    public CooltimeScript S2Cool;
    public UltCooltimeScript UltCool;



    [Header("Map")]
    public CinemachineConfiner cinemachineConfiner;
    public int Map; //0 MagneticMap, 1 CaputreMap
    public GameObject MagneticMap;
    public CinemachineVirtualCamera MagneticMapCMCamera;
    public GameObject MageneticMapCMRange;

    public GameObject CaptureMap;
    public GameObject CaptureMapPanel;
    public CaptureAreaScript CaptureAreaScript;
    public CinemachineVirtualCamera CaptureMapCMCamera;
    public GameObject CaptureMapCMRange;
    public GameObject CaptureMapRespawnPanel;
    public Text CaptureMapRespawnText;

    public PortalScript CaptureMapBluePortal;
    public PortalScript CaptureMapRedPortal;

    public Text BaronTimer;
    public BaronScript baronScript;

    public int PlayerCount;//몇인용인지

    public bool GameStarted;

    [Header("Event")]
    public UnityEvent OnReady;//현재 안쓰는 중인듯
    public UnityEvent OnExitRoom;
    public UnityEvent CooltimerCountZeroEvent;//죽었을 때 쿨타임 초기화시키기. CooltimeScript에 있는 GetCooltimerCount을 0f로 해서 스킬 사용 가능하게끔 만듬.
    

    public HurricaneScript tempHurricaneScript;

    [Header("AccountInfo")]
    public int RankPoint;

    public float[] FirstArray = new float[2]; // 0 ActorNumber, 1 RankPoint
    public float[] SecondArray = new float[2];
    public float[] ThirdArray = new float[2];
    public float[] FourthArray = new float[2];

    public int WinReward;
    public int LoseReward;

    public int RankPointReady;
    bool RankPointReady1Time;

    int ReadyForCharacterSelect;
    bool ReadyForCharacterSelectBool;

    public int myPlayerNumber;

    public float myTotalDamageInThisGame;//내가 이판동안 넣은 데미지.
    public int myKillInThisGame;//이판동안 킬 횟수
    public int myDeathInThisGame;//이판동안 데스 횟수

    public bool isMyTeamWin;//우리팀이 이겼는지, 졌는지. 로비에서 결과 보여줄 때 사용하기 위해서 씀.

    public int Blue1RankPoint;//결과창에 랭크포인트 표시하려고 넣음. 캐릭터선택창에서 받아온 값 집어넣음.
    public int Blue2RankPoint;
    public int Red1RankPoint;
    public int Red2RankPoint;

    public bool isTempAccount;

    public bool englishMode;



    void Awake()
    {
        Application.runInBackground = true;
        PhotonNetwork.KeepAliveInBackground = 1300f;
        Application.targetFrameRate = 60;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        PlayerCount = 4;//4인용되면 바꾸기
        RankPoint = 1200;
        myNickName = "tempID";
        NickNameText.text = myNickName;
        BtnVectorSetting();
        PhotonNetwork.ConnectUsingSettings();

        if (instance == null) instance = this;
    }

    public void Connect()
    {
    }

    public override void OnConnectedToMaster()
    {
    }

    public void SetNickName(string nickName)
    {
        PhotonNetwork.LocalPlayer.NickName = nickName;
        myNickName = nickName;
        NickNameText.text = nickName;
        playfabManager.GetStat();
        playfabManager.GetLeaderboard();//랭킹목록 받아옴
    }

    public void temp()
    {
        playfabManager.GetLeaderboard();
        //playfabManager.LogText.text = Convert.ToString(RankPoint);
    }


    public void BtnVectorSetting()
    {
        AttackBtnVector2 = new Vector2(-632f, -395f);
        JumpBtnVector2 = new Vector2(-337f, -322f);
        S1BtnVector2 = new Vector2(-585f, -231f);
        S2BtnVector2 = new Vector2(-483f, -95f);
        UltBtnVector2 = new Vector2(-326f, -21f);
        Item1BtnVector2 = new Vector2(-746f, -260f);
        Item2BtnVector2 = new Vector2(-691f, -121f);
        Item3BtnVector2 = new Vector2(-595f, 2f);

        AttackBtnSizeVector2 = new Vector2(140f, 140f);
        JumpBtnSizeVector2 = new Vector2(224f, 224f);
        S1BtnSizeVector2 = new Vector2(140f, 140f);
        S2BtnSizeVector2 = new Vector2(140f, 140f);
        UltBtnSizeVector2 = new Vector2(140f, 140f);
        Item1BtnSizeVector2 = new Vector2(100f, 100f);
        Item2BtnSizeVector2 = new Vector2(100f, 100f);
        Item3BtnSizeVector2 = new Vector2(100f, 100f);
    }

    public void OnBtnSettingBtn()
    {
        DisconnectPanel.SetActive(false);
        ControlUI.SetActive(true);
        BtnSettingPanel.SetActive(true);
    }


    public void MatchingStart()
    {
        playfabManager.LogText.enabled = false;
        PhotonNetwork.LocalPlayer.NickName = myNickName;
        PhotonNetwork.JoinRandomRoom();
        DisconnectPanel.SetActive(false);
        matchingLoadingScript.gameObject.SetActive(true);
    }

    public void FriendModeStart()
    {
        if (RoomNumberInput.text == "")
        {
            StartCoroutine(FriendModeErrorMessageCRT());
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = myNickName;
            PhotonNetwork.JoinOrCreateRoom(RoomNumberInput.text, RO, null);
            FriendModePanel.SetActive(false);
            DisconnectPanel.SetActive(false);
            matchingLoadingScript.gameObject.SetActive(true);
            RoomNumberInput.text = "";
        }
    }

    IEnumerator FriendModeErrorMessageCRT()
    {
        FriendModeErrorMessage.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        FriendModeErrorMessage.SetActive(false);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RO.MaxPlayers = (byte)PlayerCount;
        PhotonNetwork.CreateRoom("", RO);
    }

    public void MatchingCancel()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            StartCoroutine(MatchingCancelCRT());
        }
        else
        {
            matchingLoadingScript.gameObject.SetActive(false);
            DisconnectPanel.SetActive(true);
        }
        WaitingPeople = false;
    }

    IEnumerator MatchingCancelCRT()
    {
        matchingLoadingScript.MatchingPeople.text = "취소중입니다\n\n";
        yield return new WaitForSeconds(0.6f);
        matchingLoadingScript.gameObject.SetActive(false);
        DisconnectPanel.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        WaitingPeople = true;
        ReadyForCharacterSelect = 0;
        ReadyForCharacterSelectBool = false;
    }



    public void MapSelect()
    {
        //자기장모드는 개발할때 불편해서 꺼둠
        //Map = Random.Range(0, 2);
        Map = 1;
        PV.RPC("MapRPC", RpcTarget.All, Map);
    }

    [PunRPC]
    void MapRPC(int Map)
    {
        this.Map = Map;
        if (Map == 0)
        {
            MagneticMap.SetActive(true);
        }
        else if (Map == 1)
        {
            CaptureMapBluePortal.Init();
            CaptureMapRedPortal.Init();
            CaptureMap.SetActive(true);
        }
    }




    private void LinkerS1Init()
    {
        if (Blue1statScript != null)
        {
            Blue1statScript.LinkerS1CircleInit();
        }
        if (Blue2statScript != null)
        {
            Blue2statScript.LinkerS1CircleInit();
        }
        if (Red1statScript != null)
        {
            Red1statScript.LinkerS1CircleInit();
        }
        if (Red2statScript != null)
        {
            Red2statScript.LinkerS1CircleInit();
        }
    }





    [PunRPC]
    void RankPointReadyRPC(float actorNumber, float rankPoint)
    {
        if (FirstArray[0] == 0f)
        {
            FirstArray[0] = actorNumber;
            FirstArray[1] = rankPoint;
        }
        else if (SecondArray[0] == 0f)
        {
            SecondArray[0] = actorNumber;
            SecondArray[1] = rankPoint;
        }
        else if (ThirdArray[0] == 0f)
        {
            ThirdArray[0] = actorNumber;
            ThirdArray[1] = rankPoint;
        }
        else if (FourthArray[0] == 0f)
        {
            FourthArray[0] = actorNumber;
            FourthArray[1] = rankPoint;
        }

        RankPointReady++;
        if (RankPointReady == PlayerCount)
        {
            RankPointReady = 0;
            PV.RPC("RankPointReady1TimeRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    void RankPointReady1TimeRPC()
    {
        playfabManager.GoogleLoginText.text = "처음받은결과" + FirstArray[0] + " " + FirstArray[1] + " " + SecondArray[0] + " " + SecondArray[1] + " " + ThirdArray[0] + " " + ThirdArray[1] + " " + FourthArray[0] + " " + FourthArray[1];
        RankPointReady1Time = true;
    }

    void ArrangeRankPointArray()//4인용일때 이거로 정렬하고 2인용일때 함수 따로만들기
    {
        float[] TempArray = new float[2];
        TempArray[0] = 0; TempArray[1] = 0;
        if (FirstArray[1] > SecondArray[1])
        {
            Array.Copy(SecondArray, TempArray, 2);
            Array.Copy(FirstArray, SecondArray, 2);
            Array.Copy(TempArray, FirstArray, 2);
        }
        if (FirstArray[1] > ThirdArray[1])
        {
            Array.Copy(ThirdArray, TempArray, 2);
            Array.Copy(FirstArray, ThirdArray, 2);
            Array.Copy(TempArray, FirstArray, 2);
        }
        if (FirstArray[1] > FourthArray[1])
        {
            Array.Copy(FourthArray, TempArray, 2);
            Array.Copy(FirstArray, FourthArray, 2);
            Array.Copy(TempArray, FirstArray, 2);
        }
        if (SecondArray[1] < FirstArray[1])
        {
            Array.Copy(FirstArray, TempArray, 2);
            Array.Copy(SecondArray, FirstArray, 2);
            Array.Copy(TempArray, SecondArray, 2);
        }
        if (SecondArray[1] > ThirdArray[1])
        {
            Array.Copy(ThirdArray, TempArray, 2);
            Array.Copy(SecondArray, ThirdArray, 2);
            Array.Copy(TempArray, SecondArray, 2);
        }
        if (SecondArray[1] > FourthArray[1])
        {
            Array.Copy(FourthArray, TempArray, 2);
            Array.Copy(SecondArray, FourthArray, 2);
            Array.Copy(TempArray, SecondArray, 2);
        }
        if (ThirdArray[1] < FirstArray[1])
        {
            Array.Copy(FirstArray, TempArray, 2);
            Array.Copy(ThirdArray, FirstArray, 2);
            Array.Copy(TempArray, ThirdArray, 2);
        }
        if (ThirdArray[1] < SecondArray[1])
        {
            Array.Copy(SecondArray, TempArray, 2);
            Array.Copy(ThirdArray, SecondArray, 2);
            Array.Copy(TempArray, ThirdArray, 2);
        }
        if (ThirdArray[1] > FourthArray[1])
        {
            Array.Copy(FourthArray, TempArray, 2);
            Array.Copy(ThirdArray, FourthArray, 2);
            Array.Copy(TempArray, ThirdArray, 2);
        }

        //각자 점수에 따라 이기면 오를점수, 지면 내릴점수 계산해서 보내기
        float BlueTeamAverage = (FirstArray[1] + FourthArray[1]) / 2;
        float RedTeamAverage = (SecondArray[1] + ThirdArray[1]) / 2;
        float[] RewardRankPoint = new float[2];
        Array.Copy(CalculateRewardRankPointByTeamAverage(BlueTeamAverage - RedTeamAverage), RewardRankPoint, 2);
        float TotalAverage = (BlueTeamAverage + RedTeamAverage) / 2;
        float FirstArrayPlayerRewardByTotalAverage = CalculateRewardRankPointByTotalAverage(FirstArray[1] - TotalAverage);
        float SecondArrayPlayerRewardByTotalAverage = CalculateRewardRankPointByTotalAverage(SecondArray[1] - TotalAverage);
        float ThirdArrayPlayerRewardByTotalAverage = CalculateRewardRankPointByTotalAverage(ThirdArray[1] - TotalAverage);
        float FourthArrayPlayerRewardByTotalAverage = CalculateRewardRankPointByTotalAverage(FourthArray[1] - TotalAverage);
        playfabManager.LogText.text = FirstArray[0] + " " + FirstArray[1] + " " + SecondArray[0] + " " + SecondArray[1] + " " + ThirdArray[0] + " " + ThirdArray[1] + " " + FourthArray[0] + " " + FourthArray[1];
        PV.RPC("SetTeamRPC", RpcTarget.All, FirstArray[0], 1, RewardRankPoint[0] + FirstArrayPlayerRewardByTotalAverage, -RewardRankPoint[1] + FirstArrayPlayerRewardByTotalAverage);
        PV.RPC("SetTeamRPC", RpcTarget.All, SecondArray[0], 2, RewardRankPoint[1] + SecondArrayPlayerRewardByTotalAverage, -RewardRankPoint[0] + SecondArrayPlayerRewardByTotalAverage);
        PV.RPC("SetTeamRPC", RpcTarget.All, ThirdArray[0], 3, RewardRankPoint[1] + ThirdArrayPlayerRewardByTotalAverage, -RewardRankPoint[0] + ThirdArrayPlayerRewardByTotalAverage);
        PV.RPC("SetTeamRPC", RpcTarget.All, FourthArray[0], 4, RewardRankPoint[0] + FourthArrayPlayerRewardByTotalAverage, -RewardRankPoint[1] + FourthArrayPlayerRewardByTotalAverage);
    }

    public float[] CalculateRewardRankPointByTeamAverage(float AverageDifference)
    {
        float[] result = new float[2];//0 블루가 이길경우, 1 레드가 이길경우
        if (AverageDifference > 270f)
        {
            result[0] = 0; result[1] = 61;
        }
        else if (AverageDifference > 240f)
        {
            result[0] = 3; result[1] = 57;
        }
        else if (AverageDifference > 210f)
        {
            result[0] = 7; result[1] = 53;
        }
        else if (AverageDifference > 180f)
        {
            result[0] = 11; result[1] = 49;
        }
        else if (AverageDifference > 150f)
        {
            result[0] = 15; result[1] = 45;
        }
        else if (AverageDifference > 120f)
        {
            result[0] = 18; result[1] = 42;
        }
        else if (AverageDifference > 90f)
        {
            result[0] = 21; result[1] = 39;
        }
        else if (AverageDifference > 60f)
        {
            result[0] = 24; result[1] = 36;
        }
        else if (AverageDifference > 30f)
        {
            result[0] = 26; result[1] = 34;
        }
        else if (AverageDifference > 10f)
        {
            result[0] = 28; result[1] = 32;
        }
        else if (AverageDifference > 0f)
        {
            result[0] = 29; result[1] = 31;
        }
        else if (AverageDifference == 0f)
        {
            result[0] = 30; result[1] = 30;
        }
        else if (AverageDifference > -10f)
        {
            result[0] = 31; result[1] = 29;
        }
        else if (AverageDifference > -30f)
        {
            result[0] = 32; result[1] = 28;
        }
        else if (AverageDifference > -60f)
        {
            result[0] = 34; result[1] = 26;
        }
        else if (AverageDifference > -90f)
        {
            result[0] = 36; result[1] = 24;
        }
        else if (AverageDifference > -120f)
        {
            result[0] = 39; result[1] = 21;
        }
        else if (AverageDifference > -150f)
        {
            result[0] = 42; result[1] = 18;
        }
        else if (AverageDifference > -180f)
        {
            result[0] = 45; result[1] = 15;
        }
        else if (AverageDifference > -210f)
        {
            result[0] = 49; result[1] = 11;
        }
        else if (AverageDifference > -240f)
        {
            result[0] = 53; result[1] = 7;
        }
        else if (AverageDifference > -270f)
        {
            result[0] = 57; result[1] = 3;
        }
        else
        {
            result[0] = 61; result[1] = 0;
        }

        return result;
    }

    public float CalculateRewardRankPointByTotalAverage(float AverageDifference)
    {
        float result = 0f;
        if (AverageDifference > 270f)
        {
            result = -31;
        }
        else if (AverageDifference > 240f)
        {
            result = -27;
        }
        else if (AverageDifference > 210f)
        {
            result = -23;
        }
        else if (AverageDifference > 180f)
        {
            result = -19;
        }
        else if (AverageDifference > 150f)
        {
            result = -15;
        }
        else if (AverageDifference > 120f)
        {
            result = -12;
        }
        else if (AverageDifference > 90f)
        {
            result = -9;
        }
        else if (AverageDifference > 60f)
        {
            result = -6;
        }
        else if (AverageDifference > 30f)
        {
            result = -4;
        }
        else if (AverageDifference > 10f)
        {
            result = -2;
        }
        else if (AverageDifference > 0f)
        {
            result = -1;
        }
        else if (AverageDifference == 0f)
        {
            result = 0;
        }
        else if (AverageDifference > -10f)
        {
            result = 1;
        }
        else if (AverageDifference > -30f)
        {
            result = 2;
        }
        else if (AverageDifference > -60f)
        {
            result = 4;
        }
        else if (AverageDifference > -90f)
        {
            result = 6;
        }
        else if (AverageDifference > -120f)
        {
            result = 9;
        }
        else if (AverageDifference > -150f)
        {
            result = 12;
        }
        else if (AverageDifference > -180f)
        {
            result = 15;
        }
        else if (AverageDifference > -210f)
        {
            result = 19;
        }
        else if (AverageDifference > -240f)
        {
            result = 23;
        }
        else if (AverageDifference > -270f)
        {
            result = 27;
        }
        else
        {
            result = 31;
        }

        return result;
    }


    [PunRPC]
    void SetTeamRPC(float actorNumber, int arrangeValue, float WinReward, float LoseReward)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == (int)actorNumber)
        {
            myPlayerNumber = arrangeValue;
            this.WinReward = (int)WinReward;
            this.LoseReward = (int)LoseReward;
            if (this.WinReward < 5) this.WinReward = 5;
            if (this.LoseReward > -5) this.LoseReward = -5;
            playfabManager.log.text = myPlayerNumber + " " + PhotonNetwork.LocalPlayer.ActorNumber + " " + actorNumber + " " + arrangeValue;
            PV.RPC("ReadyForCharacterSelectRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    void ReadyForCharacterSelectRPC()
    {
        ReadyForCharacterSelect++;
        if (ReadyForCharacterSelect == PlayerCount)
        {
            ReadyForCharacterSelect = 0;
            ReadyForCharacterSelectBool = true;
        }
    }

    void ArrangeRankPointArrayFor2Players()
    {
        float[] TempArray = new float[2];
        if (FirstArray[1] < SecondArray[1])
        {
            TempArray = SecondArray;
            SecondArray = FirstArray;
            FirstArray = TempArray;
        }

        PV.RPC("SetTeamRPC", RpcTarget.All, FirstArray[0], 1);
        PV.RPC("SetTeamRPC", RpcTarget.All, SecondArray[0], 2);
    }

    [PunRPC]
    void NicknameTextRPC(string MyNickName, int WhatText, int RankPoint)
    {
        if (WhatText == 1)
        {
            CharacterSelectScript.Blue1Text.text = MyNickName;
            CharacterSelectScript.Blue1RP.text = RankPoint + "";
            Blue1RankPoint = RankPoint;
        }
        else if (WhatText == 2)
        {
            CharacterSelectScript.Red1Text.text = MyNickName;
            CharacterSelectScript.Red1RP.text = RankPoint + "";
            Red1RankPoint = RankPoint;
        }
        else if (WhatText == 3)
        {
            CharacterSelectScript.Red2Text.text = MyNickName;
            CharacterSelectScript.Red2RP.text = RankPoint + "";
            Red2RankPoint = RankPoint;
        }
        else if (WhatText == 4)
        {
            CharacterSelectScript.Blue2Text.text = MyNickName;
            CharacterSelectScript.Blue2RP.text = RankPoint + "";
            Blue2RankPoint = RankPoint;
        }
    }


    void Update()
    {
        if (WaitingPeople && PhotonNetwork.CurrentRoom.PlayerCount >= PlayerCount)
        {
            WaitingPeople = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            matchingLoadingScript.gameObject.SetActive(false);
            DisconnectPanel.SetActive(false);

            //여기에 로딩중 화면 띄우기 넣기. 굉장히 짧을거임

            PV.RPC("RankPointReadyRPC", RpcTarget.MasterClient, (float)PhotonNetwork.LocalPlayer.ActorNumber, (float)RankPoint);
        }

        if (RankPointReady1Time)
        {
            RankPointReady1Time = false;
            RankPointReady = 0;

            if (PhotonNetwork.IsMasterClient)
            {
                if (PlayerCount == 4)
                {
                    ArrangeRankPointArray();
                }
                else if (PlayerCount == 2)
                {
                    ArrangeRankPointArrayFor2Players();
                }
            }

            if (PhotonNetwork.IsMasterClient)
            {
                MapSelect();
            }
        }

        if (ReadyForCharacterSelectBool)
        {
            ReadyForCharacterSelectBool = false;
            ReadyForCharacterSelect = 0;

            CharacterSelectCanvas.SetActive(true);
            InstantiateItemSlot();
        }

        //나중에 4인용으로 바꾸기
        if (ReadyPlayers1Time && ReadyPlayers == PlayerCount)
        {
            ReadyPlayers1Time = false;

            if (Blue1statScript != null)
                Blue1statScript.SetStatScripts();
            if (Blue2statScript != null)
                Blue2statScript.SetStatScripts();
            if (Red1statScript != null)
                Red1statScript.SetStatScripts();
            if (Red2statScript != null)
                Red2statScript.SetStatScripts();

            OnReady.Invoke();

            LinkerS1Init();

            if (CharacterSelectScript.SelectedCharacter == 2)
            {
                if (myPlayerNumber == 1)
                {
                    if (Red1statScript != null)
                        Red1statScript.BladerS1AuraImage.enabled = true;
                    if (Red2statScript != null)
                        Red2statScript.BladerS1AuraImage.enabled = true;
                }
                if (myPlayerNumber == 2)
                {
                    if (Blue1statScript != null)
                        Blue1statScript.BladerS1AuraImage.enabled = true;
                    if (Blue2statScript != null)
                        Blue2statScript.BladerS1AuraImage.enabled = true;
                }
                if (myPlayerNumber == 3)
                {
                    if (Blue1statScript != null)
                        Blue1statScript.BladerS1AuraImage.enabled = true;
                    if (Blue2statScript != null)
                        Blue2statScript.BladerS1AuraImage.enabled = true;
                }
                if (myPlayerNumber == 4)
                {
                    if (Red1statScript != null)
                        Red1statScript.BladerS1AuraImage.enabled = true;
                    if (Red2statScript != null)
                        Red2statScript.BladerS1AuraImage.enabled = true;
                }
            }
        }

        if (ItemSetDone1Time && slider.value >= 0.25f * PlayerCount)
        {
            ItemSetDone1Time = false;
            ItemSetDone();
            killLogManager.Init();
            slider.value = 0f;
        }
    }


    public void ItemSetDone()
    {
        //LoaingPanel.SetActive(false);20220803 밑으로옮김
        //맵에 따라서 사운드 실행
        if (Map == 0)
        {

        }
        else if (Map == 1)
        {
            soundManagerScript.AS_Setting(Sound_Background.CaptureMap);
        }
        countPanelScript.CountText.text = "3";
        countPanelScript.count = 2.99f;
        countPanelScript.startText1Time = true;
        //countPanelScript.gameObject.SetActive(true);20220803 밑으로옮김
        MapSetting();
    }

    public void MapSetting()
    {
        if (Map == 0)
        {
            MagneticMapCMCamera.gameObject.SetActive(true);
            MageneticMapCMRange.SetActive(true);
        }
        else if (Map == 1)
        {
            CaptureMapPanel.SetActive(true);
            CaptureAreaScript.GameEnd = false;
            CaptureAreaScript.gameObject.SetActive(true);
            CaptureMapCMCamera.gameObject.SetActive(true);
            CaptureMapCMRange.SetActive(true);
            countPanelScript.ProhibitedAreaSR.enabled = true;
            countPanelScript.ProhibitedAreaCol.enabled = true;

            LoaingPanel.SetActive(false);//20220803 밑으로옮김
            countPanelScript.gameObject.SetActive(true);//20220803 밑으로옮김
            if (PhotonNetwork.IsMasterClient)
            {
                //HpKit 만들기
                PhotonNetwork.Instantiate("HpKitPrefab", new Vector2(-14.5f, -1.5f), Quaternion.identity);
                PhotonNetwork.Instantiate("HpKitPrefab", new Vector2(14.5f, -1.5f), Quaternion.identity);
                //Baron 만들기
                PhotonNetwork.Instantiate("BaronPrefab", new Vector2(0f, 12.1f), Quaternion.identity).GetComponent<GetDamageScript>().PV.RPC("InitRPC", RpcTarget.All);
            }
        }
    }


    public void InstantiateItemSlot()
    {
        PhotonNetwork.Instantiate("ItemSlot", Vector3.zero, Quaternion.identity);
    }


    public void ApplyItemBool()
    {
        if (myPlayerNumber == 1)
        {
            ReadItemSlot(Blue1statScript);
            ItemBtnSpawn();
        }
        if (myPlayerNumber == 2)
        {
            ReadItemSlot(Red1statScript);
            ItemBtnSpawn();
        }
        if (myPlayerNumber == 3)
        {
            ReadItemSlot(Red2statScript);
            ItemBtnSpawn();
        }
        if (myPlayerNumber == 4)
        {
            ReadItemSlot(Blue2statScript);
            ItemBtnSpawn();
        }
    }

    public void ReadItemSlot(StatScript statScript)
    {
        if (ItemManager.Slot1 == 1)
        {
            statScript.Zonya = true;
            ItemBtnList.Add(1);
        }
        else if (ItemManager.Slot1 == 2)
        {
            statScript.SunFire = true;
            statScript.SunFireCol.enabled = true;
        }
        else if (ItemManager.Slot1 == 3)
        {
            statScript.shoesATK = true;
        }

        if (ItemManager.Slot2 == 1)
        {
            statScript.Zonya = true;
            ItemBtnList.Add(1);
        }
        else if (ItemManager.Slot2 == 2)
        {
            statScript.SunFire = true;
            statScript.SunFireCol.enabled = true;
        }
        else if (ItemManager.Slot2 == 3)
        {
            statScript.shoesATK = true;
        }

        if (ItemManager.Slot3 == 1)
        {
            statScript.Zonya = true;
            ItemBtnList.Add(1);
        }
        else if (ItemManager.Slot3 == 2)
        {
            statScript.SunFire = true;
            statScript.SunFireCol.enabled = true;
        }
        else if (ItemManager.Slot3 == 3)
        {
            statScript.shoesATK = true;
        }

        //statScript.Item_Apply();
    }

    public void ItemBtnSpawn()
    {
        if (ItemBtnList.Count > 0)
        {
            if (ItemBtnList[0] == 1)
            {
                Zonya zonya = Instantiate(ZonyaBtn, ControlUItransform).GetComponent<Zonya>();
                zonya.RT.anchoredPosition = Item1BtnVector2;
                zonya.RT.sizeDelta = Item1BtnSizeVector2;
                zonya.Init();
            }
        }

        if (ItemBtnList.Count > 1)
        {
            if (ItemBtnList[1] == 1)
            {
                Zonya zonya = Instantiate(ZonyaBtn, ControlUItransform).GetComponent<Zonya>();
                zonya.RT.anchoredPosition = Item2BtnVector2;
                zonya.RT.sizeDelta = Item2BtnSizeVector2;
                zonya.Init();
            }
        }

        if (ItemBtnList.Count > 2)
        {
            if (ItemBtnList[2] == 1)
            {
                Zonya zonya = Instantiate(ZonyaBtn, ControlUItransform).GetComponent<Zonya>();
                zonya.RT.anchoredPosition = Item3BtnVector2;
                zonya.RT.sizeDelta = Item3BtnSizeVector2;
                zonya.Init();
            }
        }
    }




    public void GameStart()
    {
        Inventory.SetActive(false);

        //게임시작할때 초기화할 부분
        myTotalDamageInThisGame = 0f;
        myKillInThisGame = 0;
        myDeathInThisGame = 0;

        //캐릭터 선택 안했을 경우. 나중에 4,5 완성하면 주석 해제하기. 나중에는 선택 안한순간 안고른 것중에 랜덤하게 선택되게 해야됨. 지금은 4명인데 4캐릭이라 그냥 남는거 고름
        if (CharacterSelectScript.SelectedCharacter == 0)
        {
            if (!CharacterSelectScript.Character1Selected)
            {
                CharacterSelectScript.SelectedCharacter = 1;
            }
            else if (!CharacterSelectScript.Character2Selected)
            {
                CharacterSelectScript.SelectedCharacter = 2;
            }
            else if (!CharacterSelectScript.Character3Selected)
            {
                CharacterSelectScript.SelectedCharacter = 3;
            }
            else if (!CharacterSelectScript.Character4Selected)
            {
                CharacterSelectScript.SelectedCharacter = 4;
            }
            else if (!CharacterSelectScript.Character5Selected)
            {
                CharacterSelectScript.SelectedCharacter = 5;
            }
            else if (!CharacterSelectScript.Character6Selected)
            {
                CharacterSelectScript.SelectedCharacter = 6;
            }
        }
        BtnSpawn(CharacterSelectScript.SelectedCharacter);
        CharacterSelectCanvas.SetActive(false);
        floatingJoystick.SetActive(true);
        ControlUI.SetActive(true);
        Spawn(CharacterSelectScript.SelectedCharacter);
        LoaingPanel.SetActive(true);
    }

    public void BtnSpawn(int selectedCharacter)
    {
        AttackBtnRT.anchoredPosition = AttackBtnVector2;
        JumpBtnRT.anchoredPosition = JumpBtnVector2;
        S1BtnRT.anchoredPosition = S1BtnVector2;
        S2BtnRT.anchoredPosition = S2BtnVector2;
        UltBtnRT.anchoredPosition = UltBtnVector2;

        AttackBtnRT.sizeDelta = AttackBtnSizeVector2;
        JumpBtnRT.sizeDelta = JumpBtnSizeVector2;
        S1BtnRT.sizeDelta = S1BtnSizeVector2;
        S2BtnRT.sizeDelta = S2BtnSizeVector2;
        UltBtnRT.sizeDelta = UltBtnSizeVector2;

        if (selectedCharacter == 1)//Archer
        {
            ArcherAttackBtnRT.anchoredPosition = AttackBtnVector2;
            ArcherAttackBtnRT.sizeDelta = AttackBtnSizeVector2;
            ArcherJumpBtnRT.anchoredPosition = JumpBtnVector2;
            ArcherJumpBtnRT.sizeDelta = JumpBtnSizeVector2;
            ArcherS1BtnRT.anchoredPosition = S1BtnVector2;
            ArcherS1BtnRT.sizeDelta = S1BtnSizeVector2;
            ArcherS2BtnRT.anchoredPosition = S2BtnVector2;
            ArcherS2BtnRT.sizeDelta = S2BtnSizeVector2;
            ArcherUltBtnRT.anchoredPosition = UltBtnVector2;
            ArcherUltBtnRT.sizeDelta = UltBtnSizeVector2;

            Instantiate(ArcherAttackBtn, ControlUItransform).name = "AttackBtn";
            Instantiate(ArcherJumpBtn, ControlUItransform).name = "JumpBtn";
            Instantiate(ArcherS1Btn, ControlUItransform).name = "S1Btn";
            Instantiate(ArcherS2Btn, ControlUItransform).name = "S2Btn";
            Instantiate(ArcherUltBtn, ControlUItransform).name = "UltBtn";
            return;
        }
        else if (selectedCharacter == 2)//Blader
        {
            BladerAttackBtnRT.anchoredPosition = AttackBtnVector2;
            BladerAttackBtnRT.sizeDelta = AttackBtnSizeVector2;
            BladerJumpBtnRT.anchoredPosition = JumpBtnVector2;
            BladerJumpBtnRT.sizeDelta = JumpBtnSizeVector2;
            BladerS1BtnRT.anchoredPosition = S1BtnVector2;
            BladerS1BtnRT.sizeDelta = S1BtnSizeVector2;
            BladerS2BtnRT.anchoredPosition = S2BtnVector2;
            BladerS2BtnRT.sizeDelta = S2BtnSizeVector2;
            BladerUltBtnRT.anchoredPosition = UltBtnVector2;
            BladerUltBtnRT.sizeDelta = UltBtnSizeVector2;
            Instantiate(BladerAttackBtn, ControlUItransform).name = "AttackBtn";
            Instantiate(BladerJumpBtn, ControlUItransform).name = "JumpBtn";
            Instantiate(BladerS1Btn, ControlUItransform).name = "S1Btn";
            Instantiate(BladerS2Btn, ControlUItransform).name = "S2Btn";
            Instantiate(BladerUltBtn, ControlUItransform).name = "UltBtn";
            return;
        }
        else if (selectedCharacter == 3)//Moai
        {
            MoaiAttackBtnRT.anchoredPosition = AttackBtnVector2;
            MoaiAttackBtnRT.sizeDelta = AttackBtnSizeVector2;
            MoaiJumpBtnRT.anchoredPosition = JumpBtnVector2;
            MoaiJumpBtnRT.sizeDelta = JumpBtnSizeVector2;
            MoaiS1BtnRT.anchoredPosition = S1BtnVector2;
            MoaiS1BtnRT.sizeDelta = S1BtnSizeVector2;
            MoaiS2BtnRT.anchoredPosition = S2BtnVector2;
            MoaiS2BtnRT.sizeDelta = S2BtnSizeVector2;
            MoaiUltBtnRT.anchoredPosition = UltBtnVector2;
            MoaiUltBtnRT.sizeDelta = UltBtnSizeVector2;
            Instantiate(MoaiAttackBtn, ControlUItransform).name = "AttackBtn";
            Instantiate(MoaiJumpBtn, ControlUItransform).name = "JumpBtn";
            Instantiate(MoaiS1Btn, ControlUItransform).name = "S1Btn";
            Instantiate(MoaiS2Btn, ControlUItransform).name = "S2Btn";
            Instantiate(MoaiUltBtn, ControlUItransform).name = "UltBtn";
            return;
        }
        else if (selectedCharacter == 4)//Lux
        {
            LuxAttackBtnRT.anchoredPosition = AttackBtnVector2;
            LuxAttackBtnRT.sizeDelta = AttackBtnSizeVector2;
            LuxJumpBtnRT.anchoredPosition = JumpBtnVector2;
            LuxJumpBtnRT.sizeDelta = JumpBtnSizeVector2;
            LuxS1BtnRT.anchoredPosition = S1BtnVector2;
            LuxS1BtnRT.sizeDelta = S1BtnSizeVector2;
            LuxS2BtnRT.anchoredPosition = S2BtnVector2;
            LuxS2BtnRT.sizeDelta = S2BtnSizeVector2;
            LuxUltBtnRT.anchoredPosition = UltBtnVector2;
            LuxUltBtnRT.sizeDelta = UltBtnSizeVector2;
            Instantiate(LuxAttackBtn, ControlUItransform).name = "AttackBtn";
            Instantiate(LuxJumpBtn, ControlUItransform).name = "JumpBtn";
            Instantiate(LuxS1Btn, ControlUItransform).name = "S1Btn";
            Instantiate(LuxS2Btn, ControlUItransform).name = "S2Btn";
            Instantiate(LuxUltBtn, ControlUItransform).name = "UltBtn";
            return;
        }
        else if (selectedCharacter == 5)//Linker
        {
            LinkerAttackBtnRT.anchoredPosition = AttackBtnVector2;
            LinkerAttackBtnRT.sizeDelta = AttackBtnSizeVector2;
            LinkerJumpBtnRT.anchoredPosition = JumpBtnVector2;
            LinkerJumpBtnRT.sizeDelta = JumpBtnSizeVector2;
            LinkerS1BtnRT.anchoredPosition = S1BtnVector2;
            LinkerS1BtnRT.sizeDelta = S1BtnSizeVector2;
            LinkerS2BtnRT.anchoredPosition = S2BtnVector2;
            LinkerS2BtnRT.sizeDelta = S2BtnSizeVector2;
            LinkerUltBtnRT.anchoredPosition = UltBtnVector2;
            LinkerUltBtnRT.sizeDelta = UltBtnSizeVector2;
            Instantiate(LinkerAttackBtn, ControlUItransform).name = "AttackBtn";
            Instantiate(LinkerJumpBtn, ControlUItransform).name = "JumpBtn";
            Instantiate(LinkerS1Btn, ControlUItransform).name = "S1Btn";
            Instantiate(LinkerS2Btn, ControlUItransform).name = "S2Btn";
            Instantiate(LinkerUltBtn, ControlUItransform).name = "UltBtn";
            return;
        }
        else if (selectedCharacter == 6)//Tara
        {
            TaraAttackBtnRT.anchoredPosition = AttackBtnVector2;
            TaraAttackBtnRT.sizeDelta = AttackBtnSizeVector2;
            TaraJumpBtnRT.anchoredPosition = JumpBtnVector2;
            TaraJumpBtnRT.sizeDelta = JumpBtnSizeVector2;
            TaraS1BtnRT.anchoredPosition = S1BtnVector2;
            TaraS1BtnRT.sizeDelta = S1BtnSizeVector2;
            TaraS2BtnRT.anchoredPosition = S2BtnVector2;
            TaraS2BtnRT.sizeDelta = S2BtnSizeVector2;
            TaraUltBtnRT.anchoredPosition = UltBtnVector2;
            TaraUltBtnRT.sizeDelta = UltBtnSizeVector2;
            Instantiate(TaraAttackBtn, ControlUItransform).name = "AttackBtn";
            Instantiate(TaraJumpBtn, ControlUItransform).name = "JumpBtn";
            Instantiate(TaraS1Btn, ControlUItransform).name = "S1Btn";
            Instantiate(TaraS2Btn, ControlUItransform).name = "S2Btn";
            Instantiate(TaraUltBtn, ControlUItransform).name = "UltBtn";
            return;
        }
    }

    public void Spawn(int selectedCharacter)
    {
        Vector3 tempVector = Vector3.zero;

        if (Map == 1)
        {
            if (myPlayerNumber == 1)
            {
                tempVector = new Vector3(-12.5f, -1.5f, 0);
            }
            else if (myPlayerNumber == 2)
            {
                tempVector = new Vector3(12.5f, -1.5f, 0);
            }
            else if (myPlayerNumber == 3)
            {
                tempVector = new Vector3(10.5f, 1.5f, 0);
            }
            else if (myPlayerNumber == 4)
            {
                tempVector = new Vector3(-10.5f, 1.5f, 0);
            }
        }

        if (selectedCharacter == 1)
        {
            PhotonNetwork.Instantiate("Archer", tempVector, Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
        else if (selectedCharacter == 2)
        {
            PhotonNetwork.Instantiate("Blader", tempVector, Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
        else if (selectedCharacter == 3)
        {
            PhotonNetwork.Instantiate("Moai", tempVector, Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
        else if (selectedCharacter == 4)
        {
            PhotonNetwork.Instantiate("Lux", tempVector, Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
        else if (selectedCharacter == 5)
        {
            PhotonNetwork.Instantiate("Linker", tempVector, Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
        else if (selectedCharacter == 6)
        {
            PhotonNetwork.Instantiate("Tara", tempVector, Quaternion.identity).GetComponent<PhotonView>().RPC("InitRPC", RpcTarget.All);
        }
    }


    [PunRPC]
    void SliderPlusRPC()
    {
        slider.value += 0.25f;
    }


    public void RespawnMyCharacter()
    {
        RespawnPanel.SetActive(false);
        ControlUI.SetActive(true);
        if (myPlayerNumber == 1)
        {
            PV.RPC("RespawnMyCharacterRPC", RpcTarget.All, 1);
        }
        else if (myPlayerNumber == 2)
        {
            PV.RPC("RespawnMyCharacterRPC", RpcTarget.All, 2);
        }
        else if (myPlayerNumber == 3)
        {
            PV.RPC("RespawnMyCharacterRPC", RpcTarget.All, 3);
        }
        else if (myPlayerNumber == 4)
        {
            PV.RPC("RespawnMyCharacterRPC", RpcTarget.All, 4);
        }
    }

    [PunRPC]
    void RespawnMyCharacterRPC(int value)
    {
        if (value == 1)
        {
            StartCoroutine(Blue1statScript.InvincibleCRT());
            if (Blue1statScript.SunFire)
            {
                Blue1statScript.SunFireImage.SetActive(true);
            }

            if (Map == 0)
            {
                Blue1.transform.position = new Vector3(-6.2f, 5.7f, 0);
            }
            else if (Map == 1)
            {
                Blue1.transform.position = new Vector3(-12.5f, -1.5f, 0);
            }
            Blue1statScript.isDead = false;
            Blue1statScript.PlayerHP = Blue1statScript.PlayerMaxHP;
            Blue1statScript.SR.enabled = true;
            Blue1statScript.MyCol.enabled = true;
            Blue1statScript.RB.simulated = true;
            Blue1statScript.RB.gravityScale = Blue1statScript.OriginalGravityScale;
            Blue1statScript.RB.velocity = Vector2.zero;
            Blue1statScript.canvas.SetActive(true);
            Blue1statScript.MoveLock = false;
            Blue1statScript.JumpLock = false;
            Blue1statScript.SkillLock = false;
            Blue1statScript.speed = Blue1statScript.originalSpeed;
            Blue1statScript.MoaiS2Pull = false;
            Blue1statScript.Aggroed = false;
            Blue1statScript.AN.SetBool(Blue1statScript.character_Base.deathID, false);
            if (Blue1statScript.BaronBuffOn)
            {
                Blue1statScript.BaronBuffAN.enabled = true;
                Blue1statScript.BaronBuffSR.enabled = true;
            }
        }
        else if (value == 2)
        {
            StartCoroutine(Red1statScript.InvincibleCRT());
            if (Red1statScript.SunFire)
            {
                Red1statScript.SunFireImage.SetActive(true);
            }

            if (Map == 0)
            {
                Red1.transform.position = new Vector3(19.2f, 5.7f, 0);
            }
            else if (Map == 1)
            {
                Red1.transform.position = new Vector3(12.5f, -1.5f, 0);
            }
            Red1statScript.isDead = false;
            Red1statScript.PlayerHP = Red1statScript.PlayerMaxHP;
            Red1statScript.SR.enabled = true;
            Red1statScript.MyCol.enabled = true;
            Red1statScript.RB.simulated = true;
            Red1statScript.RB.gravityScale = Red1statScript.OriginalGravityScale;
            Red1statScript.RB.velocity = Vector2.zero;
            Red1statScript.canvas.SetActive(true);
            Red1statScript.MoveLock = false;
            Red1statScript.JumpLock = false;
            Red1statScript.SkillLock = false;
            Red1statScript.speed = Red1statScript.originalSpeed;
            Red1statScript.MoaiS2Pull = false;
            Red1statScript.Aggroed = false;
            Red1statScript.AN.SetBool(Red1statScript.character_Base.deathID, false);
            if (Red1statScript.BaronBuffOn)
            {
                Red1statScript.BaronBuffAN.enabled = true;
                Red1statScript.BaronBuffSR.enabled = true;
            }
        }
        else if (value == 3)
        {
            StartCoroutine(Red2statScript.InvincibleCRT());
            if (Red2statScript.SunFire)
            {
                Red2statScript.SunFireImage.SetActive(true);
            }

            if (Map == 0)
            {
                Red2.transform.position = new Vector3(17.2f, 3.7f, 0);
            }
            else if (Map == 1)
            {
                Red2.transform.position = new Vector3(12.5f, -1.5f, 0);
            }
            Red2statScript.isDead = false;
            Red2statScript.PlayerHP = Red2statScript.PlayerMaxHP;
            Red2statScript.SR.enabled = true;
            Red2statScript.MyCol.enabled = true;
            Red2statScript.RB.simulated = true;
            Red2statScript.RB.gravityScale = Red2statScript.OriginalGravityScale;
            Red2statScript.RB.velocity = Vector2.zero;
            Red2statScript.canvas.SetActive(true);
            Red2statScript.MoveLock = false;
            Red2statScript.JumpLock = false;
            Red2statScript.SkillLock = false;
            Red2statScript.speed = Red2statScript.originalSpeed;
            Red2statScript.MoaiS2Pull = false;
            Red2statScript.Aggroed = false;
            Red2statScript.AN.SetBool(Red2statScript.character_Base.deathID, false);
            if (Red2statScript.BaronBuffOn)
            {
                Red2statScript.BaronBuffAN.enabled = true;
                Red2statScript.BaronBuffSR.enabled = true;
            }
        }
        else if (value == 4)
        {
            StartCoroutine(Blue2statScript.InvincibleCRT());
            if (Blue2statScript.SunFire)
            {
                Blue2statScript.SunFireImage.SetActive(true);
            }

            if (Map == 0)
            {
                Blue2.transform.position = new Vector3(-4.2f, 3.7f, 0);
            }
            else if (Map == 1)
            {
                Blue2.transform.position = new Vector3(-12.5f, -1.5f, 0);
            }
            Blue2statScript.isDead = false;
            Blue2statScript.PlayerHP = Blue2statScript.PlayerMaxHP;
            Blue2statScript.SR.enabled = true;
            Blue2statScript.MyCol.enabled = true;
            Blue2statScript.RB.simulated = true;
            Blue2statScript.RB.gravityScale = Blue2statScript.OriginalGravityScale;
            Blue2statScript.RB.velocity = Vector2.zero;
            Blue2statScript.canvas.SetActive(true);
            Blue2statScript.MoveLock = false;
            Blue2statScript.JumpLock = false;
            Blue2statScript.SkillLock = false;
            Blue2statScript.speed = Blue2statScript.originalSpeed;
            Blue2statScript.MoaiS2Pull = false;
            Blue2statScript.Aggroed = false;
            Blue2statScript.AN.SetBool(Blue2statScript.character_Base.deathID, false);
            if (Blue2statScript.BaronBuffOn)
            {
                Blue2statScript.BaronBuffAN.enabled = true;
                Blue2statScript.BaronBuffSR.enabled = true;
            }
        }
    }


    public void ResultPanel_Exit()
    {
        floatingJoystick.SetActive(false);
        ResultPanel.SetActive(false);
        VictoryImage.SetActive(false);
        DefeatImage.SetActive(false);
        CaptureMapRespawnPanel.SetActive(false);
        BaronTimer.enabled = false;

        OnExitRoom.Invoke();

        EndLobbyScript.gameObject.SetActive(true);
    }

    [PunRPC]
    public void EndLobbyInitRPC(int myPlayerNumber, int killCount, int deathCount, int totalDamage, int RankPointChanged)
    {
        string temp_string = RankPointChanged > 0 ? "(+" : "(";

        if (myPlayerNumber == 1)
        {
            EndLobbyScript.Blue1_Kill.text = killCount + "";
            EndLobbyScript.Blue1_Death.text = deathCount + "";
            EndLobbyScript.Blue1_TotalDamage.text = totalDamage + "";
            EndLobbyScript.Blue1_RankPoint.text = NetworkManager.instance.Blue1RankPoint + RankPointChanged + "";
            if (RankPointChanged > 0)
            {
                EndLobbyScript.Blue1_RankPointChanged.color = new Color(57 / 255f, 180/255f, 82/255f, 1f);
            }
            else
            {
                EndLobbyScript.Blue1_RankPointChanged.color = new Color(1f, 0f, 0f, 136/255f);
            }
            EndLobbyScript.Blue1_RankPointChanged.text = temp_string + RankPointChanged + ")";
        }
        else if (myPlayerNumber == 2)
        {
            EndLobbyScript.Red1_Kill.text = killCount + "";
            EndLobbyScript.Red1_Death.text = deathCount + "";
            EndLobbyScript.Red1_TotalDamage.text = totalDamage + "";
            EndLobbyScript.Red1_RankPoint.text = NetworkManager.instance.Red1RankPoint + RankPointChanged + "";
            if (RankPointChanged > 0)
            {
                EndLobbyScript.Red1_RankPointChanged.color = new Color(57 / 255f, 180 / 255f, 82 / 255f, 1f);
            }
            else
            {
                EndLobbyScript.Red1_RankPointChanged.color = new Color(1f, 0f, 0f, 136 / 255f);
            }
            EndLobbyScript.Red1_RankPointChanged.text = temp_string + RankPointChanged + ")";
        }
        else if (myPlayerNumber == 3)
        {
            EndLobbyScript.Red2_Kill.text = killCount + "";
            EndLobbyScript.Red2_Death.text = deathCount + "";
            EndLobbyScript.Red2_TotalDamage.text = totalDamage + "";
            EndLobbyScript.Red2_RankPoint.text = NetworkManager.instance.Red2RankPoint + RankPointChanged + "";
            if (RankPointChanged > 0)
            {
                EndLobbyScript.Red2_RankPointChanged.color = new Color(57 / 255f, 180 / 255f, 82 / 255f, 1f);
            }
            else
            {
                EndLobbyScript.Red2_RankPointChanged.color = new Color(1f, 0f, 0f, 136 / 255f);
            }
            EndLobbyScript.Red2_RankPointChanged.text = temp_string + RankPointChanged + ")";
        }
        else if (myPlayerNumber == 4)
        {
            EndLobbyScript.Blue2_Kill.text = killCount + "";
            EndLobbyScript.Blue2_Death.text = deathCount + "";
            EndLobbyScript.Blue2_TotalDamage.text = totalDamage + "";
            EndLobbyScript.Blue2_RankPoint.text = NetworkManager.instance.Blue2RankPoint + RankPointChanged + "";
            if (RankPointChanged > 0)
            {
                EndLobbyScript.Blue2_RankPointChanged.color = new Color(57 / 255f, 180 / 255f, 82 / 255f, 1f);
            }
            else
            {
                EndLobbyScript.Blue2_RankPointChanged.color = new Color(1f, 0f, 0f, 136 / 255f);
            }
            EndLobbyScript.Blue2_RankPointChanged.text = temp_string + RankPointChanged + ")";
        }
    }

    public void GameEndReset()
    {
        EndLobbyScript.gameObject.SetActive(false);

        if (Map == 0)
        {
            MagneticMap.SetActive(false);
        }
        else if (Map == 1)
        {
            CaptureMapPanel.SetActive(false);
            CaptureMap.SetActive(false);
            CaptureAreaScript.captureAreaColOnOff(false);
        }
        ItemBtnList.Clear();
        PhotonNetwork.LeaveRoom();
        System.GC.Collect();
        ReadyPlayers = 0;
        Blue1Col = null;
        Blue2Col = null;
        Red1Col = null;
        Red2Col = null;
        Blue1statScript = null;
        Blue2statScript = null;
        Red1statScript = null;
        Red2statScript = null;
        Blue1 = null;
        Blue2 = null;
        Red1 = null;
        Red2 = null;
        ReadyPlayers1Time = true;
        MagneticMapCMCamera.gameObject.SetActive(false);
        MageneticMapCMRange.SetActive(false);
        CaptureMapCMCamera.gameObject.SetActive(false);
        CaptureMapCMRange.SetActive(false);
        MainCamera.transform.position = new Vector3(6.5f, 2.8f, -10f);
        FirstArray[0] = 0f;
        SecondArray[0] = 0f;
        ThirdArray[0] = 0f;
        FourthArray[0] = 0f;
        RankPointReady = 0;
        slider.value = 0f;
        DisconnectPanel.SetActive(true);
        MainCamera.transform.position = new Vector3(0f, 1f, -10f);
        soundManagerScript.AS_Setting(Sound_Background.Main);
        AL.enabled = true;
        GameStarted = false;
    }

    //백그라운드에서 복귀 까지 걸린 시간
    [Header("PausedTime")]
    public float PausedTime;
    public float UnPausedTime;


    public void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            var now = DateTime.Now.ToLocalTime();
            var span = (now - new DateTime(2022, 1, 1, 0, 0, 0, 0).ToLocalTime());
            PausedTime = (float)span.TotalSeconds;
        }
        else
        {
            var now = DateTime.Now.ToLocalTime();
            var span = (now - new DateTime(2022, 1, 1, 0, 0, 0, 0).ToLocalTime());
            UnPausedTime = (float)span.TotalSeconds;
            StartCoroutine(ClearPausedTime());
        }
    }

    public IEnumerator ClearPausedTime()
    {
        yield return new WaitForSeconds(0.1f);
        PausedTime = 0f;
        UnPausedTime = 0f;
    }

    public float PausedDeltaTime()
    {
        return UnPausedTime - PausedTime;
    }
}
