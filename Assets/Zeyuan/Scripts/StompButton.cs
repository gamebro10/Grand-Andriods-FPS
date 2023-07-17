using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class StompButton : MonoBehaviour
{
    [SerializeField] Color colorInactive;
    [SerializeField] Color colorActive;
    [SerializeField] Color colorPressed;
    [SerializeField] Stat initialStat;
    [SerializeField] UnityEvent registeredEvent;

    [Header("---------------------------------")]
    [SerializeField] GameObject stompText;
    [SerializeField] GameObject button;
    


    float glowColorG;
    float stompTextFloatingVal;
    Material mt;
    Stat currStat;

    enum Stat
    {
        None,
        Inactive,
        Active,
        Pressed,
    }

    private void Start()
    {
        mt = button.GetComponent<MeshRenderer>().material;
        currStat = initialStat;
        switch (currStat)
        {
            case Stat.Inactive:
                mt.color = colorInactive;
                break;
            case Stat.Active:
                mt.color = colorActive;
                stompText.SetActive(true);
                break;
            case Stat.Pressed:
                mt.color = colorPressed;
                transform.localPosition = new Vector3(0, 0.92f, 0);
                PressedButton();
                break;
        };
    }

    private void Update()
    {
        switch (currStat)
        {
            case Stat.Active:
                GlowMaterial();
                UpdateStompText();
                break;
            case Stat.Pressed:
                PressedButton();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.playerMovement.GetRb().velocity.y <= -30)
            {
                StartCoroutine(IPressed());
            }
        }
    }

    IEnumerator IPressed()
    {
        currStat = Stat.Pressed;
        registeredEvent.Invoke();
        while (transform.localPosition.y > 0.92)
        {
            transform.localPosition = new Vector3(0, transform.localPosition.y - Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    void GlowMaterial()
    {
        if (currStat == Stat.Active)
        {
            if (!mt.IsKeywordEnabled("_EMISSION"))
            {
                mt.EnableKeyword("_EMISSION");
            }
            float g = Mathf.Abs(Mathf.Sin(glowColorG));
            mt.SetColor("_EmissionColor", new Color(0, g, 0) * Mathf.LinearToGammaSpace(2f));
            glowColorG += Time.deltaTime * 2;
        }
    }
    
    void UpdateStompText()
    {
        if (!GameManager.Instance.isPaused)
        {
            Vector3 playerDir = GameManager.Instance.player.transform.position - stompText.transform.position;
            Quaternion rot = Quaternion.LookRotation(new Vector3(-playerDir.x, 0, -playerDir.z));
            stompText.transform.rotation = Quaternion.Lerp(stompText.transform.rotation, rot, Time.deltaTime * 100);
            stompText.transform.position += new Vector3(0, Mathf.Sin(stompTextFloatingVal) * .1f, 0);
            stompTextFloatingVal += Time.deltaTime * 10;
        }
    }

    public void ActivateButton()
    {
        currStat = Stat.Active;
        stompText.SetActive(true);
        GetComponent<MeshCollider>().enabled = true;
        mt.color = colorActive;
        transform.localPosition = new Vector3(0, 1.64f, 0);
    }

    void PressedButton()
    {
        mt.DisableKeyword("_EMISSION");
        mt.color = colorPressed;
        stompText.SetActive(false);
        GetComponent<MeshCollider>().enabled = false;
        currStat = Stat.None;
    }
    
}
