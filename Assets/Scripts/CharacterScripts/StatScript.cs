using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class StatScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;
    public Collider2D MyCol;
    public BoxCollider2D MyBoxCol;
    public Character_Base character_Base;
    public GameObject canvas;
    public AudioListener AL;

    public HPBar hpBar;
    public Image hpBar_fillImgae;
    public float PlayerHP;
    public float PlayerMaxHP;
    public float PlayerTotalShield;    //현재 Player가 가지고있는 Shield의 통합량

    public float speed;
    public float originalSpeed;
    public List<SpeedScript> SpeedList_ADD = new List<SpeedScript>();
    public List<SpeedScript> SpeedList_SUB = new List<SpeedScript>();
    public List<SpeedScript> SpeedList_MUL = new List<SpeedScript>();
    public List<SpeedScript> SpeedList_DIV = new List<SpeedScript>();
    public void Speed_Set()
    {
        speed = originalSpeed;
        if (SpeedList_ADD.Count > 0)
        {
            for (int i = 0; i < SpeedList_ADD.Count; i++)
            {
                speed += SpeedList_ADD[i].speed;
            }
        }
        if (SpeedList_SUB.Count > 0)
        {
            for (int i = 0; i < SpeedList_SUB.Count; i++)
            {
                speed -= SpeedList_SUB[i].speed;
            }
        }
        if (SpeedList_MUL.Count > 0)
        {
            for (int i = 0; i < SpeedList_MUL.Count; i++)
            {
                speed *= SpeedList_MUL[i].speed;
            }
        }
        if (SpeedList_DIV.Count > 0)
        {
            for (int i = 0; i < SpeedList_DIV.Count; i++)
            {
                speed /= SpeedList_DIV[i].speed;
            }
        }
    }
    public void Speed_ADD(int operatorKind, float time, float speed)//시간용
    {
        if (operatorKind == 0)//더하기
        {
            SpeedList_ADD.Add(new SpeedScript(time, speed));
        }
        else if (operatorKind == 1)//빼기
        {
            SpeedList_SUB.Add(new SpeedScript(time, speed));
        }
        else if (operatorKind == 2)//곱하기
        {
            SpeedList_MUL.Add(new SpeedScript(time, speed));
        }
        else if (operatorKind == 3)//나누기
        {
            SpeedList_DIV.Add(new SpeedScript(time, speed));
        }
        Speed_Set();
    }
    public void Speed_ADD(int operatorKind, SpeedScript speedScript)//onoff용, off하고 싶을때 time에 -1넣으면 됨.
    {
        if (operatorKind == 0)//더하기
        {
            SpeedList_ADD.Add(speedScript);
        }
        else if (operatorKind == 1)//빼기
        {
            SpeedList_SUB.Add(speedScript);
        }
        else if (operatorKind == 2)//곱하기
        {
            SpeedList_MUL.Add(speedScript);
        }
        else if (operatorKind == 3)//나누기
        {
            SpeedList_DIV.Add(speedScript);
        }
        Speed_Set();
    }


    public int myPlayerNumber;

    [Header("Shield")]
    public Queue<ShieldScript> ShieldQue = new Queue<ShieldScript>();
    public ShieldScript tempShieldScript;
    public ShieldScript TempShieldScript;
    public bool ShieldRemain;

    [Header("GetDamage")]
    public bool isInvincible;
    public bool isDead;

    public Text NickNameText;
    public FloatingJoystick floatingJoystick;
    public Rigidbody2D RB;
    public Animator AN;
    public Vector3 curPos;
    public SpriteRenderer SR;
    public Material OriginalMaterial;
    public Material FlashWhiteMaterial;
    public bool BlueTeam;
    public StatScript teamStatScript;
    public StatScript enemyStatScript1;
    public StatScript enemyStatScript2;

    [Header("Hit")]
    public bool ArcherHurricaneHit;

    public bool BladerAttack1Hit;
    public bool BladerS1Hit;
    public bool BladerS1Aura = true;
    public float BladerS1AuraCount = 10f;
    public Image BladerS1AuraImage;
    public bool DontHitByProjectile;

    public bool MoaiAttackHit;
    public bool MoaiS1Hit;
    public bool MoaiS2Hit;
    public bool MoaiS2Pull;
    public bool MoaiS2End;

    public bool LuxS1Hit;
    public bool LuxUltHit;

    public bool LinkerAttackHit;
    public bool LinkerS1Connected;
    public bool LinkerUltHit;

    public bool TaraAttackHit;
    public bool TaraUlt1Hit;

    public bool MagneticCircleHit;

    public LinkerS1CircleScript linkerS1CircleScript;

    public bool dontChangeGravity;

    [Header("Stat")]
    public float DEF;
    public float ATK;
    public float FixATK;
    public float PEN;
    public float PerATK;
    public float traitATK;
    public float itemATK;
    public float DamageReduction;

    public int JobCode;//1Archer , 2Blader , 3Moai , 4Lux , 5Linker , 6Tara
    public ArcherScript archerScript;
    public BladerScript bladerScript;
    public MoaiScript moaiScript;
    public LuxScript luxScript;
    public LinkerScript linkerScript;
    public TaraScript taraScript;

    [Header("Item")]
    public bool Zonya;
    public float zonyaCool = 30f;
    public bool OnZonya;
    public bool SunFire;
    public bool OnSunFire;
    public GameObject SunFireImage;
    public SpriteRenderer SunFireImage_SR;
    public GameObject SunFireEffect;
    public CircleCollider2D SunFireCol;
    public bool shoesATK;

    [Header("Character State")]
    public bool isJumping;
    public bool isLuxS2ing;

    public bool MoveLock;
    public bool JumpLock;
    public bool SkillLock;
    public bool StunMoveLock;
    public bool StunJumpLock;
    public bool StunSkillLock;

    public float OriginalGravityScale;
    public bool Aggroed;
    public Vector2 AggroedVector;
    public bool noHurtMotion;
    public SpriteRenderer stunSR;

    [Header("Map")]
    public int Map;
    public bool isMagneticMap;
    public float RespawnTimer;
    public bool RespawnTimerBool;
    public GameObject RespawnPanel;//인스펙터 비움
    public Text RespawnTimerText;//인스펙터 비움

    [Header("Bush")]
    public bool OnBush;
    public bool DontChangeByOutBush;

    [Header("BaronBuff")]
    public SpriteRenderer BaronBuffSR;
    public Animator BaronBuffAN;
    public bool BaronBuffOn;

    

    public IEnumerator BaronBuffCRT()
    {
        BaronBuffOn = true;
        if (!isDead)
        {
            BaronBuffAN.enabled = true;
            BaronBuffSR.enabled = true;
        }
        yield return new WaitForSeconds(15f);
        BaronBuffOn = false;
        BaronBuffAN.enabled = false;
        BaronBuffSR.enabled = false;
    }

    [Header("Option")]
    public bool imuneStun;
    public bool imuneStiff;

    //옵션이 켜져있으면 효과받음
    public bool ArcherHurricaneOption;
    public bool BladerS1AuraOption;
    public bool MoaiS1Option;
    public bool MoaiS2Option;
    public bool LinkerS1Option;

    [Header("BtnOption")]
    public bool CanUltOnAir;

    [Header("Motion")]
    public float stiffTime;
    public float stunTime;
    public float hurtColorTime;
    public bool hurtColorChange1Time;

    public bool KillLog1Time;

    public int bushState;//0 부쉬밖, 1 왼쪽부쉬속, 2 오른쪽부쉬속


    [PunRPC]
    public void InitRPC()
    {
        Init();
    }

    public void Init()
    {
        Map = NetworkManager.instance.Map;
        MapCheck();

        if (PV.IsMine)
        {
            NetworkManager.instance.MyStatScript = this;

            if (Map == 0)
            {
                NetworkManager.instance.MagneticMapCMCamera.Follow = transform;
                NetworkManager.instance.MagneticMapCMCamera.LookAt = transform;
            }
            else if (Map == 1)
            {
                NetworkManager.instance.CaptureMapCMCamera.Follow = transform;
                NetworkManager.instance.CaptureMapCMCamera.LookAt = transform;
            }

            if (NetworkManager.instance.myPlayerNumber == 1)
            {
                PV.RPC("Blue1Col", RpcTarget.All);
                NetworkManager.instance.CaptureAreaScript.SetPointColor(true);
            }
            else if (NetworkManager.instance.myPlayerNumber == 2)
            {
                PV.RPC("Red1Col", RpcTarget.All);
                NetworkManager.instance.CaptureAreaScript.SetPointColor(false);
            }
            else if (NetworkManager.instance.myPlayerNumber == 3)
            {
                PV.RPC("Red2Col", RpcTarget.All);
                NetworkManager.instance.CaptureAreaScript.SetPointColor(false);
            }
            else if (NetworkManager.instance.myPlayerNumber == 4)
            {
                PV.RPC("Blue2Col", RpcTarget.All);
                NetworkManager.instance.CaptureAreaScript.SetPointColor(true);
            }

            NetworkManager.instance.AL.enabled = false;
            AL.enabled = true;
        }
    }

    [PunRPC]
    public void Blue1Col()
    {
        BlueTeam = true;
        NetworkManager.instance.Blue1Col = MyCol;
        NetworkManager.instance.Blue1 = gameObject;
        NetworkManager.instance.Blue1statScript = this;
        NetworkManager.instance.ReadyPlayers++;
        myPlayerNumber = 1;
        NickNameText.text = PV.Owner.NickName;

        if (PV.IsMine)
        {
            NetworkManager.instance.ApplyItemBool();
            PV.RPC("SetItems", RpcTarget.All, Zonya, SunFire, shoesATK);
        }
        else
        {
            if (NetworkManager.instance.myPlayerNumber == 4)
            {
                NickNameText.color = Color.white;
                hpBar_fillImgae.color = new Color(64f / 255f, 147f / 255f, 245f / 255f);
            }
            else
            {
                NickNameText.color = Color.white;
                hpBar_fillImgae.color = new Color(214f / 255f, 53f / 255f, 52f / 255f);
            }
        }
    }

    [PunRPC]
    public void Blue2Col()
    {
        BlueTeam = true;
        NetworkManager.instance.Blue2Col = MyCol;
        NetworkManager.instance.Blue2 = gameObject;
        NetworkManager.instance.Blue2statScript = this;
        NetworkManager.instance.ReadyPlayers++;
        myPlayerNumber = 4;
        NickNameText.text = PV.Owner.NickName;

        if (PV.IsMine)
        {
            NetworkManager.instance.ApplyItemBool();
            PV.RPC("SetItems", RpcTarget.All, Zonya, SunFire, shoesATK);
        }
        else
        {
            if (NetworkManager.instance.myPlayerNumber == 1)
            {
                NickNameText.color = Color.white;
                hpBar_fillImgae.color = new Color(64f / 255f, 147f / 255f, 245f / 255f);
            }
            else
            {
                NickNameText.color = Color.white;
                hpBar_fillImgae.color = new Color(214f / 255f, 53f / 255f, 52f / 255f);
            }
        }
    }

    [PunRPC]
    public void Red1Col()
    {
        BlueTeam = false;
        NetworkManager.instance.Red1Col = MyCol;
        NetworkManager.instance.Red1 = gameObject;
        NetworkManager.instance.Red1statScript = this;
        NetworkManager.instance.ReadyPlayers++;
        myPlayerNumber = 2;
        NickNameText.text = PV.Owner.NickName;
        SR.flipX = true;

        if (PV.IsMine)
        {
            NetworkManager.instance.ApplyItemBool();
            PV.RPC("SetItems", RpcTarget.All, Zonya, SunFire, shoesATK);
        }
        else
        {
            if (NetworkManager.instance.myPlayerNumber == 3)
            {
                NickNameText.color = Color.white;
                hpBar_fillImgae.color = new Color(64f / 255f, 147f / 255f, 245f / 255f);
            }
            else
            {
                NickNameText.color = Color.white;
                hpBar_fillImgae.color = new Color(214f / 255f, 53f / 255f, 52f / 255f);
            }
        }
    }

    [PunRPC]
    public void Red2Col()
    {
        BlueTeam = false;
        NetworkManager.instance.Red2Col = MyCol;
        NetworkManager.instance.Red2 = gameObject;
        NetworkManager.instance.Red2statScript = this;
        NetworkManager.instance.ReadyPlayers++;
        myPlayerNumber = 3;
        NickNameText.text = PV.Owner.NickName;
        SR.flipX = true;

        if (PV.IsMine)
        {
            NetworkManager.instance.ApplyItemBool();
            PV.RPC("SetItems", RpcTarget.All, Zonya, SunFire, shoesATK);
        }
        else
        {
            if (NetworkManager.instance.myPlayerNumber == 2)
            {
                NickNameText.color = Color.white;
                hpBar_fillImgae.color = new Color(64f / 255f, 147f / 255f, 245f / 255f);
            }
            else
            {
                NickNameText.color = Color.white;
                hpBar_fillImgae.color = new Color(214f / 255f, 53f / 255f, 52f / 255f);
            }
        }
    }

    [PunRPC]
    public void SetItems(bool Zonya, bool SunFire, bool shoesATK)
    {
        if (!PV.IsMine)
        {
            this.Zonya = Zonya;
            this.SunFire = SunFire;
            this.shoesATK = shoesATK;
            //Item_Apply();
        }
        if (PV.IsMine)
        {
            NetworkManager.instance.PV.RPC("SliderPlusRPC", RpcTarget.All);
            NetworkManager.instance.ItemSetDone1Time = true;
        }

        if (SunFire)
        {
            SunFireImage.SetActive(true);
        }
    }
    //item
    public void Item_Apply()
    {
        if (shoesATK)
        {
            Speed_ADD(0, new SpeedScript(0f, 0.1f));
            ATK += 20f;
        }
    }

    public void MapCheck()
    {
        if (Map == 0)
        {
            isMagneticMap = true;
        }
    }

    public void SetStatScripts()
    {
        if (myPlayerNumber == 1)
        {
            if (NetworkManager.instance.Blue2statScript != null)
                teamStatScript = NetworkManager.instance.Blue2statScript;
            if (NetworkManager.instance.Red1statScript != null)
                enemyStatScript1 = NetworkManager.instance.Red1statScript;
            if (NetworkManager.instance.Red2statScript != null)
                enemyStatScript2 = NetworkManager.instance.Red2statScript;
        }
        else if (myPlayerNumber == 2)
        {
            if (NetworkManager.instance.Red2statScript != null)
                teamStatScript = NetworkManager.instance.Red2statScript;
            if (NetworkManager.instance.Blue1statScript != null)
                enemyStatScript1 = NetworkManager.instance.Blue1statScript;
            if (NetworkManager.instance.Blue2statScript != null)
                enemyStatScript2 = NetworkManager.instance.Blue2statScript;
        }
        else if (myPlayerNumber == 3)
        {
            if (NetworkManager.instance.Red1statScript != null)
                teamStatScript = NetworkManager.instance.Red1statScript;
            if (NetworkManager.instance.Blue1statScript != null)
                enemyStatScript1 = NetworkManager.instance.Blue1statScript;
            if (NetworkManager.instance.Blue2statScript != null)
                enemyStatScript2 = NetworkManager.instance.Blue2statScript;
        }
        else if (myPlayerNumber == 4)
        {
            if (NetworkManager.instance.Blue1statScript != null)
                teamStatScript = NetworkManager.instance.Blue1statScript;
            if (NetworkManager.instance.Red1statScript != null)
                enemyStatScript1 = NetworkManager.instance.Red1statScript;
            if (NetworkManager.instance.Red2statScript != null)
                enemyStatScript2 = NetworkManager.instance.Red2statScript;
        }
    }

    public void LinkerS1CircleInit()
    {
        if (myPlayerNumber == 1 && NetworkManager.instance.Blue2statScript != null)
        {
            linkerS1CircleScript.teamStatScript = teamStatScript;
            linkerS1CircleScript.linkerS1LRScript.Player2 = teamStatScript.gameObject;
        }
        else if (myPlayerNumber == 2 && NetworkManager.instance.Red2statScript != null)
        {
            linkerS1CircleScript.teamStatScript = teamStatScript;
            linkerS1CircleScript.linkerS1LRScript.Player2 = teamStatScript.gameObject;
        }
        else if (myPlayerNumber == 3 && NetworkManager.instance.Red1statScript != null)
        {
            linkerS1CircleScript.teamStatScript = teamStatScript;
            linkerS1CircleScript.linkerS1LRScript.Player2 = teamStatScript.gameObject;
        }
        else if (myPlayerNumber == 4 && NetworkManager.instance.Blue1statScript != null)
        {
            linkerS1CircleScript.teamStatScript = teamStatScript;
            linkerS1CircleScript.linkerS1LRScript.Player2 = teamStatScript.gameObject;
        }
    }


    void Awake()
    {
        if (PV.IsMine)
        {
            floatingJoystick = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();
        }
    }

    void Start()
    {
        OriginalGravityScale = RB.gravityScale;
        originalSpeed = speed;
        zonyaCool = 30f;
        KillLog1Time = true;
    }

    void Update()
    {
        hpBar.setHp(PlayerHP, PlayerMaxHP, PlayerTotalShield);

        if (PV.IsMine)
        {
            /*
            if (isMagneticMap && !MagneticCircleHit && MagneticCircle.IsOutsideCircle_Static(transform.position))
            {
                MagneticCircleHit = true;
                StartCoroutine(MagneticCircleCRT());
                GetDamage(0f, 100f, 0f, 0f, 0f, 0f,);
            }
            */

            float axis = 0;

            if (!MoveLock && !StunMoveLock && !OnZonya)
            {
                if (floatingJoystick.Horizontal > 0)
                {
                    axis = 1;
                }
                else if (floatingJoystick.Horizontal < 0)
                {
                    axis = -1;
                }
                RB.velocity = new Vector2(4 * axis * speed, RB.velocity.y);
            }

            if (axis != 0)
            {
                if (!Aggroed)
                {
                    if (character_Base.isGround)
                    {
                        character_Base.ANWalk();
                    }
                    else
                    {
                        character_Base.ANWalkFalse();
                    }
                    SR.flipX = axis < 0;
                }
                else
                {
                    if (AggroedVector.x < transform.position.x)
                    {
                        SR.flipX = true;
                    }
                    else
                    {
                        SR.flipX = false;
                    }
                    if (SR.flipX == (axis < 0))
                    {
                        if (character_Base.isGround)
                        {
                            character_Base.ANWalk();
                        }
                        else
                        {
                            character_Base.ANWalkFalse();
                        }
                    }
                    else
                    {
                        //나중에 뒷걸음질 애니메이션 넣기
                        if (character_Base.isGround)
                        {
                            character_Base.ANWalk();
                        }
                        else
                        {
                            character_Base.ANWalkFalse();
                        }
                    }

                }
            }
            else character_Base.ANWalkFalse();
        }
        //IsMine이 아닌 것들은 부드럽게 위치 동기화
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);


        //맞을 때 색 변화
        if (!OnZonya)
        {
            if (hurtColorTime > 0)
            {
                SR.color = new Color(1f, 0f, 0f, SR.color.a);
                hurtColorTime += -Time.deltaTime;
                hurtColorChange1Time = true;
            }
            else if (hurtColorChange1Time)
            {
                hurtColorChange1Time = false;
                SR.color = new Color(1f, 1f, 1f, SR.color.a);
            }
        }

        //bush에서 투명하게
        //if (OnBush)
        //{
        //    if (NetworkManager.instance.MyStatScript.BlueTeam == BlueTeam) SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 0.5f);
        //    else SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 0f);
        //}
        else if (!DontChangeByOutBush)
        {
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 1f);
        }

        if (!isDead)
        {
            character_Base.HurtMotion();
        }



        if (!BladerS1Aura)
        {
            if (BladerS1AuraCount > 0f)
            {
                BladerS1AuraCount += -Time.deltaTime;
            }
            else
            {
                BladerS1Aura = true;
                BladerS1AuraImage.enabled = true;
                BladerS1AuraCount = 10f;
            }
        }

        float tempShield = 0;

        if (ShieldQue.Count > 0)
        {
            ShieldScript[] tempArray = new ShieldScript[10];
            ShieldQue.CopyTo(tempArray, 0);
            ShieldQue.Clear();

            for (int i = 0; i < tempArray.Length; i++)
            {
                if (tempArray[i] == null)
                {
                    break;
                }

                for (int k = i + 1; k < tempArray.Length; k++)
                {
                    if (tempArray[k] == null)
                    {
                        break;
                    }
                    if (tempArray[i].duration > tempArray[k].duration)
                    {
                        tempShieldScript = tempArray[i];
                        tempArray[i] = tempArray[k];
                        tempArray[k] = tempShieldScript;
                    }
                }
            }
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (tempArray[i] == null)
                {
                    break;
                }
                tempArray[i].duration -= Time.deltaTime;
            }
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (tempArray[i] == null)
                {
                    break;
                }
                tempShield += tempArray[i].shield;

                if (tempArray[i].duration > 0f)
                {
                    ShieldQue.Enqueue(tempArray[i]);
                }
            }
        }
        PlayerTotalShield = tempShield;

        if (ArcherHurricaneHit && !forceMovement_Portal)
        {
            transform.position = NetworkManager.instance.tempHurricaneScript.transform.position;
        }

        if (MoaiS2Pull)
        {
            RB.gravityScale = 6f;
        }

        if (RespawnTimerBool)
        {
            RespawnTimerText.text = "" + Mathf.FloorToInt(RespawnTimer);
            RespawnTimer -= Time.deltaTime;
            if (RespawnTimer < 0f)
            {
                NetworkManager.instance.RespawnMyCharacter();
                RespawnPanel.SetActive(false);
                RespawnTimerBool = false;
                MoveLock = false;
                JumpLock = false;
                SkillLock = false;
                speed = originalSpeed;
                RB.gravityScale = OriginalGravityScale;
            }
        }

        #region speed
        //speed 관련 부분
        bool speedSet = false;
        if (SpeedList_ADD.Count > 0)
        {
            for (int i = 0; i < SpeedList_ADD.Count; i++)
            {
                if (!SpeedList_ADD[i].isOnOff)
                {
                    SpeedList_ADD[i].time -= Time.deltaTime;
                    if (SpeedList_ADD[i].time < 0f)
                    {
                        SpeedList_ADD.RemoveAt(i);
                        speedSet = true;
                    }
                }
            }
        }
        if (SpeedList_SUB.Count > 0)
        {
            for (int i = 0; i < SpeedList_SUB.Count; i++)
            {
                if (!SpeedList_SUB[i].isOnOff)
                {
                    SpeedList_SUB[i].time -= Time.deltaTime;
                    if (SpeedList_SUB[i].time < 0f)
                    {
                        SpeedList_SUB.RemoveAt(i);
                        speedSet = true;
                    }
                }
            }
        }
        if (SpeedList_MUL.Count > 0)
        {
            for (int i = 0; i < SpeedList_MUL.Count; i++)
            {
                if (!SpeedList_MUL[i].isOnOff)
                {
                    SpeedList_MUL[i].time -= Time.deltaTime;
                    if (SpeedList_MUL[i].time < 0f)
                    {
                        SpeedList_MUL.RemoveAt(i);
                        speedSet = true;
                    }
                }
            }
        }
        if (SpeedList_DIV.Count > 0)
        {
            for (int i = 0; i < SpeedList_DIV.Count; i++)
            {
                if (!SpeedList_DIV[i].isOnOff)
                {
                    SpeedList_DIV[i].time -= Time.deltaTime;
                    if (SpeedList_DIV[i].time < 0f)
                    {
                        SpeedList_DIV.RemoveAt(i);
                        speedSet = true;
                    }
                }
            }
        }
        if (speedSet) Speed_Set();
        #endregion
    }

    public IEnumerator InvincibleCRT()//리스폰무적3초
    {
        isInvincible = true;
        for (int i = 0; i < 7; i++)
        {
            SR.material = FlashWhiteMaterial;
            yield return new WaitForSeconds(0.2f);
            SR.material = OriginalMaterial;
            yield return new WaitForSeconds(0.2f);
        }
        SR.material = FlashWhiteMaterial;
        yield return new WaitForSeconds(0.2f);
        SR.material = OriginalMaterial;
        isInvincible = false;
    }





    public void GetDamage(float ATK, float FixATK, float PerDamage, float PEN, float PerPEN, float StiffTime, int attackerPlayerNumber)
    {
        if (LinkerS1Connected)
        {
            teamStatScript.LinkerS1ConnectedGetDamage(ATK, FixATK, PerDamage, PEN, PerPEN, StiffTime, attackerPlayerNumber);
        }

        if (StiffTime > 0f)
        {
            PV.RPC("StiffRPC", RpcTarget.All, StiffTime);
        }
        float tempDEF = DEF * (1f - PerPEN * 0.01f) - PEN;
        if (tempDEF < 0f)
        {
            tempDEF = 0f;
        }


        float overDamage = 0f;
        float totalDamage = (100f / (tempDEF + 100f) * ATK + FixATK + PlayerHP * PerDamage * 0.01f) * (1f - DamageReduction);//계산 한번만 할겸, 총 입힌 피해량 계산도 할겸 넣음.2022.08.26
        NetworkManager.instance.myTotalDamageInThisGame += totalDamage;

        while (true)
        {
            if (ShieldQue.Count < 1)
            {
                ShieldRemain = false;
                break;
            }

            TempShieldScript = ShieldQue.Dequeue();

            if (overDamage > 0f)
            {
                TempShieldScript.shield -= overDamage;
            }
            else
            {
                TempShieldScript.shield -= totalDamage;//(100f / (tempDEF + 100f) * ATK + FixATK + PlayerHP * PerDamage * 0.01f) * (1f - DamageReduction);
            }

            if (TempShieldScript.shield < 0f)
            {
                overDamage -= TempShieldScript.shield;
            }
            else if (TempShieldScript.shield > 0f)
            {
                ShieldQue.Enqueue(TempShieldScript);
                overDamage = 0f;
                ShieldRemain = true;
                break;
            }
            else if (TempShieldScript.shield == 0f)
            {
                overDamage = 0f;
                ShieldRemain = true;
                break;
            }
        }


        if (overDamage > 0f)
        {
            PlayerHP -= overDamage;
        }
        else if (!ShieldRemain)
        {
            PlayerHP -= totalDamage;//(100f / (tempDEF + 100f) * ATK + FixATK + PlayerHP * PerDamage * 0.01f) * (1f - DamageReduction);
        }
        PV.RPC("ShieldChangeRPC", RpcTarget.All, ShieldQueForRPC());
        PV.RPC("PlayerHPChangeRPC", RpcTarget.All, PlayerHP, attackerPlayerNumber);
    }



    public void LinkerS1ConnectedGetDamage(float ATK, float FixATK, float PerDamage, float PEN, float PerPEN, float StiffTime, int attackerPlayerNumber)
    {
        if (StiffTime > 0f)
        {
            PV.RPC("StiffRPC", RpcTarget.All, StiffTime);
        }
        float tempDEF = DEF * (1f - PerPEN * 0.01f) - PEN;
        if (tempDEF < 0f)
        {
            tempDEF = 0f;
        }


        float overDamage = 0f;

        while (true)
        {
            if (ShieldQue.Count < 1)
            {
                ShieldRemain = false;
                break;
            }

            TempShieldScript = ShieldQue.Dequeue();

            if (overDamage > 0f)
            {
                TempShieldScript.shield -= overDamage;
            }
            else
            {
                TempShieldScript.shield -= (100f / (tempDEF + 100f) * ATK + FixATK + PlayerHP * PerDamage * 0.01f) * (1f - DamageReduction);
            }

            if (TempShieldScript.shield < 0f)
            {
                overDamage -= TempShieldScript.shield;
            }
            else if (TempShieldScript.shield > 0f)
            {
                ShieldQue.Enqueue(TempShieldScript);
                overDamage = 0f;
                ShieldRemain = true;
                break;
            }
            else if (TempShieldScript.shield == 0f)
            {
                overDamage = 0f;
                ShieldRemain = true;
                break;
            }
        }

        if (overDamage > 0f)
        {
            PlayerHP -= overDamage;
        }
        else if (!ShieldRemain)
        {
            PlayerHP -= (100f / (tempDEF + 100f) * ATK + FixATK + PlayerHP * PerDamage * 0.01f) * (1f - DamageReduction);
        }
        PV.RPC("ShieldChangeRPC", RpcTarget.All, ShieldQueForRPC());
        PV.RPC("PlayerHPChangeRPC", RpcTarget.All, PlayerHP, attackerPlayerNumber);
    }

    

    public void DeathCheck(int attackerPlayerNumber)
    {
        if (PlayerHP <= 0f)
        {
            //킬로그 띄우기 넣기
            if (KillLog1Time)
            {
                KillLog1Time = false;
                NetworkManager.instance.killLogManager.ShowKillLog(attackerPlayerNumber, myPlayerNumber);
                StartCoroutine(KillLog1TimeCRT());
            }
            isDead = true;
            canvas.SetActive(false);
            stunSR.enabled = false;
            RB.gravityScale = 0f;
            RB.velocity = Vector2.zero;
            RB.simulated = false;
            MyCol.enabled = false;
            stiffTime = 0f;
            stunTime = 0f;

            character_Base.Death();

            if (SunFire)
            {
                SunFireImage.SetActive(false);
            }

            if (PV.IsMine)
            {
                if (Map == 1)
                {
                    RespawnPanel = NetworkManager.instance.CaptureMapRespawnPanel;
                    RespawnTimerText = NetworkManager.instance.CaptureMapRespawnText;
                    RespawnPanel.SetActive(true);
                    NetworkManager.instance.ControlUI.SetActive(false);
                    RespawnTimer = 15f;
                    RespawnTimerBool = true;
                }
                else
                {
                    NetworkManager.instance.RespawnPanel.SetActive(true);
                    NetworkManager.instance.ControlUI.SetActive(false);
                }
            }
        }
    }

    public IEnumerator KillLog1TimeCRT()
    {
        yield return new WaitForSeconds(5f);
        KillLog1Time = true;
    }


    public float[] ShieldQueForRPC()
    {
        float[] shieldArray = new float[20];
        ShieldScript[] ShieldQueArray = new ShieldScript[10];
        ShieldQue.CopyTo(ShieldQueArray, 0);
        for (int i = 0; i < 10; i++)
        {
            if (ShieldQueArray[i] == null)
            {
                break;
            }
            shieldArray[i * 2] = ShieldQueArray[i].duration;
            shieldArray[i * 2 + 1] = ShieldQueArray[i].shield;
        }
        return shieldArray;
    }

    [PunRPC]
    public void ShieldChangeRPC(float[] value)
    {
        ShieldQue.Clear();
        for (int i = 0; i < 10; i++)
        {
            if (value[i * 2] == 0 && value[i * 2 + 1] == 0)
            {
                break;
            }
            ShieldQue.Enqueue(new ShieldScript(value[i * 2], value[i * 2 + 1]));
        }
    }

    public float SumHPShield()
    {
        return PlayerHP + PlayerTotalShield;
    }





    [PunRPC]
    public void MoveLockRPC(float value)
    {
        StartCoroutine(MoveLockCRT(value));
    }

    public IEnumerator MoveLockCRT(float value)
    {
        JumpLock = true;
        MoveLock = true;
        SkillLock = true;
        if (!dontChangeGravity && !OnZonya)
        {
            RB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        RB.velocity = new Vector2(0.0f, 0.0f);
        yield return new WaitForSeconds(value);
        if (!dontChangeGravity && !OnZonya)
        {
            RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        RB.velocity = new Vector2(0.0f, 0.0000001f);
        JumpLock = false;
        MoveLock = false;
        SkillLock = false;
    }



    [PunRPC]
    public void DontChangeGravityRPC(float value)
    {
        StartCoroutine(DontChangeGravityCRT(value));
    }

    public IEnumerator DontChangeGravityCRT(float value)
    {
        dontChangeGravity = true;
        yield return new WaitForSeconds(value);
        dontChangeGravity = false;
    }


    [PunRPC]
    public void StunRPC(float time)
    {
        StartCoroutine(Stun(time));
    }

    public IEnumerator Stun(float time)
    {
        hurtColorTime = 0.3f;
        if (!isDead)
        {
            ANStun(time);
            StunSkillLock = true;
            StunJumpLock = true;
            StunMoveLock = true;
            if (!dontChangeGravity && !OnZonya)
            {
                RB.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            RB.velocity = new Vector2(0.0f, 0.0f);
            yield return new WaitForSeconds(time);
            if (!dontChangeGravity && !OnZonya)
            {
                RB.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            RB.velocity = new Vector2(0.0f, 0.0000001f);
            StunSkillLock = false;
            StunJumpLock = false;
            StunMoveLock = false;
        }
    }

    public void ANStun(float value)
    {
        if (!imuneStun)
        {
            if (stunTime < value)
            {
                stunTime = value;
            }
        }
    }

    [PunRPC]
    public void ImuneStunRPC(bool value)
    {
        imuneStun = value;
    }


    [PunRPC]
    public void StiffRPC(float time)
    {
        StartCoroutine(Stiff(time));
    }

    public IEnumerator Stiff(float time)
    {
        if (!imuneStiff)
        {
            ANHurt(time);
            SkillLock = true;
            JumpLock = true;
            MoveLock = true;
            if (!dontChangeGravity)
            {
                RB.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            RB.velocity = new Vector2(0.0f, 0.0f);
            yield return new WaitForSeconds(time);
            if (!dontChangeGravity)
            {
                RB.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            RB.velocity = new Vector2(0.0f, 0.0000001f);
            SkillLock = false;
            JumpLock = false;
            MoveLock = false;
        }
    }

    [PunRPC]
    public void ImuneStiffRPC(bool value)
    {
        imuneStiff = value;
    }


    [PunRPC]
    public void ANHurtRPC(float value)
    {
        ANHurt(value);
    }

    public void ANHurt(float value)
    {
        hurtColorTime = 0.3f;
        if (!noHurtMotion)
        {
            if (stiffTime < value)
            {
                stiffTime = value;
            }
        }
    }



    [PunRPC]
    public void NoHurtMotionRPC(bool value)
    {
        noHurtMotion = value;
    }


    [PunRPC]
    public void DontHitByArrowRPC(bool value)
    {
        DontHitByProjectile = value;
    }

    [PunRPC]
    public void AggroedRPC(float x, float y)
    {
        AggroedVector = new Vector2(x, y);
        Aggroed = true;
    }

    [PunRPC]
    public void NotAggroedRPC()
    {
        Aggroed = false;
    }


    public IEnumerator For1HitByBladerAttack()
    {
        BladerAttack1Hit = true;
        yield return new WaitForSeconds(0.08f);
        BladerAttack1Hit = false;
    }

    public IEnumerator For1HitByBladerS1()
    {
        BladerS1Hit = true;
        yield return new WaitForSeconds(0.05f);
        BladerS1Hit = false;
    }


    public IEnumerator For1HitByMoaiAttack()
    {
        MoaiAttackHit = true;
        yield return new WaitForSeconds(0.08f);
        MoaiAttackHit = false;
    }


    public IEnumerator For1HitByMoaiS1()
    {
        MoaiS1Hit = true;
        yield return new WaitForSeconds(0.3f);
        MoaiS1Hit = false;
    }

    [PunRPC]
    public void MoaiSkill1RPC()
    {
        if (MoaiS1Option)
        {
            StartCoroutine(MoaiSkill1());
            ANHurt(1.1f);
        }
    }

    public IEnumerator MoaiSkill1()
    {
        SkillLock = true;
        JumpLock = true;
        MoveLock = true;
        RB.gravityScale = OriginalGravityScale;
        RB.velocity = new Vector2(0f, 13f);
        yield return new WaitForSeconds(1.1f);
        SkillLock = false;
        JumpLock = false;
        MoveLock = false;
    }


    public IEnumerator For1HitByMoaiS2()
    {
        dontChangeGravity = true;
        MoaiS2Hit = true;
        yield return new WaitForSeconds(0.15f);
        MoaiS2Hit = false;
        dontChangeGravity = false;
    }

    public void HitByMoaiS2(Vector3 MoaiPosition)
    {
        if (MoaiS2Option)
        {
            Vector2 PullVector = (MoaiPosition - transform.position) * 150f;
            PV.RPC("MoaiS2PullOnRPC", RpcTarget.All, PullVector);
            PV.RPC("AggroedRPC", RpcTarget.All, MoaiPosition.x, MoaiPosition.y);
        }
    }

    public SpeedScript MoaiS2SpeedScript;

    [PunRPC]
    public void MoaiS2PullOnRPC(Vector2 PullVector)
    {
        RB.AddForce(PullVector);
        MoaiS2SpeedScript = new SpeedScript(4f, 0.5f);
        Speed_ADD(2, MoaiS2SpeedScript);
        MoaiS2Pull = true;
    }

    [PunRPC]
    public void MoaiS2PullOffRPC()
    {
        MoaiS2Pull = false;
        Aggroed = false;
        if (RB.gravityScale > 3f)
        {
            RB.gravityScale = OriginalGravityScale;
        }
        MoaiS2SpeedScript.time = -1f;
    }

    public IEnumerator For1HitByMoaiS2End()
    {
        MoaiS2End = true;
        yield return new WaitForSeconds(0.2f);
        MoaiS2End = false;
    }

    [PunRPC]
    public void MoaiS2OptionRPC(bool value)
    {
        MoaiS2Option = value;
    }


    public IEnumerator For1HitByLuxS1()
    {
        LuxS1Hit = true;
        yield return new WaitForSeconds(0.08f);
        LuxS1Hit = false;
    }

    public IEnumerator For1HitByLuxUlt()
    {
        LuxUltHit = true;
        yield return new WaitForSeconds(0.08f);
        LuxUltHit = false;
    }


    public IEnumerator For1HitByLinkerAttack()
    {
        LinkerAttackHit = true;
        yield return new WaitForSeconds(0.08f);
        LinkerAttackHit = false;
    }

    public IEnumerator For1HitByLinkerUlt()
    {
        LinkerUltHit = true;
        yield return new WaitForSeconds(0.12f);
        LinkerUltHit = false;
    }


    public IEnumerator For1HitByTaraAttack()
    {
        TaraAttackHit = true;
        yield return new WaitForSeconds(0.08f);
        TaraAttackHit = false;
    }

    public IEnumerator For1HitByTaraUlt()
    {
        TaraUlt1Hit = true;
        yield return new WaitForSeconds(10f);
        TaraUlt1Hit = false;
    }


    [PunRPC]
    public void ZonyaRPC()
    {
        StartCoroutine(ZonyaCRT());
    }

    public IEnumerator ZonyaCRT()
    {
        NetworkManager.instance.soundObjectPool.GetObject((Vector2)transform.position, 27);
        AN.StartPlayback();
        dontChangeGravity = true;
        OnZonya = true;
        MoveLock = true;
        SkillLock = true;
        JumpLock = true;
        RB.constraints = RigidbodyConstraints2D.FreezeAll;
        RB.velocity = Vector2.zero;
        SR.color = new Color(0.9296875f, 1f, 0f, SR.color.a);
        MyCol.enabled = false;
        yield return new WaitForSeconds(2.5f);
        RB.velocity = Vector2.down * 0.000001f;
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        AN.StopPlayback();
        MyCol.enabled = true;
        SR.color = new Color(1f, 1f, 1f, SR.color.a);
        RB.gravityScale = OriginalGravityScale;
        MoveLock = false;
        SkillLock = false;
        JumpLock = false;
        OnZonya = false;
        dontChangeGravity = false;
    }

    public Coroutine tempSunFireCRT;

    public IEnumerator SunFireCRT(int attackerPlayerNumber)
    {
        OnSunFire = true;
        while (OnSunFire)
        {
            yield return new WaitForSeconds(2f);
            if (OnSunFire && !isInvincible)
            {
                PV.RPC("SunFireEffectRPC", RpcTarget.All, attackerPlayerNumber);
            }
        }
    }

    [PunRPC]
    public void SunFireEffectRPC(int attackerPlayerNumber)
    {
        StartCoroutine(SunFireEffectCRT());
        if (PV.IsMine)
        {
            GetDamage(0f, 30f, 0f, 0f, 0f, 0f, attackerPlayerNumber);
        }
    }

    public IEnumerator SunFireEffectCRT()
    {
        NetworkManager.instance.soundObjectPool.GetObject(transform.position, 26);
        SunFireEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SunFireEffect.SetActive(false);
    }

    public IEnumerator MagneticCircleCRT()
    {
        yield return new WaitForSeconds(1f);
        MagneticCircleHit = false;
    }



    [PunRPC]
    public void PlayerHPChangeRPC(float PlayerHP, int attackerPlayerNumber)
    {
        this.PlayerHP = PlayerHP;
        DeathCheck(attackerPlayerNumber);
    }

    [PunRPC]
    public void FlipXRPC(float axis) => SR.flipX = axis < 0;





    [PunRPC]
    public void DamageReductionRPC(float value)
    {
        DamageReduction += value;
        if (DamageReduction >= 1f)
        {
            DamageReduction = 1f;
        }
        else if (DamageReduction <= 0f)
        {
            DamageReduction = 0f;
        }
    }


    [PunRPC]
    public void LinkS1ConnectedRPC(bool value)
    {
        LinkerS1Connected = value;

        if (value)
        {
            linkerS1CircleScript.linkerS1EyeEffectScript.StartAN();
            NetworkManager.instance.soundObjectPool.GetObject((Vector2)transform.position, 36);
        }
        else
        {
            linkerS1CircleScript.linkerS1EyeEffectScript.EndAN();
            NetworkManager.instance.soundObjectPool.GetObject((Vector2)transform.position, 37);
        }
    }





    [PunRPC]
    public void OnHurricaneRPC()
    {
        if (ArcherHurricaneOption)
        {
            RB.gravityScale = 0f;
            ArcherHurricaneHit = true;
            MoveLock = true;
            JumpLock = true;
        }
    }

    [PunRPC]
    public void OffHurricaneRPC()
    {
        if (ArcherHurricaneOption)
        {
            RB.velocity = Vector2.zero;
            RB.gravityScale = OriginalGravityScale;
            ArcherHurricaneHit = false;
            MoveLock = false;
            JumpLock = false;
        }
    }


    [PunRPC]
    public void OnLuxBuff()
    {
        StartCoroutine(DEFCRT(2f, 15f));
        ShieldQue.Enqueue(new ShieldScript(2f, 50f));
        StartCoroutine(ATKCRT(2f, 15f));
    }

    public IEnumerator DEFCRT(float time, float DEF)
    {
        this.DEF += DEF;
        yield return new WaitForSeconds(time);
        this.DEF -= DEF;
    }

    public IEnumerator ATKCRT(float time, float ATK)
    {
        this.ATK += ATK;
        yield return new WaitForSeconds(time);
        this.ATK -= ATK;
    }

    [PunRPC]
    public void GravityRPC(float value)
    {
        RB.gravityScale = value;
    }

    //포탈이동
    [PunRPC]
    public void Portal(Vector2 value)
    {
        forceMovement_Portal = true;
        StartCoroutine(PortalCRT());
        transform.position = value + new Vector2(0f, MyBoxCol.size.y / 2 + 0.05f);
    }

    public bool forceMovement_Portal;

    public IEnumerator PortalCRT()
    {
        yield return new WaitForSeconds(0.2f);
        forceMovement_Portal = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(SR.flipX);
        }
        else if (stream.IsReading)
        {
            curPos = (Vector3)stream.ReceiveNext();
            SR.flipX = (bool)stream.ReceiveNext();
        }
    }
}
