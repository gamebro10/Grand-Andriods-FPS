using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI memberList;

    private void Start()
    {
        StartCoroutine(IShowCredits());
    }

    IEnumerator IShowCredits()
    {
        while (memberList.color.a <= 1)
        {
            memberList.color += new Color(0, 0, 0, Time.deltaTime) * 10f;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2f);
        while (memberList.color.a >= 0)
        {
            memberList.color -= new Color(0, 0, 0, Time.deltaTime) * 40f;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2f);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
