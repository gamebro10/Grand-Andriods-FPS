using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] bool updateUVOffset;
    [SerializeField] int damage;
    [SerializeField] float damageInterval;

    bool canDoDamage = true;
    Material mt;

    // Start is called before the first frame update
    void Start()
    {
        mt = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (updateUVOffset)
        {
            mt.mainTextureOffset += new Vector2(1f, .5f) * Time.deltaTime * Random.Range(0.8f, 1.2f) * 0.01f;
            mt.SetTextureOffset("_DetailAlbedoMap", mt.GetTextureOffset("_DetailAlbedoMap") + new Vector2(1f, .5f) * Time.deltaTime * Random.Range(0.8f, 1.2f) * 0.02f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canDoDamage)
        {
            StartCoroutine(DoDamage());
            other.GetComponent<IDamage>().OnTakeDamage(damage);
        }
    }

    IEnumerator DoDamage()
    {
        canDoDamage = false;
        yield return new WaitForSeconds(damageInterval);
        canDoDamage = true;
    }
}
