using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectBounce : MonoBehaviour
{
    [SerializeField] AudioSource bounceEffect;
    [SerializeField] AudioClip bounceClip;

    void Start()
    {
        AudioManager.Instance.RegisterSFX(bounceEffect);
    }

    private void OnCollisionEnter(Collision collision)
    {
         if (collision.gameObject.CompareTag("Player") )
         {
            bounceEffect.PlayOneShot(bounceClip);
         }
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UnregisterSFX(bounceEffect);
        }
    }
}
