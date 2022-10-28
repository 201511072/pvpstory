using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkerS1EyeEffectScript : MonoBehaviour
{
    public SpriteRenderer SR;
    public Animator AN;

    public void StartAN()
    {
        gameObject.SetActive(true);
        AN.SetBool("end", false);
        AN.SetBool("start", true);
    }

    public void EndAN()
    {
        AN.SetBool("start", false);
        AN.SetBool("end", true);
        StartCoroutine(EndANCRT());
    }

    public IEnumerator EndANCRT()
    {
        yield return new WaitForSeconds(0.6f);
        gameObject.SetActive(false);
        AN.SetBool("end", false);
    }
}
