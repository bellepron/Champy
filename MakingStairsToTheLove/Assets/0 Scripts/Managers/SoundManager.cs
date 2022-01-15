using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip fail;
    [SerializeField] AudioClip pop;
    [SerializeField] AudioClip coin;

    bool a;

    public void Success()
    {
        audioSource.PlayOneShot(success, 0.5f);
    }

    public void Fail()
    {
        audioSource.PlayOneShot(fail, 0.5f);
    }

    public void Pop()
    {
        if (a == false)
        {
            audioSource.PlayOneShot(pop, 0.5f);
            a = true;
            StartCoroutine(DontMultiply());
        }
    }
    IEnumerator DontMultiply()
    {
        yield return new WaitForEndOfFrame();
        a = false;
    }

    public void Coin()
    {
        audioSource.PlayOneShot(coin, 0.5f);
    }
}