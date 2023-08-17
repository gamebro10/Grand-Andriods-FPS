using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] float listScrollingSpeed;
    [SerializeField] TextMeshProUGUI memberList;
    [SerializeField] Image panel;

    private void Start()
    {
        memberList.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -540);
        panel.color = new Color(0, 0, 0, 1);
        StartCoroutine(IDisablePanel());
        StartCoroutine(IShowCredits());
    }

    IEnumerator IDisablePanel()
    {
        while (panel.color.a > 0)
        {
            panel.color = new Color(0, 0, 0, panel.color.a - Time.deltaTime * 1.5f);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator IEnablePanel()
    {
        while (panel.color.a < 1)
        {
            panel.color = new Color(0, 0, 0, panel.color.a + Time.deltaTime * .5f);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator IShowCredits()
    {
        //while (memberList.color.a <= 1)
        //{
        //    memberList.color += new Color(0, 0, 0, Time.deltaTime) * 10f;
        //    yield return new WaitForEndOfFrame();
        //}
        //yield return new WaitForSeconds(2f);
        //while (memberList.color.a >= 0)
        //{
        //    memberList.color -= new Color(0, 0, 0, Time.deltaTime) * 40f;
        //    yield return new WaitForEndOfFrame();
        //}
        //yield return new WaitForSeconds(2f);
        RectTransform tran = memberList.GetComponent<RectTransform>();
        //float originalPos = tran.anchoredPosition.y;
        float destinationPos = memberList.preferredHeight + 540;
        while (tran.anchoredPosition.y < destinationPos)
        {
            tran.anchoredPosition += new Vector2(tran.anchoredPosition.x, Time.deltaTime * listScrollingSpeed);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(IEnablePanel());
        yield return new WaitForSeconds(4f);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
