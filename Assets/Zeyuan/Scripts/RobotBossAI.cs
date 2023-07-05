using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBossAI : EnemyBase
{
    [SerializeField] float cannonRotateSpeed;
    [SerializeField] float cannonRotateRange;
    [SerializeField] float cannonLastTime;
    [SerializeField] float cannonDelay;

    [SerializeField] GameObject head;
    [SerializeField] GameObject cannon;
    [SerializeField] GameObject laser;

    [SerializeField] Transform cannonFirePos;
    [SerializeField] Transform cannonGroundSmokePos;

    [SerializeField] ParticleSystem cannonGroundSmoke;

    float cannonRotateParam;

    string prepCannonStr = "PrepCannon";
    string afterCannonStr = "AfterCannon";
    string recoverStr = "Recover";
    string blockStr = "Block";

    Animator animator;
    AnimationClip animationClip;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        //cannonGroundSmoke.Stop();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        InCombat();
        
    }

    IEnumerator DoPrepCannon()
    {
        if (CanDoAction())
        {
            animator.SetBool(prepCannonStr, true);
            yield return new WaitForSeconds(3 + cannonDelay);
            animator.enabled = false;
            isShooting = true;
            yield return new WaitForSeconds(cannonLastTime);
            animator.enabled = true;
            animator.SetBool(prepCannonStr, false);
            isShooting = false;
            DisableLaser();

            StartCoroutine(DoAfterCannon());
        }
    }

    IEnumerator DoAfterCannon()
    {
        animator.SetBool(afterCannonStr, true);
        yield return new WaitForSeconds(8);
        animator.SetBool(afterCannonStr, false);
        StartCoroutine(DoRecover());
    }

    IEnumerator DoRecover()
    {
        animator.SetBool(recoverStr, true);
        yield return new WaitForSeconds(3);
        animator.SetBool(recoverStr, false);
    }
    
    public IEnumerator DoBlock()
    {
        if (CanDoAction())
        {
            animator.SetBool(blockStr, true);
            yield return new WaitForSeconds(1);
            animator.SetBool(blockStr, false);
        }
    }

    bool CanDoAction()
    {
        return !animator.GetBool(prepCannonStr) && !animator.GetBool(afterCannonStr) && 
            !animator.GetBool(recoverStr) && !animator.GetBool(blockStr);
    }

    void ShootCannon()
    {
        if (isShooting)
        {
            EnableLaser();
            cannonRotateParam = cannonRotateParam > 180 ? 0 : cannonRotateParam;
            float y = Mathf.Cos(cannonRotateParam) * Mathf.PI / 180 * cannonRotateSpeed;
            cannonRotateParam += Time.deltaTime * cannonRotateRange;
            Vector3 rot = new Vector3(0, y, 0);
            cannon.transform.Rotate(rot, Space.World);
        }
    }

    void EnableLaser()
    {
        if (!cannonGroundSmoke.isPlaying)
        {
            cannonGroundSmoke.Play();
        }
        laser.gameObject.SetActive(true);
        cannonGroundSmoke.transform.position = cannonGroundSmokePos.position;
    }

    void DisableLaser()
    {
        if (cannonGroundSmoke.isPlaying)
        {
            cannonGroundSmoke.Stop();
        }
        cannonRotateParam = 0;
        laser.SetActive(false);
    }

    void InCombat()
    {
        StartCoroutine(DoPrepCannon());
        ShootCannon();
    }
}
