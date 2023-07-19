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

    int phase = 0;

    float cannonRotateParam;
    float maxHp;

    bool isDown;
    bool isMissile;
    bool shouldCannon;
    bool canTakeDamage = true;

    string prepCannonStr = "PrepCannon";
    string afterCannonStr = "AfterCannon";
    string recoverStr = "Recover";
    string blockStr = "Block";

    Animator animator;

    BossHealthBar bossHealthBar;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        bossHealthBar = GameManager.Instance.bossHealthBar;

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

    public void OnTakeDamage(int amount, Renderer[] renderers = null)
    {
        if (canTakeDamage)
        {
            float tempHp = hp;
            bool loc = false;
            hp -= amount;
            if (renderers != null)
            {
                StartCoroutine(IFlashMaterial(renderers));
            }
            
            if (tempHp >= maxHp * 0.75 && hp < maxHp * 0.75)
            {
                shouldCannon = true;
                loc = true;
                hp = maxHp * 0.749f;
                phase++;
                BossScene.Instance.OpenSecurity(1);
            }

            if (tempHp >= maxHp * 0.5 && hp < maxHp * 0.5)
            {
                shouldCannon = true;
                loc = true;
                hp = maxHp * 0.499f;
                phase++;
                BossScene.Instance.OpenSecurity(2);
            }

            if (tempHp >= maxHp * 0.25 && hp < maxHp * 0.25)
            {
                shouldCannon = true;
                loc = true;
                hp = maxHp * 0.249f;
                phase++;
            }

            bossHealthBar.FillHealthBar(hp / maxHp);
            if (loc)
            {
                LockHealthBar(true);
            }

            if (hp <= 0)
            {
                GameManager.Instance.updateEnemy(-1);
                Destroy(gameObject);
            }
            
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
        StartCoroutine(BossScene.Instance.IPutDownPlatform());
        isDown = true;
        animator.SetBool(afterCannonStr, true);
        yield return new WaitForSeconds(downTime);
        animator.SetBool(afterCannonStr, false);
        StartCoroutine(DoRecover());
    }

    IEnumerator DoRecover()
    {
        animator.SetBool(afterCannonStr, false);
        animator.SetBool(recoverStr, true);
        yield return new WaitForSeconds(3);
        isDown = false;
        shouldCannon = false;
        LockHealthBar(false);
        GameManager.Instance.bossHealthBar.Phase(phase);
        animator.SetBool(recoverStr, false);
    }

    public void Recover()
    {
        StopAllCoroutines();
        StartCoroutine(DoRecover());
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

    public int GetPhase()
    {
        return phase;
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

    public void LockHealthBar(bool shouldLock)
    {
        canTakeDamage = !shouldLock;
        GameManager.Instance.bossHealthBar.LockHealthBar(shouldLock);
    }
}
