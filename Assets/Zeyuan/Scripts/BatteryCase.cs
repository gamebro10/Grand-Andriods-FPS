using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCase : Interactable
{
    [SerializeField] GameObject battery;
    [SerializeField] GameObject handle;
    [SerializeField] GameObject wire;
    [SerializeField] GameObject batteryLight;
    private void Update()
    {
        CheckInteraction();
        OnInteract();
    }

    protected override void OnInteract()
    {
        if (Input.GetKeyDown(KeyCode.E) && canInteract)
        {
            battery.SetActive(true);
            BossScene.Instance.ResetPlatform();
            BossScene.Instance.stompButton.ActivateButton();
            handle.transform.localRotation = Quaternion.Euler(new Vector3(52f, -52f, -94f));
            batteryLight.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            wire.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            int phase = FindObjectOfType<RobotBossAI>().GetPhase();
            if (phase == 3)
            {
                BossScene.Instance.EnableLaser();
                FindObjectOfType<RobotBossAI>().LockHealthBar(false);
                GameManager.Instance.bossHealthBar.Phase(3);
            }
            base.OnInteract();
        }
    }
}
