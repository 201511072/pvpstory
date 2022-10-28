using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionLightScript : MonoBehaviour
{
    public Animator AN;
    public SpriteRenderer SR;
    public PortalScript portalScript;

    public void Init(PortalScript portalScript)
    {
        this.portalScript = portalScript;
        transform.SetParent(portalScript.transform);
        portalScript.poolingObjectQueue.Enqueue(this);
    }

    public IEnumerator ReturnCRT()
    {
        yield return new WaitForSeconds(0.33333f);
        portalScript.ReturnObject(this);
    }
}
