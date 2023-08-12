using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShockWave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
        StartCoroutine(IEnableShockWave());
        Destroy(gameObject, 5f);
    }

    IEnumerator IEnableShockWave()
    {
        float timer = 2f;
        while (timer > 0)
        {
            transform.localScale += new Vector3(Time.deltaTime * 250, 0, Time.deltaTime * 250);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
