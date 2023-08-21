using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDash : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] float cd;

    bool canDash = true;
    Rigidbody rb;
    Transform orientation;
    private PlayerMovement2 boost;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        orientation = rb.transform.Find("Orientation");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(IDash());
        }
    }

    IEnumerator IDash()
    {
        Vector3 originalVelocity = rb.velocity;
        StartCoroutine(ICD());
        float timer = .3f;
        while (timer > 0)
        {
            rb.velocity = orientation.transform.forward.normalized * force;

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        rb.velocity = orientation.transform.forward.normalized * originalVelocity.magnitude;
    }

    IEnumerator ICD()
    {
        canDash = false;
        yield return new WaitForSeconds(cd);
        canDash = true;
    }
}

