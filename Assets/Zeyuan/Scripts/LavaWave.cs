using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaWave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IAttack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator IAttack()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(.05f);
        }
        for (int j = 0; j < transform.childCount; j++)
        {
            transform.GetChild(j).gameObject.SetActive(false);
            yield return new WaitForSeconds(.05f);
        }
    }
}
