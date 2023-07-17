using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected float range;
    [SerializeField] protected Outline outLine;
    [SerializeField] protected LayerMask layerMask;

    protected bool canInteract;
    protected virtual void CheckInteraction()
    {
        if ((transform.position - GameManager.Instance.player.transform.position).magnitude <= range)
        {
            RaycastHit hit;
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, layerMask))
            {
                if (hit.collider.gameObject == gameObject) 
                {
                    outLine.OutlineWidth = 5f;
                    canInteract = true;
                }
            }
            else
            {
                outLine.OutlineWidth = 0f;
                canInteract = false;
            }
        }
    }
    protected virtual void OnInteract()
    {
        outLine.OutlineWidth = 0f;
        Destroy(this);
    }
}
