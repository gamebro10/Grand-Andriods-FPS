using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaWave : MonoBehaviour
{
    [SerializeField] int damage;

    [SerializeField] BoxCollider coll;

    [SerializeField] GameObject damageArea;

    [SerializeField] Transform wavesParent;

    bool damaged;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IAttack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator IAttack()
    {
        float childCount = wavesParent.childCount;
        for (int i = 0; i < wavesParent.childCount; i++)
        {
            wavesParent.GetChild(i).gameObject.SetActive(true);
            float sizeZ = 98 / 16 * (i + 1);
            coll.size = new Vector3(coll.size.x, coll.size.y, sizeZ);
            coll.center = new Vector3(coll.center.x, coll.center.y, 0.88f + (sizeZ / 2));
            yield return new WaitForSeconds(.03f);
        }
        damageArea.SetActive(true);
        damageArea.transform.SetParent(null);
        for (int j = 0; j < wavesParent.childCount; j++)
        {
            wavesParent.GetChild(j).gameObject.SetActive(false);
            float sizeZ = 98 / 16 * (childCount);
            float sizeZTemp = 98 / 16 * (j + 1);
            childCount--;
            coll.size = new Vector3(coll.size.x, coll.size.y, sizeZ);
            coll.center = new Vector3(coll.center.x, coll.center.y, 45.88f + (sizeZTemp / 2));
            yield return new WaitForSeconds(.03f);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            damaged = true;
            IDamage damageable = other.GetComponent<IDamage>();
            damageable.OnTakeDamage(damage);
        }
    }
}
