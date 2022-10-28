using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TaraS1Script : GetDamageScript
{
    public TaraScript taraScript;
    public StatScript statScript;
    TaraS1ObjectPool taraS1ObjectPool;
    public TaraS1AttackRangeScript taraS1AttackRangeScript;
    public TaraS1AttackObjectPool taraS1AttackObjectPool;

    public BoxCollider2D S1Col;
    public bool isEnabled;

    public bool isHpBarColorChagned;


    public void Init()
    {
        taraScript = GameObject.Find("Tara(Clone)").GetComponent<TaraScript>();
        statScript = taraScript.statScript;
        BlueTeam = statScript.BlueTeam;
        taraS1ObjectPool = taraScript.taraS1ObjectPool;
        taraS1ObjectPool.poolingObjectQueue.Enqueue(this);
        transform.SetParent(taraS1ObjectPool.transform);
        taraS1AttackObjectPool = taraScript.taraS1AttackObjectPool;
    }

    [PunRPC]
    void InitRPC()
    {
        Init();
    }

    public override void DeathCheck(int attackerPlayerNumber)
    {
        if (PlayerHP <= 0f)
        {
            if (isEnabled)
            {
                isEnabled = false;
                if (statScript.PV.IsMine)
                {
                    taraS1AttackRangeScript.gameObject.SetActive(false);
                    taraS1AttackRangeScript.EnemyStatScriptArrClear();
                }
                StartCoroutine(DisappearCRT());
                NetworkManager.instance.soundObjectPool.GetObject((Vector2)transform.position, 18);
            }
        }
    }


    [PunRPC]
    void ANReloadRPC()
    {
        if (!AN.GetBool("disappear") && !AN.GetBool("shoot"))
        {
            NetworkManager.instance.soundObjectPool.GetObject((Vector2)transform.position, 20);
            AN.SetBool("appear", false);
            AN.SetBool("shoot", false);
            AN.SetBool("reload", true);
        }
    }

    //[PunRPC]
    //void AttackRPC(Vector2 enemyVector2)
    //{
    //    //sound
    //    NetworkManager.instance.soundObjectPool.GetObject((Vector2)transform.position, 19);
    //
    //    if (enemyVector2.x < transform.position.x)
    //    {
    //        SR.flipX = true;
    //    }
    //    else
    //    {
    //        SR.flipX = false;
    //    }
    //
    //    if (!AN.GetBool("disappear"))
    //    {
    //        AN.SetBool("appear", false);
    //        AN.SetBool("reload", false);
    //        AN.SetBool("reload2", false);
    //        AN.SetBool("shoot", true);
    //    }
    //    float rotationZ;
    //    if (Aggroed)
    //    {
    //        Vector2 tempVector2 = AggroedVector - (Vector2)transform.position;
    //        rotationZ = Mathf.Rad2Deg * Mathf.Atan2(tempVector2.y, tempVector2.x);
    //    }
    //    else
    //    {
    //        rotationZ = Mathf.Rad2Deg * Mathf.Atan2(enemyVector2.y, enemyVector2.x);
    //    }
    //    StartCoroutine(AttackCRT(rotationZ));
    //}

    public void Attack(Vector2 enemyVector2)
    {
        if (enemyVector2.x < transform.position.x)
        {
            SR.flipX = true;
        }
        else
        {
            SR.flipX = false;
        }

        PV.RPC("AttackANRPC", RpcTarget.All, SR.flipX);

        float rotationZ;
        if (Aggroed)
        {
            Vector2 tempVector2 = AggroedVector - (Vector2)transform.position;
            rotationZ = Mathf.Rad2Deg * Mathf.Atan2(tempVector2.y, tempVector2.x);
        }
        else
        {
            rotationZ = Mathf.Rad2Deg * Mathf.Atan2(enemyVector2.y, enemyVector2.x);
        }
        StartCoroutine(AttackCRT(rotationZ));
    }

    [PunRPC]
    public void AttackANRPC(bool flipX)
    {
        NetworkManager.instance.soundObjectPool.GetObject((Vector2)transform.position, 19);
        SR.flipX = flipX;
        if (!AN.GetBool("disappear"))
        {
            AN.SetBool("appear", false);
            AN.SetBool("reload", false);
            AN.SetBool("reload2", false);
            AN.SetBool("shoot", true);
        }
    }

    public IEnumerator AttackCRT(float rotationZ)
    {
        yield return new WaitForSeconds(0.2667f);
        PV.RPC("AttackRPC", RpcTarget.All, transform.position.x, transform.position.y, BlueTeam, rotationZ);
    }

    [PunRPC]
    public void AttackRPC(float x, float y, bool BlueTeam,float rotationZ)
    {
        taraS1AttackObjectPool.GetObject(x, y, BlueTeam, rotationZ);
        taraS1AttackObjectPool.GetObject(x, y, BlueTeam, rotationZ + 20f);
        taraS1AttackObjectPool.GetObject(x, y, BlueTeam, rotationZ - 20f);
    }

    [PunRPC]
    void ReturnRPC()
    {
        if (isEnabled)
        {
            isEnabled = false;
            NetworkManager.instance.soundObjectPool.GetObject((Vector2)transform.position, 18);
            if (statScript.PV.IsMine)
            {
                taraS1AttackRangeScript.gameObject.SetActive(false);
                taraS1AttackRangeScript.EnemyStatScriptArrClear();
            }
            StartCoroutine(DisappearCRT());
        }
    }

    public IEnumerator DisappearCRT()
    {
        AN.SetBool("appear", false);
        AN.SetBool("reload", false);
        AN.SetBool("reload2", false);
        AN.SetBool("shoot", false);
        AN.SetBool("disappear", true);

        canvas.SetActive(false);
        S1Col.enabled = false;

        yield return new WaitForSeconds(0.6f);

        taraS1ObjectPool.ReturnObject(this);
    }


    public void ANAppearEnd()
    {
        if (AN.GetBool("appear"))
        {
            AN.SetBool("appear", false);
        }
    }

    public void ANShootEnd()
    {
        if (AN.GetBool("shoot"))
        {
            AN.SetBool("shoot", false);

            if (!AN.GetBool("disappear"))
            {
                AN.SetBool("reload2", true);
            }
        }
    }

    public void ANReload2End()
    {
        if (AN.GetBool("reload2"))
        {
            AN.SetBool("reload2", false);
        }
    }

    public void ANDisappearEnd()
    {
        if (AN.GetBool("disappear"))
        {
            AN.SetBool("disappear", false);
        }
    }


    //HpBar색 관련
    public void HPBarColorChange()
    {
        isHpBarColorChagned = true;
        if (NetworkManager.instance.MyStatScript.BlueTeam == BlueTeam)
        {
            hpBar_fillImgae.color = new Color(64f / 255f, 147f / 255f, 245f / 255f);
        }
        else
        {
            hpBar_fillImgae.color = new Color(214f / 255f, 53f / 255f, 52f / 255f);
        }
    }
}