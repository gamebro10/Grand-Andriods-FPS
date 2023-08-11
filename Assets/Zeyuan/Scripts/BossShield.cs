using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour
{
    [SerializeField] GameObject charge;
    [SerializeField] GameObject damageArea;
    [SerializeField] GameObject shockWave;
    [SerializeField] Transform endPoint;
    [SerializeField] Transform midPoint;
    [SerializeField] Renderer render;

    bool canShootCharge;
    bool isOnPosition;
    bool isMovingToRandomPos;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ILiftUp());
    }

    // Update is called once per frame
    void Update()
    {
        render.material.mainTextureOffset = new Vector2(render.material.mainTextureOffset.x + Time.deltaTime * .1f, render.material.mainTextureOffset.y);
        damageArea.transform.Rotate(new Vector3(0, Time.deltaTime * 50f, 0));
        if (isOnPosition && !isMovingToRandomPos)
        {
            StartCoroutine(IMoveToRandomPos());
        }

        if (canShootCharge)
        {
            StartCoroutine(IShootCharge());
        }
    }

    IEnumerator ILiftUp()
    {
        while (damageArea.transform.localPosition.y < 1.3f)
        {
            damageArea.transform.localPosition = new Vector3(0, damageArea.transform.localPosition.y + Time.deltaTime * .25f, 0);
            yield return new WaitForEndOfFrame();
        }
        canShootCharge = true;
        isOnPosition = true;
    }

    IEnumerator IMoveToRandomPos()
    {
        isMovingToRandomPos = true;
        Vector3 randomPos = new Vector3(0, 1.3f, 0) + UnityEngine.Random.onUnitSphere * .5f;
        while ((damageArea.transform.localPosition - randomPos).magnitude > 0.1f)
        {
            damageArea.transform.localPosition = Vector3.MoveTowards(damageArea.transform.localPosition, randomPos, Time.deltaTime * .1f);
            yield return new WaitForEndOfFrame();
        }
        isMovingToRandomPos = false;
    }

    IEnumerator IShootCharge()
    {
        canShootCharge = false;
        GameObject go = Instantiate(charge, damageArea.transform.position, Quaternion.identity);
        go.SetActive(true);
        Vector3 p0 = damageArea.transform.position;
        Vector3 p1 = midPoint.position;
        Vector3 p2 = endPoint.position;
        float amount = 0;
        while (go.transform.position.y >= endPoint.position.y)
        {
            Vector3 p0p1 = (1 - amount) * p0 + amount * p1;
            Vector3 p1p2 = (1 - amount) * p1 + amount * p2;
            Vector3 result = (1 - amount) * p0p1 + amount * p1p2;
            go.transform.position = result;
            amount += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(IChargeHitShield(go));
        
    }

    IEnumerator IChargeHitShield(GameObject go)
    {
        Destroy(go);
        canShootCharge = true;
        float timer = 1f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            render.materials[1].mainTextureOffset = new Vector2(0, render.materials[1].mainTextureOffset.y + Time.deltaTime);
            float alpha = render.materials[1].color.a - Time.deltaTime * .5f < 0 ? 0 : render.materials[1].color.a - Time.deltaTime * .5f;
            render.materials[1].color = new Color(0, 0, 0, alpha);
            yield return new WaitForEndOfFrame();
        }
        render.materials[1].mainTextureOffset = new Vector2(0, -0.08f);
        render.materials[1].color = new Color(0, 0, 0, 50f/250f);
    }

    public void OnDisabled()
    {
        shockWave.SetActive(true);
        shockWave.transform.SetParent(null);
    }

}
