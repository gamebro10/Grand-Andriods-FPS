using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectBounce : MonoBehaviour
{
    [SerializeField] AudioSource bounceEffect;
    [SerializeField] AudioClip bounceClip;

    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
         if (collision.gameObject.CompareTag("Player") )
         {
            bounceEffect.PlayOneShot(bounceClip);
         }
    }
}
