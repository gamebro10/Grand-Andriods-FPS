using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    float floatAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            float pos = Mathf.Sin(floatAmount) * Mathf.PI / 180 * .2f;
            floatAmount += Time.deltaTime * 4;
            transform.position += new Vector3(0, pos, 0);
            transform.Rotate(new Vector3(0, Time.deltaTime, 0) * 40, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.statePaused();
        GameManager.Instance.activeMenu = GameManager.Instance.winMenu;
        GameManager.Instance.activeMenu.SetActive(true);
        Destroy(gameObject);
    }
}
