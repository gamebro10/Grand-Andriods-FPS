using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour, IDamage
{
    public static PlayerScript Instance;

    [Header("-----Player Values-----")]
    [SerializeField] int playerHP;

    [Header("-----Player Attributes-----")]
    public float playerSpeed;
    public float moveSpeed = PlayerMovement.Instance.moveSpeed;
    public float runSpeed =PlayerMovement.Instance.runSpeed;
    public int HPOrig;

    void Start()
    {

    }

    void Update()
    {
        updatePlayerUI();
    }

    public void OnTakeDamage(int amount)
    {
        playerHP -= amount;
        /*StartCoroutine(GameManager.Instance.playerFlashDamage());
         updatePlayerUI();
        */
        if (playerHP <= 0)
        {
            GameManager.Instance.youLose();
        }
    }

    public void updatePlayerUI()
    {
        playerSpeed = PlayerMovement.Instance.rb.velocity.magnitude;
        GameManager.Instance.playerHPBar.fillAmount = (float)playerHP / HPOrig;
        GameManager.Instance.speedometerText.text = playerSpeed.ToString();
        GameManager.Instance.speedometerBar.fillAmount = playerSpeed / (moveSpeed * runSpeed);
    }

    public void spawnPlayer()
    {

        //transform.position = GameManager.Instance.playerSpawnPos.transform.position;

        HPOrig = playerHP;
        //playerCollider = GetComponent<Collider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //readyToJump = true;
        //wallNormalVector = Vector3.up;
    }
}
