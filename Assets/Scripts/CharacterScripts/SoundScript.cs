using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    public AudioSource AS;
    public SoundObjectPool soundObjectPool;

    public void Init(SoundObjectPool soundObjectPool)
    {
        this.soundObjectPool = soundObjectPool;
        transform.SetParent(soundObjectPool.transform);
        soundObjectPool.poolingObjectQueue.Enqueue(this);
    }
}
