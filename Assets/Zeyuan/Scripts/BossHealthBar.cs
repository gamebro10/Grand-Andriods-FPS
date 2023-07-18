using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] Image fg;
    [SerializeField] Image fgLocked;
    [SerializeField] Image fgDamaged;
    [SerializeField] GameObject phase1;
    [SerializeField] GameObject phase2;
    [SerializeField] GameObject phase3;


    bool canShakeHealthBar;
    bool isDamaging;
    Vector3 originalPosition;
    RobotBossAI bossScript;

    // Start is called before the first frame update
    void Start()
    {
        bossScript = FindObjectOfType<RobotBossAI>();
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            if (canShakeHealthBar)
            {
                transform.position = new Vector3(Random.Range(-6, 6), Random.Range(-3, 3), 0) + originalPosition;
            }
        }
    }

    public void FillHealthBar(float amount)
    {
        fg.fillAmount = amount;
        StartCoroutine(HealthBarShake());
        StartCoroutine(ILerpHealthBar());
    }

    IEnumerator ILerpHealthBar()
    {
        if (!isDamaging)
        {
            isDamaging = true;
            yield return new WaitForSeconds(.5f);
            float originalAmount = fgDamaged.fillAmount;
            float val = 0;
            float goal = fg.fillAmount;
            while (fgDamaged.fillAmount > goal)
            {
                fgDamaged.fillAmount = Mathf.Lerp(originalAmount, goal, val);
                val += Time.deltaTime * 5f;
                yield return new WaitForEndOfFrame();
            }
            isDamaging = false;
        }
    }

    public void Phase(int phase)
    {
        switch (phase)
        {
            case 1:
                if (phase1.activeSelf)
                {
                    phase1.SetActive(false);
                }
                break;
            case 2:
                if (phase2.activeSelf)
                {
                    phase2.SetActive(false);
                }
                break;
            case 3:
                if (phase3.activeSelf)
                {
                    phase3.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    public void LockHealthBar(bool shouldLock)
    {
        fg.gameObject.SetActive(!shouldLock);
        fgDamaged.gameObject.SetActive(!shouldLock);
        StopCoroutine(ILerpHealthBar());
        fgDamaged.fillAmount = fg.fillAmount;
        fgLocked.fillAmount = fg.fillAmount;
        fgLocked.gameObject.SetActive(shouldLock);
    }

    IEnumerator HealthBarShake()
    {
        canShakeHealthBar = true;
        yield return new WaitForSeconds(.3f);
        canShakeHealthBar = false;
        transform.position = originalPosition;
    }
}
