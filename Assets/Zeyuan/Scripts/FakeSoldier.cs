using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeSoldier : MonoBehaviour
{
    [SerializeField] float listScrollingTime;
    [SerializeField] Image volumetricLight1;
    [SerializeField] Image volumetricLight2;
    [SerializeField] Image volumetricLight3;

    float tweenSpeed = .1f;

    // Start is called before the first frame update
    void Start()
    {
        volumetricLight1.color = new Color(255, 255, 255, 0);
        volumetricLight2.color = new Color(255, 255, 255, 0);
        volumetricLight3.color = new Color(255, 255, 255, 0);
        StartCoroutine(IShowLight());
    }

    IEnumerator IShowLight()
    {
        while (volumetricLight1.color.a < 100f / 255f)
        {
            volumetricLight1.color = new Color(255, 255, 255, volumetricLight1.color.a + Time.deltaTime * 10f);
            volumetricLight2.color = new Color(255, 255, 255, volumetricLight2.color.a + Time.deltaTime * 10f);
            volumetricLight3.color = new Color(255, 255, 255, volumetricLight3.color.a + Time.deltaTime * 10f);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(IDoLightTween(volumetricLight1));
        StartCoroutine(IDoLightTween(volumetricLight2));
        StartCoroutine(IDoLightTween(volumetricLight3));
    }

    IEnumerator IDoLightTween(Image image)
    {
        float rand;
        if (image == volumetricLight1)
        {
            rand = Random.Range(90f, 110f);
        }
        else
        {
            rand = Random.Range(20f, 100f);
        }
        float randTweenSpeed = tweenSpeed * Random.Range(.8f, 1.2f);
        while (Mathf.Abs(image.color.a - rand / 255f) > 0.01f)
        {
            if (image.color.a < rand / 255f)
            {
                image.color = new Color(255, 255, 255, (image.color.a + Time.deltaTime * randTweenSpeed) > rand / 255f ? rand / 255f : image.color.a + Time.deltaTime * randTweenSpeed);
            }
            else
            {
                image.color = new Color(255, 255, 255, (image.color.a - Time.deltaTime * randTweenSpeed) < rand / 255f ? rand / 255f : image.color.a - Time.deltaTime * randTweenSpeed);
            }
            yield return new WaitForEndOfFrame();
        }
        float rand2 = Random.Range(1f, 2f);
        yield return new WaitForSeconds(rand2);
        StartCoroutine(IDoLightTween(image));
    }
}
