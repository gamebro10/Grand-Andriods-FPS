using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour, IDamage
{
    public static PlayerScript Instance;
    public GameManager GameManager;

    [Header("-----Player Values-----")]
    [SerializeField] int playerHP;
    [SerializeField] public Camera playerCam;

    [Header("-----Player Attributes-----")]
    public float playerSpeed;
    public float moveSpeed;
    public float runSpeed;
    public int HPOrig;
    public  int InvinMax;
    public int InvinTimer;

    void Awake()
    {
        GameManager = GetComponent<GameManager>();
    }

    void Start()
    {
        Instance = this;
        moveSpeed = PlayerMovement.Instance.moveSpeed;
        runSpeed = PlayerMovement.Instance.runSpeed;
        HPOrig = playerHP;
        spawnPlayer();
    }

    void Update()
    {
        if (GameManager.Instance.isPaused == false)
        {
            if (InvinTimer < InvinMax)
            {
                InvinTimer += 1;
            }
        }
    }

    public void OnTakeDamage(int amount)
    {
        if (amount > 0)
        {
            if (InvinTimer == InvinMax)
            {
                playerHP -= amount;
                StartCoroutine(GameManager.Instance.playerFlashDamage(true));
                InvinTimer = 0;
            }
        }
        else
        {
            playerHP -= amount;
            StartCoroutine(GameManager.Instance.playerFlashDamage(false));
        }
        GameManager.Instance.playerHPBar.fillAmount = (float)playerHP / HPOrig;
        if (playerHP <= 0)
        {
            GameManager.Instance.youLose();
        }
        if (playerHP >= HPOrig)
        {
            playerHP = HPOrig;
        }
    }

    public void spawnPlayer()
    {
        //GameManager.Instance.playerMovement.enabled = false;
        GameManager.Instance.playerMovement.GetRb().MovePosition(GameManager.Instance.playerSpawnPos.transform.position);
        GameManager.Instance.playerMovement.enabled = true;
        playerHP = HPOrig;
        GameManager.Instance.playerHPBar.fillAmount = (float)playerHP / HPOrig;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
