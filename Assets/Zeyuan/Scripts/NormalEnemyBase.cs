using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyBase : EnemyBase
{
    [SerializeField] protected AudioClip shootSound;
    [SerializeField] protected AudioClip dyingSound;
    [SerializeField] protected AudioClip hit1Sound;
    [SerializeField] protected AudioClip hit2Sound;

    protected override void Start()
    {
        base.Start();
        audioSource.pitch = Random.Range(.9f, 1f);
    }

    public override void OnTakeDamage(int amount)
    {
        base.OnTakeDamage(amount);

        int altHitSound = Random.Range(1, 3);
        switch (altHitSound)
        {
            case 1:
                audioSource.PlayOneShot(hit1Sound, 3f);
                break;
            case 2:
                audioSource.PlayOneShot(hit2Sound, 3f);
                break;
            default:
                break;
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        audioSource.PlayOneShot(dyingSound, 7f);
        StartCoroutine(ISmoothTurnOffSound());
    }

    IEnumerator ISmoothTurnOffSound()
    {
        while (true)
        {
            audioSource.volume -= Time.deltaTime * .1f;
            yield return new WaitForEndOfFrame();
        }
    }
}
