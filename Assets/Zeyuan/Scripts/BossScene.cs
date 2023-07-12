using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class BossScene : MonoBehaviour
{
    [SerializeField] float platformMoveSpeed;
    [SerializeField] float ropeScaleSpeed;
    [SerializeField] float headRotateSpeed;
    [SerializeField] GameObject craneHead;
    [SerializeField] GameObject platform;
    [SerializeField] GameObject rope;
    [SerializeField] GameObject sniperPrefab;
    [SerializeField] Transform platformStopY;
    [SerializeField] Transform platformStopLowerY;
    [SerializeField] Transform[] sniperSpawnPoints;

    public static BossScene Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InstantiateSnipers();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator ILiftUpPlatform()
    {
        while (!(platform.transform.position.y >= platformStopY.position.y))
        {
            platform.transform.position += new Vector3(0, Time.deltaTime * platformMoveSpeed, 0);
            rope.transform.localScale -= new Vector3(0, Time.deltaTime * ropeScaleSpeed, 0);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(IRotatePlatformLeft());
    }

    public IEnumerator IPutDownPlatform()
    {
        while (!(platform.transform.position.y <= platformStopLowerY.position.y))
        {
            platform.transform.position -= new Vector3(0, Time.deltaTime * platformMoveSpeed, 0);
            rope.transform.localScale += new Vector3(0, Time.deltaTime * ropeScaleSpeed, 0);
            yield return new WaitForEndOfFrame();
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
            yield return new WaitForEndOfFrame();
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
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator ResetPlatformPosition()
    {
        yield return new WaitForSeconds(1);
        while (craneHead.transform.rotation.eulerAngles.y != 0)
        {
            craneHead.transform.rotation = Quaternion.RotateTowards(craneHead.transform.rotation, Quaternion.identity, Time.deltaTime * headRotateSpeed);
            yield return new WaitForEndOfFrame();
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
}
