using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour, IDamage
{
    public static PlayerScript Instance;

    [Header("-----Player Values-----")]
    [SerializeField] int playerHP;
    [SerializeField] public Camera playerCam;

    [Header("-----Player Attributes-----")]
    public float playerSpeed;
    public float moveSpeed;
    public float runSpeed;
    public int HPOrig;


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
       GameManager.Instance.speedometerBar.fillAmount = playerSpeed/(runSpeed*moveSpeed);
       //GameManager.Instance.speedometerText.text = playerSpeed.ToString();
    }

    public void OnTakeDamage(int amount)
    {
        playerHP -= amount;
        StartCoroutine(GameManager.Instance.playerFlashDamage());
        GameManager.Instance.playerHPBar.fillAmount = (float)playerHP / HPOrig;
        if (playerHP <= 0)
        {
            GameManager.Instance.youLose();
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
