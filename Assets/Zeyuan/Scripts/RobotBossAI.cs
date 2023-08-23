using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
    [SerializeField] GameObject boostEffects;
    [SerializeField] GameObject lavaWave;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject handDamageArea;
    [SerializeField] GameObject laserEffect;

    [SerializeField] Transform cannonFirePos;
    [SerializeField] Transform missileRightPos;
    [SerializeField] Transform missileLeftPos;
    [SerializeField] Transform slamPoint;

    [SerializeField] ParticleSystem cannonGroundSmoke;
    [SerializeField] ParticleSystem missileRightFX;
    [SerializeField] ParticleSystem missileLeftFX;
    [SerializeField] ParticleSystem lavaEffect;

    [SerializeField] Renderer[] boostArm;

    [SerializeField] AudioSource laserAudioSource;
    [SerializeField] AudioSource missileAudioSource;

    [SerializeField] AudioClip missileLaunchingSound;
    [SerializeField] AudioClip alarmSound;
    [SerializeField] AudioClip hit1Sound;
    [SerializeField] AudioClip hit2Sound;
    [SerializeField] AudioClip prepCannonSound;
    [SerializeField] AudioClip getDownSound;
    [SerializeField] AudioClip bossRecoverSound;
    [SerializeField] AudioClip bossShieldBreakSound;
    [SerializeField] AudioClip bossSlamSound;
    [SerializeField] AudioClip bossBostArmSound;

    int phase = 0;

    float cannonRotateParam;
    float maxHp;

    bool isDown;
    bool isMissile = true;
    bool shouldCannon;
    bool shouldSlam;
    bool isSlamFinished = true;
    bool canTakeDamage = true;
    public bool isEasyBoss;
    bool easyBossDown;

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

        StartCoroutine(IStartMissileDelay());

        foreach (Renderer renderer in boostArm)
        {
            if (renderer.material.IsKeywordEnabled("_EMISSION"))
            {
                renderer.material.DisableKeyword("_EMISSION");
            }
        }

        AudioManager.Instance.RegisterSFX(laserAudioSource);
        AudioManager.Instance.RegisterSFX(missileAudioSource);
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

            //temp codes
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                OnTakeDamage(10);
            }

            if (Missile.count > 0 && !missileAudioSource.isPlaying)
            {
                missileAudioSource.Play();
            }
            else if (Missile.count == 0 && missileAudioSource.isPlaying)
            {
                missileAudioSource.Stop();
            }
        }
    }

    public void OnTakeDamage(float amount, Renderer[] renderers = null)
    {
        if (canTakeDamage)
        {
            if (!isEasyBoss)
            {
                float tempHp = hp;
                bool loc = false;
                hp -= amount;
                if (renderers != null)
                {
                    StartCoroutine(IFlashMaterial(renderers));
                }

                if (tempHp >= maxHp * 0.83 && hp < maxHp * 0.83)
                {
                    shouldCannon = true;
                    loc = true;
                    hp = maxHp * 0.829f;
                    phase++;
                    BossScene.Instance.OpenSecurity(1);
                    audioSource.PlayOneShot(alarmSound, .4f);
                }

                if (tempHp >= maxHp * 0.67 && hp < maxHp * 0.67)
                {
                    shouldCannon = true;
                    loc = true;
                    hp = maxHp * 0.669f;
                    phase++;
                    BossScene.Instance.OpenSecurity(2);
                    audioSource.PlayOneShot(alarmSound, .4f);
                }

                if (tempHp >= maxHp * 0.5 && hp < maxHp * 0.5)
                {
                    if (shield.activeSelf)
                    {
                        shield.GetComponent<BossShield>().OnDisabled();
                        shield.SetActive(false);
                        audioSource.PlayOneShot(bossShieldBreakSound);
                    }
                    shouldCannon = true;
                    loc = true;
                    hp = maxHp * 0.499f;
                    phase++;
                    audioSource.PlayOneShot(alarmSound, .4f);
                }

                bossHealthBar.FillHealthBar(hp / maxHp);
                if (loc)
                {
                    LockHealthBar(true);
                }

                if (hp <= 0)
                {
                    GameManager.Instance.bossHealthBar.gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
            else
            {
                if (!easyBossDown)
                {
                    StopAllCoroutines();
                    animator.SetTrigger("EasyBoss");
                    StartCoroutine(DoAfterCannon());
                    hp = maxHp * 0.499f;
                    bossHealthBar.FillHealthBar(hp / maxHp);
                    LockHealthBar(true);
                    phase = 3;
                    easyBossDown = true;
                }
                else
                {
                    hp -= amount;
                    bossHealthBar.FillHealthBar(hp / maxHp);
                    if (hp <= 0)
                    {
                        GameManager.Instance.bossHealthBar.gameObject.SetActive(false);
                        Destroy(gameObject);
                    }
                }
                
            }
            if (audioSource.enabled == true)
            {
                audioSource.PlayOneShot(hit1Sound, .7f);
            }
        }
        else
        {
            audioSource.PlayOneShot(hit2Sound, .7f);
        }
    }

    IEnumerator DoPrepCannon()
    {
        if (CanPrepCannon())
        {
            animator.SetBool(prepCannonStr, true);
            audioSource.PlayOneShot(prepCannonSound, .8f);

            yield return new WaitForSeconds(3f);
            laserAudioSource.Play();
            laserEffect.SetActive(true);
            yield return new WaitForSeconds(cannonDelay);
            animator.enabled = false;
            isShooting = true;
            CameraShake();
            StartCoroutine(IBrightScreen());
            StartCoroutine(ILensDistortion());
            
            yield return new WaitForSeconds(.25f);
            StartCoroutine(ICameraShake(10f, .5f));
            yield return new WaitForSeconds(cannonLastTime);
            animator.enabled = true;
            animator.SetBool(prepCannonStr, false);
            isShooting = false;
            DisableLaser();

            StartCoroutine(DoAfterCannon());
        }
    }

    IEnumerator IBrightScreen()
    {
        PostProcessVolume postProcess = FindObjectOfType<PostProcessVolume>();
        AutoExposure exposure = postProcess.profile.GetSetting<AutoExposure>();
        float val = -20f;
        exposure.maxLuminance.value = val;
        while (val < 0f)
        {
            exposure.maxLuminance.value = val;
            val += Time.deltaTime * 20f;
            if (val > 0)
            {
                val = 0;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ILensDistortion()
    {
        PostProcessVolume postProcess = FindObjectOfType<PostProcessVolume>();
        LensDistortion len = postProcess.profile.GetSetting<LensDistortion>();
        float val = 70f;
        while (val > 0f)
        {
            len.intensity.value = val;
            val -= Time.deltaTime * 40f;
            if (val < 0)
            {
                val = 0;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator DoAfterCannon()
    {
        StartCoroutine(BossScene.Instance.IPutDownPlatform());
        isDown = true;
        animator.SetBool(afterCannonStr, true);
        audioSource.PlayOneShot(getDownSound, .8f);

        foreach (Renderer renderer in boostArm)
        {
            renderer.material.DisableKeyword("_EMISSION");
            boostEffects.SetActive(false);
        }

        yield return new WaitForSeconds(downTime);
        animator.SetBool(afterCannonStr, false);
        StartCoroutine(DoRecover());
    }

    IEnumerator DoRecover()
    {
        animator.SetBool(afterCannonStr, false);
        animator.SetBool(recoverStr, true);
        audioSource.PlayOneShot(bossRecoverSound, .8f);
        if (phase == 2)
        {
            shield.SetActive(true);
        }
        yield return new WaitForSeconds(3);
        if (phase == 2)
        {
            animator.Play("RobotBoss_BoostArm");
            audioSource.PlayOneShot(bossBostArmSound, .8f);
            yield return new WaitForSeconds(6f);
            isDown = false;
            shouldCannon = false;
        }
        isDown = false;
        shouldCannon = false;
        LockHealthBar(false);
        GameManager.Instance.bossHealthBar.Phase(phase);
        animator.SetBool(recoverStr, false);
    }

    IEnumerator DoSlam()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("RobotBoss_Slam"))
        {
            audioSource.PlayOneShot(bossSlamSound);
            shouldSlam = false;
            isSlamFinished = false;
            animator.Play("RobotBoss_Slam");
            yield return new WaitForSeconds(10f);
            isSlamFinished = true;
            if (!shouldCannon)
            {
                shouldSlam = true;
            }
            else
            {
                handDamageArea.SetActive(true);
            }
        }
    }

    public void BoostArm()
    {
        handDamageArea.SetActive(false);
        foreach (Renderer renderer in boostArm)
        {
            //renderer.material.color = Color.red;
            if (!renderer.material.IsKeywordEnabled("_EMISSION"))
            {
                renderer.material.EnableKeyword("_EMISSION");
            }
            boostEffects.SetActive(true);
        }
    }

    public void PlayLavaEffect()
    {
        lavaEffect.Play();
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
            //float y = Mathf.Sin(cannonRotateParam) * Mathf.PI / 180 * cannonRotateRange;
            //cannonRotateParam += (Time.deltaTime * cannonRotateSpeed);
            //Vector3 rot = new Vector3(0, y, 0);
            //cannon.transform.Rotate(rot, Space.World);
            cannonRotateParam += Time.deltaTime * cannonRotateSpeed;
            float y = Mathf.Sin(cannonRotateParam) * Time.deltaTime * cannonRotateRange;
            Vector3 rot = new Vector3(0, y, 0);
            cannon.transform.rotation = Quaternion.Euler(cannon.transform.rotation.eulerAngles + rot);
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
        laserEffect.SetActive(false);
        laser.SetActive(false);
    }

    void InCombat()
    {
        if (shouldCannon && !shouldSlam && isSlamFinished)
        {
            StartCoroutine(DoPrepCannon());
        }
        else if (shouldSlam)
        {
            StartCoroutine(DoSlam());
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

    public void InstantiateLavaWave()
    {
        Instantiate(lavaWave, slamPoint.transform.position, slamPoint.transform.rotation);
    }

    public void ShouldSlam()
    {
        shouldSlam = true;
    }

    //this should be in cameramanager, but it should be fine for now.
    public void CameraShake()
    {
        StartCoroutine(ICameraShake());
    }

    IEnumerator ICameraShake(float timer = 0.25f, float amount = 3f)
    {
        UnityEngine.Camera cam = UnityEngine.Camera.main;
        while (timer > 0)
        {
            cam.transform.localRotation = Quaternion.Euler(cam.transform.rotation.x, 0, cam.transform.rotation.z + UnityEngine.Random.Range(-amount, amount));
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cam.transform.localRotation = Quaternion.Euler(cam.transform.rotation.x, 0, 0);


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
                audioSource.PlayOneShot(missileLaunchingSound, .5f);
                yield return new WaitForSeconds(missileLaunchRate);
            }
            yield return new WaitForSeconds(missileLaunchCD);
            isMissile = false;
        }
        
    }

    IEnumerator IStartMissileDelay()
    {
        float timer = UnityEngine.Random.Range(3f, 5f);
        yield return new WaitForSeconds(timer);
        isMissile = false;
    }

    public void LockHealthBar(bool shouldLock)
    {
        canTakeDamage = !shouldLock;
        GameManager.Instance.bossHealthBar.LockHealthBar(shouldLock);
    }

    public void DisableAudio()
    {
        audioSource.volume = 0;
    }

    protected override void OnDestroy()
    {
        if (AudioManager.Instance != null)
        {
            base.OnDestroy();
            AudioManager.Instance.UnregisterSFX(laserAudioSource);
            AudioManager.Instance.UnregisterSFX(missileAudioSource);
        }
    }
}
