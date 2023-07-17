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
    [SerializeField] GameObject craneHead;
    [SerializeField] GameObject platform;
    [SerializeField] GameObject rope;
    [SerializeField] GameObject sniperPrefab;
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] GameObject dronePrefab;
    [SerializeField] Transform platformStopTopY;
    [SerializeField] Transform platformStopUpperY;
    [SerializeField] Transform platformStopLowerY;
    [SerializeField] Transform[] sniperSpawnPoints;
    [SerializeField] NavMeshSurface navMesh;

    RobotBossAI robotBossAI;

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


        InstantiateSnipers();
        StartCoroutine(ISpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ILiftUpPlatform()
    {
        while (!(platform.transform.position.y >= platformStopUpperY.position.y))
        {
            platform.transform.position += new Vector3(0, Time.deltaTime * platformMoveSpeed, 0);
            rope.transform.localScale -= new Vector3(0, Time.deltaTime * ropeScaleSpeed, 0);
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator IPutDownPlatform()
    {
        while (!(platform.transform.position.y <= platformStopLowerY.position.y))
        {
            platform.transform.position -= new Vector3(0, Time.deltaTime * platformMoveSpeed, 0);
            rope.transform.localScale += new Vector3(0, Time.deltaTime * ropeScaleSpeed, 0);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator IRotatePlatformRight()
    {
        while (!(craneHead.transform.rotation.eulerAngles.y >= 48))
        {
            Vector3 prevPos = platform.transform.position;
            craneHead.transform.rotation = Quaternion.RotateTowards(craneHead.transform.rotation, Quaternion.Euler(0, 48, 0), Time.deltaTime * headRotateSpeed);
            Vector3 displacement = platform.transform.position - prevPos;
            GameManager.Instance.playerMovement.GetRb().position += displacement;
            yield return new WaitForFixedUpdate();
        }
        
    }

    IEnumerator IRotatePlatformLeft()
    {
        while (!(craneHead.transform.rotation.eulerAngles.y - 360 <= -48) || craneHead.transform.rotation.y == 0)
        {
            Vector3 prevPos = platform.transform.position;
            craneHead.transform.rotation = Quaternion.RotateTowards(craneHead.transform.rotation, Quaternion.Euler(0, -48, 0), Time.deltaTime * headRotateSpeed);
            Vector3 displacement = platform.transform.position - prevPos;
            GameManager.Instance.playerMovement.GetRb().position += displacement;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator ILiftPlatformToTop()
    {
        while (!(platform.transform.position.y >= platformStopTopY.position.y))
        {
            platform.transform.position += new Vector3(0, Time.deltaTime * platformMoveSpeed, 0);
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

    

    void InstantiateSnipers()
    {
        foreach (Transform transform in sniperSpawnPoints)
        {
            int rad = Random.Range(0, 2);
            if (rad == 0)
            {
                GameObject sniper = Instantiate(sniperPrefab, transform.position, Quaternion.identity);
                sniper.GetComponent<SniperAI>().TargetToPlayer();
            }
        }
    }

    IEnumerator ISpawnEnemies()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPos = UnityEngine.Random.insideUnitSphere * 150;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, 150, 1);

            int j = Random.Range(0, 2);
            if (j == 0)
            {
                Instantiate(soldierPrefab, hit.position, Quaternion.identity);
            }
            else
            {
                Instantiate(dronePrefab, hit.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(.5f);
        }
        
    }
}
