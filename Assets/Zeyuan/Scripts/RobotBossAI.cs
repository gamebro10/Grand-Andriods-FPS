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
    [SerializeField] float cannonUVSpeed;
    [SerializeField] float downTime;
    [SerializeField] float missileLaunchCount;
    [SerializeField] float missileLaunchRate;
    [SerializeField] float missileLaunchCD;

    [SerializeField] GameObject head;
    [SerializeField] GameObject cannon;
    [SerializeField] GameObject laser;
    [SerializeField] GameObject blockade;
    [SerializeField] GameObject walkUp;
    [SerializeField] GameObject missile;

    [SerializeField] Transform cannonFirePos;
    [SerializeField] Transform missileRightPos;
    [SerializeField] Transform missileLeftPos;

    [SerializeField] ParticleSystem cannonGroundSmoke;
    [SerializeField] ParticleSystem missileRightFX;
    [SerializeField] ParticleSystem missileLeftFX;

    float cannonRotateParam;
    float maxHp;

    bool isDown;
    bool isMissile;
    bool shouldCannon;

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

        maxHp = hp;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            base.Update();
            if (!isDown)
            {
                FaceToPlayer();
            }
            
            InCombat();
        }
    }

    public void OnTakeDamage(int amount, Renderer[] renderers)
    {
        int tempHp = hp;
        hp -= amount;
        StartCoroutine(IFlashMaterial(renderers));

        if (tempHp >= maxHp * 0.8 && hp < maxHp * 0.8)
        {
            shouldCannon = true;
        }

        if (hp <= 0)
        {
            GameManager.Instance.updateEnemy(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator DoPrepCannon()
    {
        if (CanPrepCannon())
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
        isDown = true;
        animator.SetBool(afterCannonStr, true);
        yield return new WaitForSeconds(downTime);
        animator.SetBool(afterCannonStr, false);
        StartCoroutine(DoRecover());
    }

    IEnumerator DoRecover()
    {
        animator.SetBool(recoverStr, true);
        yield return new WaitForSeconds(3);
        isDown = false;
        shouldCannon = false;
        animator.SetBool(recoverStr, false);
    }
    
    public IEnumerator DoBlock()
    {
        if (CanPrepCannon())
        {
            animator.SetBool(blockStr, true);
            yield return new WaitForSeconds(1);
            animator.SetBool(blockStr, false);
        }
    }

    bool CanPrepCannon()
    {
        return !animator.GetBool(prepCannonStr) && !animator.GetBool(afterCannonStr) && 
            !animator.GetBool(recoverStr) && !animator.GetBool(blockStr);
    }

    void ShootCannon()
    {
        if (isShooting)
        {
            EnableLaser();
            Material mt = laser.GetComponent<Renderer>().material;
            mt.mainTextureOffset = new Vector2(mt.mainTextureOffset.x, mt.mainTextureOffset.y - Time.deltaTime * cannonUVSpeed);
            float y = Mathf.Sin(cannonRotateParam) * Mathf.PI / 180 * cannonRotateRange;
            cannonRotateParam += Time.deltaTime * cannonRotateSpeed;
            Vector3 rot = new Vector3(0, y, 0);
            cannon.transform.Rotate(rot, Space.World);
        }
    }

    void EnableLaser()
    {
        if (!laser.gameObject.activeSelf)
        {
            if (!cannonGroundSmoke.isPlaying)
            {
                cannonGroundSmoke.Play();
            }
            Material mt = laser.GetComponent<Renderer>().material;
            mt.mainTextureOffset = new Vector2(0, 0);
            laser.gameObject.SetActive(true);
        }
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
        if (GetAngleToPlayer() <= 20 && shouldCannon)
        {
            StartCoroutine(DoPrepCannon());
        }
        else if(!isMissile && !shouldCannon)
        {
            StartCoroutine(IFireMissile());
        }
        ShootCannon();
    }

    public void EnableWalkUp()
    {
        blockade.SetActive(false);
        walkUp.SetActive(true);
    }

    public void DisableWalkUp()
    {
        walkUp.SetActive(false);
        blockade.SetActive(true);
    }

    IEnumerator IFireMissile()
    {
        if (!isMissile)
        {
            isMissile = true;
            for (int i = 0; i < missileLaunchCount * 2; i++)
            {
                if (i < missileLaunchCount)
                {
                    Instantiate(missile, missileRightPos.position, missileRightPos.rotation);
                    missileRightFX.Play();
                }
                else
                {
                    Instantiate(missile, missileLeftPos.position, missileLeftPos.rotation);
                    missileLeftFX.Play();
                }
                yield return new WaitForSeconds(missileLaunchRate);
            }
            yield return new WaitForSeconds(missileLaunchCD);
            isMissile = false;
        }
        
    }
}
