using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraS2Script : MonoBehaviour
{
    public TaraScript taraScript;
    public StatScript statScript;
    TaraS2ObjectPool taraS2ObjectPool;

    public BoxCollider2D S2Col;
    public SpriteRenderer SR;
    public Animator AN;
    public bool isBlueTeam;
    public bool isEnabled;

    public bool enemy1OnS2;
    public bool enemy2OnS2;

    public int skill2ID = Animator.StringToHash("skill2");

    public float time;
    public bool return1Time;

    public AudioSource AS_Appear;
    public AudioSource AS_Skill2;


    public void Init()
    {
        taraScript = GameObject.Find("Tara(Clone)").GetComponent<TaraScript>();
        statScript = taraScript.statScript;
        isBlueTeam = statScript.BlueTeam;
        taraS2ObjectPool = taraScript.taraS2ObjectPool;
        transform.SetParent(taraS2ObjectPool.transform);
        taraS2ObjectPool.poolingObjectQueue.Enqueue(this);
    }

    [PunRPC]
    void InitRPC()
    {
        Init();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (statScript.PV.IsMine)
        {
            if (col.CompareTag("Player"))
            {
                StatScript colStatScript = col.GetComponent<StatScript>();
                if (statScript.enemyStatScript1 != null && statScript.enemyStatScript1 == colStatScript)
                {
                    if (!colStatScript.isInvincible)
                    {
                        if (!taraScript.Ulting)
                        {
                            taraScript.UltGauge += 10f;
                        }
                        colStatScript.GetDamage(statScript.ATK * 0.3f + 171f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                        NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)col.transform.position, 22);
                    }
                    enemy1OnS2 = true;
                    StartCoroutine(TaraS2Enemy1CRT());
                }
                else if (statScript.enemyStatScript2 != null && statScript.enemyStatScript2 == colStatScript)
                {
                    if (!colStatScript.isInvincible)
                    {
                        if (!taraScript.Ulting)
                        {
                            taraScript.UltGauge += 10f;
                        }
                        colStatScript.GetDamage(statScript.ATK * 0.3f + 171f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
                        NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)col.transform.position, 22);
                    }
                    enemy2OnS2 = true;
                    StartCoroutine(TaraS2Enemy2CRT());
                }
            }
        }
        else if (col.CompareTag("Projectile") && col.TryGetComponent<ProjectileScript>(out ProjectileScript projectileScript) && col.GetComponent<ProjectileScript>().isBlueTeam != statScript.BlueTeam && col.GetComponent<PhotonView>().IsMine)
        {
            col.GetComponent<PhotonView>().RPC("ReturnRPC", RpcTarget.All);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (statScript.PV.IsMine)
        {
            if (col.CompareTag("Player"))
            {
                StatScript colStatScript = col.GetComponent<StatScript>();
                if (statScript.enemyStatScript1 != null && statScript.enemyStatScript1 == colStatScript)
                {
                    enemy1OnS2 = false;
                }
                else if (statScript.enemyStatScript2 != null && statScript.enemyStatScript2 == colStatScript)
                {
                    enemy2OnS2 = false;
                }
            }
        }
    }

    IEnumerator TaraS2Enemy1CRT()
    {
        yield return new WaitForSeconds(0.3f);
        if (statScript.enemyStatScript1.isDead) enemy1OnS2 = false;
        if (enemy1OnS2)
        {
            if (!statScript.enemyStatScript1.isInvincible)
            {
                NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)statScript.enemyStatScript1.transform.position, 22);
                if (!taraScript.Ulting)
                {
                    taraScript.UltGauge += 10f;
                }
                statScript.enemyStatScript1.GetDamage(statScript.ATK * 0.3f + 171f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
            }
            StartCoroutine(TaraS2Enemy1CRT());
        }
    }

    IEnumerator TaraS2Enemy2CRT()
    {
        yield return new WaitForSeconds(0.3f);
        if (statScript.enemyStatScript2.isDead) enemy2OnS2 = false;
        if (enemy2OnS2)
        {
            if (!statScript.enemyStatScript2.isInvincible)
            {
                NetworkManager.instance.PV.RPC("SoundRPC", RpcTarget.All, (Vector2)statScript.enemyStatScript2.transform.position, 22);
                if (!taraScript.Ulting)
                {
                    taraScript.UltGauge += 10f;
                }
                statScript.enemyStatScript2.GetDamage(statScript.ATK * 0.3f + 171f, 0f, 0f, 0f, 0f, 0.1f, statScript.myPlayerNumber);
            }
            StartCoroutine(TaraS2Enemy2CRT());
        }
    }


    public IEnumerator ANSkill2CRT()
    {
        yield return new WaitForSeconds(0.4f);
        AS_Skill2.Play();
        AN.SetBool(skill2ID, true);
        yield return new WaitForSeconds(4.2f);
        AN.SetBool(skill2ID, false);
        AN.SetTrigger("disappear");
        yield return new WaitForSeconds(0.4f);
        AS_Skill2.Stop();
    }


    private void Update()
    {
        if (statScript != null)
        {
            if (return1Time && (NetworkManager.instance.PausedDeltaTime() + time) >= 5f)
            {
                return1Time = false;
                taraS2ObjectPool.ReturnObject(this);
            }
            else if (return1Time)
            {
                time += Time.deltaTime;
            }
        }
    }
}
