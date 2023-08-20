using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] float listScrollingSpeed;
    [SerializeField] TextMeshProUGUI memberList;
    [SerializeField] Image panel;
    [SerializeField] Image skip;
    [SerializeField] Texture2D cursorSprite;
    [SerializeField] AudioSource musicAudioSource;

    float pressTimer;
    float originalSpeed;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        skip.fillAmount = 0;
        originalSpeed = listScrollingSpeed;
        AudioManager.Instance.RegisterMusic(musicAudioSource);

        memberList.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -540);
        panel.color = new Color(0, 0, 0, 1);
        StartCoroutine(IDisablePanel());
        StartCoroutine(IShowCredits());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            pressTimer += Time.deltaTime;
        }
        else
        {
        pressTimer = pressTimer - Time.deltaTime * 2 < 0 ? 0 : pressTimer - Time.deltaTime;
        }

        if (Input.GetButton("Shoot"))
        {
            listScrollingSpeed = originalSpeed * 3f;
            Cursor.SetCursor(cursorSprite, Vector2.zero, CursorMode.Auto);
        }
        else if (Input.GetButtonUp("Shoot"))
        {
            listScrollingSpeed = originalSpeed;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        skip.fillAmount = pressTimer / 2f;
        if (pressTimer >= 2f)
        {
            StopAllCoroutines();
            StartCoroutine(ISkipped());
        }
    }

    IEnumerator ISkipped()
    {
        StartCoroutine(IEnablePanel(10f));
        yield return new WaitForSeconds(.1f);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    IEnumerator IDisablePanel()
    {
        while (panel.color.a > 0)
        {
            panel.color = new Color(0, 0, 0, panel.color.a - Time.deltaTime * 1.5f);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator IEnablePanel(float speedMultiplier = 1f)
    {
        while (panel.color.a < 1)
        {
            panel.color = new Color(0, 0, 0, panel.color.a + Time.deltaTime * .5f * speedMultiplier);
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

    private void OnDestroy()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UnregisterMusic(musicAudioSource);
        }
    }
}
