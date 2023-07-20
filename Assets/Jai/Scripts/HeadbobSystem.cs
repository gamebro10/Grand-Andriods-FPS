using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadbobSystem : MonoBehaviour
{
    [Range(0.001f, 0.1f)][SerializeField] public float Amount;

    [Range(1f, 30f)][SerializeField] public float Frequency;

    [Range(10f, 100f)][SerializeField] public float Smooth;

    Vector3 StartPos;

    void Start()
    {

        StartPos = transform.localPosition;
    }

    void Update()
    {
        if (!GameManager.Instance.playerMovement.crouching && GameManager.Instance.playerMovement.grounded)
        {
            Debug.Log("bobbing");
            CheckForHeadbobTrigger();
            StopHeadbob();
        }
    }

    private void CheckForHeadbobTrigger()
    {

        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0)
        {

            StartHeadBob();
        }
    }

    private Vector3 StartHeadBob()
    {

        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);
        transform.localPosition += pos;

        return pos;
    }

    private void StopHeadbob()
    {

        if (transform.localPosition == StartPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, 1 * Time.deltaTime);

    }
}
