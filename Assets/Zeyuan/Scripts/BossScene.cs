using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BossScene : MonoBehaviour
{
    [SerializeField] float platformMoveSpeed;
    [SerializeField] float ropeScaleSpeed;
    [SerializeField] float headRotateSpeed;
    [SerializeField] int roofLaserDamage;
    [SerializeField] int maxSnipers;
    [SerializeField] int maxSoldiers;
    [SerializeField] GameObject craneHead;
    [SerializeField] GameObject platform;
    [SerializeField] GameObject rope;
    [SerializeField] GameObject sniperPrefab;
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] GameObject dronePrefab;
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    [SerializeField] GameObject laser;
    [SerializeField] GameObject laserEffect;
    [SerializeField] GameObject damageArea;
    [SerializeField] GameObject camBlackBar;
    [SerializeField] GameObject thumbsUp;
    [SerializeField] GameObject memberList;
    [SerializeField] GameObject credits;
    [SerializeField] Transform enemyParent;
    [SerializeField] Transform platformStopTopY;
    [SerializeField] Transform platformStopUpperY;
    [SerializeField] Transform platformStopLowerY;
    [SerializeField] Transform[] sniperSpawnPoints;
    [SerializeField] AudioSource platformStartAudioSource;
    [SerializeField] AudioSource platformStopAudioSource;
    [SerializeField] AudioSource platformAudioSource2;

    RobotBossAI robotBossAI;
    Vector3 platformLastPos;

    public bool playerOnPlatform;
    public StompButton stompButton;

    public static BossScene Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.bossHealthBar.gameObject.SetActive(true);
        robotBossAI = FindObjectOfType<RobotBossAI>();
        platformLastPos = platform.transform.position;

        AudioManager.Instance.RegisterSFX(platformStartAudioSource);
        AudioManager.Instance.RegisterSFX(platformStopAudioSource);
        AudioManager.Instance.RegisterSFX(platformAudioSource2);

        SpawnWave();
    }

    private void FixedUpdate()
    {

        if (!platform.transform.position.Equals(platformLastPos) && !platformAudioSource2.isPlaying)
        {
            platformAudioSource2.Play();
        }
        else if (platform.transform.position.Equals(platformLastPos) && platformAudioSource2.isPlaying)
        {
            platformAudioSource2.Stop();
        }
        platformLastPos = platform.transform.position;
    }

    IEnumerator ILiftUpPlatform()
    {
        TryPlayStartSound();
        while (!(platform.transform.position.y >= platformStopUpperY.position.y))
        {
            Vector3 prevPos = platform.transform.position;
            platform.transform.position += new Vector3(0, Time.deltaTime * platformMoveSpeed, 0);
            if (playerOnPlatform)
            {
                Vector3 displacement = platform.transform.position - prevPos;
                GameManager.Instance.playerMovement.GetRb().position += displacement;
            }
            rope.transform.localScale -= new Vector3(0, Time.deltaTime * ropeScaleSpeed, 0);
            yield return new WaitForFixedUpdate();
        }
        TryPlayStopSound();
    }

    public IEnumerator IPutDownPlatform()
    {
        TryPlayStartSound();
        while (!(platform.transform.position.y <= platformStopLowerY.position.y))
        {
            Vector3 prevPos = platform.transform.position;
            platform.transform.position -= new Vector3(0, Time.deltaTime * platformMoveSpeed, 0);
            if (playerOnPlatform)
            {
                Vector3 displacement = platform.transform.position - prevPos;
                GameManager.Instance.playerMovement.GetRb().position += displacement;
            }
            rope.transform.localScale += new Vector3(0, Time.deltaTime * ropeScaleSpeed, 0);
            yield return new WaitForFixedUpdate();
        }
        TryPlayStopSound();
    }

    IEnumerator IRotatePlatformRight()
    {
        TryPlayStartSound();
        while (!(craneHead.transform.rotation.eulerAngles.y >= 55))
        {
            Vector3 prevPos = platform.transform.position;
            craneHead.transform.rotation = Quaternion.RotateTowards(craneHead.transform.rotation, Quaternion.Euler(0, 55, 0), Time.deltaTime * headRotateSpeed);
            Vector3 displacement = platform.transform.position - prevPos;
            GameManager.Instance.playerMovement.GetRb().position += displacement;
            yield return new WaitForFixedUpdate();
        }
        TryPlayStopSound();
    }

    IEnumerator IRotatePlatformLeft()
    {
        TryPlayStartSound();
        while (!(craneHead.transform.rotation.eulerAngles.y - 360 <= -55) || craneHead.transform.rotation.y == 0)
        {
            Vector3 prevPos = platform.transform.position;
            craneHead.transform.rotation = Quaternion.RotateTowards(craneHead.transform.rotation, Quaternion.Euler(0, -55, 0), Time.deltaTime * headRotateSpeed);
            Vector3 displacement = platform.transform.position - prevPos;
            GameManager.Instance.playerMovement.GetRb().position += displacement;
            yield return new WaitForFixedUpdate();
        }
        TryPlayStopSound();
    }

    IEnumerator ILiftPlatformToTop()
    {
        TryPlayStartSound();
        while (!(platform.transform.position.y >= platformStopTopY.position.y))
        {
            Vector3 prevPos = platform.transform.position;
            platform.transform.position += new Vector3(0, Time.deltaTime * platformMoveSpeed, 0);
            if (playerOnPlatform)
            {
                Vector3 displacement = platform.transform.position - prevPos;
                GameManager.Instance.playerMovement.GetRb().position += displacement;
            }
            rope.transform.localScale -= new Vector3(0, Time.deltaTime * ropeScaleSpeed, 0);
            yield return new WaitForFixedUpdate();
        }
        TryPlayStopSound();
    }

    public IEnumerator IResetPlatformRotation()
    {
        TryPlayStartSound();
        while (craneHead.transform.rotation.eulerAngles.y != 0)
        {
            craneHead.transform.rotation = Quaternion.RotateTowards(craneHead.transform.rotation, Quaternion.identity, Time.deltaTime * headRotateSpeed);
            yield return new WaitForFixedUpdate();
        }
        TryPlayStopSound();
    }

    void TryPlayStopSound()
    {
        if (!platformStopAudioSource.isPlaying)
        {
            platformStopAudioSource.Play();
        }
    }

    void TryPlayStartSound()
    {
        if (!platformStartAudioSource.isPlaying)
        {
            platformStartAudioSource.Play();
        }
    }

    public void SendDownPlatform()
    {
        StopAllCoroutines();
        StartCoroutine(IResetPlatformRotation());
        StartCoroutine(IPutDownPlatform());
    }

    public void ResetPlatform()
    {
        StopAllCoroutines();
        StartCoroutine(IResetPlatformRotation());
        StartCoroutine(ILiftUpPlatform());
    }

    public void ToDestination()
    {
        StopAllCoroutines();
        switch (robotBossAI.GetPhase())
        {
            case 1:
                StartCoroutine(IRotatePlatformLeft());
                StartCoroutine(ILiftUpPlatform());
                break;
            case 2:
                StartCoroutine(IRotatePlatformRight());
                StartCoroutine(ILiftUpPlatform());
                break;
            case 3:
                StartCoroutine(ILiftPlatformToTop());
                break;
            default:
                break;
        }
    }

    

    IEnumerator ISpawnSnipers()
    {
        Transform sniperParent = enemyParent.Find("Snipers");
        foreach (Transform transform in sniperSpawnPoints)
        {
            if (sniperParent.childCount >= maxSnipers)
            {
                break;
            }
            int rad = Random.Range(0, 2);
            if (rad == 0)
            {
                GameObject sniper = Instantiate(sniperPrefab, transform.position, Quaternion.identity, sniperParent);
                sniper.GetComponent<SniperAI>().TargetToPlayer();
            }
            yield return new WaitForSeconds(.5f);
        }
    }

    IEnumerator ISpawnEnemies()
    {
        Transform soldierParent = enemyParent.Find("Soldiers");
        for (int i = soldierParent.childCount; i < maxSoldiers; i++)
        {
            Vector3 randomPos = UnityEngine.Random.insideUnitSphere * 120;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, 150, 1);

            int j = Random.Range(0, 2);
            if (j == 0)
            {
                GameObject go = Instantiate(soldierPrefab, hit.position, Quaternion.identity, soldierParent);
                go.GetComponent<EnemyBase>().TargetToPlayer();
            }
            else
            {
                GameObject go = Instantiate(dronePrefab, hit.position, Quaternion.identity, soldierParent);
                go.GetComponent<EnemyBase>().TargetToPlayer();
            }
            yield return new WaitForSeconds(.5f);
        }
        
    }

    public void SpawnWave()
    {
        StartCoroutine(ISpawnSnipers());
        StartCoroutine(ISpawnEnemies());
    }

    public void OpenSecurity(int num)
    {
        switch (num)
        {
            case 1:
                door1.transform.position += new Vector3(0, 22, 0);
                break;
            case 2:
                door2.transform.position += new Vector3(0, 22, 0);
                break;
            default:
                break;
        }
    }

    public void EnableLaser()
    {
        StartCoroutine(IEnableLaser());
    }

    IEnumerator IEnableLaser()
    {
        foreach (Transform tran in enemyParent)
        {
            foreach (Transform enemy in tran)
            {
                enemy.GetComponent<EnemyBase>().enabled = false;
            }
        }
        laserEffect.SetActive(true);
        StartCoroutine(DoCameraAnimation());
        yield return new WaitForSeconds(4f);
        laserEffect.SetActive(false);
        laser.SetActive(true);
        thumbsUp.SetActive(true);
        StartCoroutine(IKillBossWithLaser());
        Material mt = laser.GetComponent<Renderer>().material;
        float timer = 5f;
        damageArea.transform.position += new Vector3(0, 0.75f, 0);
        
        while (laser.transform.localScale.x > 5f)
        {
            mt.mainTextureOffset = new Vector2(mt.mainTextureOffset.x, mt.mainTextureOffset.y - Time.deltaTime * .4f);
            if (timer < 0)
            {
                laser.transform.localScale -= new Vector3(30f, 0f, 30f) * Time.deltaTime;
            }
            timer -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        laser.SetActive(false);
        yield return new WaitForSeconds(1f);
        Transform go = GameManager.Instance.transform.GetChild(0);
        Transform up = go.transform.GetChild(0);
        Transform down = go.transform.GetChild(1);
        timer = 1.5f;
        float timer2 = 2f;
        while (timer2 >= 0)
        {
            thumbsUp.transform.position -= new Vector3(0, Time.deltaTime, 0) * 10;
            timer -= Time.deltaTime;
            timer2 -= Time.deltaTime;
            if (timer <= 0)
            {
                up.localScale += new Vector3(0, Time.deltaTime, 0) * 15;
                down.localScale += new Vector3(0, Time.deltaTime, 0) * 15;
            }
            yield return new WaitForEndOfFrame();
        }

        //foreach (Transform item in GameManager.Instance.transform)
        //{
        //    item.gameObject.SetActive(false);
        //}
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(8);

    }

    IEnumerator DoCameraAnimation()
    {
        GameManager.Instance.playerScript.enabled = false;
        GameManager.Instance.playerMovement.enabled = false;
        
        UnityEngine.Camera cam = UnityEngine.Camera.main;
        foreach (Transform child in cam.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in GameManager.Instance.transform)
        {
            if (child.name != "BossHealthBar" && child.name != "EventSystem")
            {
                child.gameObject.SetActive(false);
            }
        }
        GameObject go = Instantiate(camBlackBar, GameManager.Instance.transform);
        go.transform.SetSiblingIndex(0);
        RectTransform up = go.transform.GetChild(0).GetComponent<RectTransform>();
        RectTransform down = go.transform.GetChild(1).GetComponent<RectTransform>();

        Vector3 destinationPos = new Vector3(0, 120, -60);
        Quaternion destinationRot = Quaternion.Euler(new Vector3(-75, 0, 0));
        StartCoroutine(CamLookDown());
        while (cam.transform.rotation != destinationRot || cam.transform.position != destinationPos)
        {
            up.anchoredPosition = Vector2.MoveTowards(up.anchoredPosition, new Vector2(0, 550), Time.deltaTime * 200);
            down.anchoredPosition = Vector2.MoveTowards(down.anchoredPosition, new Vector2(0, -550), Time.deltaTime * 200);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, destinationRot, Time.deltaTime * 100);
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, destinationPos, Time.deltaTime * 100);
            yield return new WaitForEndOfFrame();
        }
        
    }

    IEnumerator CamLookDown()
    {
        yield return new WaitForSeconds(3.7f);
        StopCoroutine(DoCameraAnimation());
        UnityEngine.Camera cam = UnityEngine.Camera.main;
        Quaternion destinationRot2 = Quaternion.Euler(new Vector3(60, 0, 0));
        while (cam.transform.rotation != destinationRot2)
        {
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, destinationRot2, Time.deltaTime * 300);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator IKillBossWithLaser()
    {
        while (robotBossAI.GetCurHP() > 0)
        {
            robotBossAI.OnTakeDamage(roofLaserDamage);
            yield return new WaitForSeconds(.2f);
        }
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UnregisterSFX(platformStartAudioSource);
            AudioManager.Instance.UnregisterSFX(platformStopAudioSource);
            AudioManager.Instance.UnregisterSFX(platformAudioSource2);
        }
    }
}
