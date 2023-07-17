using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCase : Interactable
{
    [SerializeField] GameObject battery;
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
            base.OnInteract();
        }
    }
}
