using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] Transform enemyParent;
    [SerializeField] Transform platformStopTopY;
    [SerializeField] Transform platformStopUpperY;
    [SerializeField] Transform platformStopLowerY;
    [SerializeField] Transform[] sniperSpawnPoints;

    RobotBossAI robotBossAI;

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

        SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        //temp testing code
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(IEnableLaser());
        }
    }

    IEnumerator ILiftUpPlatform()
    {
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
    }

    public IEnumerator IPutDownPlatform()
    {
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
    }

    IEnumerator IRotatePlatformRight()
    {
        while (!(craneHead.transform.rotation.eulerAngles.y >= 55))
        {
            Vector3 prevPos = platform.transform.position;
            craneHead.transform.rotation = Quaternion.RotateTowards(craneHead.transform.rotation, Quaternion.Euler(0, 55, 0), Time.deltaTime * headRotateSpeed);
            Vector3 displacement = platform.transform.position - prevPos;
            GameManager.Instance.playerMovement.GetRb().position += displacement;
            yield return new WaitForFixedUpdate();
        }
        
    }

    IEnumerator IRotatePlatformLeft()
    {
        while (!(craneHead.transform.rotation.eulerAngles.y - 360 <= -55) || craneHead.transform.rotation.y == 0)
        {
            Vector3 prevPos = platform.transform.position;
            craneHead.transform.rotation = Quaternion.RotateTowards(craneHead.transform.rotation, Quaternion.Euler(0, -55, 0), Time.deltaTime * headRotateSpeed);
            Vector3 displacement = platform.transform.position - prevPos;
            GameManager.Instance.playerMovement.GetRb().position += displacement;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator ILiftPlatformToTop()
    {
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
    }

    public IEnumerator IResetPlatformRotation()
    {
        while (craneHead.transform.rotation.eulerAngles.y != 0)
        {
            craneHead.transform.rotation = Quaternion.RotateTowards(craneHead.transform.rotation, Quaternion.identity, Time.deltaTime * headRotateSpeed);
            yield return new WaitForFixedUpdate();
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
        laserEffect.SetActive(true);
        yield return new WaitForSeconds(4f);
        laserEffect.SetActive(false);
        laser.SetActive(true);
        StartCoroutine(IKillBossWithLaser());
        Material mt = laser.GetComponent<Renderer>().material;
        float timer = 5f;
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
    }

    IEnumerator IKillBossWithLaser()
    {
        while (robotBossAI.GetCurHP() > 0)
        {
            robotBossAI.OnTakeDamage(roofLaserDamage);
            yield return new WaitForSeconds(.2f);
        }
    }
}
